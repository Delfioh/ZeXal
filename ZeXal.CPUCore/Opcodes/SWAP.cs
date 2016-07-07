using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //SWAP n
    //Description:
    //Swap upper & lower nibles of n.
    //Use with:
    //n = A,B,C,D,E,H,L,(HL)
    //Flags affected:
    //Z - Set if result is zero.
    //N - Reset.
    //H - Reset.
    //C - Reset.

    class SWAP : Opcode
    {
        protected void PerformSWAP(ref byte v, CPU cpu)
        {
            byte high_nibble = (byte)((v & 0xF0) >> 4);
            byte low_nibble = (byte)(v & 0x0F);
            v = (byte)((low_nibble << 4) + high_nibble);

            if (v == 0) cpu.regs.SetFlag(Flags.Z);
            else cpu.regs.ClearFlag(Flags.Z);

            cpu.regs.ClearFlag(Flags.N);
            cpu.regs.ClearFlag(Flags.H);
            cpu.regs.ClearFlag(Flags.C);
        }
    }


    //0xCB37
    class SWAP_A : SWAP
    {
        public override int Execute(CPU cpu)
        {
            PerformSWAP(ref cpu.regs.A, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "swap a";
        }
    }

    //0xCB30
    class SWAP_B : SWAP
    {
        public override int Execute(CPU cpu)
        {
            PerformSWAP(ref cpu.regs.B, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "swap b";
        }
    }

    //0xCB31
    class SWAP_C : SWAP
    {
        public override int Execute(CPU cpu)
        {
            PerformSWAP(ref cpu.regs.C, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "swap c";
        }
    }

    //0xCB32
    class SWAP_D : SWAP
    {
        public override int Execute(CPU cpu)
        {
            PerformSWAP(ref cpu.regs.D, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "swap d";
        }
    }

    //0xCB33
    class SWAP_E : SWAP
    {
        public override int Execute(CPU cpu)
        {
            PerformSWAP(ref cpu.regs.E, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "swap e";
        }
    }

    //0xCB34
    class SWAP_H : SWAP
    {
        public override int Execute(CPU cpu)
        {
            PerformSWAP(ref cpu.regs.H, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "swap h";
        }
    }

    //0xCB35
    class SWAP_L : SWAP
    {
        public override int Execute(CPU cpu)
        {
            PerformSWAP(ref cpu.regs.L, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "swap l";
        }
    }

    //0xCB36
    class SWAP_indexHL : SWAP
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte(cpu.regs.HL);
            PerformSWAP(ref value, cpu);
            cpu.memorymap.SetByte(cpu.regs.HL, value);
            cpu.regs.IncreasePC(1);
            return 16;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "swap (hl)";
        }
    }

}
