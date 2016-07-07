using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //RLC n
    //Description:
    //Rotate n left. Old bit 7 to Carry flag.
    //Use with:
    //n = A,B,C,D,E,H,L,(HL)
    //Flags affected:
    //Z - Set if result is zero.
    //N - Reset.
    //H - Reset.
    //C - Contains old bit 7 data

    
    class RLC : Opcode
    {
        protected void PerformRLC(ref byte v, CPU cpu)
        {
            cpu.regs.ClearFlag(Flags.N);
            cpu.regs.ClearFlag(Flags.H);

            byte rot_bit = (byte)((v & 0x80) >> 7);

            if (rot_bit == 0x01) cpu.regs.SetFlag(Flags.C);
            else cpu.regs.ClearFlag(Flags.C);

            v = (byte)((v << 1) | rot_bit);

            if (v == 0) cpu.regs.SetFlag(Flags.Z);
            else cpu.regs.ClearFlag(Flags.Z);

        }
    }

    //0xCB07
    class RLC_A : RLC
    {
        public override int Execute(CPU cpu)
        {
            PerformRLC(ref cpu.regs.A, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rlc a";
        }
    }

    //0xCB00
    class RLC_B : RLC
    {
        public override int Execute(CPU cpu)
        {
            PerformRLC(ref cpu.regs.B, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rlc b";
        }
    }

    //0xCB01
    class RLC_C : RLC
    {
        public override int Execute(CPU cpu)
        {
            PerformRLC(ref cpu.regs.C, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rlc c";
        }
    }

    //0xCB02
    class RLC_D : RLC
    {
        public override int Execute(CPU cpu)
        {
            PerformRLC(ref cpu.regs.D, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rlc d";
        }
    }

    //0xCB03
    class RLC_E : RLC
    {
        public override int Execute(CPU cpu)
        {
            PerformRLC(ref cpu.regs.E, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rlc e";
        }
    }

    //0xCB04
    class RLC_H : RLC
    {
        public override int Execute(CPU cpu)
        {
            PerformRLC(ref cpu.regs.H, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rlc h";
        }
    }

    //0xCB05
    class RLC_L : RLC
    {
        public override int Execute(CPU cpu)
        {
            PerformRLC(ref cpu.regs.L, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rlc l";
        }
    }

    //0xCB06
    class RLC_indexHL : RLC
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte(cpu.regs.HL);
            PerformRLC(ref value, cpu);
            cpu.memorymap.SetByte(cpu.regs.HL, value);
            cpu.regs.IncreasePC(1);
            return 16;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rlc (hl)";
        }
    }

}
