using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using ZeXal.DMG;
using Button = ZeXal.DMG.Button;

namespace ZeXal.Viewer
{
    //Window showing the DMG-001 display in real time.
    //Emulation runs on a background thread at ~59.73 frames per second;
    //every completed frame is copied into a bitmap and painted scaled (nearest neighbor).
    class MainForm : Form
    {
        private const double FrameSeconds = (double)PPU.CyclesPerFrame / 4194304.0;

        private readonly object emulator_lock = new object();
        private readonly object pixels_lock = new object();

        private GameBoy gb;
        private string rom_path;

        private Thread emulation_thread;
        private volatile bool running;
        private volatile bool paused;
        private volatile bool turbo;
        private volatile bool frame_pending;
        private volatile uint[] palette = PPU.DMGPalette;

        private readonly int[] pixels = new int[PPU.ScreenWidth * PPU.ScreenHeight];
        private readonly Bitmap screen = new Bitmap(PPU.ScreenWidth, PPU.ScreenHeight, PixelFormat.Format32bppRgb);

        private readonly DisplayPanel display;
        private readonly MenuStrip menu;
        private ToolStripMenuItem pause_item;
        private ToolStripMenuItem[] scale_items;
        private ToolStripMenuItem green_item, gray_item;

        private int scale;
        private int frames_shown;
        private readonly Stopwatch fps_watch = new Stopwatch();
        private double fps;

        public MainForm(string initial_rom, int initial_scale)
        {
            Text = "ZeXal";
            BackColor = Color.Black;
            AllowDrop = true;
            KeyPreview = true;
            StartPosition = FormStartPosition.CenterScreen;

            display = new DisplayPanel(screen);
            display.Dock = DockStyle.Fill;

            menu = BuildMenu();

            Controls.Add(display);
            Controls.Add(menu);
            MainMenuStrip = menu;

            SetScale(initial_scale);

            DragEnter += OnDragEnter;
            DragDrop += OnDragDrop;
            KeyUp += OnKeyUp;
            Deactivate += delegate { ReleaseAllButtons(); };

            if (initial_rom != null) LoadROM(initial_rom);
            else UpdateTitle();
        }

        #region Menu

        private MenuStrip BuildMenu()
        {
            MenuStrip strip = new MenuStrip();

            ToolStripMenuItem file = new ToolStripMenuItem("&File");
            file.DropDownItems.Add(MenuItem("&Open ROM...", Keys.Control | Keys.O, delegate { OpenROMDialog(); }));
            file.DropDownItems.Add(MenuItem("&Reset", Keys.F5, delegate { if (rom_path != null) LoadROM(rom_path); }));
            file.DropDownItems.Add(MenuItem("Save &screenshot...", Keys.Control | Keys.S, delegate { SaveScreenshot(); }));
            file.DropDownItems.Add(new ToolStripSeparator());
            file.DropDownItems.Add(MenuItem("E&xit", Keys.Alt | Keys.F4, delegate { Close(); }));

            ToolStripMenuItem emulation = new ToolStripMenuItem("&Emulation");
            pause_item = MenuItem("&Pause", Keys.Control | Keys.P, delegate { SetPaused(!paused); });
            emulation.DropDownItems.Add(pause_item);

            ToolStripMenuItem view = new ToolStripMenuItem("&View");
            scale_items = new ToolStripMenuItem[6];
            for (int i = 0; i < scale_items.Length; i++)
            {
                int s = i + 1;
                scale_items[i] = MenuItem(String.Format("{0}x", s), Keys.Control | (Keys)((int)Keys.D1 + i), delegate { SetScale(s); });
                view.DropDownItems.Add(scale_items[i]);
            }
            view.DropDownItems.Add(new ToolStripSeparator());
            green_item = MenuItem("DMG &green palette", Keys.None, delegate { SetPalette(PPU.DMGPalette); });
            gray_item = MenuItem("&Grayscale palette", Keys.None, delegate { SetPalette(PPU.GrayPalette); });
            green_item.Checked = true;
            view.DropDownItems.Add(green_item);
            view.DropDownItems.Add(gray_item);

            ToolStripMenuItem help = new ToolStripMenuItem("&Help");
            help.DropDownItems.Add(MenuItem("&Controls", Keys.F1, delegate { ShowControls(); }));

            strip.Items.Add(file);
            strip.Items.Add(emulation);
            strip.Items.Add(view);
            strip.Items.Add(help);
            return strip;
        }

        private static ToolStripMenuItem MenuItem(string text, Keys shortcut, EventHandler handler)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(text, null, handler);
            if (shortcut != Keys.None) item.ShortcutKeys = shortcut;
            return item;
        }

