using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //SUB A,n
    //Description:
    //Subtract n from A.
    //Use with:
    //n = A,B,C,D,E,H,L,(HL),#
    //Flags affected:
    //Z - Set if result is zero.
    //N - Set.
    //H - Set if no borrow from bit 4.
    //C - Set if no borrow.

    //Generic implementation
    class SUB : Opcode
    {
        protected byte PerformSUB(byte v1, byte v2, CPU cpu)
        {
            byte result = 0;

            if (result == 0) cpu.regs.SetFlag(Flags.Z);
            else cpu.regs.ClearFlag(Flags.Z);

            cpu.regs.SetFlag(Flags.N);

            if ((byte)(v1 & 0x0F) < (byte)(v2 & 0x0F)) cpu.regs.SetFlag(Flags.H);
            else cpu.regs.ClearFlag(Flags.H);

            if (v1 < v2)
            {
                result = (byte)(0xFF - (v2 - v1));
                cpu.regs.SetFlag(Flags.C);
            }
            else
            {
                result = (byte)(v1 - v2);
                cpu.regs.ClearFlag(Flags.C);
            }

            return result;
        }
    }

    //0x07
    class SUB_AA : SUB
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformSUB(cpu.regs.A, cpu.regs.A, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sub a,a";
        }
    }

    //0x90
    class SUB_AB : SUB
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformSUB(cpu.regs.A, cpu.regs.B, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sub a,b";
        }
    }

    //0x91
    class SUB_AC : SUB
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformSUB(cpu.regs.A, cpu.regs.C, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sub a,c";
        }
    }

    //0x92
    class SUB_AD : SUB
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformSUB(cpu.regs.A, cpu.regs.D, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sub a,d";
        }
    }

    //0x93
    class SUB_AE : SUB
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformSUB(cpu.regs.A, cpu.regs.E, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sub a,e";
        }
    }

    //0x94
    class SUB_AH : SUB
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformSUB(cpu.regs.A, cpu.regs.H, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sub a,h";
        }
    }

    //0x95
    class SUB_AL : SUB
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformSUB(cpu.regs.A, cpu.regs.L, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sub a,l";
        }
    }

    //0x96
    class SUB_AindexHL : SUB
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte(cpu.regs.HL);
            cpu.regs.A = PerformSUB(cpu.regs.A, value, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sub a,(hl)";
        }
    }

    //0xD6
    class SUB_An : SUB
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte((ushort)(cpu.regs.PC + 1));
            cpu.regs.A = PerformSUB(cpu.regs.A, value, cpu);
            cpu.regs.IncreasePC(2);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            byte value = memorymap.GetByte((ushort)(address + 1));
            n_bytes = 2;
            builder.AppendFormat("sub a,0x{0:X2}", value);
            return builder.ToString();
        }
    }

}
