using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeXal.CPUCore;

namespace ZeXal.Test
{
    class Program
    {
        static int Main(string[] args)
        {
            TestRomOptions test_options;
            string error;

            if (!ParseArguments(args, out test_options, out error))
            {
                if (error != null) Console.Error.WriteLine(error);
                PrintUsage();
                return error != null ? (int)TestResult.Error : 0;
            }

            if (test_options != null)
            {
                TestRomRunner runner = new TestRomRunner(test_options);
                return (int)runner.Run();
            }

            RunInteractive();
            return 0;
        }

        static bool ParseArguments(string[] args, out TestRomOptions options, out string error)
        {
            options = null;
            error = null;

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                switch (arg)
                {
                    case "-h":
                    case "--help":
                        return false;

                    case "-t":
                    case "--test-rom":
                        if (i + 1 >= args.Length) { error = arg + " requires a ROM path"; return false; }
                        if (options == null) options = new TestRomOptions();
                        options.RomPath = args[++i];
                        break;

                    case "--frames":
                        {
                            int frames;
                            if (i + 1 >= args.Length || !int.TryParse(args[++i], out frames) || frames <= 0)
                            {
                                error = "--frames requires a positive number";
                                return false;
                            }
                            if (options == null) options = new TestRomOptions();
                            options.MaxFrames = frames;
                            break;
                        }

                    case "--screenshot":
                        if (i + 1 >= args.Length) { error = "--screenshot requires a file path"; return false; }
                        if (options == null) options = new TestRomOptions();
                        options.ScreenshotPath = args[++i];
                        break;

                    case "--trace":
                        if (options == null) options = new TestRomOptions();
                        options.Trace = true;
                        break;

                    default:
                        error = "Unknown argument: " + arg;
                        return false;
                }
            }

            if (options != null && options.RomPath == null)
            {
                error = "--frames, --screenshot and --trace require --test-rom";
                return false;
            }

            return true;
        }

        static void PrintUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  ZeXal.Test                       Step through Binaries/test.vm interactively");
            Console.WriteLine("  ZeXal.Test --test-rom <file.gb>  Run a test ROM on the emulated DMG-001");
            Console.WriteLine();
            Console.WriteLine("Test ROM options:");
            Console.WriteLine("  -t, --test-rom <file>   ROM to run (ROM only, MBC1, MBC3, MBC5)");
            Console.WriteLine("  --frames <n>            Stop after n frames if no result is detected (default 6000)");
            Console.WriteLine("  --screenshot <file>     Save the last frame as a BMP image");
            Console.WriteLine("  --trace                 Print every executed instruction (slow)");
            Console.WriteLine();
            Console.WriteLine("Exit code: 0 passed, 1 failed, 2 no result (timeout), 3 error");
        }

        static void RunInteractive()
        {
            TestMemoryMap memory = new TestMemoryMap();
            memory.LoadExecutable("Binaries/test.vm");

            TestIRHandler irhandler = new TestIRHandler();

            CPU cpu = new CPU(memory, irhandler, 0x0000);

            while (true)
            {
                byte opcode_size;
                Console.WriteLine(cpu.GetOpcodeData(out opcode_size));
                cpu.ExecuteNextOpcode();
                WriteTrace(cpu);
                Console.ReadLine();
            }
        }

        static void WriteTrace(CPU cpu)
        {
            Console.WriteLine("PC:0x{0:X4}", cpu.regs.PC);
            Console.WriteLine("SP:0x{0:X4}", cpu.regs.SP);
            Console.WriteLine("A:0x{0:X2} B:0x{1:X2} C:0x{2:X2} D:0x{3:X2}", cpu.regs.A, cpu.regs.B,cpu.regs.C, cpu.regs.D);
            Console.WriteLine("E:0x{0:X2} F:0x{1:X2} H:0x{2:X2} L:0x{3:X2}", cpu.regs.E, cpu.regs.F, cpu.regs.H, cpu.regs.L);
        }
    }



}
