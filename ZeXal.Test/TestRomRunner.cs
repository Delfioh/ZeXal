using System;
using System.Text;
using ZeXal.DMG;

namespace ZeXal.Test
{
    enum TestResult
    {
        Passed = 0,
        Failed = 1,
        Timeout = 2,
        Error = 3
    }

    class TestRomOptions
    {
        public string RomPath;
        public int MaxFrames = 6000;   //~100 seconds of emulated time
        public string ScreenshotPath;
        public bool Trace;
    }

    //Runs a test ROM headless on the DMG hardware and detects its result.
    //Supported conventions:
    // - Blargg (cpu_instrs, instr_timing, ...): text on the serial port ending with "Passed" or "Failed",
    //   or the result in cartridge RAM (0xA000 status, signature DE B0 61 at 0xA001, text from 0xA004)
    // - Mooneye: "LD B,B" breakpoint with B,C,D,E,H,L = 3,5,8,13,21,34 (pass) or all 0x42 (fail)
    // - Anything else (e.g. dmg-acid2): runs until --frames, use --screenshot to check the output
    class TestRomRunner
    {
        private readonly TestRomOptions options;
        private readonly StringBuilder serial_output = new StringBuilder();

        public TestRomRunner(TestRomOptions options)
        {
            this.options = options;
        }

        public TestResult Run()
        {
            GameBoy gb;
            try
            {
                gb = new GameBoy(options.RomPath);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Can't load ROM: " + e.Message);
                return TestResult.Error;
            }

            Console.WriteLine("ROM:   {0}", options.RomPath);
            Console.WriteLine("Title: {0}  (type 0x{1:X2}, {2})", gb.Cartridge.Title, gb.Cartridge.CartridgeType, gb.Cartridge.MBC);
            Console.WriteLine("Running for at most {0} frames...", options.MaxFrames);
            Console.WriteLine();

            gb.Memory.SerialByteSent += delegate(byte b)
            {
                char c = (char)b;
                serial_output.Append(c);
                Console.Write(c);
            };

            TestResult result = TestResult.Timeout;
            long max_cycles = (long)options.MaxFrames * PPU.CyclesPerFrame;
            long next_check = PPU.CyclesPerFrame;

            try
            {
                while (gb.TotalCycles < max_cycles)
                {
                    if (options.Trace) TraceInstruction(gb);

                    if (CheckMooneye(gb, ref result)) break;

                    gb.Step();

                    //Checking the text output once per frame is enough
                    if (gb.TotalCycles >= next_check)
                    {
                        next_check += PPU.CyclesPerFrame;
                        if (CheckBlarggSerial(ref result)) break;
                        if (CheckBlarggMemory(gb, ref result)) break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.Error.WriteLine("Emulation error at PC=0x{0:X4}: {1}", gb.CPU.regs.PC, e.Message);
                result = TestResult.Error;
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Result: {0} after {1} frames ({2:F1} s emulated)",
                result.ToString().ToUpper(), gb.TotalCycles / PPU.CyclesPerFrame, gb.TotalCycles / 4194304.0);

            if (options.ScreenshotPath != null)
            {
                Screenshot.SaveBMP(gb.PPU, options.ScreenshotPath, PPU.GrayPalette);
                Console.WriteLine("Screenshot saved to {0}", options.ScreenshotPath);
            }

            return result;
        }

        private bool CheckBlarggSerial(ref TestResult result)
        {
            string text = serial_output.ToString();
            if (text.Contains("Passed"))
            {
                result = TestResult.Passed;
                return true;
            }
            if (text.Contains("Failed"))
            {
                result = TestResult.Failed;
                return true;
            }
            return false;
        }

        private bool CheckBlarggMemory(GameBoy gb, ref TestResult result)
        {
            Cartridge cart = gb.Cartridge;
            if (!cart.HasRAM) return false;
            if (cart.PeekRAM(1) != 0xDE || cart.PeekRAM(2) != 0xB0 || cart.PeekRAM(3) != 0x61) return false;

            byte status = cart.PeekRAM(0);
            if (status == 0x80) return false; //Still running

            StringBuilder text = new StringBuilder();
            for (int i = 4; i < 0x2000 && cart.PeekRAM(i) != 0; i++) text.Append((char)cart.PeekRAM(i));
            if (serial_output.Length == 0) Console.Write(text.ToString());

            result = status == 0 ? TestResult.Passed : TestResult.Failed;
            return true;
        }

        private static bool CheckMooneye(GameBoy gb, ref TestResult result)
        {
            if (gb.CPU.halted || gb.Memory.GetByte(gb.CPU.regs.PC) != 0x40) return false;

            var r = gb.CPU.regs;
            if (r.B == 3 && r.C == 5 && r.D == 8 && r.E == 13 && r.H == 21 && r.L == 34)
            {
                result = TestResult.Passed;
                return true;
            }
            if (r.B == 0x42 && r.C == 0x42 && r.D == 0x42 && r.E == 0x42 && r.H == 0x42 && r.L == 0x42)
            {
                result = TestResult.Failed;
                return true;
            }
            return false;
        }

        private static void TraceInstruction(GameBoy gb)
        {
            var r = gb.CPU.regs;
            byte n;
            string op = gb.CPU.halted ? "(halted)" : gb.CPU.GetOpcodeData(out n);
            Console.WriteLine("{0:X4}  {1,-20} AF={2:X4} BC={3:X4} DE={4:X4} HL={5:X4} SP={6:X4} LY={7:X2}",
                r.PC, op, r.AF, r.BC, r.DE, r.HL, r.SP, gb.Memory.GetByte(0xFF44));
        }
    }
}
