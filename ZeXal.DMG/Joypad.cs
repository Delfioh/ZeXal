using System;

namespace ZeXal.DMG
{
    [Flags]
    public enum Button
    {
        None = 0,
        Right = 1 << 0,
        Left = 1 << 1,
        Up = 1 << 2,
        Down = 1 << 3,
        A = 1 << 4,
        B = 1 << 5,
        Select = 1 << 6,
        Start = 1 << 7
    }

    //P1/JOYP (0xFF00). Bits are active low.
    public class Joypad
    {
        private readonly InterruptController irq;
        private byte select;    //bits 4-5 as written by the CPU
        private Button pressed;

        public Joypad(InterruptController irq)
        {
            this.irq = irq;
            select = 0x30;
            pressed = Button.None;
        }

        public byte P1
        {
            get
            {
                int lines = 0x0F;
                int p = (int)pressed;
                if ((select & 0x10) == 0) lines &= ~(p & 0x0F);         //Direction keys
                if ((select & 0x20) == 0) lines &= ~((p >> 4) & 0x0F);  //Action keys
                return (byte)(0xC0 | select | lines);
            }
            set { select = (byte)(value & 0x30); }
        }

        public void Press(Button button)
        {
            if ((pressed & button) != button) irq.Request(Interrupt.Joypad);
            pressed |= button;
        }

        public void Release(Button button)
        {
            pressed &= ~button;
        }
    }
}
