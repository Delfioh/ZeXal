using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //0x10
    //STOP
    //Description:
    //Halt CPU & LCD display until button pressed.


    class STOP : Opcode
    {
        public override int Execute(CPU cpu)
        {
            //CURRENTLY UNIMPLEMENTED
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "stop";
        }
    }
}
