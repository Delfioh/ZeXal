using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //CALL nn
    //Decrement Stack Pointer (SP) twice.
    //Push address of next instruction onto stack and then
    //jump to address nn.
    //Use with:
    //nn = two byte immediate value. (LS byte first.)
    //0xCD
    class CALL : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.SP -= 2;
            cpu.memorymap.SetUShort(cpu.regs.SP, (ushort)(cpu.regs.PC + 3));
            ushort jump_address = cpu.memorymap.GetUShort((ushort)(cpu.regs.PC + 1));
            cpu.regs.SetPC(jump_address);
            return 12;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            ushort jump_address = memorymap.GetUShort((ushort)(address + 1));
            n_bytes = 3;
            builder.AppendFormat("call 0x{0:X4}", jump_address);
            return builder.ToString();
        }
    }


    //CALL nn
    //Decrement Stack Pointer (SP) twice and
    //push address of next instruction onto stack and then
    //jump to address nn if following condition is true:
    //cc = NZ, Call if Z flag is reset.
     //cc = Z, Call if Z flag is set.
    //cc = NC, Call if C flag is reset.
    //cc = C, Call if C flag is set.
    //Use with:
    //nn = two byte immediate value. (LS byte first.)

    //0xC4
    class CALL_NZ : Opcode
    {
        public override int Execute(CPU cpu)
        {
            if (!cpu.regs.IsFlagSet(Flags.Z))
            {
                cpu.regs.SP -= 2;
                cpu.memorymap.SetUShort(cpu.regs.SP, (ushort)(cpu.regs.PC + 3));
                ushort jump_address = cpu.memorymap.GetUShort((ushort)(cpu.regs.PC + 1));
                cpu.regs.SetPC(jump_address);
            }
            else
            {
                cpu.regs.IncreasePC(3);
            }
            return 12;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            ushort jump_address = memorymap.GetUShort((ushort)(address + 1));
            n_bytes = 3;
            builder.AppendFormat("call nz,0x{0:X4}", jump_address);
            return builder.ToString();
        }
    }

    //0xCC
    class CALL_Z : Opcode
    {
        public override int Execute(CPU cpu)
        {
            if (cpu.regs.IsFlagSet(Flags.Z))
            {
                cpu.regs.SP -= 2;
                cpu.memorymap.SetUShort(cpu.regs.SP, (ushort)(cpu.regs.PC + 3));
                ushort jump_address = cpu.memorymap.GetUShort((ushort)(cpu.regs.PC + 1));
                cpu.regs.SetPC(jump_address);
            }
            else
            {
                cpu.regs.IncreasePC(3);
            }
            return 12;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            ushort jump_address = memorymap.GetUShort((ushort)(address + 1));
            n_bytes = 3;
            builder.AppendFormat("call z,0x{0:X4}", jump_address);
            return builder.ToString();
        }
    }

    //0xD4
    class CALL_NC : Opcode
    {
        public override int Execute(CPU cpu)
        {
            if (!cpu.regs.IsFlagSet(Flags.C))
            {
                cpu.regs.SP -= 2;
                cpu.memorymap.SetUShort(cpu.regs.SP, (ushort)(cpu.regs.PC + 3));
                ushort jump_address = cpu.memorymap.GetUShort((ushort)(cpu.regs.PC + 1));
                cpu.regs.SetPC(jump_address);
            }
            else
            {
                cpu.regs.IncreasePC(3);
            }
            return 12;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            ushort jump_address = memorymap.GetUShort((ushort)(address + 1));
            n_bytes = 3;
            builder.AppendFormat("call nc,0x{0:X4}", jump_address);
            return builder.ToString();
        }
    }

    //0xDC
    class CALL_C : Opcode
    {
        public override int Execute(CPU cpu)
        {
            if (cpu.regs.IsFlagSet(Flags.C))
            {
                cpu.regs.SP -= 2;
                cpu.memorymap.SetUShort(cpu.regs.SP, (ushort)(cpu.regs.PC + 3));
                ushort jump_address = cpu.memorymap.GetUShort((ushort)(cpu.regs.PC + 1));
                cpu.regs.SetPC(jump_address);
            }
            else
            {
                cpu.regs.IncreasePC(3);
            }
            return 12;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            ushort jump_address = memorymap.GetUShort((ushort)(address + 1));
            n_bytes = 3;
            builder.AppendFormat("call c,0x{0:X4}", jump_address);
            return builder.ToString();
        }
    }
}
