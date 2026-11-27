using System;
using System.IO;
using System.Text;

namespace ZeXal.DMG
{
    public enum MBCType
    {
        None,
        MBC1,
        MBC3,
        MBC5
    }

    //Game Pak: ROM, optional external RAM and memory bank controller.
    //Supported: ROM only, MBC1, MBC3 (without RTC), MBC5.
    public class Cartridge
    {
        private readonly byte[] rom;
        private readonly byte[] ram;
        private readonly MBCType mbc;

        private bool ram_enabled;
        private int rom_bank_low;  //MBC1: 5 bits, MBC3: 7 bits, MBC5: 9 bits
        private int bank_high;     //MBC1: 2 bit register, MBC3/MBC5: RAM bank
        private int banking_mode;  //MBC1 only

        public string Title { get; private set; }
        public byte CartridgeType { get; private set; }

        public MBCType MBC
        {
            get { return mbc; }
        }

        public bool HasRAM
        {
            get { return ram.Length > 0; }
        }

        public Cartridge(byte[] data)
        {
            if (data.Length < 0x8000)
            {
                //Pad small images (e.g. hand written test programs) to 32KB
                byte[] padded = new byte[0x8000];
                for (int i = 0; i < padded.Length; i++) padded[i] = 0xFF;
                Array.Copy(data, padded, data.Length);
                data = padded;
            }
            rom = data;

            CartridgeType = rom[0x0147];
            Title = ReadTitle();

            switch (CartridgeType)
            {
                case 0x00: case 0x08: case 0x09: mbc = MBCType.None; break;
                case 0x01: case 0x02: case 0x03: mbc = MBCType.MBC1; break;
                case 0x0F: case 0x10: case 0x11: case 0x12: case 0x13: mbc = MBCType.MBC3; break;
                case 0x19: case 0x1A: case 0x1B: case 0x1C: case 0x1D: case 0x1E: mbc = MBCType.MBC5; break;
                default:
                    throw new NotSupportedException(String.Format("Unsupported cartridge type 0x{0:X2}", CartridgeType));
            }

            ram = new byte[RamSize(rom[0x0149])];
            ram_enabled = false;
            rom_bank_low = 1;
            bank_high = 0;
            banking_mode = 0;
        }

        public static Cartridge FromFile(string filename)
        {
            return new Cartridge(File.ReadAllBytes(filename));
        }

        private string ReadTitle()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0x0134; i <= 0x0143; i++)
            {
                if (rom[i] == 0 || rom[i] >= 0x80) break;
                builder.Append((char)rom[i]);
            }
            return builder.ToString();
        }

        private static int RamSize(byte code)
        {
            switch (code)
            {
                case 0x01: return 0x800;
                case 0x02: return 0x2000;
                case 0x03: return 0x8000;
                case 0x04: return 0x20000;
                case 0x05: return 0x10000;
                default: return 0;
            }
        }

        private int RomBankCount
        {
            get { return rom.Length / 0x4000; }
        }

        //0x0000-0x7FFF
        public byte ReadROM(ushort address)
        {
            int bank;
            if (address < 0x4000)
            {
                bank = (mbc == MBCType.MBC1 && banking_mode == 1) ? (bank_high << 5) : 0;
            }
            else
            {
                switch (mbc)
                {
                    case MBCType.MBC1: bank = (bank_high << 5) | rom_bank_low; break;
                    case MBCType.MBC3:
                    case MBCType.MBC5: bank = rom_bank_low; break;
                    default: bank = 1; break;
                }
            }

            bank %= RomBankCount;
            return rom[bank * 0x4000 + (address & 0x3FFF)];
        }

        //Writes to the ROM area are MBC register writes
        public void WriteROM(ushort address, byte value)
        {
            switch (mbc)
            {
                case MBCType.MBC1:
                    if (address < 0x2000) ram_enabled = (value & 0x0F) == 0x0A;
                    else if (address < 0x4000)
                    {
                        rom_bank_low = value & 0x1F;
                        if (rom_bank_low == 0) rom_bank_low = 1;
                    }
                    else if (address < 0x6000) bank_high = value & 0x03;
                    else banking_mode = value & 0x01;
                    break;

                case MBCType.MBC3:
                    if (address < 0x2000) ram_enabled = (value & 0x0F) == 0x0A;
                    else if (address < 0x4000)
                    {
                        rom_bank_low = value & 0x7F;
                        if (rom_bank_low == 0) rom_bank_low = 1;
                    }
                    else if (address < 0x6000) bank_high = value; //0x00-0x03 RAM, 0x08-0x0C RTC (not emulated)
                    break;

                case MBCType.MBC5:
                    if (address < 0x2000) ram_enabled = (value & 0x0F) == 0x0A;
                    else if (address < 0x3000) rom_bank_low = (rom_bank_low & 0x100) | value;
                    else if (address < 0x4000) rom_bank_low = (rom_bank_low & 0xFF) | ((value & 0x01) << 8);
                    else if (address < 0x6000) bank_high = value & 0x0F;
                    break;
            }
        }

        private int RamOffset(ushort address)
        {
            if (ram.Length == 0 || !ram_enabled) return -1;

            int bank;
            switch (mbc)
            {
                case MBCType.MBC1: bank = banking_mode == 1 ? bank_high : 0; break;
                case MBCType.MBC3:
                    if (bank_high > 0x03) return -1; //RTC registers
                    bank = bank_high;
                    break;
                case MBCType.MBC5: bank = bank_high; break;
                default: bank = 0; break;
            }

            return (bank * 0x2000 + (address & 0x1FFF)) % ram.Length;
        }

        //0xA000-0xBFFF
        public byte ReadRAM(ushort address)
        {
            int offset = RamOffset(address);
            return offset < 0 ? (byte)0xFF : ram[offset];
        }

        public void WriteRAM(ushort address, byte value)
        {
            int offset = RamOffset(address);
            if (offset >= 0) ram[offset] = value;
        }

        //Direct access to external RAM (ignores the enable latch), for debugging/test harnesses
        public byte PeekRAM(int offset)
        {
            return offset < ram.Length ? ram[offset] : (byte)0xFF;
        }

        //Battery backed saves: copy of the external RAM, and restore from a previous copy
        public byte[] GetRAM()
        {
            return (byte[])ram.Clone();
        }

        public void LoadRAM(byte[] data)
        {
            if (data == null) return;
            Array.Copy(data, ram, Math.Min(data.Length, ram.Length));
        }

        //Header checksum (0x014D) and global checksum (0x014E-0x014F): identify a ROM for its save file
        public string Identifier
        {
            get { return String.Format("{0}_{1:X2}{2:X2}{3:X2}", Title.Length > 0 ? Title : "ROM", rom[0x014D], rom[0x014E], rom[0x014F]); }
        }
    }
}