        private void ShowControls()
        {
            MessageBox.Show(this,
                "Arrows\tD-pad\n" +
                "X\tA\n" +
                "Z\tB\n" +
                "Enter\tStart\n" +
                "Backspace\tSelect\n" +
                "Tab (hold)\tFast forward\n\n" +
                "Ctrl+O open ROM (or drag a .gb file on the window), F5 reset,\n" +
                "Ctrl+P pause, Ctrl+1..6 window scale, Ctrl+S screenshot.",
                "ZeXal - Controls", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region ROM loading

        private void OpenROMDialog()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Game Boy ROM (*.gb)|*.gb|All files (*.*)|*.*";
                dialog.Title = "Open ROM";
                if (dialog.ShowDialog(this) == DialogResult.OK) LoadROM(dialog.FileName);
            }
        }

        private void LoadROM(string path)
        {
            GameBoy new_gb;
            try
            {
                new_gb = new GameBoy(path);
            }
            catch (Exception e)
            {
                MessageBox.Show(this, "Can't load the ROM:\n" + e.Message, "ZeXal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lock (emulator_lock)
            {
                gb = new_gb;
                rom_path = path;
            }

            SetPaused(false);
            StartEmulationThread();
            UpdateTitle();
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void OnDragDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files != null && files.Length > 0) LoadROM(files[0]);
        }

        #endregion

        #region View

        private void SetScale(int new_scale)
        {
            scale = new_scale;
            for (int i = 0; i < scale_items.Length; i++) scale_items[i].Checked = (i + 1 == scale);
            ClientSize = new Size(PPU.ScreenWidth * scale, PPU.ScreenHeight * scale + menu.Height);
        }

        private void SetPalette(uint[] new_palette)
        {
            palette = new_palette;
            green_item.Checked = new_palette == PPU.DMGPalette;
            gray_item.Checked = new_palette == PPU.GrayPalette;
            CopyFrame();
            PresentFrame();
        }

        private void SetPaused(bool value)
        {
            paused = value;
            pause_item.Checked = value;
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            if (gb == null)
            {
                Text = "ZeXal - Ctrl+O or drag a ROM here";
                return;
            }

            string title = gb.Cartridge.Title.Length > 0 ? gb.Cartridge.Title : Path.GetFileName(rom_path);
            string state = paused ? "paused" : String.Format("{0:F1} fps", fps);
            Text = String.Format("ZeXal - {0} - {1}", title, state);
        }

        private void SaveScreenshot()
        {
            if (gb == null) return;
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "PNG image (*.png)|*.png";
                dialog.FileName = "screenshot.png";
                if (dialog.ShowDialog(this) != DialogResult.OK) return;

                using (Bitmap copy = new Bitmap(screen)) copy.Save(dialog.FileName, ImageFormat.Png);
            }
        }

        #endregion

        #region Emulation thread

        private void StartEmulationThread()
        {
            if (emulation_thread != null) return;

            running = true;
            fps_watch.Start();
            emulation_thread = new Thread(EmulationLoop);
            emulation_thread.IsBackground = true;
            emulation_thread.Name = "ZeXal emulation";
            emulation_thread.Start();
        }

