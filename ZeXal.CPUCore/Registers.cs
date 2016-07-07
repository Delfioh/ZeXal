using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore
{
    public class Registers
    {
        public byte A;
        public byte F;
        public byte B;
        public byte C;
        public byte D;
        public byte E;
        public byte H;
        public byte L;

        public ushort SP;

        private ushort _PC;
        public bool skip_next_pc_increase;

        //public byte flags;

        public Registers()
        {
            A = 0x00;
            F = 0x00;
            B = 0x00;
            C = 0x00;
            D = 0x00;
            E = 0x00;
            H = 0x00;
            L = 0x00;
            SP = 0x0000;
            _PC = 0x0000;
            //flags = 0x00;
            skip_next_pc_increase = false; //Implemented because of freaking HALT Instruction
        }

        public byte flags
        {
            get
            {
                return F;
            }

            set
            {
                F = value;
            }
        }

        //Implemented because of freaking HALT Instruction
        public ushort PC
        {
            get
            {
                return _PC;
            }
        }
        public void IncreasePC(ushort value)
        {
            if (!skip_next_pc_increase) _PC += value;
            else skip_next_pc_increase = !skip_next_pc_increase;
        }
        public void SetPC(ushort value)
        {
            _PC = value;
        }


        public ushort AF
        {
            get
            {
                return Get16BitRegister(A, F);
            }

            set
            {
                Set16BitRegister(ref A, ref F, value);
            }
        }

        public ushort BC
        {
            get
            {
                return Get16BitRegister(B, C);
            }

            set
            {
                Set16BitRegister(ref B, ref C, value);
            }
        }

        public ushort DE
        {
            get
            {
                return Get16BitRegister(D, E);
            }

            set
            {
                Set16BitRegister(ref D, ref E, value);
            }
        }

        public ushort HL
        {
            get
            {
                return Get16BitRegister(H, L);
            }

            set
            {
                Set16BitRegister(ref H, ref L, value);
            }
        }

        private ushort Get16BitRegister(byte reg_high, byte reg_low)
        {
            return (ushort)((reg_high << 8) + reg_low);
        }

        private void Set16BitRegister(ref byte reg_high, ref byte reg_low, ushort value)
        {
            reg_high = (byte)(value >> 8);
            reg_low = (byte)(value & 0x00FF);
        }

        public byte GetFlag(Flags flag_type)
        {
            byte ret_value = (byte)(flags >> (byte)flag_type);
            ret_value &= 0x01;
            return ret_value;
        }

        public void SetFlag(Flags flag_type)
        {
            byte mask = (byte)(0x01 << (byte)flag_type);            
            flags = (byte)(flags | mask);
        }

        public void ClearFlag(Flags flag_type)
        {
            byte mask = (byte)(0x01 << (byte)flag_type);
            mask ^= 0xFF;
            flags = (byte)(flags & mask);
        }

        public bool IsFlagSet(Flags flags_type)
        {
            return GetFlag(flags_type) == 0x01;
        }
    }

    public enum Flags
    {
        Z = 7,
        N = 6,
        H = 5,
        C = 4
    }
}
