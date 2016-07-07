using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //POP nn
    //Pop two bytes off stack into register pair nn.
    //Increment Stack Pointer (SP) twice.

    //0xF1
    class POP_AF : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.AF = cpu.memorymap.GetUShort(cpu.regs.SP);
            cpu.regs.SP += 2;
            cpu.regs.IncreasePC(1);
            return 12;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "pop af";
        }
    }

    //0xC1
    class POP_BC : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.BC = cpu.memorymap.GetUShort(cpu.regs.SP);
            cpu.regs.SP += 2;
            cpu.regs.IncreasePC(1);
            return 12;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "pop bc";
        }
    }

    //0xD1
    class POP_DE : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.DE = cpu.memorymap.GetUShort(cpu.regs.SP);
            cpu.regs.SP += 2;
            cpu.regs.IncreasePC(1);
            return 12;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "pop de";
        }
    }

    //0xE1
    class POP_HL : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.HL = cpu.memorymap.GetUShort(cpu.regs.SP);
            cpu.regs.SP += 2;
            cpu.regs.IncreasePC(1);
            return 12;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "pop hl";
        }
    }
}
