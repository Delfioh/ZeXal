using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeXal.CPUCore;

namespace ZeXal.Test
{
    class TestIRHandler : IIRHandler
    {
        bool ir_enabled = true;

        public void IR_Enable()
        {
            ir_enabled = true;
        }

        public void IR_Disable()
        {
            ir_enabled = false;
        }

        public void HandleIR(CPU cpu)
        {
            if (ir_enabled)
            {
                //DO SOMETHING
            }
        }
    }
}
