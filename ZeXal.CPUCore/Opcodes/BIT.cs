using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //BIT b,r
    //Description:
    //Test bit b in register r.

    //Use with:
    //b=0-7 n = A,B,C,D,E,H,L,(HL)
    //Flags affected:
    //Z - Set if bit b of register r is 0.
    //N - Reset.
    //H - Set.
    //C - Not affected.

    
    class BIT : Opcode
    {
        protected byte bit_n;

        public BIT(byte bit_n)
        {
            this.bit_n = bit_n;
        }

        protected void PerformBIT(byte v, CPU cpu)
        {
            byte tested_bit = (byte)((v >> bit_n) & 0x01);
            if (tested_bit == 0x01) cpu.regs.ClearFlag(Flags.Z);
            else cpu.regs.SetFlag(Flags.Z);
            cpu.regs.ClearFlag(Flags.N);
            cpu.regs.SetFlag(Flags.H);
        }
    }

    //0xCB40
    class BIT_B : BIT
    {
        public BIT_B(byte bit_n)
            : base(bit_n)
        {
        }

        public override int Execute(CPU cpu)
        {
            PerformBIT(cpu.regs.B, cpu); ;
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            n_bytes = 1;
            builder.AppendFormat("bit b,{0}", bit_n);
            return builder.ToString();
        }

    }

    //0xCB41
    class BIT_C : BIT
    {
        public BIT_C(byte bit_n)
            : base(bit_n)
        {
        }

        public override int Execute(CPU cpu)
        {
            PerformBIT(cpu.regs.C, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            n_bytes = 1;
            builder.AppendFormat("bit c,{0}", bit_n);
            return builder.ToString();
        }
    }

    //0xCB42
    class BIT_D : BIT
    {
        public BIT_D(byte bit_n)
            : base(bit_n)
        {
        }

        public override int Execute(CPU cpu)
        {
            PerformBIT(cpu.regs.D, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            n_bytes = 1;
            builder.AppendFormat("bit d,{0}", bit_n);
            return builder.ToString();
        }
    }

    //0xCB43
    class BIT_E : BIT
    {

        public BIT_E(byte bit_n)
            : base(bit_n)
        {
        }

        public override int Execute(CPU cpu)
        {
            PerformBIT(cpu.regs.E, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            n_bytes = 1;
            builder.AppendFormat("bit e,{0}", bit_n);
            return builder.ToString();
        }
    }


    class BIT_H : BIT
    {
        public BIT_H(byte bit_n)
            : base(bit_n)
        {
        }

        public override int Execute(CPU cpu)
        {
            PerformBIT(cpu.regs.H, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            n_bytes = 1;
            builder.AppendFormat("bit h,{0}", bit_n);
            return builder.ToString();
        }
    }

    //0xCB45
    class BIT_L : BIT
    {
        public BIT_L(byte bit_n)
            : base(bit_n)
        {
        }

        public override int Execute(CPU cpu)
        {
            PerformBIT(cpu.regs.L, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            n_bytes = 1;
            builder.AppendFormat("bit l,{0}", bit_n);
            return builder.ToString();
        }
    }

    //0xCB46
    class BIT_indexHL : BIT
    {
        public BIT_indexHL(byte bit_n)
            : base(bit_n)
        {
        }

        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte(cpu.regs.HL);
            PerformBIT(value, cpu);
            cpu.regs.IncreasePC(1);
            return 16;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            n_bytes = 1;
            builder.AppendFormat("bit (hl),{0}", bit_n);
            return builder.ToString();
        }
    }

    //0xCB47
    class BIT_A : BIT
    {
        public BIT_A(byte bit_n)
            : base(bit_n)
        {
        }

        public override int Execute(CPU cpu)
        {
            PerformBIT(cpu.regs.A, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            n_bytes = 1;
            builder.AppendFormat("bit a,{0}", bit_n);
            return builder.ToString();
        }
    }



}
