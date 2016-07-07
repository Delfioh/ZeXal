using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //ADD A,n
    //Description:
    //ADD n to A.
    //Use with:
    //n = A,B,C,D,E,H,L,(HL),#
    //Flags affected:
    //Z - Set if result is zero.
    //N - Reset.
    //H - Set if carry from bit 3.
    //C - Set if carry from bit 7.

    //Generic implementation
    class ADD : Opcode
    {
        protected byte PerformADD(byte v1, byte v2, CPU cpu)
        {
            ushort result = (ushort)(v1 + v2);

            if (result == 0) cpu.regs.SetFlag(Flags.Z);
            else cpu.regs.ClearFlag(Flags.Z);

            cpu.regs.ClearFlag(Flags.N);

            if (result > 0xFF) cpu.regs.SetFlag(Flags.C);
            else cpu.regs.ClearFlag(Flags.C);

            if (((byte)(v1 & 0x0F) + (byte)(v2 & 0x0F) + cpu.regs.GetFlag(Flags.C)) > 0x0F) cpu.regs.SetFlag(Flags.H);
            else cpu.regs.ClearFlag(Flags.H);

            return (byte)result;
        }
    }

    //0x87
    class ADD_AA : ADD
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformADD(cpu.regs.A, cpu.regs.A, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "add a,a";
        }
    }

    //0x80
    class ADD_AB : ADD
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformADD(cpu.regs.A, cpu.regs.B, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "add a,b";
        }
    }

    //0x81
    class ADD_AC : ADD
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformADD(cpu.regs.A, cpu.regs.C, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "add a,c";
        }
    }

    //0x82
    class ADD_AD : ADD
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformADD(cpu.regs.A, cpu.regs.D, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "add a,d";
        }
    }

    //0x83
    class ADD_AE : ADD
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformADD(cpu.regs.A, cpu.regs.E, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "add a,e";
        }
    }

    //0x84
    class ADD_AH : ADD
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformADD(cpu.regs.A, cpu.regs.H, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "add a,h";
        }
    }

    //0x85
    class ADD_AL : ADD
    {
        public override int Execute(CPU cpu)
        {

            cpu.regs.A = PerformADD(cpu.regs.A, cpu.regs.L, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "add a,l";
        }
    }

    //0x86
    class ADD_AindexHL : ADD
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte(cpu.regs.HL);
            cpu.regs.A = PerformADD(cpu.regs.A, value, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "add a,(hl)";
        }
    }

    //0xC6
    class ADD_An : ADD
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte((ushort)(cpu.regs.PC + 1));
            cpu.regs.A = PerformADD(cpu.regs.A, value, cpu);
            cpu.regs.IncreasePC(2);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            byte value = memorymap.GetByte((ushort)(address + 1));
            n_bytes = 2;
            builder.AppendFormat("add a,0x{0:X2}", value);
            return builder.ToString();
        }
    }

}
