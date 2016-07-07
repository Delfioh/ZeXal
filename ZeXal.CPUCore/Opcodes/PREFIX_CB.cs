using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    class PREFIX_CB : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.prefix_cb = true;
            cpu.regs.IncreasePC(1);
            return 4;        
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "prefix CB";
        }
    }
}
