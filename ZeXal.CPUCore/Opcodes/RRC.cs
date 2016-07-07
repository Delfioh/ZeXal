using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //RRC n
    //Description:
    //Rotate n left. Old bit 0 to Carry flag.
    //Use with:
    //n = A,B,C,D,E,H,L,(HL)
    //Flags affected:
    //Z - Set if result is zero.
    //N - Reset.
    //H - Reset.
    //C - Contains old bit 0 data

    
    class RRC : Opcode
    {
        protected void PerformRRC(ref byte v, CPU cpu)
        {
            cpu.regs.ClearFlag(Flags.N);
            cpu.regs.ClearFlag(Flags.H);

            byte rot_bit = (byte)(v & 0x01);

            if (rot_bit == 0x01) cpu.regs.SetFlag(Flags.C);
            else cpu.regs.ClearFlag(Flags.C);

            v = (byte)((v >> 1) | (rot_bit << 7));

            if (v == 0) cpu.regs.SetFlag(Flags.Z);
            else cpu.regs.ClearFlag(Flags.Z);

        }
    }

    //0xCB0F
    class RRC_A : RRC
    {
        public override int Execute(CPU cpu)
        {
            PerformRRC(ref cpu.regs.A, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rrc a";
        }
    }

    //0xCB08
    class RRC_B : RRC
    {
        public override int Execute(CPU cpu)
        {
            PerformRRC(ref cpu.regs.B, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rrc b";
        }
    }

    //0xCB09
    class RRC_C : RRC
    {
        public override int Execute(CPU cpu)
        {
            PerformRRC(ref cpu.regs.C, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rrc c";
        }
    }

    //0xCB0A
    class RRC_D : RRC
    {
        public override int Execute(CPU cpu)
        {
            PerformRRC(ref cpu.regs.D, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rrc d";
        }
    }

    //0xCB0B
    class RRC_E : RRC
    {
        public override int Execute(CPU cpu)
        {
            PerformRRC(ref cpu.regs.E, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rrc e";
        }
    }

    //0xCB0C
    class RRC_H : RRC
    {
        public override int Execute(CPU cpu)
        {
            PerformRRC(ref cpu.regs.H, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rrc h";
        }
    }

    //0xCB0D
    class RRC_L : RRC
    {
        public override int Execute(CPU cpu)
        {
            PerformRRC(ref cpu.regs.L, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rrc l";
        }
    }

    //0xCB0E
    class RRC_indexHL : RRC
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte(cpu.regs.HL);
            PerformRRC(ref value, cpu);
            cpu.memorymap.SetByte(cpu.regs.HL, value);
            cpu.regs.IncreasePC(1);
            return 16;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rrc (hl)";
        }
    }

}
