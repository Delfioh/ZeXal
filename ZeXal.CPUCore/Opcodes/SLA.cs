using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //SLA n
    //Description:
    //Shift n left into Carry. LSB of n set to 0.

    //Use with:
    //n = A,B,C,D,E,H,L,(HL)
    //Flags affected:
    //Z - Set if result is zero.
    //N - Reset.
    //H - Reset.
    //C - Contains old bit 7 data

    
    class SLA : Opcode
    {
        protected void PerformSLA(ref byte v, CPU cpu)
        {
            cpu.regs.ClearFlag(Flags.N);
            cpu.regs.ClearFlag(Flags.H);

            byte carry_bit = (byte)((v & 0x80) >> 7);

            if (carry_bit == 0x01) cpu.regs.SetFlag(Flags.C);
            else cpu.regs.ClearFlag(Flags.C);

            v = (byte)((v << 1) & 0xFE);

            if (v == 0) cpu.regs.SetFlag(Flags.Z);
            else cpu.regs.ClearFlag(Flags.Z);

        }
    }

    //0xCB27
    class SLA_A : SLA
    {
        public override int Execute(CPU cpu)
        {
            PerformSLA(ref cpu.regs.A, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sla a";
        }
    }

    //0xCB20
    class SLA_B : SLA
    {
        public override int Execute(CPU cpu)
        {
            PerformSLA(ref cpu.regs.B, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sla b";
        }
    }

    //0xCB21
    class SLA_C : SLA
    {
        public override int Execute(CPU cpu)
        {
            PerformSLA(ref cpu.regs.C, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sla c";
        }
    }

    //0xCB22
    class SLA_D : SLA
    {
        public override int Execute(CPU cpu)
        {
            PerformSLA(ref cpu.regs.D, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sla d";
        }
    }

    //0xCB23
    class SLA_E : SLA
    {
        public override int Execute(CPU cpu)
        {
            PerformSLA(ref cpu.regs.E, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sla e";
        }
    }

    //0xCB24
    class SLA_H : SLA
    {
        public override int Execute(CPU cpu)
        {
            PerformSLA(ref cpu.regs.H, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sla h";
        }
    }

    //0xCB25
    class SLA_L : SLA
    {
        public override int Execute(CPU cpu)
        {
            PerformSLA(ref cpu.regs.L, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sla l";
        }
    }

    //0xCB26
    class SLA_indexHL : SLA
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte(cpu.regs.HL);
            PerformSLA(ref value, cpu);
            cpu.memorymap.SetByte(cpu.regs.HL, value);
            cpu.regs.IncreasePC(1);
            return 16;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "sla (hl)";
        }
    }

}
