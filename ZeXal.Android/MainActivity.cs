using System;
using System.IO;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using ZeXal.DMG;
using GBButton = ZeXal.DMG.Button;

namespace ZeXal.Android
{
    [Activity(Label = "ZeXal", MainLauncher = true, Icon = "@drawable/icon",
              Theme = "@android:style/Theme.Material.NoActionBar.Fullscreen",
              ScreenOrientation = ScreenOrientation.FullUser,
              ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout |
                                     ConfigChanges.SmallestScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden |
                                     ConfigChanges.Navigation | ConfigChanges.UiMode)]
    public class MainActivity : Activity
    {
        private const int OpenRomRequest = 1;
        private const string PrefLastRom = "last_rom";
        private const double FrameSeconds = (double)PPU.CyclesPerFrame / 4194304.0;

        private readonly object emulator_lock = new object();
        private readonly object pixels_lock = new object();
        private readonly int[] pixels = new int[PPU.ScreenWidth * PPU.ScreenHeight];

        private GameView view;
        private GameBoy gb;
        private Thread thread;
        private volatile bool running;
        private volatile bool paused;
        private GBButton keys_pressed = GBButton.None;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Blank (lightest) screen until a ROM is loaded
            for (int i = 0; i < pixels.Length; i++) pixels[i] = unchecked((int)PPU.DMGPalette[0]);

            view = new GameView(this, pixels, pixels_lock);
            view.ButtonsChanged += OnButtonsChanged;
            view.MenuRequested += OpenRomPicker;
            view.Message = "Tocca qui o \"ROM\"\nper aprire un file .gb";
            SetContentView(view);
            view.RequestFocus();

            Window.AddFlags(WindowManagerFlags.KeepScreenOn);
            HideSystemBars();

            string last = GetPreferences(FileCreationMode.Private).GetString(PrefLastRom, null);
            if (last != null) LoadRom(global::Android.Net.Uri.Parse(last), false);
        }

