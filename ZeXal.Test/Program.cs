using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeXal.CPUCore;

namespace ZeXal.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            TestMemoryMap memory = new TestMemoryMap();
            memory.LoadExecutable("Binaries/test.vm");

            TestIRHandler irhandler = new TestIRHandler();

            CPU cpu = new CPU(memory, irhandler, 0x0000);
            ASCIIEncoding encoder = new ASCIIEncoding();

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
