using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //ADD HL,nn
    //Description:
    //Add nn to HL.
    //Use with:
    //n = BC,DE,HL,SP
    //Flags affected:
    //Z - Not affected.
    //N - Reset.
    //H - Set if carry from bit 11.
    //C - Set if carry from bit 15.

    //Generic implementation
    class ADD_HL : Opcode
    {
        protected ushort PerformADD_HL(uint v, CPU cpu)
        {
            uint result = cpu.regs.HL + v;

            cpu.regs.ClearFlag(Flags.N);

            if (result > 0xFFFF) cpu.regs.SetFlag(Flags.C);
            else cpu.regs.ClearFlag(Flags.C);

            if (((cpu.regs.HL & 0x0FFF) + (v & 0x0FFF) + cpu.regs.GetFlag(Flags.C)) > 0x0FFF) cpu.regs.SetFlag(Flags.H);
            else cpu.regs.ClearFlag(Flags.H);

            return (ushort)result;
        }
    }

    //0x09
    class ADD_HL_BC : ADD_HL
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.HL = PerformADD_HL(cpu.regs.BC, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "add hl,bc";
        }
    }

    //0x19
    class ADD_HL_DE : ADD_HL
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.HL = PerformADD_HL(cpu.regs.DE, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "add hl,de";
        }
    }

    //0x29
    class ADD_HL_HL : ADD_HL
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.HL = PerformADD_HL(cpu.regs.HL, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "add hl,hl";
        }
    }

    //0x39
    class ADD_HL_SP : ADD_HL
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.HL = PerformADD_HL(cpu.regs.SP, cpu);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "add hl,sp";
        }
    }

    //ADD SP,nn
    //Description:
    //Add nn to SP.
    //Use with:
    //nn = one byte signed immediate value
    //Flags affected:
    //Z - Reset.
    //N - Reset.
    //H - Set if carry from bit 11.
    //C - Set if carry from bit 15.
    
    //0xE8
    class ADD_SPn : Opcode
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte((ushort)(cpu.regs.PC + 1));
            uint new_sp_value = (uint)(cpu.regs.SP + value);

            cpu.regs.ClearFlag(Flags.Z);
            cpu.regs.ClearFlag(Flags.N);

            if (new_sp_value > 0xFFFF) cpu.regs.SetFlag(Flags.C);
            else cpu.regs.ClearFlag(Flags.C);

            if ((((uint)cpu.regs.SP & 0x0FFF) + (value & 0x0FFF) + cpu.regs.GetFlag(Flags.C)) > 0x0FFF) cpu.regs.SetFlag(Flags.H);
            else cpu.regs.ClearFlag(Flags.H);

            cpu.regs.SP = (ushort)new_sp_value;

            cpu.regs.IncreasePC(2);
            return 16;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            byte value = memorymap.GetByte((ushort)(address + 1));
            n_bytes = 2;
            builder.AppendFormat("add sp,0x{0:X2}", value);
            return builder.ToString();
        }
    }


}
