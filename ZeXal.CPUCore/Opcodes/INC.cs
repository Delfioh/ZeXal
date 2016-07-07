using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //INC n
    //Description:
    //Increment register n.
    //Use with:
    //n = A,B,C,D,E,H,L,(HL)
    //Flags affected:
    //Z - Set if result is zero.
    //N - Reset.
    //H - Set if carry from bit 3.
    //C - Not affected.

    //Generic implementation
    class INC : Opcode
    {
        protected byte PerformINC(byte v, CPU cpu)
        {
            v++;

            if (v == 0) cpu.regs.SetFlag(Flags.Z);
            else cpu.regs.ClearFlag(Flags.Z);

            cpu.regs.ClearFlag(Flags.N);

            if ((byte)(v & 0x0F) == 0) cpu.regs.SetFlag(Flags.H);
            else cpu.regs.ClearFlag(Flags.H);

            return v;
        }
    }

    //0x3C
    class INC_A : INC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformINC(cpu.regs.A, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "inc a";
        }
    }

    //0x04
    class INC_B : INC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.B = PerformINC(cpu.regs.B, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "inc b";
        }
    }

    //0x0C
    class INC_C : INC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.C = PerformINC(cpu.regs.C, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "inc c";
        }
    }

    //0x14
    class INC_D : INC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.D = PerformINC(cpu.regs.D, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "inc d";
        }
    }

    //0x1C
    class INC_E : INC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.E = PerformINC(cpu.regs.E, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "inc e";
        }
    }

    //0x24
    class INC_H : INC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.H = PerformINC(cpu.regs.H, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "inc h";
        }
    }

    //0x2C
    class INC_L : INC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.L = PerformINC(cpu.regs.L, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "inc l";
        }
    }

    //0x34
    class INC_indexHL : INC
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte(cpu.regs.HL);
            value = PerformINC(value, cpu);
            cpu.memorymap.SetByte(cpu.regs.HL, value);
            cpu.regs.IncreasePC(1);
            return 12;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "inc (hl)";
        }
    }

}
