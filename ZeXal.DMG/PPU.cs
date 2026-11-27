using System;

namespace ZeXal.DMG
{
    public enum PPUMode
    {
        HBlank = 0,
        VBlank = 1,
        OAMScan = 2,
        Transfer = 3
    }

    //Picture Processing Unit of the DMG-001.
    //
    //Timing (T-cycles / dots): 456 dots per line, 154 lines per frame (70224 dots).
    //Lines 0-143:   mode 2 (OAM scan, 80 dots) -> mode 3 (pixel transfer, 172 dots) -> mode 0 (HBlank)
    //Lines 144-153: mode 1 (VBlank)
    //
    //The image is produced one scanline at a time at the end of mode 3 (scanline renderer):
    //mid-scanline register changes are not visible, but every per-line effect
    //(SCX/SCY/palette/window changes in HBlank or STAT interrupts) is.
    //Mode 3 has a fixed length and CPU access to VRAM/OAM is never blocked.
    public class PPU
    {
        public const int ScreenWidth = 160;
        public const int ScreenHeight = 144;
        public const int CyclesPerFrame = 70224;

        private const int DotsPerLine = 456;
        private const int OAMScanDots = 80;
        private const int TransferDots = 172;
        private const int LinesPerFrame = 154;
        private const int MaxSpritesPerLine = 10;

        //Classic DMG-001 green shades, ARGB, from lightest (0) to darkest (3)
        public static readonly uint[] DMGPalette = { 0xFF9BBC0F, 0xFF8BAC0F, 0xFF306230, 0xFF0F380F };
        public static readonly uint[] GrayPalette = { 0xFFFFFFFF, 0xFFAAAAAA, 0xFF555555, 0xFF000000 };

        private readonly InterruptController irq;

        private readonly byte[] vram = new byte[0x2000];
        private readonly byte[] oam = new byte[0xA0];

        //Shade (0-3) of every pixel, after palette mapping
        private readonly byte[] back_buffer = new byte[ScreenWidth * ScreenHeight];
        private readonly byte[] front_buffer = new byte[ScreenWidth * ScreenHeight];

        //Scanline work buffers
        private readonly byte[] line_bg_color = new byte[ScreenWidth];
        private readonly bool[] line_sprite_done = new bool[ScreenWidth];
        private readonly int[] line_sprites = new int[MaxSpritesPerLine];

        private byte lcdc, stat, scy, scx, ly, lyc, bgp, obp0, obp1, wy, wx;

        private PPUMode mode;
        private int dot;
        private int window_line;
        private bool window_y_triggered;
        private bool stat_irq_line;

        public long FrameCount { get; private set; }

        //Raised at the start of VBlank, when FrameBuffer holds a complete frame
        public event EventHandler FrameReady;

        public PPU(InterruptController irq)
        {
            this.irq = irq;
            Reset();
        }

        //Post boot ROM state
        public void Reset()
        {
            lcdc = 0x91;
            stat = 0x00;
            scy = 0x00;
            scx = 0x00;
            ly = 0x00;
            lyc = 0x00;
            bgp = 0xFC;
            obp0 = 0xFF;
            obp1 = 0xFF;
            wy = 0x00;
            wx = 0x00;

            mode = PPUMode.OAMScan;
            dot = 0;
            window_line = 0;
            window_y_triggered = false;
            stat_irq_line = false;
            FrameCount = 0;

            Array.Clear(back_buffer, 0, back_buffer.Length);
            Array.Clear(front_buffer, 0, front_buffer.Length);
        }

        #region Public state

        //Last complete frame: one shade (0 = lightest, 3 = darkest) per pixel, row major
        public byte[] FrameBuffer
        {
            get { return front_buffer; }
        }

        public byte[] VRAM
        {
            get { return vram; }
        }

        public byte[] OAM
        {
            get { return oam; }
        }

        public PPUMode Mode
        {
            get { return mode; }
        }

        public bool LCDEnabled
        {
            get { return (lcdc & 0x80) != 0; }
        }

        //Converts the frame buffer to 32 bit ARGB pixels using the given 4 color palette
        public uint[] GetARGBFrame(uint[] palette)
        {
            if (palette == null || palette.Length < 4) palette = DMGPalette;
            uint[] pixels = new uint[front_buffer.Length];
            for (int i = 0; i < pixels.Length; i++) pixels[i] = palette[front_buffer[i]];
            return pixels;
        }

        #endregion

        #region Memory interface

        //0x8000-0x9FFF
        public byte ReadVRAM(ushort address)
        {
            return vram[address & 0x1FFF];
        }

        public void WriteVRAM(ushort address, byte value)
        {
            vram[address & 0x1FFF] = value;
        }

        //0xFE00-0xFE9F
        public byte ReadOAM(ushort address)
        {
            return oam[address - 0xFE00];
        }

