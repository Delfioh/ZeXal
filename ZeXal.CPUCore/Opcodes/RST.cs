using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //RST n
    //Description:
    //Push present address onto stack.
    //Jump to address $0000 + n.
    //Use with:
    //n = $00,$08,$10,$18,$20,$28,$30,$38
    
    //0xC7
    class RST_00H : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.SP -= 2;
            cpu.memorymap.SetUShort(cpu.regs.SP, cpu.regs.PC);
            cpu.regs.SetPC(0x0000);
            return 32;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rst 00h";
        }
    }

    //0xCF
    class RST_08H : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.SP -= 2;
            cpu.memorymap.SetUShort(cpu.regs.SP, cpu.regs.PC);
            cpu.regs.SetPC(0x0008);
            return 32;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rst 08h";
        }
    }

    //0xD7
    class RST_10H : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.SP -= 2;
            cpu.memorymap.SetUShort(cpu.regs.SP, cpu.regs.PC);
            cpu.regs.SetPC(0x0010);
            return 32;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rst 10h";
        }
    }

    //0xDF
    class RST_18H : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.SP -= 2;
            cpu.memorymap.SetUShort(cpu.regs.SP, cpu.regs.PC);
            cpu.regs.SetPC(0x0018);
            return 32;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rst 18h";
        }
    }

    //0xE7
    class RST_20H : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.SP -= 2;
            cpu.memorymap.SetUShort(cpu.regs.SP, cpu.regs.PC);
            cpu.regs.SetPC(0x0020);
            return 32;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rst 20h";
        }
    }

    //0xEF
    class RST_28H : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.SP -= 2;
            cpu.memorymap.SetUShort(cpu.regs.SP, cpu.regs.PC);
            cpu.regs.SetPC(0x0028);
            return 32;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rst 28h";
        }
    }

    //0xF7
    class RST_30H : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.SP -= 2;
            cpu.memorymap.SetUShort(cpu.regs.SP, cpu.regs.PC);
            cpu.regs.SetPC(0x0030);
            return 32;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rst 30h";
        }
    }

    //0xFF
    class RST_38H : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.SP -= 2;
            cpu.memorymap.SetUShort(cpu.regs.SP, cpu.regs.PC);
            cpu.regs.SetPC(0x0038);
            return 32;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "rst 38h";
        }
    }
}
