using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //SRL n
    //Description:
    //Shift n right into Carry. MSB set to 0.
    //Use with:
    //n = A,B,C,D,E,H,L,(HL)
    //Flags affected:
    //Z - Set if result is zero.
    //N - Reset.
    //H - Reset.
    //C - Contains old bit 0 data

    
    class SRL : Opcode
    {
        protected void PerformSRL(ref byte v, CPU cpu)
        {
            cpu.regs.ClearFlag(Flags.N);
            cpu.regs.ClearFlag(Flags.H);

            byte carry_bit = (byte)(v & 0x01);

            if (carry_bit == 0x01) cpu.regs.SetFlag(Flags.C);
            else cpu.regs.ClearFlag(Flags.C);

            v = (byte)((v >> 1) & 0x7F);

            if (v == 0) cpu.regs.SetFlag(Flags.Z);
            else cpu.regs.ClearFlag(Flags.Z);

        }
    }

    //0xCB3F
    class SRL_A : SRL
    {
        public override int Execute(CPU cpu)
        {
            PerformSRL(ref cpu.regs.A, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "srl a";
        }
    }

    //0xCB38
    class SRL_B : SRL
    {
        public override int Execute(CPU cpu)
        {
            PerformSRL(ref cpu.regs.B, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "srl b";
        }
    }

    //0xCB39
    class SRL_C : SRL
    {
        public override int Execute(CPU cpu)
        {
            PerformSRL(ref cpu.regs.C, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "srl c";
        }
    }

    //0xCB3A
    class SRL_D : SRL
    {
        public override int Execute(CPU cpu)
        {
            PerformSRL(ref cpu.regs.D, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "srl d";
        }
    }

    //0xCB3B
    class SRL_E : SRL
    {
        public override int Execute(CPU cpu)
        {
            PerformSRL(ref cpu.regs.E, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "srl e";
        }
    }

    //0xCB3C
    class SRL_H : SRL
    {
        public override int Execute(CPU cpu)
        {
            PerformSRL(ref cpu.regs.H, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "srl h";
        }
    }

    //0xCB3D
    class SRL_L : SRL
    {
        public override int Execute(CPU cpu)
        {
            PerformSRL(ref cpu.regs.L, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "srl l";
        }
    }

    //0xCB3E
    class SRL_indexHL : SRL
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte(cpu.regs.HL);
            PerformSRL(ref value, cpu);
            cpu.memorymap.SetByte(cpu.regs.HL, value);
            cpu.regs.IncreasePC(1);
            return 16;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "srl (hl)";
        }
    }

}
