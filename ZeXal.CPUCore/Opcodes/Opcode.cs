using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    class Opcode
    {
        public virtual int Execute(CPU cpu)
        {
            return 0;
        }

        public virtual string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes=0;
            return "invalid";
        }
    }
}
