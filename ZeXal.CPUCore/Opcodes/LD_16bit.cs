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
            builder.AppendFormat("lb bc,0x{0:X4}", value);
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
            builder.AppendFormat("lb de,0x{0:X4}", value);
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
            builder.AppendFormat("lb bc,0x{0:X4}", value);
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
            builder.AppendFormat("lb sp,0x{0:X4}", value);
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

            if (((uint)cpu.regs.SP + val) > 0xFFFF) cpu.regs.SetFlag(Flags.C);
            else cpu.regs.ClearFlag(Flags.C);

            if ((((uint)cpu.regs.SP & 0x0FFF) + (val & 0x0FFF) + cpu.regs.GetFlag(Flags.C)) > 0x0FFF) cpu.regs.SetFlag(Flags.H);
            else cpu.regs.ClearFlag(Flags.H);

            cpu.regs.ClearFlag(Flags.Z);
            cpu.regs.ClearFlag(Flags.N);

            cpu.regs.HL = (ushort)(cpu.regs.SP + val);

            cpu.regs.IncreasePC(2);
            return 12;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            byte value = memorymap.GetByte((ushort)(address + 1));
            n_bytes = 2;
            builder.AppendFormat("lb hl,sp+0x{0:X2}", value);
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
