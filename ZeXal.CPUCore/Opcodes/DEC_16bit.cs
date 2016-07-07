using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{

    //DEC A,n
    //Description:
    //Decrement register nn.
    //Use with:
    //n = BC,DE,HL,SP
    //Flags affected:
    //None.

    //0x0B
    class DEC_BC : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.BC--;
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "dec bc";
        }

    }

    //0x1B
    class DEC_DE : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.DE--;
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "dec de";
        }
    }

    //0x2B
    class DEC_HL : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.HL--;
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "dec hl";
        }
    }

    //0x3B
    class DEC_SP : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.SP--;
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "dec sp";
        }
    }
}
