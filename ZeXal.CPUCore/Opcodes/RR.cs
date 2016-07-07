using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //RR n
    //Description:
    //Rotate n right through Carry flag.
    //Use with:
    //n = A,B,C,D,E,H,L,(HL)
    //Flags affected:
    //Z - Set if result is zero.
    //N - Reset.
    //H - Reset.
    //C - Contains old bit 0 data

    
    class RR : Opcode
    {
        protected void PerformRR(ref byte v, CPU cpu)
        {
            cpu.regs.ClearFlag(Flags.N);
            cpu.regs.ClearFlag(Flags.H);

            byte rot_bit = cpu.regs.GetFlag(Flags.C);

            byte lsb = (byte)(v & 0x01);
            if (lsb == 0x01) cpu.regs.SetFlag(Flags.C);
            else cpu.regs.ClearFlag(Flags.C);

            v = (byte)((v >> 1) | (rot_bit << 7));

            if (v == 0) cpu.regs.SetFlag(Flags.Z);
            else cpu.regs.ClearFlag(Flags.Z);

        }
    }

    //0xCB1F
    class RR_A : RR
    {
        public override int Execute(CPU cpu)
        {
            PerformRR(ref cpu.regs.A, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rr a";
        }
    }

    //0xCB18
    class RR_B : RR
    {
        public override int Execute(CPU cpu)
        {
            PerformRR(ref cpu.regs.B, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rr b";
        }
    }

    //0xCB19
    class RR_C : RR
    {
        public override int Execute(CPU cpu)
        {
            PerformRR(ref cpu.regs.C, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rr c";
        }
    }

    //0xCB1A
    class RR_D : RR
    {
        public override int Execute(CPU cpu)
        {
            PerformRR(ref cpu.regs.D, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rr d";
        }
    }

    //0xCB1B
    class RR_E : RR
    {
        public override int Execute(CPU cpu)
        {
            PerformRR(ref cpu.regs.E, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rr e";
        }

    }

    //0xCB1C
    class RR_H : RR
    {
        public override int Execute(CPU cpu)
        {
            PerformRR(ref cpu.regs.H, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rr h";
        }
    }

    //0xCB1D
    class RR_L : RR
    {
        public override int Execute(CPU cpu)
        {
            PerformRR(ref cpu.regs.L, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rr l";
        }
    }

    //0xCB1E
    class RR_indexHL : RR
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte(cpu.regs.HL);
            PerformRR(ref value, cpu);
            cpu.memorymap.SetByte(cpu.regs.HL, value);
            cpu.regs.IncreasePC(1);
            return 16;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rr (hl)";
        }
    }

}
