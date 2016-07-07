using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //DEC A,n
    //Description:
    //Decrement register n.
    //Use with:
    //n = A,B,C,D,E,H,L,(HL)
    //Flags affected:
    //Z - Set if result is zero.
    //N - Set.
    //H - Set if no borrow from bit 4.
    //C - Not affected.

    //Generic implementation
    class DEC : Opcode
    {
        protected byte PerformDEC(byte v, CPU cpu)
        {
            byte old_v = v;
            v--;

            if (v == 0) cpu.regs.SetFlag(Flags.Z);
            else cpu.regs.ClearFlag(Flags.Z);

            cpu.regs.SetFlag(Flags.N);

            if ((byte)(old_v & 0x0F) < (byte)(v & 0x0F)) cpu.regs.SetFlag(Flags.H);
            else cpu.regs.ClearFlag(Flags.H);

            return v;
        }
    }

    //0x3D
    class DEC_A : DEC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformDEC(cpu.regs.A, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "dec a";
        }
    }

    //0x05
    class DEC_B : DEC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.B = PerformDEC(cpu.regs.B, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "dec b";
        }
    }

    //0x0D
    class DEC_C : DEC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.C = PerformDEC(cpu.regs.C, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "dec c";
        }
    }

    //0x15
    class DEC_D : DEC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.D = PerformDEC(cpu.regs.D, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "dec d";
        }
    }

    //0x1D
    class DEC_E : DEC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.E = PerformDEC(cpu.regs.E, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "dec e";
        }
    }

    //0x25
    class DEC_H : DEC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.H = PerformDEC(cpu.regs.H, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "dec h";
        }
    }

    //0x2D
    class DEC_L : DEC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.L = PerformDEC(cpu.regs.L, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "dec l";
        }
    }

    //0x35
    class DEC_indexHL : DEC
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte(cpu.regs.HL);
            value = PerformDEC(value, cpu);
            cpu.memorymap.SetByte(cpu.regs.HL, value);
            cpu.regs.IncreasePC(1);
            return 12;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "dec (hl)";
        }
    }

}
