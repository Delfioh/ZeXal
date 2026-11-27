using System;
using System.IO;

namespace ZeXal.DMG
{
    public static class Screenshot
    {
        //Saves the last complete frame as a 24 bit uncompressed BMP
        public static void SaveBMP(PPU ppu, string filename, uint[] palette)
        {
            uint[] pixels = ppu.GetARGBFrame(palette);
            int width = PPU.ScreenWidth;
            int height = PPU.ScreenHeight;
            int row_size = width * 3; //160*3 is already a multiple of 4
            int image_size = row_size * height;

            using (BinaryWriter w = new BinaryWriter(File.Create(filename)))
            {
                //BITMAPFILEHEADER
                w.Write((byte)'B');
                w.Write((byte)'M');
                w.Write(54 + image_size);
                w.Write(0);
                w.Write(54);

                //BITMAPINFOHEADER
                w.Write(40);
                w.Write(width);
                w.Write(height);
                w.Write((short)1);
                w.Write((short)24);
                w.Write(0);
                w.Write(image_size);
                w.Write(2835);
                w.Write(2835);
                w.Write(0);
                w.Write(0);

                //Rows bottom-up, pixels as BGR
                for (int y = height - 1; y >= 0; y--)
                {
                    for (int x = 0; x < width; x++)
                    {
                        uint argb = pixels[y * width + x];
                        w.Write((byte)(argb & 0xFF));
                        w.Write((byte)((argb >> 8) & 0xFF));
                        w.Write((byte)((argb >> 16) & 0xFF));
                    }
                }
            }
        }
    }
}
