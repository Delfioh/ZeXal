using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //RET
    //Decrement Stack Pointer (SP) twice.
    //Pop two bytes from stack & jump to that address.
    //0xC9
    class RET : Opcode
    {
        public override int Execute(CPU cpu)
        {
            ushort jump_address = cpu.memorymap.GetUShort(cpu.regs.SP);
            cpu.regs.SP += 2;
            cpu.regs.SetPC(jump_address);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ret";
        }
    }


    //RET cc
    //Pop two bytes from stack & jump to that address
    //if following condition is true:
    //cc = NZ, Call if Z flag is reset.
    //cc = Z, Return if Z flag is set.
    //cc = NC, Call if C flag is reset.
    //cc = C, Call if C flag is set.

    //0xC0
    class RET_NZ : Opcode
    {
        public override int Execute(CPU cpu)
        {
            ushort jump_address = cpu.memorymap.GetUShort(cpu.regs.SP);
            if (!cpu.regs.IsFlagSet(Flags.Z))
            {
                cpu.regs.SP += 2;
                cpu.regs.SetPC(jump_address);
            }
            else
            {
                cpu.regs.IncreasePC(1);
            }
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ret nz";
        }
    }

    //0xC8
    class RET_Z : Opcode
    {
        public override int Execute(CPU cpu)
        {
            ushort jump_address = cpu.memorymap.GetUShort(cpu.regs.SP);
            if (cpu.regs.IsFlagSet(Flags.Z))
            {         
                cpu.regs.SP += 2;
                cpu.regs.SetPC(jump_address);
            }
            else
            {
                cpu.regs.IncreasePC(1);
            }
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ret cc";
        }
    }

    //0xD0
    class RET_NC : Opcode
    {
        public override int Execute(CPU cpu)
        {
            ushort jump_address = cpu.memorymap.GetUShort(cpu.regs.SP);
            if (!cpu.regs.IsFlagSet(Flags.C))
            {
                cpu.regs.SP += 2;
                cpu.regs.SetPC(jump_address);
            }
            else
            {
                cpu.regs.IncreasePC(1);
            }
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ret nc";
        }
    }

    //0xD8
    class RET_C : Opcode
    {
        public override int Execute(CPU cpu)
        {
            ushort jump_address = cpu.memorymap.GetUShort(cpu.regs.SP);
            if (cpu.regs.IsFlagSet(Flags.C))
            {
                cpu.regs.SP += 2;
                cpu.regs.SetPC(jump_address);
            }
            else
            {
                cpu.regs.IncreasePC(1);
            }
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ret c";
        }
    }


    //RETI
    //Decrement Stack Pointer (SP) twice.
    //Pop two bytes from stack & jump to that address.
    //then enable interrupts.
    //0xD9
    class RETI : Opcode
    {
        public override int Execute(CPU cpu)
        {
            ushort jump_address = cpu.memorymap.GetUShort(cpu.regs.SP);
            cpu.regs.SP += 2;
            cpu.regs.SetPC(jump_address);
            cpu.IR_Enable();
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "reti";
        }
    }
}
