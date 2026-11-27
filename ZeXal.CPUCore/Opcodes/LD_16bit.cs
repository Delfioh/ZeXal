using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //0x01
    class LD_BCnn : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.BC = cpu.memorymap.GetUShort((ushort)(cpu.regs.PC + 1));
            cpu.regs.IncreasePC(3);
            return 12;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            ushort value = memorymap.GetUShort((ushort)(address + 1));
            n_bytes = 3;
            builder.AppendFormat("ld bc,0x{0:X4}", value);
            return builder.ToString();
        }
    }

    //0x11
    class LD_DEnn : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.DE = cpu.memorymap.GetUShort((ushort)(cpu.regs.PC + 1));
            cpu.regs.IncreasePC(3);
            return 12;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            ushort value = memorymap.GetUShort((ushort)(address + 1));
            n_bytes = 3;
            builder.AppendFormat("ld de,0x{0:X4}", value);
            return builder.ToString();
        }
    }

    //0x21
    class LD_HLnn : Opcode
    {
        public override int Execute(CPU cpu)           
        {          
            cpu.regs.HL = cpu.memorymap.GetUShort((ushort)(cpu.regs.PC + 1));
            cpu.regs.IncreasePC(3);
            return 12;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            ushort value = memorymap.GetUShort((ushort)(address + 1));
            n_bytes = 3;
            builder.AppendFormat("ld hl,0x{0:X4}", value);
            return builder.ToString();
        }
    }

    //0x31
    class LD_SPnn : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.SP = cpu.memorymap.GetUShort((ushort)(cpu.regs.PC + 1));
            cpu.regs.IncreasePC(3);
            return 12;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            ushort value = memorymap.GetUShort((ushort)(address + 1));
            n_bytes = 3;
            builder.AppendFormat("ld sp,0x{0:X4}", value);
            return builder.ToString();
        }
    }

    //0xF9
    class LD_SPHL : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.SP = cpu.regs.HL;
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld sp,hl";
        }
    }

    //0xF8
    class LD_HLSPn : Opcode
    {
        public override int Execute(CPU cpu)
        {
            byte val = cpu.memorymap.GetByte((ushort)(cpu.regs.PC + 1));
            ushort sp = cpu.regs.SP;

            cpu.regs.ClearFlag(Flags.Z);
            cpu.regs.ClearFlag(Flags.N);

            //H and C are computed on the unsigned low byte
            if (((sp & 0x0F) + (val & 0x0F)) > 0x0F) cpu.regs.SetFlag(Flags.H);
            else cpu.regs.ClearFlag(Flags.H);

            if (((sp & 0xFF) + val) > 0xFF) cpu.regs.SetFlag(Flags.C);
            else cpu.regs.ClearFlag(Flags.C);

            cpu.regs.HL = (ushort)(sp + (sbyte)val);

            cpu.regs.IncreasePC(2);
            return 12;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            sbyte value = (sbyte)memorymap.GetByte((ushort)(address + 1));
            n_bytes = 2;
            builder.AppendFormat("ld hl,sp{0:+0;-0}", value);
            return builder.ToString();
        }
        }

    //0x08
    class LD_indexnnSP : Opcode
    {
        public override int Execute(CPU cpu)
        {
            ushort address = cpu.memorymap.GetUShort((ushort)(cpu.regs.PC + 1));
            cpu.memorymap.SetUShort(address, cpu.regs.SP);

            cpu.regs.IncreasePC(3);
            return 20;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            ushort index = memorymap.GetUShort((ushort)(address + 1));
            n_bytes = 3;
            builder.AppendFormat("ld (0x{0:X4}),sp", index);
            return builder.ToString();
        }
    }
}
