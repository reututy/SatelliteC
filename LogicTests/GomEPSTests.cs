using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using DataModel.EPS;

namespace Logic.Tests
{
    [TestClass()]
    public class GomEPSTests
    {
        private static byte add = 20;
        [TestMethod()]
        public void GomEpsInitialize_repeat_Test()
        {
            GomEPS gom = new GomEPS();
            Assert.AreEqual(Constants.E_NO_SS_ERR, gom.GomEpsInitialize(add, 1));
            Assert.AreEqual(Constants.E_IS_INITIALIZED, gom.GomEpsInitialize(add, 1));
        }

        [TestMethod()]
        public void GomEpsInitialize_n_greater_then_1_Test()
        {
            GomEPS gom = new GomEPS();
            Assert.AreEqual(Constants.E_NO_SS_ERR, gom.GomEpsInitialize(add, 2));
        }

        [TestMethod()]
        public void GomEpsInitializeRepeat_incorrect_number_Test()
        {
            GomEPS gom = new GomEPS();
            Assert.AreEqual(Constants.E_INDEX_ERROR, gom.GomEpsInitialize(add, 0));
        }

        [TestMethod()]
        public void GomEpsPing_eps_not_initiallized_Test()
        {
            GomEPS gom = new GomEPS();
            Output<Byte> output = new Output<Byte>();
            Assert.AreEqual(Constants.E_NOT_INITIALIZED, gom.GomEpsPing(0,1, output));
        }


        [TestMethod()]
        public void GomEpsPing_eps_not_an_index_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Output<Byte> output = new Output<Byte>();
            Assert.AreEqual(Constants.E_INDEX_ERROR, gom.GomEpsPing(1, 1, output));
        }

