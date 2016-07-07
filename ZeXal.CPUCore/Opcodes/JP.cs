using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //JP
    //Description:
    //Jump to address nn.
    //
    //use with:
    //nn = two byte immediate value. (LS byte first.)

    //0xC3
    class JP : Opcode
    {
        public override int Execute(CPU cpu)
        {
            ushort jump_address = cpu.memorymap.GetUShort((ushort)(cpu.regs.PC + 1));
            cpu.regs.SetPC(jump_address);
            return 12;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            ushort jump_address = memorymap.GetUShort((ushort)(address + 1));
            n_bytes = 3;
            builder.AppendFormat("jp 0x{0:X4}", jump_address);
            return builder.ToString();
        }
    }

    //JP CC
    //Description:
    //Jump to address nn if following condition is true:
    //cc = NZ, Jump if Z flag is reset.
    //cc = Z, Jump if Z flag is set.
    //cc = NC, Jump if C flag is reset.
    //cc = C, Jump if C flag is set.
    //
    //use with:
    //nn = two byte immediate value. (LS byte first.)

    //0xC2
    class JP_NZ : Opcode
    {
        public override int Execute(CPU cpu)
        {
            ushort jump_address = cpu.memorymap.GetUShort((ushort)(cpu.regs.PC + 1));
            if (!cpu.regs.IsFlagSet(Flags.Z)) cpu.regs.SetPC(jump_address);
            else cpu.regs.IncreasePC(3);
            return 12;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            ushort jump_address = memorymap.GetUShort((ushort)(address + 1));
            n_bytes = 3;
            builder.AppendFormat("jp nz,0x{0:X4}", jump_address);
            return builder.ToString();
        }
    }

    //0xCA
    class JP_Z : Opcode
    {
        public override int Execute(CPU cpu)
        {
            ushort jump_address = cpu.memorymap.GetUShort((ushort)(cpu.regs.PC + 1));
            if (cpu.regs.IsFlagSet(Flags.Z)) cpu.regs.SetPC(jump_address);
            else cpu.regs.IncreasePC(3);
            return 12;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            ushort jump_address = memorymap.GetUShort((ushort)(address + 1));
            n_bytes = 3;
            builder.AppendFormat("jp z,0x{0:X4}", jump_address);
            return builder.ToString();
        }
    }

    //0xD2
    class JP_NC : Opcode
    {
        public override int Execute(CPU cpu)
        {
            ushort jump_address = cpu.memorymap.GetUShort((ushort)(cpu.regs.PC + 1));
            if (!cpu.regs.IsFlagSet(Flags.C)) cpu.regs.SetPC(jump_address);
            else cpu.regs.IncreasePC(3);
            return 12;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            ushort jump_address = memorymap.GetUShort((ushort)(address + 1));
            n_bytes = 3;
            builder.AppendFormat("jp nc,0x{0:X4}", jump_address);
            return builder.ToString();
        }

    }

    //0xDA
    class JP_C : Opcode
    {
        public override int Execute(CPU cpu)
        {
            ushort jump_address = cpu.memorymap.GetUShort((ushort)(cpu.regs.PC + 1));
            if (cpu.regs.IsFlagSet(Flags.C)) cpu.regs.SetPC(jump_address);
            else cpu.regs.IncreasePC(3);
            return 12;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            ushort jump_address = memorymap.GetUShort((ushort)(address + 1));
            n_bytes = 3;
            builder.AppendFormat("jp c,0x{0:X4}", jump_address);
            return builder.ToString();
        }
    }

    //JP (HL)
    //Description:
    //Jump to address contained in HL.
    //0xE9
    class JP_indexHL : Opcode
    {
        public override int Execute(CPU cpu)
        {
            ushort jump_address = cpu.regs.HL;
            cpu.regs.SetPC(jump_address);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "jp hl";
        }
    }

    //JR n
    //Description:
    //Add n to current address and jump to it.
    //use with:
    //n = one byte signed immediate value
    //0x18
    class JR_n : Opcode
    {
        public override int Execute(CPU cpu)
        {
            sbyte relative_jump = (sbyte)cpu.memorymap.GetByte((ushort)(cpu.regs.PC + 1));
            ushort jump_address = (ushort)(cpu.regs.PC + 2 + relative_jump);
            cpu.regs.SetPC(jump_address);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            sbyte relative_jump = (sbyte)memorymap.GetByte((ushort)(address + 1));
            ushort jump_address = (ushort)(address + 2 + relative_jump);
            n_bytes = 2;
            builder.AppendFormat("jr 0x{0:X4}", jump_address);
            return builder.ToString();
        }
    }


    //JR CC,n
    //Description:
    //If following condition is true then add n to current
    //address and jump to it:
    //cc = NZ, Jump if Z flag is reset.
    //cc = Z, Jump if Z flag is set.
    //cc = NC, Jump if C flag is reset.
    //cc = C, Jump if C flag is set.
    //
    //use with:
    //n = one byte signed immediate value

    //0x20
    class JR_NZn : Opcode
    {
        public override int Execute(CPU cpu)
        {
            sbyte relative_jump = (sbyte)cpu.memorymap.GetByte((ushort)(cpu.regs.PC + 1));
            ushort jump_address = (ushort)(cpu.regs.PC + 2 + relative_jump);
            if (!cpu.regs.IsFlagSet(Flags.Z)) cpu.regs.SetPC(jump_address);
            else cpu.regs.IncreasePC(2);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            sbyte relative_jump = (sbyte)memorymap.GetByte((ushort)(address + 1));
            ushort jump_address = (ushort)(address + 2 + relative_jump);
            n_bytes = 2;
            builder.AppendFormat("jr nz,0x{0:X4}", jump_address);
            return builder.ToString();
        }
    }

    //0x28
    class JR_Zn : Opcode
    {
        public override int Execute(CPU cpu)
        {
            sbyte relative_jump = (sbyte)cpu.memorymap.GetByte((ushort)(cpu.regs.PC + 1));
            ushort jump_address = (ushort)(cpu.regs.PC + 2 + relative_jump);
            if (cpu.regs.IsFlagSet(Flags.Z)) cpu.regs.SetPC(jump_address);
            else cpu.regs.IncreasePC(2);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            sbyte relative_jump = (sbyte)memorymap.GetByte((ushort)(address + 1));
            ushort jump_address = (ushort)(address + 2 + relative_jump);
            n_bytes = 2;
            builder.AppendFormat("jr z,0x{0:X4}", jump_address);
            return builder.ToString();
        }
    }

    //0x30
    class JR_NCn : Opcode
    {
        public override int Execute(CPU cpu)
        {
            sbyte relative_jump = (sbyte)cpu.memorymap.GetByte((ushort)(cpu.regs.PC + 1));
            ushort jump_address = (ushort)(cpu.regs.PC + 2 + relative_jump);
            if (!cpu.regs.IsFlagSet(Flags.C)) cpu.regs.SetPC(jump_address);
            else cpu.regs.IncreasePC(2);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            sbyte relative_jump = (sbyte)memorymap.GetByte((ushort)(address + 1));
            ushort jump_address = (ushort)(address + 2 + relative_jump);
            n_bytes = 2;
            builder.AppendFormat("jr nc,0x{0:X4}", jump_address);
            return builder.ToString();
        }
    }

    //0x38
    class JR_Cn : Opcode
    {
        public override int Execute(CPU cpu)
        {
            sbyte relative_jump = (sbyte)cpu.memorymap.GetByte((ushort)(cpu.regs.PC + 1));
            ushort jump_address = (ushort)(cpu.regs.PC + 2 + relative_jump);
            if (cpu.regs.IsFlagSet(Flags.C)) cpu.regs.SetPC(jump_address);
            else cpu.regs.IncreasePC(2);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            sbyte relative_jump = (sbyte)memorymap.GetByte((ushort)(address + 1));
            ushort jump_address = (ushort)(address + 2 + relative_jump);
            n_bytes = 2;
            builder.AppendFormat("jr c,0x{0:X4}", jump_address);
            return builder.ToString();
        }
    }
}
