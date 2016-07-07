using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{

    //PUSH nn
    //Push register pair nn onto stack.
    //Decrement Stack Pointer (SP) twice.

    //0xF5
    class PUSH_AF : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.SP -= 2;
            cpu.memorymap.SetUShort(cpu.regs.SP, cpu.regs.AF);

            cpu.regs.IncreasePC(1);
            return 16;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "push af";
        }
    }

    //0xC5
    class PUSH_BC : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.SP -= 2;
            cpu.memorymap.SetUShort(cpu.regs.SP, cpu.regs.BC);

            cpu.regs.IncreasePC(1);
            return 16;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "push bc";
        }
    }

    //0xD5
    class PUSH_DE : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.SP -= 2;
            cpu.memorymap.SetUShort(cpu.regs.SP, cpu.regs.DE);

            cpu.regs.IncreasePC(1);
            return 16;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "push de";
        }
    }

    //0xE5
    class PUSH_HL : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.SP -= 2;
            cpu.memorymap.SetUShort(cpu.regs.SP, cpu.regs.HL);

            cpu.regs.IncreasePC(1);
            return 16;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "push hl";
        }
    }
}
