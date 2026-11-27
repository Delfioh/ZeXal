using System;
using ZeXal.CPUCore;

namespace ZeXal.DMG
{
    //Interrupt sources, in priority order (bit number in IE/IF)
    public enum Interrupt
    {
        VBlank = 0,
        LcdStat = 1,
        Timer = 2,
        Serial = 3,
        Joypad = 4
    }

    //IME + IE (0xFFFF) + IF (0xFF0F), and interrupt dispatch.
    public class InterruptController : IIRHandler
    {
        private bool ime;
        private byte ie;
        private byte if_reg;
        private int dispatch_cycles;

        public bool IME
        {
            get { return ime; }
        }

        //0xFFFF
        public byte IE
        {
            get { return ie; }
            set { ie = value; }
        }

        //0xFF0F: only the lower 5 bits exist, the upper ones read as 1
        public byte IF
        {
            get { return (byte)(if_reg | 0xE0); }
            set { if_reg = (byte)(value & 0x1F); }
        }

        public InterruptController()
        {
            ime = false;
            ie = 0x00;
            if_reg = 0x01; //Post boot ROM value (VBlank pending)
        }

        public void Request(Interrupt interrupt)
        {
            if_reg |= (byte)(1 << (int)interrupt);
        }

        public void IR_Enable()
        {
            ime = true;
        }

        public void IR_Disable()
        {
            ime = false;
        }

        public void HandleIR(CPU cpu)
        {
            byte pending = (byte)(ie & if_reg & 0x1F);
            if (pending == 0) return;

            //Any pending interrupt wakes the CPU from HALT, even with IME=0
            cpu.halted = false;

            if (!ime) return;

            int bit = 0;
            while (((pending >> bit) & 0x01) == 0) bit++;

            if_reg &= (byte)~(1 << bit);
            ime = false;

            cpu.regs.SP -= 2;
            cpu.memorymap.SetUShort(cpu.regs.SP, cpu.regs.PC);
            cpu.regs.SetPC((ushort)(0x0040 + bit * 8));

            dispatch_cycles += 20;
        }

        //Cycles spent dispatching interrupts since the last call
        public int TakeDispatchCycles()
        {
            int c = dispatch_cycles;
            dispatch_cycles = 0;
            return c;
        }
    }
}