        [TestMethod()]
        public void GomEpsPing_eps_right_ping_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Output<Byte> output = new Output<Byte>();
            gom.GomEpsPing(0, 1, output);
            Assert.AreEqual(1, output.output);
        }

        [TestMethod()]
        public void GomEpsPing_eps_no_error_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Output<Byte> output = new Output<Byte>();
            Assert.AreEqual(Constants.E_NO_SS_ERR, gom.GomEpsPing(0, 1, output));
        }
        
        [TestMethod()]
        public void GomEpsSoftReset_not_an_index_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Assert.AreEqual(Constants.E_INDEX_ERROR, gom.GomEpsSoftReset(1));
        }

        [TestMethod()]
        public void GomEpsSoftReset_no_error_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Assert.AreEqual(Constants.E_NO_SS_ERR, gom.GomEpsSoftReset(0));
        }

        [TestMethod()]
        public void GomEpsHardReset_not_an_index_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Assert.AreEqual(Constants.E_INDEX_ERROR, gom.GomEpsHardReset(1));
        }

        [TestMethod()]
        public void GomEpsHardReset_no_error_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Assert.AreEqual(Constants.E_NO_SS_ERR, gom.GomEpsHardReset(0));
        }

        [TestMethod()]
        public void GomEpsGetHkData_param_not_an_index_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Output<EPS.hkparam_t> output = new Output<EPS.hkparam_t>();
            Assert.AreEqual(Constants.E_INDEX_ERROR, gom.GomEpsGetHkData_param(1, output));
        }

        [TestMethod()]
        public void GomEpsGetHkData_param_no_error_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Output<EPS.hkparam_t> output = new Output<EPS.hkparam_t>();
            Assert.AreEqual(Constants.E_NO_SS_ERR, gom.GomEpsGetHkData_param(0, output));
        }

        [TestMethod()]
        public void GomEpsGetHkData_general_not_an_index_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Output<EPS.eps_hk_t> output = new Output<EPS.eps_hk_t>();
            Assert.AreEqual(Constants.E_INDEX_ERROR, gom.GomEpsGetHkData_general(1, output));
        }

        [TestMethod()]
        public void GomEpsGetHkData_general_no_error_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Output<EPS.eps_hk_t> output = new Output<EPS.eps_hk_t>();
            Assert.AreEqual(Constants.E_NO_SS_ERR, gom.GomEpsGetHkData_general(0, output));
        }

        [TestMethod()]
        public void GomEpsGetHkData_vi_not_an_index_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Output<EPS.eps_hk_vi_t> output = new Output<EPS.eps_hk_vi_t>();
            Assert.AreEqual(Constants.E_INDEX_ERROR, gom.GomEpsGetHkData_vi(1, output));
        }

        [TestMethod()]
        public void GomEpsGetHkData_vi_no_error_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Output<EPS.eps_hk_vi_t> output = new Output<EPS.eps_hk_vi_t>();
            Assert.AreEqual(Constants.E_NO_SS_ERR, gom.GomEpsGetHkData_vi(0, output));
        }

        [TestMethod()]
        public void GomEpsGetHkData_out_not_an_index_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Output<EPS.eps_hk_out_t> output = new Output<EPS.eps_hk_out_t>();
            Assert.AreEqual(Constants.E_INDEX_ERROR, gom.GomEpsGetHkData_out(1, output));
        }

        [TestMethod()]
        public void GomEpsGetHkData_out_no_error_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Output<EPS.eps_hk_out_t> output = new Output<EPS.eps_hk_out_t>();
            Assert.AreEqual(Constants.E_NO_SS_ERR, gom.GomEpsGetHkData_out(0, output));
        }

        [TestMethod()]
        public void GomEpsGetHkData_wdt_not_an_index_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Output<EPS.eps_hk_wdt_t> output = new Output<EPS.eps_hk_wdt_t>();
            Assert.AreEqual(Constants.E_INDEX_ERROR, gom.GomEpsGetHkData_wdt(1, output));
        }

        [TestMethod()]
        public void GomEpsGetHkData_wdt_no_error_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Output<EPS.eps_hk_wdt_t> output = new Output<EPS.eps_hk_wdt_t>();
            Assert.AreEqual(Constants.E_NO_SS_ERR, gom.GomEpsGetHkData_wdt(0, output));
        }

        [TestMethod()]
        public void GomEpsGetHkData_basic_not_an_index_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Output<EPS.eps_hk_basic_t> output = new Output<EPS.eps_hk_basic_t>();
            Assert.AreEqual(Constants.E_INDEX_ERROR, gom.GomEpsGetHkData_basic(1, output));
        }

        [TestMethod()]
        public void GomEpsGetHkData_basic_no_error_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Output<EPS.eps_hk_basic_t> output = new Output<EPS.eps_hk_basic_t>();
            Assert.AreEqual(Constants.E_NO_SS_ERR, gom.GomEpsGetHkData_basic(0, output));
        }

        [TestMethod()]
        public void GomEpsSetOutput_not_an_index_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            byte outputs = 255;
            Assert.AreEqual(Constants.E_INDEX_ERROR, gom.GomEpsSetOutput(1, outputs));
        }

        [TestMethod()]
        public void GomEpsSetOutput_no_error_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            byte outputs = 255;
            Assert.AreEqual(Constants.E_NO_SS_ERR, gom.GomEpsSetOutput(0, outputs));
        }

        [TestMethod()]
        public void GomEpsSetSingleOutput_invalid_channel_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            byte channelId = 9;
            byte val = 1;
            Assert.AreEqual(Constants.E_INVALID_INPUT, gom.GomEpsSetSingleOutput(0, channelId, val, 1));
        }

        [TestMethod()]
        public void GomEpsSetSingleOutput_invalid_value_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            byte channelId = 5;
            byte val = 2;
            Assert.AreEqual(Constants.E_INVALID_INPUT, gom.GomEpsSetSingleOutput(0, channelId, val, 1));
        }

        [TestMethod()]
        public void GomEpsSetSingleOutput_not_an_index_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            byte channelId = 5;
            byte val = 1;
            Assert.AreEqual(Constants.E_INDEX_ERROR, gom.GomEpsSetSingleOutput(1, channelId, val, 1));
        }

        [TestMethod()]
        public void GomEpsSetSingleOutput_no_error_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            byte channelId = 5;
            byte val = 1;
            Assert.AreEqual(Constants.E_NO_SS_ERR, gom.GomEpsSetSingleOutput(0, channelId, val, 1));
        }

        [TestMethod()]
        public void GomEpsSetPhotovoltaicInputs_invalid_action_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            gom.eps_table[0].CurrentConfig.PptMode = PPTMode.MPPT;
            Assert.AreEqual(Constants.E_INVALID_ACTION, gom.GomEpsSetPhotovoltaicInputs(0, 7000, 7200, 7400));
        }

        [TestMethod()]
        public void GomEpsSetPhotovoltaicInputs_not_an_index_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Assert.AreEqual(Constants.E_INDEX_ERROR, gom.GomEpsSetPhotovoltaicInputs(1, 7000, 7200, 7400));
        }

        [TestMethod()]
        public void GomEpsSetPhotovoltaicInputs_no_error_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Assert.AreEqual(Constants.E_NO_SS_ERR, gom.GomEpsSetPhotovoltaicInputs(0, 7000, 7200, 7400));
        }

        [TestMethod()]
        public void GomEpsSetPptMode_not_invalid_input_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            byte mode = 3;
            Assert.AreEqual(Constants.E_INVALID_INPUT, gom.GomEpsSetPptMode(0, mode));
        }

        [TestMethod()]
        public void GomEpsSetPptMode_not_an_index_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            byte mode = 1;
            Assert.AreEqual(Constants.E_INDEX_ERROR, gom.GomEpsSetPptMode(1, mode));
        }

        [TestMethod()]
        public void GomEpsSetPptMode_no_error_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            byte mode = 1;
            Assert.AreEqual(Constants.E_NO_SS_ERR, gom.GomEpsSetPptMode(0, mode));
        }

        [TestMethod()]
        public void GomEpsSetHeaterAutoMode_not_an_index_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Output<ushort> output = new Output<ushort>();
            byte mode = 1;
            Assert.AreEqual(Constants.E_INDEX_ERROR, gom.GomEpsSetHeaterAutoMode(1, mode, output));
        }

        [TestMethod()]
        public void GomEpsSetHeaterAutoMode_no_error_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Output<ushort> output = new Output<ushort>();
            byte mode = 1;
            Assert.AreEqual(Constants.E_NO_SS_ERR, gom.GomEpsSetHeaterAutoMode(0, mode, output));
        }

        [TestMethod()]
        public void GomEpsResetCounters_not_an_index_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Assert.AreEqual(Constants.E_INDEX_ERROR, gom.GomEpsResetCounters(1));
        }

        [TestMethod()]
        public void GomEpsResetCounters_no_error_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Assert.AreEqual(Constants.E_NO_SS_ERR, gom.GomEpsResetCounters(0));
        }

        [TestMethod()]
        public void GomEpsResetWDT_not_an_index_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Assert.AreEqual(Constants.E_INDEX_ERROR, gom.GomEpsResetWDT(1));
        }

        [TestMethod()]
        public void GomEpsResetWDT_no_error_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Assert.AreEqual(Constants.E_NO_SS_ERR, gom.GomEpsResetWDT(0));
        }


        [TestMethod()]
        public void GomEpsConfigCMD_invalid_input_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Assert.AreEqual(Constants.E_INVALID_INPUT, gom.GomEpsConfigCMD(0, 6));
        }

        [TestMethod()]
        public void GomEpsConfigCMD_not_an_index_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Assert.AreEqual(Constants.E_INDEX_ERROR, gom.GomEpsConfigCMD(1, 1));
        }

        [TestMethod()]
        public void GomEpsConfigCMD_no_error_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Assert.AreEqual(Constants.E_NO_SS_ERR, gom.GomEpsConfigCMD(0, 1));
        }

        [TestMethod()]
        public void GomEpsConfigGet_not_an_index_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Output<EPS.eps_config_t> conf = new Output<EPS.eps_config_t>();
            Assert.AreEqual(Constants.E_INDEX_ERROR, gom.GomEpsConfigGet(1, conf));
        }

        [TestMethod()]
        public void GomEpsConfigGet_no_error_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Output<EPS.eps_config_t> conf = new Output<EPS.eps_config_t>();
            Assert.AreEqual(Constants.E_NO_SS_ERR, gom.GomEpsConfigGet(0, conf));
        }

        [TestMethod()]
        public void GomEpsConfigSet_not_an_index_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Output<EPS.eps_config_t> conf = new Output<EPS.eps_config_t>();
            Assert.AreEqual(Constants.E_INDEX_ERROR, gom.GomEpsConfigSet(1, conf));
        }

        [TestMethod()]
        public void GomEpsConfigSet_no_error_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Output<EPS.eps_config_t> conf = new Output<EPS.eps_config_t>();
            /*conf.output = new EPS.eps_config_t();
            conf.output.ppt_mode = (byte)CurrentConfig.PptMode;
            ans.battheater_mode = (byte)CurrentConfig.BattheaterMode;
            ans.battheater_low = CurrentConfig.BattheaterLow;
            ans.battheater_high = CurrentConfig.BattheaterHigh;
            ans.output_initial_off_delay = new ushort[8];
            ans.output_initial_on_delay = new ushort[8];
            ans.output_normal_value = new byte[8];
            ans.output_safe_value = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                ans.output_initial_off_delay[i] = CurrentConfig.OutputInitialOffDelay[i];
                ans.output_initial_on_delay[i] = CurrentConfig.OutputInitialOnDelay[i];
                ans.output_normal_value[i] = CurrentConfig.OutputNormalValue[i];
                ans.output_safe_value[i] = CurrentConfig.OutputSafeValue[i];
            }
            ans.vboost = new ushort[3];
            for (int i = 0; i < 3; i++)
            {
                ans.vboost[i] = CurrentConfig.Vboost[i];
            }*/
            Assert.AreEqual(Constants.E_NO_SS_ERR, gom.GomEpsConfigSet(0, conf));
        }

        [TestMethod()]
        public void GomEpsConfig2CMD_invalid_input_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Assert.AreEqual(Constants.E_INVALID_INPUT, gom.GomEpsConfig2CMD(0, 6));
        }

        [TestMethod()]
        public void GomEpsConfig2CMD_not_an_index_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Assert.AreEqual(Constants.E_INDEX_ERROR, gom.GomEpsConfig2CMD(1, 1));
        }

        [TestMethod()]
        public void GomEpsConfig2CMD_no_error_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Assert.AreEqual(Constants.E_NO_SS_ERR, gom.GomEpsConfig2CMD(0, 1));
        }

        [TestMethod()]
        public void GomEpsConfig2Get_not_an_index_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Output<EPS.eps_config2_t> conf = new Output<EPS.eps_config2_t>();
            Assert.AreEqual(Constants.E_INDEX_ERROR, gom.GomEpsConfig2Get(1, conf));
        }

        [TestMethod()]
        public void GomEpsConfig2Get_no_error_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Output<EPS.eps_config2_t> conf = new Output<EPS.eps_config2_t>();
            Assert.AreEqual(Constants.E_NO_SS_ERR, gom.GomEpsConfig2Get(0, conf));
        }

        [TestMethod()]
        public void GomEpsConfig2Set_not_an_index_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Output<EPS.eps_config2_t> conf = new Output<EPS.eps_config2_t>();
            Assert.AreEqual(Constants.E_INDEX_ERROR, gom.GomEpsConfig2Set(1, conf));
        }

        [TestMethod()]
        public void GomEpsConfig2Set_no_error_Test()
        {
            GomEPS gom = new GomEPS();
            gom.GomEpsInitialize(add, 1);
            Output<EPS.eps_config2_t> conf = new Output<EPS.eps_config2_t>();
            Assert.AreEqual(Constants.E_NO_SS_ERR, gom.GomEpsConfig2Set(0, conf));
        }
    }
}