using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //DAA
    //Description:
    //Decimal adjust register A.
    //This instruction adjusts register A so that the
    //correct representation of Binary Coded Decimal (BCD)
    //is obtained.
    //
    //Flags affected:
    //Z - Set if register A is zero.
    //N - Not affected..
    //H - Reset.
    //C - Set or reset according to operation..

    //0x27
    class DAA : Opcode
    {
        public override int Execute(CPU cpu)
        {
            int a = cpu.regs.A;

            if (cpu.regs.GetFlag(Flags.N) == 0x00)
            {
                if ((cpu.regs.GetFlag(Flags.H) != 0) || ((a & 0x0F) > 0x09)) a += 0x06;
                if ((cpu.regs.GetFlag(Flags.C) != 0) || (a > 0x9F)) a += 0x60;
            }
            else
            {
                if (cpu.regs.GetFlag(Flags.H) != 0) a = (a - 0x06) & 0xFF;
                if (cpu.regs.GetFlag(Flags.C) != 0) a -= 0x60;
            }

            cpu.regs.ClearFlag(Flags.H);

            if ((a & 0x0100) == 0x0100) cpu.regs.SetFlag(Flags.C);
            a &= 0xFF;
            if (a == 0) cpu.regs.SetFlag(Flags.Z);
            else cpu.regs.ClearFlag(Flags.Z);

            cpu.regs.A = (byte)a;

            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "daa";
        }
    }
    
}
