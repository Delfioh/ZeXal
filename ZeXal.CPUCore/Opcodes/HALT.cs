using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //0x76
    //HALT
    //One of the opcodes with the weirdest behaviour and side effects on original GB

    class HALT : Opcode
    {
        public override int Execute(CPU cpu)
        {
            //The CPU stops until an interrupt is pending (IE & IF != 0).
            //The "HALT bug" (IME=0 with a pending interrupt) is not emulated.
            cpu.halted = true;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "halt";
        }
    }
}
