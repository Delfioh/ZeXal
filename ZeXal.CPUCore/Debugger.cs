using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal
{
    public static class Debugger
    {
        public static void Write(string format)
        {
            Console.WriteLine(format);
        }

        public static void Write(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }
    }
}
