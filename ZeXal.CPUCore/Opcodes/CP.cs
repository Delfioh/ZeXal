using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //CP A,n
    //Description:
    //Compare A with n.
    //Use with:
    //n = A,B,C,D,E,H,L,(HL),#
    //Flags affected:
    //Z - Set if result is zero.
    //N - Set.
    //H - Set if no borrow from bit 4..
    //C - Set if no borrow.

    class CP : Opcode
    {
        protected void PerformCP(CPU cpu, byte v)
        {
            if (cpu.regs.A == v) cpu.regs.SetFlag(Flags.Z);
            else cpu.regs.ClearFlag(Flags.Z);

            cpu.regs.SetFlag(Flags.N);

            if ((byte)(cpu.regs.A & 0x0F) < (byte)(cpu.regs.A & 0x0F)) cpu.regs.SetFlag(Flags.H);
            else cpu.regs.ClearFlag(Flags.H);

            if (cpu.regs.A < v)
            {
                cpu.regs.SetFlag(Flags.C);
            }
            else
            {
                cpu.regs.ClearFlag(Flags.C);
            }
        }
    }

    //0xBF
    class CP_A : CP
    {
        public override int Execute(CPU cpu)
        {
            PerformCP(cpu, cpu.regs.A);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "cp a,a";
        }
    }

    //0xB8
    class CP_B : CP
    {
        public override int Execute(CPU cpu)
        {
            PerformCP(cpu, cpu.regs.B);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "cp a,b";
        }
    }

    //0xB9
    class CP_C : CP
    {
        public override int Execute(CPU cpu)
        {
            PerformCP(cpu, cpu.regs.C);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "cp a,c";
        }
    }

    //0xBA
    class CP_D : CP
    {
        public override int Execute(CPU cpu)
        {
            PerformCP(cpu, cpu.regs.D);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "cp a,d";
        }
    }

    //0xBB
    class CP_E : CP
    {
        public override int Execute(CPU cpu)
        {
            PerformCP(cpu, cpu.regs.E);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "cp a,e";
        }
    }

    //0xBC
    class CP_H : CP
    {
        public override int Execute(CPU cpu)
        {
            PerformCP(cpu, cpu.regs.H);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "cp a,h";
        }
    }

    //0xBD
    class CP_L : CP
    {
        public override int Execute(CPU cpu)
        {
            PerformCP(cpu, cpu.regs.L);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "cp a,l";
        }

    }

    //0xBE
    class CP_indexHL : CP
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte(cpu.regs.HL);
            PerformCP(cpu, value);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "cp a,(hl)";
        }
    }

    //0xF6
    class CP_n : CP
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte((ushort)(cpu.regs.PC + 1));
            PerformCP(cpu, value);
            cpu.regs.IncreasePC(2);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            byte value = memorymap.GetByte((ushort)(address + 1));
            n_bytes = 2;
            builder.AppendFormat("cp a,0x{0:X2}", value);
            return builder.ToString();
        }
    }
}
