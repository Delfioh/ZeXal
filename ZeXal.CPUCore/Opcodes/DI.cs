using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //DI
    //Description:
    //This instruction disables interrupts but not
    //immediately. Interrupts are disabled after
    //instruction after DI is executed.
    //
    //Flags affected:
    //None.

    //0xF3
    class DI : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.IR_Disable();
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "di";
        }
    }
    
}
