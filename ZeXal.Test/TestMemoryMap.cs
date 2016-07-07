using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ZeXal.CPUCore;

namespace ZeXal.Test
{
    class TestMemoryMap : IMemoryMapper
    {
        private byte[] memory;

        public TestMemoryMap()
        {
            memory = new byte[0xFFFF];
        }

        public byte GetByte(ushort address)
        {
            return memory[address];
        }

        public void SetByte(ushort address, byte value)
        {
            memory[address] = value;
        }

        public byte[] GetBytes(ushort address, ushort count)
        {
            byte[] b = new byte[count];
            for (uint i = 0; i < count; i++)
            {
                b[i] = GetByte((ushort)(address + i));
            }
            return b;
        }

        public ushort GetUShort(ushort address)
        {
            byte high = GetByte((ushort)(address + 1));
            byte low = GetByte((ushort)address);
            return (ushort)((high << 8) + low);
        }

        public void SetUShort(ushort address, ushort value)
        {
            byte high = (byte)(value >> 8);
            byte low = (byte)(value & 0x00FF);

            SetByte(address, low);
            SetByte((ushort)(address + 1), high);
        }

        public void LoadExecutable(string filename)
        {
            FileStream rom_file = File.OpenRead(filename);
            FileInfo info = new FileInfo(filename);

            if (info.Length <= 0x7FFF)
            {
                rom_file.Read(memory, 0x0000, (ushort)info.Length);
                rom_file.Close();
                rom_file.Dispose();
            }
        }
    }
}
