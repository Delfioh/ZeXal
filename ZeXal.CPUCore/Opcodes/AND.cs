using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //AND A,n
    //Description:
    //Logically AND n with A, result in A.
    //Use with:
    //n = A,B,C,D,E,H,L,(HL),#
    //Flags affected:
    //Z - Set if result is zero.
    //N - Reset.
    //H - Set.
    //C - Reset.

    //Generic implementation
    class AND : Opcode
    {
        protected byte PerformAND(byte v1, byte v2, CPU cpu)
        {
            byte result = (byte)(v1 & v2);

            if (result == 0) cpu.regs.SetFlag(Flags.Z);
            else cpu.regs.ClearFlag(Flags.Z);

            cpu.regs.ClearFlag(Flags.N);
            cpu.regs.SetFlag(Flags.H);
            cpu.regs.ClearFlag(Flags.C);

            return result;
        }
    }

    //0xA7
    class AND_AA : AND
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformAND(cpu.regs.A, cpu.regs.A, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "and a,a";
        }
    }

    //0xA0
    class AND_AB : AND
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformAND(cpu.regs.A, cpu.regs.B, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "and a,b";
        }
    }

    //0xA1
    class AND_AC : AND
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformAND(cpu.regs.A, cpu.regs.C, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "and a,c";
        }
    }

    //0xA2
    class AND_AD : AND
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformAND(cpu.regs.A, cpu.regs.D, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }
        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "and a,d";
        }

    }

    //0xA3
    class AND_AE : AND
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformAND(cpu.regs.A, cpu.regs.E, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "and a,e";
        }
    }

    //0xA4
    class AND_AH : AND
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformAND(cpu.regs.A, cpu.regs.H, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "and a,h";
        }
    }

    //0xA5
    class AND_AL : AND
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformAND(cpu.regs.A, cpu.regs.L, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "and a,l";
        }
    }

    //0xA6
    class AND_AindexHL : AND
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte(cpu.regs.HL);
            cpu.regs.A = PerformAND(cpu.regs.A, value, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "and a,(hl)";
        }
    }

    //0xE6
    class AND_An : AND
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte((ushort)(cpu.regs.PC + 1));
            cpu.regs.A = PerformAND(cpu.regs.A, value, cpu);
            cpu.regs.IncreasePC(2);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            byte value = memorymap.GetByte((ushort)(address + 1));
            n_bytes = 2;
            builder.AppendFormat("and a,0x{0:X2}", value);
            return builder.ToString();
        }
    }

}
