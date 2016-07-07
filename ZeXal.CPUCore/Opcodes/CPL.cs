using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //CPL
    //Description:
    //Complement A register. (Flip all bits.)
    //
    //Flags affected:
    //Z - Not affected.
    //N - Set.
    //H - Set.
    //C - Not affected.

    //0x2F
    class CPL : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = (byte)(~cpu.regs.A);
            cpu.regs.SetFlag(Flags.N);
            cpu.regs.SetFlag(Flags.H);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "cpl";
        }

    }
    
}
