using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeXal.CPUCore.Opcodes;

namespace ZeXal.CPUCore
{
    public class CPU
    {
        public Registers regs;

        private bool enable_ir_next_op;
        private bool disable_ir_next_op; 

        public bool prefix_cb;


        private IMemoryMapper _memorymap;
        public IMemoryMapper memorymap
        {
            get { return _memorymap; }
        }

        private IIRHandler _ir_handler;
        public IIRHandler ir_handler
        {
            get { return _ir_handler; }
        }

        private Dictionary<byte, Opcode> opcodes;
        private Dictionary<byte, Opcode> cb_opcodes;

        //0x0100 is the default PC address on GB
        public CPU(IMemoryMapper memoryMap, IIRHandler IR_handler, ushort PC_start_address = 0x0100)
        {
            prefix_cb = false;

            enable_ir_next_op = false;
            disable_ir_next_op = false;

            _memorymap = memoryMap;
            _ir_handler = IR_handler;

            regs = new Registers();

            InitOpcodes();
            InitRegisters(PC_start_address);
        }

        public int ExecuteNextOpcode()
        {
            if (enable_ir_next_op)
            {
                if (ir_handler != null) ir_handler.IR_Enable();
                enable_ir_next_op = false;
            }

            if (disable_ir_next_op)
            {
                if (ir_handler != null) ir_handler.IR_Disable();
                disable_ir_next_op = false;
            }

            byte opcode_id = _memorymap.GetByte((ushort)regs.PC);
            Opcode next_opcode;
            if (prefix_cb)
            {
                next_opcode = cb_opcodes[opcode_id];
                prefix_cb = false;
            }
            else next_opcode = opcodes[opcode_id];

            int cycles = next_opcode.Execute(this);            

            //INTERRUPT HANDLING
            if (ir_handler != null) ir_handler.HandleIR(this);
            
            return cycles;
        }

        public string GetOpcodeData(out byte n_byte)
        {
            return GetOpcodeData(regs.PC, out n_byte);
        }

        public string GetOpcodeData(ushort address, out byte n_byte)
        {
            try
            {
                bool is_cb_opcode = _memorymap.GetByte(address) == 0xCB;

                if (is_cb_opcode) address++;

                byte opcode_id = _memorymap.GetByte(address);
                Opcode next_opcode;

                if (prefix_cb)
                {
                    next_opcode = cb_opcodes[opcode_id];
                    prefix_cb = false;
                }
                else next_opcode = opcodes[opcode_id];

                string opcode_data = next_opcode.GetOpcodeData(this.memorymap, address, out n_byte);
                if (is_cb_opcode) n_byte++;
                return opcode_data;

            }
            catch
            {
                n_byte = 1;
                return "-";
            }
        }

        private void InitRegisters(ushort PC_start_address)
        {
            regs.SetPC(PC_start_address);
            regs.AF = 0x01B0;
            regs.BC = 0x0013;
            regs.DE = 0x00D8;
            regs.HL = 0x014D;
            regs.SP = 0xFFFE;
        }

        public void IR_Enable()
        {
            disable_ir_next_op = false;
            enable_ir_next_op = true;
        }

        public void IR_Disable()
        {
            enable_ir_next_op = false;
            disable_ir_next_op = true;
        }


