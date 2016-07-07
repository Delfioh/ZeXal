using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //CCF
    //Description:
    //Complement carry flag
    //
    //Flags affected:
    //Z - Not affected.
    //N - Reset.
    //H - Reset.
    //C - Complemented.

    //0x3F
    class CCF : Opcode
    {
        public override int Execute(CPU cpu)
        {
            if (cpu.regs.IsFlagSet(Flags.C)) cpu.regs.ClearFlag(Flags.C);
            else cpu.regs.SetFlag(Flags.C);
            cpu.regs.ClearFlag(Flags.N);
            cpu.regs.ClearFlag(Flags.H);
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ccf";
        }
    }
    
}
