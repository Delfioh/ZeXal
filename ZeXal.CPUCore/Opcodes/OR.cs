using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //OR A,n
    //Description:
    //Logically OR n with A, result in A.
    //Use with:
    //n = A,B,C,D,E,H,L,(HL),#
    //Flags affected:
    //Z - Set if result is zero.
    //N - Reset.
    //H - Reset.
    //C - Reset.

    //Generic implementation
    class OR : Opcode
    {
        protected byte PerformOR(byte v1, byte v2, CPU cpu)
        {
            byte result = (byte)(v1 | v2);

            if (result == 0) cpu.regs.SetFlag(Flags.Z);
            else cpu.regs.ClearFlag(Flags.Z);

            cpu.regs.ClearFlag(Flags.N);
            cpu.regs.ClearFlag(Flags.H);
            cpu.regs.ClearFlag(Flags.C);

            return result;
        }
    }

    //0xB7
    class OR_AA : OR
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformOR(cpu.regs.A, cpu.regs.A, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "or a,a";
        }
    }

    //0xB0
    class OR_AB : OR
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformOR(cpu.regs.A, cpu.regs.B, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "or a,b";
        }
    }

    //0xB1
    class OR_AC : OR
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformOR(cpu.regs.A, cpu.regs.C, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "or a,c";
        }

    }

    //0xB2
    class OR_AD : OR
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformOR(cpu.regs.A, cpu.regs.D, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "or a,d";
        }
    }

    //0xB3
    class OR_AE : OR
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformOR(cpu.regs.A, cpu.regs.E, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "or a,e";
        }
    }

    //0xB4
    class OR_AH : OR
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformOR(cpu.regs.A, cpu.regs.H, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "or a,h";
        }

    }

    //0xB5
    class OR_AL : OR
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformOR(cpu.regs.A, cpu.regs.L, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "or a,l";
        }
    }

    //0xB6
    class OR_AindexHL : OR
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte(cpu.regs.HL);
            cpu.regs.A = PerformOR(cpu.regs.A, value, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "or a,(hl)";
        }
    }

    //0xF6
    class OR_An : OR
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte((ushort)(cpu.regs.PC + 1));
            cpu.regs.A = PerformOR(cpu.regs.A, value, cpu);
            cpu.regs.IncreasePC(2);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            byte value = memorymap.GetByte((ushort)(address + 1));
            n_bytes = 2;
            builder.AppendFormat("or a,0x{0:X2}", value);
            return builder.ToString();
        }
    }

}
