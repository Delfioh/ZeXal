using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //XOR A,n
    //Description:
    //Logically XOR n with A, result in A.
    //Use with:
    //n = A,B,C,D,E,H,L,(HL),#
    //Flags affected:
    //Z - Set if result is zero.
    //N - Reset.
    //H - Reset.
    //C - Reset.

    //Generic implementation
    class XOR : Opcode
    {
        protected byte PerformXOR(byte v1, byte v2, CPU cpu)
        {
            byte result = (byte)(v1 ^ v2);

            if (result == 0) cpu.regs.SetFlag(Flags.Z);
            else cpu.regs.ClearFlag(Flags.Z);

            cpu.regs.ClearFlag(Flags.N);
            cpu.regs.ClearFlag(Flags.H);
            cpu.regs.ClearFlag(Flags.C);

            return result;
        }
    }

    //0xAF
    class XOR_AA : XOR
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformXOR(cpu.regs.A, cpu.regs.A, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "xor a,a";
        }
    }

    //0xA8
    class XOR_AB : XOR
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformXOR(cpu.regs.A, cpu.regs.B, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "xor a,b";
        }
    }

    //0xA9
    class XOR_AC : XOR
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformXOR(cpu.regs.A, cpu.regs.C, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "xor a,c";
        }
    }

    //0xAA
    class XOR_AD : XOR
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformXOR(cpu.regs.A, cpu.regs.D, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "xor a,d";
        }
    }

    //0xAB
    class XOR_AE : XOR
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformXOR(cpu.regs.A, cpu.regs.E, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }
    }

    //0xAC
    class XOR_AH : XOR
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformXOR(cpu.regs.A, cpu.regs.H, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "xor a,h";
        }
    }

    //0xAD
    class XOR_AL : XOR
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformXOR(cpu.regs.A, cpu.regs.L, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "xor a,l";
        }
    }

    //0xAE
    class XOR_AindexHL : XOR
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte(cpu.regs.HL);
            cpu.regs.A = PerformXOR(cpu.regs.A, value, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "xor a,(hl)";
        }
    }

    //0xEE
    class XOR_An : XOR
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte((ushort)(cpu.regs.PC + 1));
            cpu.regs.A = PerformXOR(cpu.regs.A, value, cpu);
            cpu.regs.IncreasePC(2);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            byte value = memorymap.GetByte((ushort)(address + 1));
            n_bytes = 2;
            builder.AppendFormat("xor a,0x{0:X2}", value);
            return builder.ToString();
        }
    }

}