        public void WriteOAM(ushort address, byte value)
        {
            oam[address - 0xFE00] = value;
        }

        //0xFF40-0xFF4B (0xFF46, DMA, is handled by the memory map)
        public byte ReadRegister(ushort address)
        {
            switch (address)
            {
                case 0xFF40: return lcdc;
                case 0xFF41:
                    {
                        int coincidence = (LCDEnabled && ly == lyc) ? 0x04 : 0x00;
                        int current_mode = LCDEnabled ? (int)mode : 0;
                        return (byte)(0x80 | (stat & 0x78) | coincidence | current_mode);
                    }
                case 0xFF42: return scy;
                case 0xFF43: return scx;
                case 0xFF44: return ly;
                case 0xFF45: return lyc;
                case 0xFF47: return bgp;
                case 0xFF48: return obp0;
                case 0xFF49: return obp1;
                case 0xFF4A: return wy;
                case 0xFF4B: return wx;
                default: return 0xFF;
            }
        }

        public void WriteRegister(ushort address, byte value)
        {
            switch (address)
            {
                case 0xFF40: WriteLCDC(value); break;
                case 0xFF41:
                    stat = (byte)(value & 0x78);
                    UpdateStatInterrupt();
                    break;
                case 0xFF42: scy = value; break;
                case 0xFF43: scx = value; break;
                case 0xFF44: break; //LY is read only
                case 0xFF45:
                    lyc = value;
                    UpdateStatInterrupt();
                    break;
                case 0xFF47: bgp = value; break;
                case 0xFF48: obp0 = value; break;
                case 0xFF49: obp1 = value; break;
                case 0xFF4A: wy = value; break;
                case 0xFF4B: wx = value; break;
            }
        }

        private void WriteLCDC(byte value)
        {
            bool was_enabled = LCDEnabled;
            lcdc = value;

            if (was_enabled && !LCDEnabled)
            {
                //LCD off: LY is reset and the screen goes blank (white)
                ly = 0;
                dot = 0;
                mode = PPUMode.HBlank;
                window_line = 0;
                window_y_triggered = false;
                stat_irq_line = false;
                Array.Clear(back_buffer, 0, back_buffer.Length);
                Array.Clear(front_buffer, 0, front_buffer.Length);
            }
            else if (!was_enabled && LCDEnabled)
            {
                //LCD on: start again from the top of the screen
                ly = 0;
                dot = 0;
                window_line = 0;
                window_y_triggered = (wy == 0);
                SetMode(PPUMode.OAMScan);
            }
        }

        #endregion

        #region Timing

        public void Tick(int cycles)
        {
            if (!LCDEnabled) return;

            for (int i = 0; i < cycles; i++)
            {
                dot++;

                if (ly < ScreenHeight)
                {
                    if (dot == OAMScanDots)
                    {
                        SetMode(PPUMode.Transfer);
                    }
                    else if (dot == OAMScanDots + TransferDots)
                    {
                        RenderScanline();
                        SetMode(PPUMode.HBlank);
                    }
                }

                if (dot == DotsPerLine)
                {
                    dot = 0;
                    ly++;

                    if (ly == LinesPerFrame)
                    {
                        ly = 0;
                        window_line = 0;
                        window_y_triggered = false;
                    }

                    if (ly == ScreenHeight)
                    {
                        EnterVBlank();
                    }
                    else if (ly < ScreenHeight)
                    {
                        if (ly == wy) window_y_triggered = true;
                        SetMode(PPUMode.OAMScan);
                    }
                    else
                    {
                        UpdateStatInterrupt();
                    }
                }
            }
        }

