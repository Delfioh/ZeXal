using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //RL n
    //Description:
    //Rotate n left through Carry flag.
    //Use with:
    //n = A,B,C,D,E,H,L,(HL)
    //Flags affected:
    //Z - Set if result is zero.
    //N - Reset.
    //H - Reset.
    //C - Contains old bit 7 data

    
    class RL : Opcode
    {
        protected void PerformRL(ref byte v, CPU cpu)
        {
            cpu.regs.ClearFlag(Flags.N);
            cpu.regs.ClearFlag(Flags.H);

            byte rot_bit = cpu.regs.GetFlag(Flags.C);

            byte msb = (byte)((v & 0x80) >> 7);
            if (msb == 0x01) cpu.regs.SetFlag(Flags.C);
            else cpu.regs.ClearFlag(Flags.C);

            v = (byte)((v << 1) | rot_bit);

            if (v == 0) cpu.regs.SetFlag(Flags.Z);
            else cpu.regs.ClearFlag(Flags.Z);

        }
    }

    //0xCB17
    class RL_A : RL
    {
        public override int Execute(CPU cpu)
        {
            PerformRL(ref cpu.regs.A, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rl a";
        }
    }

    //0xCB10
    class RL_B : RL
    {
        public override int Execute(CPU cpu)
        {
            PerformRL(ref cpu.regs.B, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rl b";
        }
    }

    //0xCB11
    class RL_C : RL
    {
        public override int Execute(CPU cpu)
        {
            PerformRL(ref cpu.regs.C, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rl c";
        }
    }

    //0xCB12
    class RL_D : RL
    {
        public override int Execute(CPU cpu)
        {
            PerformRL(ref cpu.regs.D, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rl d";
        }
    }

    //0xCB13
    class RL_E : RL
    {
        public override int Execute(CPU cpu)
        {
            PerformRL(ref cpu.regs.E, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rl e";
        }

    }

    //0xCB14
    class RL_H : RL
    {
        public override int Execute(CPU cpu)
        {
            PerformRL(ref cpu.regs.H, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rl h";
        }
    }

    //0xCB15
    class RL_L : RL
    {
        public override int Execute(CPU cpu)
        {
            PerformRL(ref cpu.regs.L, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rl l";
        }
    }

    //0xCB16
    class RL_indexHL : RL
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte(cpu.regs.HL);
            PerformRL(ref value, cpu);
            cpu.memorymap.SetByte(cpu.regs.HL, value);
            cpu.regs.IncreasePC(1);
            return 16;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rl (hl)";
        }
    }

}
