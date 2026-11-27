using System;
using System.Windows.Forms;

namespace ZeXal.Viewer
{
    static class Program
    {
        //Usage: ZeXal.Viewer [rom.gb] [--scale n]
        [STAThread]
        static void Main(string[] args)
        {
            string rom = null;
            int scale = 3;

            for (int i = 0; i < args.Length; i++)
            {
                int s;
                if (args[i] == "--scale" && i + 1 < args.Length && int.TryParse(args[i + 1], out s) && s >= 1 && s <= 6)
                {
                    scale = s;
                    i++;
                }
                else rom = args[i];
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(rom, scale));
        }
    }
}
