using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeXal.CPUCore
{
    public interface IIRHandler
    {
        void IR_Enable();
        void IR_Disable();
        void HandleIR(CPU cpu);
    }
    
}
