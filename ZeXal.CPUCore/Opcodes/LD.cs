using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore.Opcodes
{
    //8-Bit Loads

    //
    //LD nn,n
    //Put value n into nn

    //0x06


    class LD_Bn : Opcode
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte((ushort)(cpu.regs.PC + 1));         
            cpu.regs.B = value;
            cpu.regs.IncreasePC(2);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            byte value = memorymap.GetByte((ushort)(address + 1));
            n_bytes = 2;
            builder.AppendFormat("ld b,0x{0:X2}", value);
            return builder.ToString();
        }
    }

    //0x0E
    class LD_Cn : Opcode
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte((ushort)(cpu.regs.PC + 1));
            cpu.regs.C = value;
            cpu.regs.IncreasePC(2);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            byte value = memorymap.GetByte((ushort)(address + 1));
            n_bytes = 2;
            builder.AppendFormat("ld c,0x{0:X2}", value);
            return builder.ToString();
        }
    }

    //0x16
    class LD_Dn : Opcode
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte((ushort)(cpu.regs.PC + 1));
            cpu.regs.D = cpu.memorymap.GetByte(value);
            cpu.regs.IncreasePC(2);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            byte value = memorymap.GetByte((ushort)(address + 1));
            n_bytes = 2;
            builder.AppendFormat("ld d,0x{0:X2}", value);
            return builder.ToString();
        }
    }

    //0x1E
    class LD_En : Opcode
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte((ushort)(cpu.regs.PC + 1));
            cpu.regs.E = cpu.memorymap.GetByte(value);
            cpu.regs.IncreasePC(2);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            byte value = memorymap.GetByte((ushort)(address + 1));
            n_bytes = 2;
            builder.AppendFormat("ld e,0x{0:X2}", value);
            return builder.ToString();
        }
    }

    //0x26
    class LD_Hn : Opcode
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte((ushort)(cpu.regs.PC + 1));
            cpu.regs.H = cpu.memorymap.GetByte(value);
            cpu.regs.IncreasePC(2);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            byte value = memorymap.GetByte((ushort)(address + 1));
            n_bytes = 2;
            builder.AppendFormat("ld h,0x{0:X2}", value);
            return builder.ToString();
        }
    }

    //0x2E
    class LD_Ln : Opcode
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte((ushort)(cpu.regs.PC + 1));
            cpu.regs.L = cpu.memorymap.GetByte(value);
            cpu.regs.IncreasePC(2);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            byte value = memorymap.GetByte((ushort)(address + 1));
            n_bytes = 2;
            builder.AppendFormat("ld l,0x{0:X2}", value);
            return builder.ToString();
        }
    }


    //LD r1,r2
    //Put value r2 into r1.
    //LD A,r2
    //0x7F
    class LD_AA : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld a,a";
        }
    }

    //0x78
    class LD_AB : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = cpu.regs.B;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld a,b";
        }
    }

    //0x79
    class LD_AC : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = cpu.regs.C;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld a,c";
        }
    }

    //0x7A
    class LD_AD : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = cpu.regs.D;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld a,d";
        }
    }

    //0x7B
    class LD_AE : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = cpu.regs.E;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld a,e";
        }
    }

    //0x7C
    class LD_AH : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = cpu.regs.H;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld a,h";
        }
    }

    //0x7D
    class LD_AL : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = cpu.regs.L;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld a,l";
        }
    }

    //0x78
    class LD_A_indexHL : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = cpu.memorymap.GetByte(cpu.regs.HL);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld a,(hl)";
        }
    }

    //LD B,r2
    //0x47
    class LD_BA : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.B = cpu.regs.A;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld b,a";
        }
    }

    //0x40
    class LD_BB : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld b,b";
        }
    }

    //0x41
    class LD_BC : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.B = cpu.regs.C;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld b,c";
        }
    }

    //0x42
    class LD_BD : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.B = cpu.regs.D;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld b,d";
        }
    }

    //0x43
    class LD_BE : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.B = cpu.regs.E;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld b,e";
        }
    }

    //0x44
    class LD_BH : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.B = cpu.regs.H;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld b,h";
        }
    }

    //0x45
    class LD_BL : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.B = cpu.regs.L;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld b,l";
        }
    }

    //0x46
    class LD_B_indexHL : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.B = cpu.memorymap.GetByte(cpu.regs.HL);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld b,(hl)";
        }
    }


    //LD C,r2
    //0x4F
    class LD_CA : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.C = cpu.regs.A;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld c,a";
        }
    }

    //0x48
    class LD_CB : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.C = cpu.regs.B;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld c,b";
        }
    }

    //0x49
    class LD_CC : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld c,c";
        }
    }

    //0x4A
    class LD_CD : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.C = cpu.regs.D;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld c,d";
        }
    }

    //0x4B
    class LD_CE : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.C = cpu.regs.E;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld c,e";
        }
    }

    //0x4C
    class LD_CH : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.C = cpu.regs.H;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld c,h";
        }
    }

    //0x4D
    class LD_CL : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.C = cpu.regs.L;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld c,l";
        }
    }

    //0x4E
    class LD_C_indexHL : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.C = cpu.memorymap.GetByte(cpu.regs.HL);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld c,(hl)";
        }
    }

    //LD D,r2
    //0x57
    class LD_DA : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.D = cpu.regs.A;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld d,a";
        }
    }

    //0x50
    class LD_DB : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.D = cpu.regs.B;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld d,b";
        }
    }

    //0x51
    class LD_DC : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.D = cpu.regs.C;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld d,c";
        }
    }

    //0x52
    class LD_DD : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld d,d";
        }
    }

    //0x53
    class LD_DE : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.D = cpu.regs.E;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld d,e";
        }
    }

    //0x54
    class LD_DH : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.D = cpu.regs.H;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld d,h";
        }
    }

    //0x55
    class LD_DL : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.D = cpu.regs.L;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld d,l";
        }
    }

    //0x56
    class LD_D_indexHL : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.D = cpu.memorymap.GetByte(cpu.regs.HL);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld d,(hl)";
        }
    }


    //LD E,r2
    //0x5F
    class LD_EA : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.E = cpu.regs.A;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld e,a";
        }
    }

    //0x58
    class LD_EB : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.E = cpu.regs.B;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld e,b";
        }
    }

    //0x59
    class LD_EC : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.E = cpu.regs.C;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld e,c";
        }
    }

    //0x5A
    class LD_ED : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.E = cpu.regs.D;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld e,d";
        }
    }

    //0x5B
    class LD_EE : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld e,e";
        }
    }

    //0x5C
    class LD_EH : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.E = cpu.regs.H;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld e,h";
        }
    }

    //0x5D
    class LD_EL : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.E = cpu.regs.L;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld e,l";
        }
    }

    //0x5E
    class LD_E_indexHL : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.E = cpu.memorymap.GetByte(cpu.regs.HL);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld e,(hl)";
        }
    }


    //LD H,r2
    //0x67
    class LD_HA : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.H = cpu.regs.A;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld h,a";
        }
    }

    //0x60
    class LD_HB : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.H = cpu.regs.B;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld h,b";
        }
    }

    //0x61
    class LD_HC : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.H = cpu.regs.C;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld h,c";
        }
    }

    //0x62
    class LD_HD : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.H = cpu.regs.D;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld h,d";
        }
    }

    //0x63
    class LD_HE : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.H = cpu.regs.E;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld h,e";
        }
    }

    //0x64
    class LD_HH : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld h,h";
        }
    }

    //0x65
    class LD_HL : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.H = cpu.regs.L;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld h,l";
        }
    }

    //0x66
    class LD_H_indexHL : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.H = cpu.memorymap.GetByte(cpu.regs.HL);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld h,(hl)";
        }
    }


    //LD L,r2
    //0x6F
    class LD_LA : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.L = cpu.regs.A;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld l,a";
        }
    }

    //0x68
    class LD_LB : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.L = cpu.regs.B;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld l,b";
        }
    }

    //0x69
    class LD_LC : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.L = cpu.regs.C;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld l,c";
        }
    }

    //0x6A
    class LD_LD : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.L = cpu.regs.D;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld l,d";
        }
    }

    //0x6B
    class LD_LE : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.L = cpu.regs.E;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld l,e";
        }
    }

    //0x6C
    class LD_LH : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.L = cpu.regs.H;
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld l,h";
        }
    }

    //0x6D
    class LD_LL : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.IncreasePC(1);
            return 4;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld l,l";
        }
    }

    //0x6E
    class LD_L_indexHL : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.L = cpu.memorymap.GetByte(cpu.regs.HL);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld l,(hl)";
        }
    }

    
    //LD (HL),r2
    //0x70
    class LD_indexHLB : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.memorymap.SetByte(cpu.regs.HL, cpu.regs.B);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld (hl),b";
        }
    }
    //0x71
    class LD_indexHLC : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.memorymap.SetByte(cpu.regs.HL, cpu.regs.C);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld (hl),c";
        }
    }
    //0x72
    class LD_indexHLD : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.memorymap.SetByte(cpu.regs.HL, cpu.regs.D);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld (hl),d";
        }
    }
    //0x73
    class LD_indexHLE : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.memorymap.SetByte(cpu.regs.HL, cpu.regs.E);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld (hl),e";
        }
    }
    //0x74
    class LD_indexHLH : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.memorymap.SetByte(cpu.regs.HL, cpu.regs.H);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld (hl),h";
        }
    }
    //0x75
    class LD_indexHLL : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.memorymap.SetByte(cpu.regs.HL, cpu.regs.L);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld (hl),l";
        }
    }
    //0x36
    class LD_indexHLn : Opcode
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte((ushort)(cpu.regs.PC + 1));
            cpu.memorymap.SetByte(cpu.regs.HL, value);
            cpu.regs.IncreasePC(2);
            return 12;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            byte value = memorymap.GetByte((ushort)(address + 1));
            n_bytes = 2;
            builder.AppendFormat("ld (hl),0x{0:X2}", value);
            return builder.ToString();
        }
    }


    //LD A,n
    //0x0A
    class LD_A_indexBC : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = cpu.memorymap.GetByte(cpu.regs.BC);
            cpu.regs.IncreasePC(2);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld a,(bc)";
        }
    }
    //0x1A
    class LD_A_indexDE : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = cpu.memorymap.GetByte(cpu.regs.DE);
            cpu.regs.IncreasePC(2);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld a,(de)";
        }
    }
    //0xFA
    class LD_A_indexnn : Opcode
    {
        public override int Execute(CPU cpu)
        {
            ushort address = cpu.memorymap.GetUShort((ushort)(cpu.regs.PC + 1));
            cpu.regs.A = cpu.memorymap.GetByte(address);
            cpu.regs.IncreasePC(3);
            return 16;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            ushort index = memorymap.GetUShort((ushort)(address + 1));
            n_bytes = 3;
            builder.AppendFormat("ld a,(0x{0:X4})", index);
            return builder.ToString();
        }

    }
    //0x3E
    class LD_An : Opcode
    {
        public override int Execute(CPU cpu)
        {
            byte value = cpu.memorymap.GetByte((ushort)(cpu.regs.PC + 1));
            cpu.regs.A = value;
            cpu.regs.IncreasePC(2);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            byte value = memorymap.GetByte((ushort)(address + 1));
            n_bytes = 2;
            builder.AppendFormat("ld a,0x{0:X2}", value);
            return builder.ToString();
        }
    }

    //0x02
    class LD_indexBCA : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.memorymap.SetByte(cpu.regs.BC, cpu.regs.A);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld (bc),a";
        }
    }

    //0x12
    class LD_indexDEA : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.memorymap.SetByte(cpu.regs.DE, cpu.regs.A);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld (de),a";
        }
    }

    //0x77
    class LD_indexHLA : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.memorymap.SetByte(cpu.regs.HL, cpu.regs.A);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld (hl),a";
        }
    }

    //0xEA
    class LD_indexnnA : Opcode
    {
        public override int Execute(CPU cpu)
        {
            ushort address = cpu.memorymap.GetUShort((ushort)(cpu.regs.PC + 1));
            cpu.memorymap.SetByte(address, cpu.regs.A);
            cpu.regs.IncreasePC(3);
            return 16;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            ushort index = memorymap.GetUShort((ushort)(address + 1));
            n_bytes = 3;
            builder.AppendFormat("ld (0x{0:X4}),a", index);
            return builder.ToString();
        }
    }

    //0xF2
    class LD_A_indexC : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = cpu.memorymap.GetByte((ushort)(0xFF00 + cpu.regs.C));
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld a,(0xFF00 + c)";
        }
    }

    //0xE2
    class LD_indexCA : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.memorymap.SetByte((ushort)(0xFF00 + cpu.regs.C), cpu.regs.A);
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ld (0xFF00 + c),a";
        }
    }

    //0x3A
    class LDD_A_indexHL : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = cpu.memorymap.GetByte(cpu.regs.HL);
            cpu.regs.HL--;
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ldd a,(hl)";
        }
    }

    //0x32
    class LDD_indexHLA : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.memorymap.SetByte(cpu.regs.HL, cpu.regs.A);
            cpu.regs.HL--;
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ldd (hl),a";
        }
    }

    //0x2A
    class LDI_A_indexHL : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.regs.A = cpu.memorymap.GetByte(cpu.regs.HL);
            cpu.regs.HL++;
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ldi a,(hl)";
        }
    }

    //0x22
    class LDI_indexHLA : Opcode
    {
        public override int Execute(CPU cpu)
        {
            cpu.memorymap.SetByte(cpu.regs.HL, cpu.regs.A);
            cpu.regs.HL++;
            cpu.regs.IncreasePC(1);
            return 8;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            n_bytes = 1;
            return "ldi (hl),a";
        }
    }

    //0xF0
    class LDH_A_indexn : Opcode
    {
        public override int Execute(CPU cpu)
        {
            byte rel_address = cpu.memorymap.GetByte((ushort)(cpu.regs.PC + 1));
            ushort address = (ushort)(0xFF00 + rel_address);
            cpu.regs.A = cpu.memorymap.GetByte(address);
            cpu.regs.IncreasePC(2);
            return 12;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            byte rel_address = memorymap.GetByte((ushort)(address + 1));
            ushort eff_address = (ushort)(0xFF00 + rel_address);
            n_bytes = 2;
            builder.AppendFormat("ldh a,(0x{0:X4})", eff_address);
            return builder.ToString();
        }
    }

    //0xE0
    class LDH_indexnA : Opcode
    {
        public override int Execute(CPU cpu)
        {
            byte rel_address = cpu.memorymap.GetByte((ushort)(cpu.regs.PC + 1));
            ushort address = (ushort)(0xFF00 + rel_address);
            cpu.memorymap.SetByte(address, cpu.regs.A);
            cpu.regs.IncreasePC(2);
            return 12;
        }

        public override string GetOpcodeData(IMemoryMapper memorymap, ushort address, out byte n_bytes)
        {
            StringBuilder builder = new StringBuilder();
            byte rel_address = memorymap.GetByte((ushort)(address + 1));
            ushort eff_address = (ushort)(0xFF00 + rel_address);
            n_bytes = 2;
            builder.AppendFormat("ldh (0x{0:X4}),a", eff_address);
            return builder.ToString();
        }
    }

}