        private void EnterVBlank()
        {
            SetMode(PPUMode.VBlank);
            irq.Request(Interrupt.VBlank);

            Array.Copy(back_buffer, front_buffer, back_buffer.Length);
            FrameCount++;

            EventHandler handler = FrameReady;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private void SetMode(PPUMode new_mode)
        {
            mode = new_mode;
            if (mode == PPUMode.OAMScan && ly == 0 && wy == 0) window_y_triggered = true;
            UpdateStatInterrupt();
        }

        //The STAT interrupt is requested on the rising edge of the OR of all enabled sources
        private void UpdateStatInterrupt()
        {
            if (!LCDEnabled)
            {
                stat_irq_line = false;
                return;
            }

            bool line =
                ((stat & 0x08) != 0 && mode == PPUMode.HBlank) ||
                ((stat & 0x10) != 0 && mode == PPUMode.VBlank) ||
                ((stat & 0x20) != 0 && mode == PPUMode.OAMScan) ||
                ((stat & 0x40) != 0 && ly == lyc);

            if (line && !stat_irq_line) irq.Request(Interrupt.LcdStat);
            stat_irq_line = line;
        }

        #endregion

        #region Rendering

        private static byte Shade(byte palette, int color)
        {
            return (byte)((palette >> (color * 2)) & 0x03);
        }

        //Color index (0-3) of a pixel of a tile, addressed through a tile map
        private byte TileMapPixel(int map_offset, int x, int y)
        {
            byte tile_index = vram[map_offset + (y >> 3) * 32 + (x >> 3)];

            int tile_offset;
            if ((lcdc & 0x10) != 0) tile_offset = tile_index * 16;               //0x8000 unsigned
            else tile_offset = 0x1000 + ((sbyte)tile_index) * 16;              //0x9000 signed

            int row = (y & 0x07) * 2;
            int bit = 7 - (x & 0x07);
            byte low = vram[tile_offset + row];
            byte high = vram[tile_offset + row + 1];

            return (byte)(((low >> bit) & 0x01) | (((high >> bit) & 0x01) << 1));
        }

        private void RenderScanline()
        {
            int line_offset = ly * ScreenWidth;

            //LCDC bit 0 on DMG: when clear, background and window are blank (color 0)
            if ((lcdc & 0x01) != 0)
            {
                RenderBackground();
                RenderWindow();
            }
            else
            {
                Array.Clear(line_bg_color, 0, ScreenWidth);
            }

            for (int x = 0; x < ScreenWidth; x++)
                back_buffer[line_offset + x] = Shade(bgp, line_bg_color[x]);

            if ((lcdc & 0x02) != 0) RenderSprites(line_offset);
        }

        private void RenderBackground()
        {
            int map_offset = (lcdc & 0x08) != 0 ? 0x1C00 : 0x1800;
            int y = (ly + scy) & 0xFF;

            for (int x = 0; x < ScreenWidth; x++)
                line_bg_color[x] = TileMapPixel(map_offset, (x + scx) & 0xFF, y);
        }

        private void RenderWindow()
        {
            if ((lcdc & 0x20) == 0 || !window_y_triggered || wx > 166) return;

            int map_offset = (lcdc & 0x40) != 0 ? 0x1C00 : 0x1800;
            int window_x = wx - 7;
            int start = window_x < 0 ? 0 : window_x;

            if (start >= ScreenWidth) return;

            for (int x = start; x < ScreenWidth; x++)
                line_bg_color[x] = TileMapPixel(map_offset, x - window_x, window_line);

            //The window has its own line counter, incremented only on lines where it is drawn
            window_line++;
        }

        private void RenderSprites(int line_offset)
        {
            int height = (lcdc & 0x04) != 0 ? 16 : 8;

            //OAM scan: the first 10 sprites (in OAM order) overlapping this line
            int count = 0;
            for (int i = 0; i < 40 && count < MaxSpritesPerLine; i++)
            {
                int sprite_y = oam[i * 4] - 16;
                if (ly >= sprite_y && ly < sprite_y + height) line_sprites[count++] = i;
            }

            //DMG priority: smaller X first, then lower OAM index (stable insertion sort)
            for (int i = 1; i < count; i++)
            {
                int current = line_sprites[i];
                int j = i - 1;
                while (j >= 0 && oam[line_sprites[j] * 4 + 1] > oam[current * 4 + 1])
                {
                    line_sprites[j + 1] = line_sprites[j];
                    j--;
                }
                line_sprites[j + 1] = current;
            }

            Array.Clear(line_sprite_done, 0, ScreenWidth);

            for (int s = 0; s < count; s++)
            {
                int index = line_sprites[s] * 4;
                int sprite_y = oam[index] - 16;
                int sprite_x = oam[index + 1] - 8;
                int tile = oam[index + 2];
                byte attributes = oam[index + 3];

                int row = ly - sprite_y;
                if ((attributes & 0x40) != 0) row = height - 1 - row; //Y flip
                if (height == 16) tile &= 0xFE;

                int tile_offset = tile * 16 + row * 2;
                byte low = vram[tile_offset];
                byte high = vram[tile_offset + 1];
                byte palette = (attributes & 0x10) != 0 ? obp1 : obp0;
                bool behind_bg = (attributes & 0x80) != 0;
                bool x_flip = (attributes & 0x20) != 0;

                for (int px = 0; px < 8; px++)
                {
                    int x = sprite_x + px;
                    if (x < 0 || x >= ScreenWidth || line_sprite_done[x]) continue;

                    int bit = x_flip ? px : 7 - px;
                    int color = ((low >> bit) & 0x01) | (((high >> bit) & 0x01) << 1);
                    if (color == 0) continue; //Transparent: lower priority sprites can show through

                    //The highest priority opaque sprite pixel wins, even if it is then hidden by the BG
                    line_sprite_done[x] = true;
                    if (behind_bg && line_bg_color[x] != 0) continue;

                    back_buffer[line_offset + x] = Shade(palette, color);
                }
            }
        }

        #endregion
    }
}
