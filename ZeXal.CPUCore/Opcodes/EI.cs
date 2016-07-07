using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //EI
    //Description:
    //This instruction enables interrupts but not
    //immediately. Interrupts are enabled after
    //instruction after EI is executed.
    //
    //Flags affected:
    //None.

    //0xFB
    class EI : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.IR_Enable();
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ei";
        }
    }
    
}
