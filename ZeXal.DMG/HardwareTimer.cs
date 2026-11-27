using System;

namespace ZeXal.DMG
{
    //DIV (0xFF04), TIMA (0xFF05), TMA (0xFF06), TAC (0xFF07)
    //DIV is the upper byte of a 16 bit counter incremented every T-cycle.
    //TIMA increments on the falling edge of the counter bit selected by TAC.
    public class HardwareTimer
    {
        private static readonly ushort[] tac_masks = { 1 << 9, 1 << 3, 1 << 5, 1 << 7 };

        private readonly InterruptController irq;

        private ushort counter;
        private byte tima;
        private byte tma;
        private byte tac;

        public HardwareTimer(InterruptController irq)
        {
            this.irq = irq;
            counter = 0xABCC; //DIV = 0xAB after the boot ROM
            tima = 0x00;
            tma = 0x00;
            tac = 0x00;
        }

        public byte DIV
        {
            get { return (byte)(counter >> 8); }
            set { SetCounter(0); }
        }

        public byte TIMA
        {
            get { return tima; }
            set { tima = value; }
        }

        public byte TMA
        {
            get { return tma; }
            set { tma = value; }
        }

        public byte TAC
        {
            get { return (byte)(tac | 0xF8); }
            set
            {
                bool before = SelectedBit();
                tac = (byte)(value & 0x07);
                if (before && !SelectedBit()) IncrementTIMA();
            }
        }

        public void Tick(int cycles)
        {
            for (int i = 0; i < cycles; i++) SetCounter((ushort)(counter + 1));
        }

        private void SetCounter(ushort value)
        {
            bool before = SelectedBit();
            counter = value;
            if (before && !SelectedBit()) IncrementTIMA();
        }

        private bool SelectedBit()
        {
            return (tac & 0x04) != 0 && (counter & tac_masks[tac & 0x03]) != 0;
        }

        private void IncrementTIMA()
        {
            if (tima == 0xFF)
            {
                tima = tma;
                irq.Request(Interrupt.Timer);
            }
            else tima++;
        }
    }
}
