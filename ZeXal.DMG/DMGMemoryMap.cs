using System;
using ZeXal.CPUCore;

namespace ZeXal.DMG
{
    //Full DMG-001 address space:
    //0x0000-0x7FFF cartridge ROM      0xA000-0xBFFF cartridge RAM
    //0x8000-0x9FFF VRAM (PPU)         0xC000-0xDFFF WRAM, mirrored at 0xE000-0xFDFF
    //0xFE00-0xFE9F OAM (PPU)          0xFF00-0xFF7F I/O registers
    //0xFF80-0xFFFE HRAM               0xFFFF        IE
    public class DMGMemoryMap : IMemoryMapper
    {
        private readonly byte[] wram = new byte[0x2000];
        private readonly byte[] hram = new byte[0x7F];
        private readonly byte[] io = new byte[0x80]; //Backing store for registers not emulated (e.g. audio)

        private readonly InterruptController irq;
        private readonly PPU ppu;
        private readonly HardwareTimer timer;
        private readonly Joypad joypad;

        private Cartridge cartridge;
        private byte serial_data;
        private byte serial_control;

        //Raised for every byte sent through the serial port (link cable).
        //Test ROMs (e.g. Blargg's) print their results this way.
        public event Action<byte> SerialByteSent;

        public DMGMemoryMap(InterruptController irq, PPU ppu, HardwareTimer timer, Joypad joypad)
        {
            this.irq = irq;
            this.ppu = ppu;
            this.timer = timer;
            this.joypad = joypad;
            serial_data = 0x00;
            serial_control = 0x7E;
        }

        public Cartridge Cartridge
        {
            get { return cartridge; }
            set { cartridge = value; }
        }

        public void LoadExecutable(string filename)
        {
            cartridge = Cartridge.FromFile(filename);
        }

        public byte GetByte(ushort address)
        {
            if (address < 0x8000) return cartridge != null ? cartridge.ReadROM(address) : (byte)0xFF;
            if (address < 0xA000) return ppu.ReadVRAM(address);
            if (address < 0xC000) return cartridge != null ? cartridge.ReadRAM(address) : (byte)0xFF;
            if (address < 0xFE00) return wram[address & 0x1FFF];
            if (address < 0xFEA0) return ppu.ReadOAM(address);
            if (address < 0xFF00) return 0xFF; //Unusable area
            if (address < 0xFF80) return ReadIO(address);
            if (address < 0xFFFF) return hram[address - 0xFF80];
            return irq.IE;
        }

        public void SetByte(ushort address, byte value)
        {
            if (address < 0x8000) { if (cartridge != null) cartridge.WriteROM(address, value); }
            else if (address < 0xA000) ppu.WriteVRAM(address, value);
            else if (address < 0xC000) { if (cartridge != null) cartridge.WriteRAM(address, value); }
            else if (address < 0xFE00) wram[address & 0x1FFF] = value;
            else if (address < 0xFEA0) ppu.WriteOAM(address, value);
            else if (address < 0xFF00) { } //Unusable area
            else if (address < 0xFF80) WriteIO(address, value);
            else if (address < 0xFFFF) hram[address - 0xFF80] = value;
            else irq.IE = value;
        }

        private byte ReadIO(ushort address)
        {
            switch (address)
            {
                case 0xFF00: return joypad.P1;
                case 0xFF01: return serial_data;
                case 0xFF02: return (byte)(serial_control | 0x7E);
                case 0xFF04: return timer.DIV;
                case 0xFF05: return timer.TIMA;
                case 0xFF06: return timer.TMA;
                case 0xFF07: return timer.TAC;
                case 0xFF0F: return irq.IF;
                case 0xFF46: return io[0x46];
            }

            if (address >= 0xFF40 && address <= 0xFF4B) return ppu.ReadRegister(address);
            return io[address - 0xFF00];
        }

        private void WriteIO(ushort address, byte value)
        {
            switch (address)
            {
                case 0xFF00: joypad.P1 = value; return;
                case 0xFF01: serial_data = value; return;
                case 0xFF02: WriteSerialControl(value); return;
                case 0xFF04: timer.DIV = value; return;
                case 0xFF05: timer.TIMA = value; return;
                case 0xFF06: timer.TMA = value; return;
                case 0xFF07: timer.TAC = value; return;
                case 0xFF0F: irq.IF = value; return;
                case 0xFF46:
                    io[0x46] = value;
                    OAMDMA(value);
                    return;
            }

            if (address >= 0xFF40 && address <= 0xFF4B) ppu.WriteRegister(address, value);
            else io[address - 0xFF00] = value;
        }

        //No link cable connected: a transfer with internal clock completes immediately
        //(the received byte is 0xFF) and raises the serial interrupt.
        private void WriteSerialControl(byte value)
        {
            serial_control = (byte)(value & 0x81);
            if ((value & 0x81) == 0x81)
            {
                Action<byte> handler = SerialByteSent;
                if (handler != null) handler(serial_data);

                serial_data = 0xFF;
                serial_control &= 0x7F;
                irq.Request(Interrupt.Serial);
            }
        }

        //Copies 160 bytes from XX00 to OAM. Done instantly instead of over 160 M-cycles.
        private void OAMDMA(byte source_page)
        {
            ushort source = (ushort)(source_page << 8);
            for (int i = 0; i < 0xA0; i++)
                ppu.OAM[i] = GetByte((ushort)(source + i));
        }

        public byte[] GetBytes(ushort address, ushort count)
        {
            byte[] b = new byte[count];
            for (int i = 0; i < count; i++) b[i] = GetByte((ushort)(address + i));
            return b;
        }

        public ushort GetUShort(ushort address)
        {
            byte low = GetByte(address);
            byte high = GetByte((ushort)(address + 1));
            return (ushort)((high << 8) | low);
        }

        public void SetUShort(ushort address, ushort value)
        {
            SetByte(address, (byte)(value & 0x00FF));
            SetByte((ushort)(address + 1), (byte)(value >> 8));
        }
    }
}
