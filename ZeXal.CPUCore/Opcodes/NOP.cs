using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //0x00
    class NOP : Opcode
    {
        public override int Execute(CPU cpu)
        {
            //WOW IT'S FUCKING NOTHING
            cpu.regs.IncreasePC(1);
            return 4;   
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "nop";
        }
    }
}
