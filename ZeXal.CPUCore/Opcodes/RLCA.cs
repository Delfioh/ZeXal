using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //RLCA
    //Description:
    //Rotate A left. Old bit 7 to Carry flag.
    //
    //Flags affected:
    //Z - Set if result is zero.
    //N - Reset.
    //H - Reset.
    //C - Contains old bit 7 data.

    //0x07
    class RLCA : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.ClearFlag(Flags.N);
            cpu.regs.ClearFlag(Flags.H);

            byte rot_bit = (byte)((cpu.regs.A & 0x80) >> 7);

            if (rot_bit == 0x01) cpu.regs.SetFlag(Flags.C);
            else cpu.regs.ClearFlag(Flags.C);

            cpu.regs.A = (byte)((cpu.regs.A << 1) | rot_bit);

            if (cpu.regs.A == 0) cpu.regs.SetFlag(Flags.Z);
            else cpu.regs.ClearFlag(Flags.Z);

            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rlca";
        }
    }
    
}
