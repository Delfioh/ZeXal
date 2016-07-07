using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //SET b,r
    //Description:
    //Set bit b in register r.
    //Use with:
    //b=0-7 n = A,B,C,D,E,H,L,(HL)
    //Flags affected:
    //None.

    
    class SET : Opcode
    {
        protected byte bit_n;

        public SET(byte bit_n)
        {
            this.bit_n = bit_n;
        }

        protected void PerformSET(ref byte v, CPU cpu)
        {
            byte mask = (byte)(0x01 << bit_n);
            v = (byte)(v | mask);
        }
    }

    //0xCBC7
    class SET_A : SET
    {
        public SET_A(byte bit_n)
            : base(bit_n)
        {
        }

        public override int Execute(CPU cpu)
        {
            PerformSET(ref cpu.regs.A, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            n_bytes = 1;
            builder.AppendFormat("set a,{0}", bit_n);
            return builder.ToString();
        }
    }

    //0xCBC0
    class SET_B : SET
    {
        public SET_B(byte bit_n)
            : base(bit_n)
        {
        }

        public override int Execute(CPU cpu)
        {
            PerformSET(ref cpu.regs.B, cpu); ;
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            n_bytes = 1;
            builder.AppendFormat("set b,{0}", bit_n);
            return builder.ToString();
        }
    }

    //0xCBC1
    class SET_C : SET
    {
        public SET_C(byte bit_n)
            : base(bit_n)
        {
        }

        public override int Execute(CPU cpu)
        {
            PerformSET(ref cpu.regs.C, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            n_bytes = 1;
            builder.AppendFormat("set c,{0}", bit_n);
            return builder.ToString();
        }
    }

    //0xCBC2
    class SET_D : SET
    {
        public SET_D(byte bit_n)
            : base(bit_n)
        {
        }

        public override int Execute(CPU cpu)
        {
            PerformSET(ref cpu.regs.D, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            n_bytes = 1;
            builder.AppendFormat("set d,{0}", bit_n);
            return builder.ToString();
        }
    }

    //0xCBC3
    class SET_E : SET
    {
        public SET_E(byte bit_n)
            : base(bit_n)
        {
        }

        public override int Execute(CPU cpu)
        {
            PerformSET(ref cpu.regs.E, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            n_bytes = 1;
            builder.AppendFormat("set e,{0}", bit_n);
            return builder.ToString();
        }
    }

    //0xCBC4
    class SET_H : SET
    {
        public SET_H(byte bit_n)
            : base(bit_n)
        {
        }

        public override int Execute(CPU cpu)
        {
            PerformSET(ref cpu.regs.H, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            n_bytes = 1;
            builder.AppendFormat("set h,{0}", bit_n);
            return builder.ToString();
        }
    }

    //0xCBC5
    class SET_L : SET
    {
        public SET_L(byte bit_n)
            : base(bit_n)
        {
        }

        public override int Execute(CPU cpu)
        {
            PerformSET(ref cpu.regs.L, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            n_bytes = 1;
            builder.AppendFormat("set l,{0}", bit_n);
            return builder.ToString();
        }
    }

    //0xCBC6
    class SET_indexHL : SET
    {
        public SET_indexHL(byte bit_n)
            : base(bit_n)
        {
        }

        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte(cpu.regs.HL);
            PerformSET(ref value, cpu);
            cpu.memorymap.SetByte(cpu.regs.HL, value);
            cpu.regs.IncreasePC(1);
            return 16;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            n_bytes = 1;
            builder.AppendFormat("set (hl),{0}", bit_n);
            return builder.ToString();
        }
    }

}
