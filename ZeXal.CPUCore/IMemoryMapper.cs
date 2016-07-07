using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore
{
    public interface IMemoryMapper
    {
        byte GetByte(ushort address);

        void SetByte(ushort address, byte value);

        byte[] GetBytes(ushort address, ushort count);

        ushort GetUShort(ushort address);

        void SetUShort(ushort address, ushort value);

        void LoadExecutable(string filename);
    }
    
}