        private void HideSystemBars()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
            {
                Window.SetDecorFitsSystemWindows(false);
                IWindowInsetsController controller = Window.InsetsController;
                if (controller != null)
                {
                    controller.Hide(WindowInsets.Type.SystemBars());
                    controller.SystemBarsBehavior = (int)WindowInsetsControllerBehavior.ShowTransientBarsBySwipe;
                }
            }
            else
            {
                Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(SystemUiFlags.ImmersiveSticky | SystemUiFlags.Fullscreen |
                    SystemUiFlags.HideNavigation | SystemUiFlags.LayoutStable | SystemUiFlags.LayoutHideNavigation | SystemUiFlags.LayoutFullscreen);
            }
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);
            if (hasFocus) HideSystemBars();
        }

        #region ROM loading and saves

        private void OpenRomPicker()
        {
            Intent intent = new Intent(Intent.ActionOpenDocument);
            intent.AddCategory(Intent.CategoryOpenable);
            intent.SetType("*/*");
            try
            {
                StartActivityForResult(intent, OpenRomRequest);
            }
            catch (ActivityNotFoundException)
            {
                Toast.MakeText(this, "Nessuna app per scegliere i file", ToastLength.Long).Show();
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode != OpenRomRequest || resultCode != Result.Ok || data == null || data.Data == null) return;

            try
            {
                ContentResolver.TakePersistableUriPermission(data.Data, ActivityFlags.GrantReadUriPermission);
            }
            catch (Exception) { } //Not every provider allows it: the ROM just won't reopen at the next start

            LoadRom(data.Data, true);
        }

        private void LoadRom(global::Android.Net.Uri uri, bool show_errors)
        {
            GameBoy new_gb;
            try
            {
                byte[] data;
                using (Stream input = ContentResolver.OpenInputStream(uri))
                using (MemoryStream memory = new MemoryStream())
                {
                    input.CopyTo(memory);
                    data = memory.ToArray();
                }
                if (data.Length > 8 * 1024 * 1024) throw new InvalidDataException("File troppo grande per una ROM Game Boy");

                new_gb = new GameBoy(new Cartridge(data));
                new_gb.Cartridge.LoadRAM(ReadSave(new_gb.Cartridge));
            }
            catch (Exception e)
            {
                if (show_errors) Toast.MakeText(this, "Impossibile aprire la ROM: " + e.Message, ToastLength.Long).Show();
                return;
            }

            WriteSave();
            lock (emulator_lock) gb = new_gb;

            GetPreferences(FileCreationMode.Private).Edit().PutString(PrefLastRom, uri.ToString()).Apply();
            view.Message = null;
            paused = false;
            StartThread();
            view.Invalidate();
        }

        private string SavePath(Cartridge cart)
        {
            return System.IO.Path.Combine(FilesDir.AbsolutePath, cart.Identifier + ".sav");
        }

        private byte[] ReadSave(Cartridge cart)
        {
            if (!cart.HasRAM) return null;
            string path = SavePath(cart);
            return File.Exists(path) ? File.ReadAllBytes(path) : null;
        }

        //External cartridge RAM is written to internal storage when the app goes to the background
        private void WriteSave()
        {
            GameBoy current = gb;
            if (current == null || !current.Cartridge.HasRAM) return;
            try
            {
                byte[] ram;
                lock (emulator_lock) ram = current.Cartridge.GetRAM();
                File.WriteAllBytes(SavePath(current.Cartridge), ram);
            }
            catch (Exception) { }
        }

        #endregion

        #region Lifecycle

        protected override void OnResume()
        {
            base.OnResume();
            paused = false;
            if (gb != null) StartThread();
        }

        protected override void OnPause()
        {
            paused = true;
            view.ReleaseAll();
            WriteSave();
            base.OnPause();
        }

        protected override void OnDestroy()
        {
            running = false;
            if (thread != null) thread.Join(500);
            thread = null;
            WriteSave();
            base.OnDestroy();
        }

        #endregion

        #region Emulation thread

        private void StartThread()
        {
            if (thread != null) return;
            running = true;
            thread = new Thread(EmulationLoop);
            thread.IsBackground = true;
            thread.Name = "ZeXal emulation";
            thread.Start();
        }

        private void EmulationLoop()
        {
            System.Diagnostics.Stopwatch clock = System.Diagnostics.Stopwatch.StartNew();
            double next_frame = 0;

            while (running)
            {
                GameBoy current = gb;
                if (paused || current == null)
                {
                    Thread.Sleep(20);
                    next_frame = clock.Elapsed.TotalSeconds;
                    continue;
                }

                byte[] shades;
                try
                {
                    lock (emulator_lock)
                    {
                        current.RunFrame();
                        shades = current.PPU.FrameBuffer;
                        lock (pixels_lock)
                        {
                            uint[] palette = PPU.DMGPalette;
                            for (int i = 0; i < pixels.Length; i++) pixels[i] = unchecked((int)palette[shades[i]]);
                        }
                    }
                }
                catch (Exception e)
                {
                    paused = true;
                    string message = e.Message;
                    RunOnUiThread(delegate
                    {
                        view.Message = "Emulazione fermata:\n" + message;
                        view.Invalidate();
                    });
                    continue;
                }

                view.PostInvalidate();

                next_frame += FrameSeconds;
                double now = clock.Elapsed.TotalSeconds;
                if (next_frame < now - 0.1) next_frame = now;
                else if (next_frame > now) Thread.Sleep((int)((next_frame - now) * 1000));
            }
        }

        private void OnButtonsChanged(GBButton now)
        {
            GameBoy current = gb;
            if (current == null) return;
            GBButton all = now | keys_pressed;
            lock (emulator_lock)
            {
                current.Joypad.Release(~all & (GBButton)0xFF);
                current.Joypad.Press(all);
            }
        }

        #endregion

        #region Hardware keys and gamepads

        private static GBButton MapKey(Keycode key)
        {
            switch (key)
            {
                case Keycode.DpadUp: return GBButton.Up;
                case Keycode.DpadDown: return GBButton.Down;
                case Keycode.DpadLeft: return GBButton.Left;
                case Keycode.DpadRight: return GBButton.Right;
                case Keycode.ButtonA: case Keycode.X: return GBButton.A;
                case Keycode.ButtonB: case Keycode.Z: return GBButton.B;
                case Keycode.ButtonStart: case Keycode.Enter: return GBButton.Start;
                case Keycode.ButtonSelect: case Keycode.Del: return GBButton.Select;
                default: return GBButton.None;
            }
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            GBButton b = MapKey(keyCode);
            if (b == GBButton.None) return base.OnKeyDown(keyCode, e);
            keys_pressed |= b;
            OnButtonsChanged(view.PressedButtons);
            return true;
        }

        public override bool OnKeyUp(Keycode keyCode, KeyEvent e)
        {
            GBButton b = MapKey(keyCode);
            if (b == GBButton.None) return base.OnKeyUp(keyCode, e);
            keys_pressed &= ~b;
            OnButtonsChanged(view.PressedButtons);
            return true;
        }

        #endregion
    }
}
