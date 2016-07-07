using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //SRA n
    //Description:
    //Shift n right into Carry. MSB doesn't change.
    //Use with:
    //n = A,B,C,D,E,H,L,(HL)
    //Flags affected:
    //Z - Set if result is zero.
    //N - Reset.
    //H - Reset.
    //C - Contains old bit 0 data

    
    class SRA : Opcode
    {
        protected void PerformSRA(ref byte v, CPU cpu)
        {
            cpu.regs.ClearFlag(Flags.N);
            cpu.regs.ClearFlag(Flags.H);

            byte carry_bit = (byte)(v & 0x01);

            if (carry_bit == 0x01) cpu.regs.SetFlag(Flags.C);
            else cpu.regs.ClearFlag(Flags.C);

            byte msb = (byte)(v & 0x80);
            v = (byte)(((v >> 1) & 0x7F) | msb);

            if (v == 0) cpu.regs.SetFlag(Flags.Z);
            else cpu.regs.ClearFlag(Flags.Z);

        }
    }

    //0xCB2F
    class SRA_A : SRA
    {
        public override int Execute(CPU cpu)
        {
            PerformSRA(ref cpu.regs.A, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sra a";
        }
    }

    //0xCB28
    class SRA_B : SRA
    {
        public override int Execute(CPU cpu)
        {
            PerformSRA(ref cpu.regs.B, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sra b";
        }
    }

    //0xCB29
    class SRA_C : SRA
    {
        public override int Execute(CPU cpu)
        {
            PerformSRA(ref cpu.regs.C, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sra c";
        }
    }

    //0xCB2A
    class SRA_D : SRA
    {
        public override int Execute(CPU cpu)
        {
            PerformSRA(ref cpu.regs.D, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sra d";
        }
    }

    //0xCB2B
    class SRA_E : SRA
    {
        public override int Execute(CPU cpu)
        {
            PerformSRA(ref cpu.regs.E, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sra e";
        }
    }

    //0xCB2C
    class SRA_H : SRA
    {
        public override int Execute(CPU cpu)
        {
            PerformSRA(ref cpu.regs.H, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sra h";
        }
    }

    //0xCB2D
    class SRA_L : SRA
    {
        public override int Execute(CPU cpu)
        {
            PerformSRA(ref cpu.regs.L, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sra l";
        }
    }

    //0xCB2E
    class SRA_indexHL : SRA
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte(cpu.regs.HL);
            PerformSRA(ref value, cpu);
            cpu.memorymap.SetByte(cpu.regs.HL, value);
            cpu.regs.IncreasePC(1);
            return 16;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sra (hl)";
        }
    }

}
