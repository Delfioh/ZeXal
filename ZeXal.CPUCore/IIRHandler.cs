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
        //Called after every instruction. Implementations must clear cpu.halted
        //when an interrupt is pending, otherwise HALT never returns.
        void HandleIR(CPU cpu);
    }
    
}
