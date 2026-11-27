using System;
using ZeXal.CPUCore;

namespace ZeXal.DMG
{
    //A DMG-001 system: CPU + PPU + timer + interrupts + memory map + cartridge.
    //The boot ROM is not emulated: the system starts at 0x0100 with post-boot register values.
    public class GameBoy
    {
        public CPU CPU { get; private set; }
        public PPU PPU { get; private set; }
        public HardwareTimer Timer { get; private set; }
        public InterruptController Interrupts { get; private set; }
        public Joypad Joypad { get; private set; }
        public DMGMemoryMap Memory { get; private set; }

        public long TotalCycles { get; private set; }

        public GameBoy(Cartridge cartridge)
        {
            Interrupts = new InterruptController();
            PPU = new PPU(Interrupts);
            Timer = new HardwareTimer(Interrupts);
            Joypad = new Joypad(Interrupts);
            Memory = new DMGMemoryMap(Interrupts, PPU, Timer, Joypad);
            Memory.Cartridge = cartridge;

            CPU = new CPU(Memory, Interrupts, 0x0100);
            TotalCycles = 0;
        }

        public GameBoy(string rom_filename)
            : this(Cartridge.FromFile(rom_filename))
        {
        }

        public Cartridge Cartridge
        {
            get { return Memory.Cartridge; }
        }

        //Executes one instruction (or one halted step) and advances the rest of the hardware
        //by the same number of T-cycles. Returns the cycles elapsed.
        public int Step()
        {
            int cycles = CPU.ExecuteNextOpcode();
            cycles += Interrupts.TakeDispatchCycles();

            Timer.Tick(cycles);
            PPU.Tick(cycles);

            TotalCycles += cycles;
            return cycles;
        }

        //Runs for one frame worth of cycles (70224), also when the LCD is off
        public void RunFrame()
        {
            long target = TotalCycles + PPU.CyclesPerFrame;
            while (TotalCycles < target) Step();
        }
    }
}
