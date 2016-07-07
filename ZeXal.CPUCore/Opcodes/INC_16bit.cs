using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{

    //INC A,n
    //Description:
    //Increment register nn.
    //Use with:
    //n = BC,DE,HL,SP
    //Flags affected:
    //None.

    //0x03
    class INC_BC : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.BC++;
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "inc bc";
        }
    }

    //0x13
    class INC_DE : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.DE++;
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "inc de";
        }
    }

    //0x23
    class INC_HL : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.HL++;
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "inc hl";
        }
    }

    //0x33
    class INC_SP : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.SP++;
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "inc sp";
        }
    }
}
