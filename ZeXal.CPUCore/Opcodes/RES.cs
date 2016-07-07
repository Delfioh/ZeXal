using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //RES b,r
    //Description:
    //Reset bit b in register r.
    //Use with:
    //b=0-7 n = A,B,C,D,E,H,L,(HL)
    //Flags affected:
    //None.

    
    class RES : Opcode
    {
        protected byte bit_n;

        public RES(byte bit_n)
        {
            this.bit_n = bit_n;
        }

        protected void PerformRES(ref byte v, CPU cpu)
        {
            byte mask = (byte)(0x01 << bit_n);
            mask ^= 0xFF;
            v = (byte)(v & mask);
        }
    }

    //0xCB87
    class RES_A : RES
    {
        public RES_A(byte bit_n)
            : base(bit_n)
        {
        }

        public override int Execute(CPU cpu)
        { 
            PerformRES(ref cpu.regs.A, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            n_bytes = 1;
            builder.AppendFormat("res a,{0}", bit_n);
            return builder.ToString();
        }
    }

    //0xCB80
    class RES_B : RES
    {
        public RES_B(byte bit_n)
            : base(bit_n)
        {
        }

        public override int Execute(CPU cpu)
        {
            PerformRES(ref cpu.regs.B, cpu); ;
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            n_bytes = 1;
            builder.AppendFormat("res b,{0}", bit_n);
            return builder.ToString();
        }
    }

    //0xCB81
    class RES_C : RES
    {
        public RES_C(byte bit_n)
            : base(bit_n)
        {
        }

        public override int Execute(CPU cpu)
        {
            PerformRES(ref cpu.regs.C, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            n_bytes = 1;
            builder.AppendFormat("res c,{0}", bit_n);
            return builder.ToString();
        }
    }

    //0xCB82
    class RES_D : RES
    {
        public RES_D(byte bit_n)
            : base(bit_n)
        {
        }

        public override int Execute(CPU cpu)
        {
            PerformRES(ref cpu.regs.D, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            n_bytes = 1;
            builder.AppendFormat("res d,{0}", bit_n);
            return builder.ToString();
        }

    }

    //0xCB83
    class RES_E : RES
    {
        public RES_E(byte bit_n)
            : base(bit_n)
        {
        }

        public override int Execute(CPU cpu)
        {
            PerformRES(ref cpu.regs.E, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            n_bytes = 1;
            builder.AppendFormat("res e,{0}", bit_n);
            return builder.ToString();
        }
    }

    //0xCB84
    class RES_H : RES
    {
        public RES_H(byte bit_n)
            : base(bit_n)
        {
        }

        public override int Execute(CPU cpu)
        {
            PerformRES(ref cpu.regs.H, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            n_bytes = 1;
            builder.AppendFormat("res h,{0}", bit_n);
            return builder.ToString();
        }
    }

    //0xCB85
    class RES_L : RES
    {
        public RES_L(byte bit_n)
            : base(bit_n)
        {
        }

        public override int Execute(CPU cpu)
        {
            PerformRES(ref cpu.regs.L, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            n_bytes = 1;
            builder.AppendFormat("res l,{0}", bit_n);
            return builder.ToString();
        }
    }

    //0xCB86
    class RES_indexHL : RES
    {
        public RES_indexHL(byte bit_n)
            : base(bit_n)
        {
        }

        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte(cpu.regs.HL);
            PerformRES(ref value, cpu);
            cpu.memorymap.SetByte(cpu.regs.HL, value);
            cpu.regs.IncreasePC(1);
            return 16;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            n_bytes = 1;
            builder.AppendFormat("res (hl),{0}", bit_n);
            return builder.ToString();
        }

    }

}
