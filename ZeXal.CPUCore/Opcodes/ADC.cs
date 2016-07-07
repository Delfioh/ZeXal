using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //ADC A,n
    //Description:
    //ADC n + Carry flag to A.
    //Use with:
    //n = A,B,C,D,E,H,L,(HL),#
    //Flags affected:
    //Z - Set if result is zero.
    //N - Reset.
    //H - Set if carry from bit 3.
    //C - Set if carry from bit 7.

    //Generic implementation
    class ADC : Opcode
    {
        protected byte PerformADC(byte v1, byte v2, CPU cpu)
        {
            ushort result = (ushort)(v1 + v2 + cpu.regs.GetFlag(Flags.C));

            if (result == 0) cpu.regs.SetFlag(Flags.Z);
            else cpu.regs.ClearFlag(Flags.Z);

            cpu.regs.ClearFlag(Flags.N);

            if (result > 0xFF) cpu.regs.SetFlag(Flags.C);
            else cpu.regs.ClearFlag(Flags.C);

            if (((byte)(v1 & 0x0F) + (byte)(v2 & 0x0F) + cpu.regs.GetFlag(Flags.C)) > 0x0F) cpu.regs.SetFlag(Flags.H);
            else cpu.regs.ClearFlag(Flags.H);

            return (byte)result;
        }
    }

    //0x8F
    class ADC_AA : ADC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformADC(cpu.regs.A, cpu.regs.A, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "adc a,a";
        }
    }

    //0x88
    class ADC_AB : ADC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformADC(cpu.regs.A, cpu.regs.B, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "adc a,b";
        }
    }

    //0x89
    class ADC_AC : ADC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformADC(cpu.regs.A, cpu.regs.C, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "adc a,c";
        }
    }

    //0x8A
    class ADC_AD : ADC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformADC(cpu.regs.A, cpu.regs.D, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "adc a,d";
        }
    }

    //0x8B
    class ADC_AE : ADC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformADC(cpu.regs.A, cpu.regs.E, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "adc a,e";
        }
    }

    //0x8C
    class ADC_AH : ADC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformADC(cpu.regs.A, cpu.regs.H, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "adc a,h";
        }
    }

    //0x8D
    class ADC_AL : ADC
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = PerformADC(cpu.regs.A, cpu.regs.L, cpu);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "adc a,l";
        }

    }

    //0x8E
    class ADC_AindexHL : ADC
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte(cpu.regs.HL);
            cpu.regs.A = PerformADC(cpu.regs.A, value, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "adc a,(hl)";
        }
    }

    //0xCE
    class ADC_An : ADC
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte((ushort)(cpu.regs.PC + 1));
            cpu.regs.A = PerformADC(cpu.regs.A, value, cpu);
            cpu.regs.IncreasePC(2);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            byte value = memorymap.GetByte((ushort)(address + 1));
            n_bytes = 2;
            builder.AppendFormat("adc a,0x{0:X2}", value);
            return builder.ToString();
        }
    }

}