        private void InitOpcodes()
        {
            opcodes = new Dictionary<byte, Opcode>();
            cb_opcodes = new Dictionary<byte, Opcode>();

            opcodes.Add(0xCB, new PREFIX_CB());
            opcodes.Add(0x06, new LD_Bn());
            opcodes.Add(0x0E, new LD_Cn());
            opcodes.Add(0x16, new LD_Dn());
            opcodes.Add(0x1E, new LD_En());
            opcodes.Add(0x26, new LD_Hn());
            opcodes.Add(0x2E, new LD_Ln());
            opcodes.Add(0x7F, new LD_AA());
            opcodes.Add(0x78, new LD_AB());
            opcodes.Add(0x79, new LD_AC());
            opcodes.Add(0x7A, new LD_AD());
            opcodes.Add(0x7B, new LD_AE());
            opcodes.Add(0x7C, new LD_AH());
            opcodes.Add(0x7D, new LD_AL());
            opcodes.Add(0x7E, new LD_A_indexHL());
            opcodes.Add(0x40, new LD_BB());
            opcodes.Add(0x41, new LD_BC());
            opcodes.Add(0x42, new LD_BD());
            opcodes.Add(0x43, new LD_BE());
            opcodes.Add(0x44, new LD_BH());
            opcodes.Add(0x45, new LD_BL());
            opcodes.Add(0x46, new LD_B_indexHL());
            opcodes.Add(0x48, new LD_CB());
            opcodes.Add(0x49, new LD_CC());
            opcodes.Add(0x4A, new LD_CD());
            opcodes.Add(0x4B, new LD_CE());
            opcodes.Add(0x4C, new LD_CH());
            opcodes.Add(0x4D, new LD_CL());
            opcodes.Add(0x4E, new LD_C_indexHL());
            opcodes.Add(0x50, new LD_DB());
            opcodes.Add(0x51, new LD_DC());
            opcodes.Add(0x52, new LD_DD());
            opcodes.Add(0x53, new LD_DE());
            opcodes.Add(0x54, new LD_DH());
            opcodes.Add(0x55, new LD_DL());
            opcodes.Add(0x56, new LD_D_indexHL());
            opcodes.Add(0x58, new LD_EB());
            opcodes.Add(0x59, new LD_EC());
            opcodes.Add(0x5A, new LD_ED());
            opcodes.Add(0x5B, new LD_EE());
            opcodes.Add(0x5C, new LD_EH());
            opcodes.Add(0x5D, new LD_EL());
            opcodes.Add(0x5E, new LD_E_indexHL());
            opcodes.Add(0x60, new LD_HB());
            opcodes.Add(0x61, new LD_HC());
            opcodes.Add(0x62, new LD_HD());
            opcodes.Add(0x63, new LD_HE());
            opcodes.Add(0x64, new LD_HH());
            opcodes.Add(0x65, new LD_HL());
            opcodes.Add(0x66, new LD_H_indexHL());
            opcodes.Add(0x68, new LD_LB());
            opcodes.Add(0x69, new LD_LC());
            opcodes.Add(0x6A, new LD_LD());
            opcodes.Add(0x6B, new LD_LE());
            opcodes.Add(0x6C, new LD_LH());
            opcodes.Add(0x6D, new LD_LL());
            opcodes.Add(0x6E, new LD_L_indexHL());
            opcodes.Add(0x70, new LD_indexHLB());
            opcodes.Add(0x71, new LD_indexHLC());
            opcodes.Add(0x72, new LD_indexHLD());
            opcodes.Add(0x73, new LD_indexHLE());
            opcodes.Add(0x74, new LD_indexHLH());
            opcodes.Add(0x75, new LD_indexHLL());
            opcodes.Add(0x36, new LD_indexHLn());
            opcodes.Add(0x0A, new LD_A_indexBC());
            opcodes.Add(0x1A, new LD_A_indexDE());
            opcodes.Add(0xFA, new LD_A_indexnn());
            opcodes.Add(0x3E, new LD_An());
            opcodes.Add(0x47, new LD_BA());
            opcodes.Add(0x4F, new LD_CA());
            opcodes.Add(0x57, new LD_DA());
            opcodes.Add(0x5F, new LD_EA());
            opcodes.Add(0x67, new LD_HA());
            opcodes.Add(0x6F, new LD_LA());
            opcodes.Add(0x02, new LD_indexBCA());
            opcodes.Add(0x12, new LD_indexDEA());
            opcodes.Add(0x77, new LD_indexHLA());
            opcodes.Add(0xEA, new LD_indexnnA());
            opcodes.Add(0xF2, new LD_A_indexC());
            opcodes.Add(0xE2, new LD_indexCA());
            opcodes.Add(0x3A, new LDD_A_indexHL());
            opcodes.Add(0x32, new LDD_indexHLA());
            opcodes.Add(0x2A, new LDI_A_indexHL());
            opcodes.Add(0x22, new LDI_indexHLA());
            opcodes.Add(0xE0, new LDH_indexnA());
            opcodes.Add(0xF0, new LDH_A_indexn());
            opcodes.Add(0x01, new LD_BCnn());
            opcodes.Add(0x11, new LD_DEnn());
            opcodes.Add(0x21, new LD_HLnn());
            opcodes.Add(0x31, new LD_SPnn());
            opcodes.Add(0xF9, new LD_SPHL());
            opcodes.Add(0xF8, new LD_HLSPn());
            opcodes.Add(0x08, new LD_indexnnSP());
            opcodes.Add(0xF5, new PUSH_AF());
            opcodes.Add(0xC5, new PUSH_BC());
            opcodes.Add(0xD5, new PUSH_DE());
            opcodes.Add(0xE5, new PUSH_HL());
            opcodes.Add(0xF1, new POP_AF());
            opcodes.Add(0xC1, new POP_BC());
            opcodes.Add(0xD1, new POP_DE());
            opcodes.Add(0xE1, new POP_HL());
            opcodes.Add(0x87, new ADD_AA());
            opcodes.Add(0x80, new ADD_AB());
            opcodes.Add(0x81, new ADD_AC());
            opcodes.Add(0x82, new ADD_AD());
            opcodes.Add(0x83, new ADD_AE());
            opcodes.Add(0x84, new ADD_AH());
            opcodes.Add(0x85, new ADD_AL());
            opcodes.Add(0x86, new ADD_AindexHL());
            opcodes.Add(0xC6, new ADD_An());
            opcodes.Add(0x8F, new ADC_AA());
            opcodes.Add(0x88, new ADC_AB());
            opcodes.Add(0x89, new ADC_AC());
            opcodes.Add(0x8A, new ADC_AD());
            opcodes.Add(0x8B, new ADC_AE());
            opcodes.Add(0x8C, new ADC_AH());
            opcodes.Add(0x8D, new ADC_AL());
            opcodes.Add(0x8E, new ADC_AindexHL());
            opcodes.Add(0xCE, new ADC_An());
            opcodes.Add(0x97, new SUB_AA());
            opcodes.Add(0x90, new SUB_AB());
            opcodes.Add(0x91, new SUB_AC());
            opcodes.Add(0x92, new SUB_AD());
            opcodes.Add(0x93, new SUB_AE());
            opcodes.Add(0x94, new SUB_AH());
            opcodes.Add(0x95, new SUB_AL());
            opcodes.Add(0x96, new SUB_AindexHL());
            opcodes.Add(0xD6, new SBC_An());
            opcodes.Add(0x9F, new SBC_AA());
            opcodes.Add(0x98, new SBC_AB());
            opcodes.Add(0x99, new SBC_AC());
            opcodes.Add(0x9A, new SBC_AD());
            opcodes.Add(0x9B, new SBC_AE());
            opcodes.Add(0x9C, new SBC_AH());
            opcodes.Add(0x9D, new SBC_AL());
            opcodes.Add(0x9E, new SBC_AindexHL());
            opcodes.Add(0xDE, new SBC_An());
            opcodes.Add(0xA7, new AND_AA());
            opcodes.Add(0xA0, new AND_AB());
            opcodes.Add(0xA1, new AND_AC());
            opcodes.Add(0xA2, new AND_AD());
            opcodes.Add(0xA3, new AND_AE());
            opcodes.Add(0xA4, new AND_AH());
            opcodes.Add(0xA5, new AND_AL());
            opcodes.Add(0xA6, new AND_AindexHL());
            opcodes.Add(0xE6, new AND_An());
            opcodes.Add(0xB7, new OR_AA());
            opcodes.Add(0xB0, new OR_AB());
            opcodes.Add(0xB1, new OR_AC());
            opcodes.Add(0xB2, new OR_AD());
            opcodes.Add(0xB3, new OR_AE());
            opcodes.Add(0xB4, new OR_AH());
            opcodes.Add(0xB5, new OR_AL());
            opcodes.Add(0xB6, new OR_AindexHL());
            opcodes.Add(0xF6, new OR_An());
            opcodes.Add(0xAF, new XOR_AA());
            opcodes.Add(0xA8, new XOR_AB());
            opcodes.Add(0xA9, new XOR_AC());
            opcodes.Add(0xAA, new XOR_AD());
            opcodes.Add(0xAB, new XOR_AE());
            opcodes.Add(0xAC, new XOR_AH());
            opcodes.Add(0xAD, new XOR_AL());
            opcodes.Add(0xAE, new XOR_AindexHL());
            opcodes.Add(0xEE, new XOR_An());
            opcodes.Add(0xBF, new CP_A());
            opcodes.Add(0xB8, new CP_B());
            opcodes.Add(0xB9, new CP_C());
            opcodes.Add(0xBA, new CP_D());
            opcodes.Add(0xBB, new CP_E());
            opcodes.Add(0xBC, new CP_H());
            opcodes.Add(0xBD, new CP_L());
            opcodes.Add(0xBE, new CP_indexHL());
            opcodes.Add(0xFE, new CP_n());
            opcodes.Add(0x3C, new INC_A());
            opcodes.Add(0x04, new INC_B());
            opcodes.Add(0x0C, new INC_C());
            opcodes.Add(0x14, new INC_D());
            opcodes.Add(0x1C, new INC_E());
            opcodes.Add(0x24, new INC_H());
            opcodes.Add(0x2C, new INC_L());
            opcodes.Add(0x34, new INC_indexHL());
            opcodes.Add(0x3D, new DEC_A());
            opcodes.Add(0x05, new DEC_B());
            opcodes.Add(0x0D, new DEC_C());
            opcodes.Add(0x15, new DEC_D());
            opcodes.Add(0x1D, new DEC_E());
            opcodes.Add(0x25, new DEC_H());
            opcodes.Add(0x2D, new DEC_L());
            opcodes.Add(0x35, new INC_indexHL());
            opcodes.Add(0x09, new ADD_HL_BC());
            opcodes.Add(0x19, new ADD_HL_DE());
            opcodes.Add(0x29, new ADD_HL_HL());
            opcodes.Add(0x39, new ADD_HL_SP());
            opcodes.Add(0xE8, new ADD_SPn());
            opcodes.Add(0x03, new INC_BC());
            opcodes.Add(0x13, new INC_DE());
            opcodes.Add(0x23, new INC_HL());
            opcodes.Add(0x33, new INC_SP());
            opcodes.Add(0x0B, new DEC_BC());
            opcodes.Add(0x1B, new DEC_DE());
            opcodes.Add(0x2B, new DEC_HL());
            opcodes.Add(0x3B, new DEC_SP());
            opcodes.Add(0x27, new DAA());
            opcodes.Add(0x2F, new CPL());
            opcodes.Add(0x3F, new CCF());
            opcodes.Add(0x37, new SCF());
            opcodes.Add(0x00, new NOP());
            opcodes.Add(0x76, new HALT());
            opcodes.Add(0x10, new STOP());
            opcodes.Add(0xF3, new DI());
            opcodes.Add(0xFB, new EI());
            opcodes.Add(0x07, new RLCA());
            opcodes.Add(0x17, new RLA());
            opcodes.Add(0x0F, new RRCA());
            opcodes.Add(0x1F, new RRA());
            opcodes.Add(0xC3, new JP());
            opcodes.Add(0xC2, new JP_NZ());
            opcodes.Add(0xCA, new JP_Z());
            opcodes.Add(0xD2, new JP_NC());
            opcodes.Add(0xDA, new JP_C());
            opcodes.Add(0xE9, new JP_indexHL());
            opcodes.Add(0x18, new JR_n());
            opcodes.Add(0x20, new JR_NZn());
            opcodes.Add(0x28, new JR_Zn());
            opcodes.Add(0x30, new JR_NCn());
            opcodes.Add(0x38, new JR_Cn());
            opcodes.Add(0xCD, new CALL());
            opcodes.Add(0xC4, new CALL_NZ());
            opcodes.Add(0xCC, new CALL_Z());
            opcodes.Add(0xD4, new CALL_NC());
            opcodes.Add(0xDC, new CALL_C());
            opcodes.Add(0xC7, new RST_00H());
            opcodes.Add(0xCF, new RST_08H());
            opcodes.Add(0xD7, new RST_10H());
            opcodes.Add(0xDF, new RST_18H());
            opcodes.Add(0xE7, new RST_20H());
            opcodes.Add(0xEF, new RST_28H());
            opcodes.Add(0xF7, new RST_30H());
            opcodes.Add(0xFF, new RST_38H());
            opcodes.Add(0xC9, new RET());
            opcodes.Add(0xC0, new RET_NZ());
            opcodes.Add(0xC8, new RET_Z());
            opcodes.Add(0xD0, new RET_NC());
            opcodes.Add(0xD8, new RET_C());
            opcodes.Add(0xD9, new RETI());

            cb_opcodes.Add(0x37, new SWAP_A());
            cb_opcodes.Add(0x30, new SWAP_B());
            cb_opcodes.Add(0x31, new SWAP_C());
            cb_opcodes.Add(0x32, new SWAP_D());
            cb_opcodes.Add(0x33, new SWAP_E());
            cb_opcodes.Add(0x34, new SWAP_H());
            cb_opcodes.Add(0x35, new SWAP_L());
            cb_opcodes.Add(0x36, new SWAP_indexHL());
            cb_opcodes.Add(0x07, new RLC_A());
            cb_opcodes.Add(0x00, new RLC_B());
            cb_opcodes.Add(0x01, new RLC_C());
            cb_opcodes.Add(0x02, new RLC_D());
            cb_opcodes.Add(0x03, new RLC_E());
            cb_opcodes.Add(0x04, new RLC_H());
            cb_opcodes.Add(0x05, new RLC_L());
            cb_opcodes.Add(0x06, new RLC_indexHL());
            cb_opcodes.Add(0x17, new RL_A());
            cb_opcodes.Add(0x10, new RL_B());
            cb_opcodes.Add(0x11, new RL_C());
            cb_opcodes.Add(0x12, new RL_D());
            cb_opcodes.Add(0x13, new RL_E());
            cb_opcodes.Add(0x14, new RL_H());
            cb_opcodes.Add(0x15, new RL_L());
            cb_opcodes.Add(0x16, new RL_indexHL());
            cb_opcodes.Add(0x0F, new RRC_A());
            cb_opcodes.Add(0x08, new RRC_B());
            cb_opcodes.Add(0x09, new RRC_C());
            cb_opcodes.Add(0x0A, new RRC_D());
            cb_opcodes.Add(0x0B, new RRC_E());
            cb_opcodes.Add(0x0C, new RRC_H());
            cb_opcodes.Add(0x0D, new RRC_L());
            cb_opcodes.Add(0x0E, new RRC_indexHL());
            cb_opcodes.Add(0x1F, new RR_A());
            cb_opcodes.Add(0x18, new RR_B());
            cb_opcodes.Add(0x19, new RR_C());
            cb_opcodes.Add(0x1A, new RR_D());
            cb_opcodes.Add(0x1B, new RR_E());
            cb_opcodes.Add(0x1C, new RR_H());
            cb_opcodes.Add(0x1D, new RR_L());
            cb_opcodes.Add(0x1E, new RR_indexHL());
            cb_opcodes.Add(0x27, new SLA_A());
            cb_opcodes.Add(0x20, new SLA_B());
            cb_opcodes.Add(0x21, new SLA_C());
            cb_opcodes.Add(0x22, new SLA_D());
            cb_opcodes.Add(0x23, new SLA_E());
            cb_opcodes.Add(0x24, new SLA_H());
            cb_opcodes.Add(0x25, new SLA_L());
            cb_opcodes.Add(0x26, new SLA_indexHL());
            cb_opcodes.Add(0x2F, new SRA_A());
            cb_opcodes.Add(0x28, new SRA_B());
            cb_opcodes.Add(0x29, new SRA_C());
            cb_opcodes.Add(0x2A, new SRA_D());
            cb_opcodes.Add(0x2B, new SRA_E());
            cb_opcodes.Add(0x2C, new SRA_H());
            cb_opcodes.Add(0x2D, new SRA_L());
            cb_opcodes.Add(0x2E, new SRA_indexHL());
            cb_opcodes.Add(0x3F, new SRL_A());
            cb_opcodes.Add(0x38, new SRL_B());
            cb_opcodes.Add(0x39, new SRL_C());
            cb_opcodes.Add(0x3A, new SRL_D());
            cb_opcodes.Add(0x3B, new SRL_E());
            cb_opcodes.Add(0x3C, new SRL_H());
            cb_opcodes.Add(0x3D, new SRL_L());
            cb_opcodes.Add(0x3E, new SRL_indexHL());

            //ALL BIT OPCODES
            cb_opcodes.Add(0x40, new BIT_B(0));
            cb_opcodes.Add(0x41, new BIT_C(0));
            cb_opcodes.Add(0x42, new BIT_D(0));
            cb_opcodes.Add(0x43, new BIT_E(0));
            cb_opcodes.Add(0x44, new BIT_H(0));
            cb_opcodes.Add(0x45, new BIT_L(0));
            cb_opcodes.Add(0x46, new BIT_indexHL(0));
            cb_opcodes.Add(0x47, new BIT_A(0));
            cb_opcodes.Add(0x48, new BIT_B(1));
            cb_opcodes.Add(0x49, new BIT_C(1));
            cb_opcodes.Add(0x4A, new BIT_D(1));
            cb_opcodes.Add(0x4B, new BIT_E(1));
            cb_opcodes.Add(0x4C, new BIT_H(1));
            cb_opcodes.Add(0x4D, new BIT_L(1));
            cb_opcodes.Add(0x4E, new BIT_indexHL(1));
            cb_opcodes.Add(0x4F, new BIT_A(1));
            cb_opcodes.Add(0x50, new BIT_B(2));
            cb_opcodes.Add(0x51, new BIT_C(2));
            cb_opcodes.Add(0x52, new BIT_D(2));
            cb_opcodes.Add(0x53, new BIT_E(2));
            cb_opcodes.Add(0x54, new BIT_H(2));
            cb_opcodes.Add(0x55, new BIT_L(2));
            cb_opcodes.Add(0x56, new BIT_indexHL(2));
            cb_opcodes.Add(0x57, new BIT_A(2));
            cb_opcodes.Add(0x58, new BIT_B(3));
            cb_opcodes.Add(0x59, new BIT_C(3));
            cb_opcodes.Add(0x5A, new BIT_D(3));
            cb_opcodes.Add(0x5B, new BIT_E(3));
            cb_opcodes.Add(0x5C, new BIT_H(3));
            cb_opcodes.Add(0x5D, new BIT_L(3));
            cb_opcodes.Add(0x5E, new BIT_indexHL(3));
            cb_opcodes.Add(0x5F, new BIT_A(3));
            cb_opcodes.Add(0x60, new BIT_B(4));
            cb_opcodes.Add(0x61, new BIT_C(4));
            cb_opcodes.Add(0x62, new BIT_D(4));
            cb_opcodes.Add(0x63, new BIT_E(4));
            cb_opcodes.Add(0x64, new BIT_H(4));
            cb_opcodes.Add(0x65, new BIT_L(4));
            cb_opcodes.Add(0x66, new BIT_indexHL(4));
            cb_opcodes.Add(0x67, new BIT_A(4));
            cb_opcodes.Add(0x68, new BIT_B(5));
            cb_opcodes.Add(0x69, new BIT_C(5));
            cb_opcodes.Add(0x6A, new BIT_D(5));
            cb_opcodes.Add(0x6B, new BIT_E(5));
            cb_opcodes.Add(0x6C, new BIT_H(5));
            cb_opcodes.Add(0x6D, new BIT_L(5));
            cb_opcodes.Add(0x6E, new BIT_indexHL(5));
            cb_opcodes.Add(0x6F, new BIT_A(5));
            cb_opcodes.Add(0x70, new BIT_B(6));
            cb_opcodes.Add(0x71, new BIT_C(6));
            cb_opcodes.Add(0x72, new BIT_D(6));
            cb_opcodes.Add(0x73, new BIT_E(6));
            cb_opcodes.Add(0x74, new BIT_H(6));
            cb_opcodes.Add(0x75, new BIT_L(6));
            cb_opcodes.Add(0x76, new BIT_indexHL(6));
            cb_opcodes.Add(0x77, new BIT_A(6));
            cb_opcodes.Add(0x78, new BIT_B(7));
            cb_opcodes.Add(0x79, new BIT_C(7));
            cb_opcodes.Add(0x7A, new BIT_D(7));
            cb_opcodes.Add(0x7B, new BIT_E(7));
            cb_opcodes.Add(0x7C, new BIT_H(7));
            cb_opcodes.Add(0x7D, new BIT_L(7));
            cb_opcodes.Add(0x7E, new BIT_indexHL(7));
            cb_opcodes.Add(0x7F, new BIT_A(7));

            //ALL RES OPCODES
            cb_opcodes.Add(0x80, new RES_B(0));
            cb_opcodes.Add(0x81, new RES_C(0));
            cb_opcodes.Add(0x82, new RES_D(0));
            cb_opcodes.Add(0x83, new RES_E(0));
            cb_opcodes.Add(0x84, new RES_H(0));
            cb_opcodes.Add(0x85, new RES_L(0));
            cb_opcodes.Add(0x86, new RES_indexHL(0));
            cb_opcodes.Add(0x87, new RES_A(0));
            cb_opcodes.Add(0x88, new RES_B(1));
            cb_opcodes.Add(0x89, new RES_C(1));
            cb_opcodes.Add(0x8A, new RES_D(1));
            cb_opcodes.Add(0x8B, new RES_E(1));
            cb_opcodes.Add(0x8C, new RES_H(1));
            cb_opcodes.Add(0x8D, new RES_L(1));
            cb_opcodes.Add(0x8E, new RES_indexHL(1));
            cb_opcodes.Add(0x8F, new RES_A(1));
            cb_opcodes.Add(0x90, new RES_B(2));
            cb_opcodes.Add(0x91, new RES_C(2));
            cb_opcodes.Add(0x92, new RES_D(2));
            cb_opcodes.Add(0x93, new RES_E(2));
            cb_opcodes.Add(0x94, new RES_H(2));
            cb_opcodes.Add(0x95, new RES_L(2));
            cb_opcodes.Add(0x96, new RES_indexHL(2));
            cb_opcodes.Add(0x97, new RES_A(2));
            cb_opcodes.Add(0x98, new RES_B(3));
            cb_opcodes.Add(0x99, new RES_C(3));
            cb_opcodes.Add(0x9A, new RES_D(3));
            cb_opcodes.Add(0x9B, new RES_E(3));
            cb_opcodes.Add(0x9C, new RES_H(3));
            cb_opcodes.Add(0x9D, new RES_L(3));
            cb_opcodes.Add(0x9E, new RES_indexHL(3));
            cb_opcodes.Add(0x9F, new RES_A(3));
            cb_opcodes.Add(0xA0, new RES_B(4));
            cb_opcodes.Add(0xA1, new RES_C(4));
            cb_opcodes.Add(0xA2, new RES_D(4));
            cb_opcodes.Add(0xA3, new RES_E(4));
            cb_opcodes.Add(0xA4, new RES_H(4));
            cb_opcodes.Add(0xA5, new RES_L(4));
            cb_opcodes.Add(0xA6, new RES_indexHL(4));
            cb_opcodes.Add(0xA7, new RES_A(4));
            cb_opcodes.Add(0xA8, new RES_B(5));
            cb_opcodes.Add(0xA9, new RES_C(5));
            cb_opcodes.Add(0xAA, new RES_D(5));
            cb_opcodes.Add(0xAB, new RES_E(5));
            cb_opcodes.Add(0xAC, new RES_H(5));
            cb_opcodes.Add(0xAD, new RES_L(5));
            cb_opcodes.Add(0xAE, new RES_indexHL(5));
            cb_opcodes.Add(0xAF, new RES_A(5));
            cb_opcodes.Add(0xB0, new RES_B(6));
            cb_opcodes.Add(0xB1, new RES_C(6));
            cb_opcodes.Add(0xB2, new RES_D(6));
            cb_opcodes.Add(0xB3, new RES_E(6));
            cb_opcodes.Add(0xB4, new RES_H(6));
            cb_opcodes.Add(0xB5, new RES_L(6));
            cb_opcodes.Add(0xB6, new RES_indexHL(6));
            cb_opcodes.Add(0xB7, new RES_A(6));
            cb_opcodes.Add(0xB8, new RES_B(7));
            cb_opcodes.Add(0xB9, new RES_C(7));
            cb_opcodes.Add(0xBA, new RES_D(7));
            cb_opcodes.Add(0xBB, new RES_E(7));
            cb_opcodes.Add(0xBC, new RES_H(7));
            cb_opcodes.Add(0xBD, new RES_L(7));
            cb_opcodes.Add(0xBE, new RES_indexHL(7));
            cb_opcodes.Add(0xBF, new RES_A(7));

            //ALL SET OPCODES
            cb_opcodes.Add(0xC0, new SET_B(0));
            cb_opcodes.Add(0xC1, new SET_C(0));
            cb_opcodes.Add(0xC2, new SET_D(0));
            cb_opcodes.Add(0xC3, new SET_E(0));
            cb_opcodes.Add(0xC4, new SET_H(0));
            cb_opcodes.Add(0xC5, new SET_L(0));
            cb_opcodes.Add(0xC6, new SET_indexHL(0));
            cb_opcodes.Add(0xC7, new SET_A(0));
            cb_opcodes.Add(0xC8, new SET_B(1));
            cb_opcodes.Add(0xC9, new SET_C(1));
            cb_opcodes.Add(0xCA, new SET_D(1));
            cb_opcodes.Add(0xCB, new SET_E(1));
            cb_opcodes.Add(0xCC, new SET_H(1));
            cb_opcodes.Add(0xCD, new SET_L(1));
            cb_opcodes.Add(0xCE, new SET_indexHL(1));
            cb_opcodes.Add(0xCF, new SET_A(1));
            cb_opcodes.Add(0xD0, new SET_B(2));
            cb_opcodes.Add(0xD1, new SET_C(2));
            cb_opcodes.Add(0xD2, new SET_D(2));
            cb_opcodes.Add(0xD3, new SET_E(2));
            cb_opcodes.Add(0xD4, new SET_H(2));
            cb_opcodes.Add(0xD5, new SET_L(2));
            cb_opcodes.Add(0xD6, new SET_indexHL(2));
            cb_opcodes.Add(0xD7, new SET_A(2));
            cb_opcodes.Add(0xD8, new SET_B(3));
            cb_opcodes.Add(0xD9, new SET_C(3));
            cb_opcodes.Add(0xDA, new SET_D(3));
            cb_opcodes.Add(0xDB, new SET_E(3));
            cb_opcodes.Add(0xDC, new SET_H(3));
            cb_opcodes.Add(0xDD, new SET_L(3));
            cb_opcodes.Add(0xDE, new SET_indexHL(3));
            cb_opcodes.Add(0xDF, new SET_A(3));
            cb_opcodes.Add(0xE0, new SET_B(4));
            cb_opcodes.Add(0xE1, new SET_C(4));
            cb_opcodes.Add(0xE2, new SET_D(4));
            cb_opcodes.Add(0xE3, new SET_E(4));
            cb_opcodes.Add(0xE4, new SET_H(4));
            cb_opcodes.Add(0xE5, new SET_L(4));
            cb_opcodes.Add(0xE6, new SET_indexHL(4));
            cb_opcodes.Add(0xE7, new SET_A(4));
            cb_opcodes.Add(0xE8, new SET_B(5));
            cb_opcodes.Add(0xE9, new SET_C(5));
            cb_opcodes.Add(0xEA, new SET_D(5));
            cb_opcodes.Add(0xEB, new SET_E(5));
            cb_opcodes.Add(0xEC, new SET_H(5));
            cb_opcodes.Add(0xED, new SET_L(5));
            cb_opcodes.Add(0xEE, new SET_indexHL(5));
            cb_opcodes.Add(0xEF, new SET_A(5));
            cb_opcodes.Add(0xF0, new SET_B(6));
            cb_opcodes.Add(0xF1, new SET_C(6));
            cb_opcodes.Add(0xF2, new SET_D(6));
            cb_opcodes.Add(0xF3, new SET_E(6));
            cb_opcodes.Add(0xF4, new SET_H(6));
            cb_opcodes.Add(0xF5, new SET_L(6));
            cb_opcodes.Add(0xF6, new SET_indexHL(6));
            cb_opcodes.Add(0xF7, new SET_A(6));
            cb_opcodes.Add(0xF8, new SET_B(7));
            cb_opcodes.Add(0xF9, new SET_C(7));
            cb_opcodes.Add(0xFA, new SET_D(7));
            cb_opcodes.Add(0xFB, new SET_E(7));
            cb_opcodes.Add(0xFC, new SET_H(7));
            cb_opcodes.Add(0xFD, new SET_L(7));
            cb_opcodes.Add(0xFE, new SET_indexHL(7));
            cb_opcodes.Add(0xFF, new SET_A(7));

        }

    }
}
