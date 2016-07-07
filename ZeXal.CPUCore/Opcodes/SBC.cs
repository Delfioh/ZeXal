using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //SBC A,n
    //Description:
    //Subtract n + Carry flag from A.
    //Use with:
    //n = A,B,C,D,E,H,L,(HL),#
    //Flags affected:
    //Z - Set if result is zero.
    //N - Set.
    //H - Set if no borrow from bit 4..
    //C - Set if no borrow.

    //Generic implementation
    class SBC : Opcode
    {
        protected byte PerformSBC(byte v1, byte v2, CPU cpu)
        {
            int result = v1 - (v2 + cpu.regs.GetFlag(Flags.C));

            if (result == 0) cpu.regs.SetFlag(Flags.Z);
            else cpu.regs.ClearFlag(Flags.Z);

            cpu.regs.SetFlag(Flags.N);

            if (result < 0)
            {
                result += 0xFF;
                cpu.regs.SetFlag(Flags.C);
            }
            else
            {
                cpu.regs.ClearFlag(Flags.C);
            }

            if ((byte)((v1 & 0x0F) - (v2 & 0x0F) - cpu.regs.GetFlag(Flags.C)) < 0) cpu.regs.SetFlag(Flags.H);
            else cpu.regs.ClearFlag(Flags.H);

            return (byte)result;
        }
    }

    //0x9F
    class SBC_AA : SBC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformSBC(cpu.regs.A, cpu.regs.A, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sbc a,a";
        }
    }

    //0x98
    class SBC_AB : SBC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformSBC(cpu.regs.A, cpu.regs.B, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sbc a,b";
        }
    }

    //0x99
    class SBC_AC : SBC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformSBC(cpu.regs.A, cpu.regs.C, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sbc a,c";
        }
    }

    //0x9A
    class SBC_AD : SBC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformSBC(cpu.regs.A, cpu.regs.D, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sbc a,d";
        }
    }

    //0x9B
    class SBC_AE : SBC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformSBC(cpu.regs.A, cpu.regs.E, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sbc a,e";
        }
    }

    //0x9C
    class SBC_AH : SBC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformSBC(cpu.regs.A, cpu.regs.H, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sbc a,h";
        }
    }

    //0x9D
    class SBC_AL : SBC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformSBC(cpu.regs.A, cpu.regs.L, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sbc a,l";
        }
    }

    //0x9E
    class SBC_AindexHL : SBC
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte(cpu.regs.HL);
            cpu.regs.A = PerformSBC(cpu.regs.A, value, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sbc a,(hl)";
        }
    }

    //0xDE
    class SBC_An : SBC
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte((ushort)(cpu.regs.PC + 1));
            cpu.regs.A = PerformSBC(cpu.regs.A, value, cpu);
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