        private void EmulationLoop()
        {
            TimerResolution.Begin();
            try
            {
                Stopwatch clock = Stopwatch.StartNew();
                double next_frame = 0;

                while (running)
                {
                    if (paused || gb == null)
                    {
                        Thread.Sleep(10);
                        next_frame = clock.Elapsed.TotalSeconds;
                        continue;
                    }

                    try
                    {
                        lock (emulator_lock) gb.RunFrame();
                    }
                    catch (Exception e)
                    {
                        paused = true;
                        string message = e.Message;
                        BeginInvokeSafe(delegate
                        {
                            SetPaused(true);
                            MessageBox.Show(this, "Emulation stopped:\n" + message, "ZeXal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        });
                        continue;
                    }

                    CopyFrame();
                    if (!frame_pending)
                    {
                        frame_pending = true;
                        BeginInvokeSafe(PresentFrame);
                    }

                    //Real time pacing (skipped while fast forwarding)
                    next_frame += FrameSeconds;
                    double now = clock.Elapsed.TotalSeconds;
                    if (turbo || next_frame < now - 0.1) next_frame = now;
                    else if (next_frame > now) Thread.Sleep((int)((next_frame - now) * 1000));
                }
            }
            finally
            {
                TimerResolution.End();
            }
        }

        private void CopyFrame()
        {
            GameBoy current = gb;
            if (current == null) return;

            byte[] shades;
            lock (emulator_lock) shades = (byte[])current.PPU.FrameBuffer.Clone();

            uint[] p = palette;
            lock (pixels_lock)
            {
                for (int i = 0; i < pixels.Length; i++) pixels[i] = (int)p[shades[i]];
            }
        }

        private void BeginInvokeSafe(MethodInvoker action)
        {
            try
            {
                if (IsHandleCreated && !IsDisposed) BeginInvoke(action);
            }
            catch (InvalidOperationException) { } //Window closing (includes ObjectDisposedException)
        }

        //UI thread: copy the pixels into the bitmap and repaint
        private void PresentFrame()
        {
            frame_pending = false;

            Rectangle area = new Rectangle(0, 0, PPU.ScreenWidth, PPU.ScreenHeight);
            BitmapData data = screen.LockBits(area, ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
            try
            {
                lock (pixels_lock)
                {
                    for (int y = 0; y < PPU.ScreenHeight; y++)
                        Marshal.Copy(pixels, y * PPU.ScreenWidth, new IntPtr(data.Scan0.ToInt64() + y * data.Stride), PPU.ScreenWidth);
                }
            }
            finally
            {
                screen.UnlockBits(data);
            }

            display.Invalidate();

            frames_shown++;
            if (fps_watch.ElapsedMilliseconds >= 1000)
            {
                fps = frames_shown * 1000.0 / fps_watch.ElapsedMilliseconds;
                frames_shown = 0;
                fps_watch.Reset();
                fps_watch.Start();
                UpdateTitle();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            running = false;
            if (emulation_thread != null) emulation_thread.Join(500);
            base.OnFormClosing(e);
        }

        #endregion

        #region Input

        private static bool MapKey(Keys key, out Button button)
        {
            switch (key)
            {
                case Keys.Right: button = Button.Right; return true;
                case Keys.Left: button = Button.Left; return true;
                case Keys.Up: button = Button.Up; return true;
                case Keys.Down: button = Button.Down; return true;
                case Keys.X: button = Button.A; return true;
                case Keys.Z: button = Button.B; return true;
                case Keys.Enter: button = Button.Start; return true;
                case Keys.Back: button = Button.Select; return true;
                default: button = Button.None; return false;
            }
        }

        //Arrow keys, Enter and Tab never reach KeyDown, so key presses are handled here
        protected override bool ProcessCmdKey(ref Message msg, Keys key_data)
        {
            if ((key_data & Keys.Modifiers) == Keys.None)
            {
                Button button;
                if (MapKey(key_data, out button))
                {
                    lock (emulator_lock) if (gb != null) gb.Joypad.Press(button);
                    return true;
                }
                if (key_data == Keys.Tab)
                {
                    turbo = true;
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, key_data);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            Button button;
            if (MapKey(e.KeyCode, out button))
            {
                lock (emulator_lock) if (gb != null) gb.Joypad.Release(button);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Tab)
            {
                turbo = false;
                e.Handled = true;
            }
        }

        private void ReleaseAllButtons()
        {
            turbo = false;
            lock (emulator_lock)
            {
                if (gb != null)
                    gb.Joypad.Release(Button.Right | Button.Left | Button.Up | Button.Down | Button.A | Button.B | Button.Select | Button.Start);
            }
        }

        #endregion
    }

    //Paints the 160x144 bitmap scaled to fit, keeping the aspect ratio, without smoothing
    class DisplayPanel : Panel
    {
        private readonly Bitmap image;

        public DisplayPanel(Bitmap image)
        {
            this.image = image;
            BackColor = Color.Black;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);

            float s = Math.Min((float)ClientSize.Width / PPU.ScreenWidth, (float)ClientSize.Height / PPU.ScreenHeight);
            if (s >= 1) s = (float)Math.Floor(s); //Integer scaling keeps pixels square and sharp
            int w = (int)(PPU.ScreenWidth * s);
            int h = (int)(PPU.ScreenHeight * s);

            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
            e.Graphics.DrawImage(image, new Rectangle((ClientSize.Width - w) / 2, (ClientSize.Height - h) / 2, w, h));
        }
    }

    //On Windows Thread.Sleep has a ~15 ms granularity by default: ask for 1 ms while emulating
    static class TimerResolution
    {
        [DllImport("winmm.dll")]
        private static extern uint timeBeginPeriod(uint period);

        [DllImport("winmm.dll")]
        private static extern uint timeEndPeriod(uint period);

        private static bool IsWindows
        {
            get { return Environment.OSVersion.Platform == PlatformID.Win32NT; }
        }

        public static void Begin()
        {
            if (!IsWindows) return;
            try { timeBeginPeriod(1); } catch (Exception) { }
        }

        public static void End()
        {
            if (!IsWindows) return;
            try { timeEndPeriod(1); } catch (Exception) { }
        }
    }
}
