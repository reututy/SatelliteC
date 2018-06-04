using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataModel.EPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.EPS.Tests
{
    [TestClass()]
    public class EPSTests
    {
        /*[TestMethod()]
        public void EPSTest()
        {
            Assert.Fail();
        }*/

        [TestMethod()]
        public void GET_HK_1Test()
        {
            EPS eps = new EPS();
            EPS.hkparam_t ans = eps.GET_HK_1();
            ushort photoCurrent = 0;
            for (int i = 0; i < 3; i++)
            {
                photoCurrent += eps.BoostConverters[i].CurrentOut;
            }
            Assert.AreEqual(ans.pc, photoCurrent);
            Assert.AreEqual(ans.bv, eps.OnboardBattery.Vbat);
            Assert.AreEqual(ans.sc, eps.OnboardBattery.CurrentOut);
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(ans.temp[i], eps.BoostConverters[i].Temperture);
            }
            Assert.AreEqual(ans.temp[3], eps.OnboardBattery.Temperture);
            Assert.AreEqual(ans.batt_temp[0], eps.OnboardBattery.Temperture);
            Assert.AreEqual(ans.batt_temp[1], eps.OnboardBattery.Temperture);
            for (int i = 0; i < 6; i++)
            {
                Assert.AreEqual(ans.latchup[i], ((Channel)eps.Outputs[i]).LatchupNum);
            }
            Assert.AreEqual(ans.reset, eps.LastResetCause);
            Assert.AreEqual(ans.bootcount, eps.RebootCount);
            Assert.AreEqual(ans.sw_errors, eps.SwErrors);
            Assert.AreEqual(ans.ppt_mode, eps.CurrentConfig.PptMode);
            Assert.AreEqual(ans.channel_status, 255);
        }

        [TestMethod()]
        public void GET_HK_2Test()
        {
            EPS eps = new EPS();
            EPS.eps_hk_t ans = eps.GET_HK_2(0);
            int i;
            for (i = 0; i < 3; i++)
            {
                Assert.AreEqual(ans.vboost[i], eps.BoostConverters[i].Volt);
                Assert.AreEqual(ans.curin[i], eps.BoostConverters[i].CurrentIn);
            }
            Assert.AreEqual(ans.vbatt, eps.OnboardBattery.Vbat);
            ushort photoCurrent = 0;
            for (i = 0; i < 3; i++)
            {
                photoCurrent += eps.BoostConverters[i].CurrentOut;
            }
            Assert.AreEqual(ans.cursun, photoCurrent);
            Assert.AreEqual(ans.cursys, eps.OnboardBattery.CurrentOut);
            for (i = 0; i < 6; i++)
                Assert.AreEqual(ans.curout[i], ((Channel)eps.Outputs[i]).CurrentOut);
            for (i = 0; i < 8; i++)
            {
                Assert.AreEqual(ans.output[i], eps.Outputs[i].Status);
                Assert.AreEqual(ans.output_on_delta[i], eps.CurrentConfig.OutputInitialOnDelay[i]);
                Assert.AreEqual(ans.output_off_delta[i], eps.CurrentConfig.OutputInitialOffDelay[i]);
            }
            for (i = 0; i < 6; i++)
                Assert.AreEqual(ans.latchup[i], ((Channel)eps.Outputs[i]).LatchupNum);
            Assert.AreEqual(ans.wdt_csp_pings_left[0], eps.Wdts[(int)WdtType.CSP0].PingLeft);
            Assert.AreEqual(ans.wdt_csp_pings_left[1], eps.Wdts[(int)WdtType.CSP1].PingLeft);
            Assert.AreEqual(ans.wdt_gnd_time_left, eps.Wdts[(int)WdtType.GND].TimeLeft);
            Assert.AreEqual(ans.wdt_i2c_time_left, eps.Wdts[(int)WdtType.I2C].TimeLeft);
            Assert.AreEqual(ans.counter_boot, eps.RebootCount);
            Assert.AreEqual(ans.counter_wdt_csp[0], eps.Wdts[(int)WdtType.CSP0].RebootCounter);
            Assert.AreEqual(ans.counter_wdt_csp[1], eps.Wdts[(int)WdtType.CSP1].RebootCounter);
            Assert.AreEqual(ans.counter_wdt_gnd, eps.Wdts[(int)WdtType.GND].RebootCounter);
            Assert.AreEqual(ans.counter_wdt_i2c, eps.Wdts[(int)WdtType.I2C].RebootCounter);
            for (i = 0; i < 3; i++)
                Assert.AreEqual(ans.temp[i], eps.BoostConverters[i].Temperture);
            Assert.AreEqual(ans.temp[3], eps.OnboardBattery.Temperture);
            Assert.AreEqual(ans.temp[4], eps.OnboardBattery.Temperture);
            Assert.AreEqual(ans.temp[5], eps.OnboardBattery.Temperture); // external - need to change
            Assert.AreEqual(ans.bootcause, eps.LastResetCause);
            Assert.AreEqual(ans.battmode, (byte)eps.OnboardBattery.BattMode);
            Assert.AreEqual(ans.pptmode, eps.CurrentConfig.PptMode);
        }

        [TestMethod()]
        public void GET_HK_2_VITest()
        {
            EPS eps = new EPS();
            EPS.eps_hk_vi_t ans = eps.GET_HK_2_VI(1);
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(ans.vboost[i], eps.BoostConverters[i].Volt);
                Assert.AreEqual(ans.curin[i], eps.BoostConverters[i].CurrentIn);
            }
            Assert.AreEqual(ans.vbatt, eps.OnboardBattery.Vbat);
            ushort systemCurrent = eps.OnboardBattery.CurrentOut;
            Assert.AreEqual(ans.cursys, systemCurrent);
            ushort photoCurrent = 0;
            for (int i =0; i<3; i++)
            {
                photoCurrent += eps.BoostConverters[i].CurrentOut;
            }
            Assert.AreEqual(ans.cursun, photoCurrent);

        }

        [TestMethod()]
        public void GET_HK_2_OUTTest()
        {
            EPS eps = new EPS();
            EPS.eps_hk_out_t ans = eps.GET_HK_2_OUT(2);
            for (int i = 0; i < 8; i++)
            {
                Assert.AreEqual(ans.output[i], eps.Outputs[i].Status);
                Assert.AreEqual(ans.output_off_delta[i], eps.CurrentConfig.OutputInitialOffDelay[i]);
                Assert.AreEqual(ans.output_on_delta[i], eps.CurrentConfig.OutputInitialOnDelay[i]);
            }
            for (int i = 0; i < 6; i++)
            {
                Assert.AreEqual(ans.latchup[i], ((Channel)eps.Outputs[i]).LatchupNum);
                Assert.AreEqual(ans.curout[i], ((Channel)eps.Outputs[i]).CurrentOut);
            }
        }
        /*[TestMethod()]
        public void ChargingFlowTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ButteryDropTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void i2c_wdt_workTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void gnd_wdt_workTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void csp_wdt_workTest()
        {
            Assert.Fail();
        }*/

        [TestMethod()]
        public void GET_HK_2_WDTTest()
        {
            EPS eps = new EPS();
            EPS.eps_hk_wdt_t ans = eps.GET_HK_2_WDT(3);
            Assert.AreEqual(ans.counter_wdt_csp[0], eps.Wdts[(int)WdtType.CSP0].RebootCounter);
            Assert.AreEqual(ans.counter_wdt_csp[1], eps.Wdts[(int)WdtType.CSP1].RebootCounter);
            Assert.AreEqual(ans.counter_wdt_gnd, eps.Wdts[(int)WdtType.GND].RebootCounter);
            Assert.AreEqual(ans.counter_wdt_i2c, eps.Wdts[(int)WdtType.I2C].RebootCounter);
            Assert.AreEqual(ans.wdt_csp_pings_left[0], eps.Wdts[(int)WdtType.CSP0].TimeLeft);
            Assert.AreEqual(ans.wdt_csp_pings_left[1], eps.Wdts[(int)WdtType.CSP1].TimeLeft);
            Assert.AreEqual(ans.wdt_gnd_time_left, eps.Wdts[(int)WdtType.GND].TimeLeft);
            Assert.AreEqual(ans.wdt_i2c_time_left, eps.Wdts[(int)WdtType.I2C].TimeLeft);
        }

        [TestMethod()]
        public void GET_HK_2_BASICTest()
        {
            EPS eps = new EPS();
            EPS.eps_hk_basic_t ans = eps.GET_HK_2_BASIC(4);
            Assert.AreEqual(ans.counter_boot, eps.RebootCount);
            Assert.AreEqual(ans.bootcause, eps.LastResetCause);
            Assert.AreEqual(ans.pptmode, eps.CurrentConfig.PptMode);
            Assert.AreEqual(ans.battmode, (byte)eps.OnboardBattery.BattMode);
            int i;
            for (i = 0; i < 3; i++)
            {
                Assert.AreEqual(ans.temp[i], eps.BoostConverters[i].Temperture);                
            }
            Assert.AreEqual(ans.temp[3], eps.OnboardBattery.Temperture);
            Assert.AreEqual(ans.temp[4], eps.OnboardBattery.Temperture);
            Assert.AreEqual(ans.temp[5], eps.OnboardBattery.Temperture);
        }

        [TestMethod()]
        public void SET_OUTPUTTest()
        {
            EPS eps = new EPS();

            byte output1 = 0;//00000000
            byte output2 = 120;//01111000
            byte output3 = 255;//11111111
            int i;
            eps.SET_OUTPUT(output1);
            for (i = 0; i < 8; i++)
                Assert.AreEqual(eps.Outputs[i].Status, 0);
            eps.SET_OUTPUT(output2);
            Assert.AreEqual(eps.Outputs[0].Status, 0);
            Assert.AreEqual(eps.Outputs[1].Status, 0);
            Assert.AreEqual(eps.Outputs[2].Status, 0);
            Assert.AreEqual(eps.Outputs[3].Status, 1);
            Assert.AreEqual(eps.Outputs[4].Status, 1);
            Assert.AreEqual(eps.Outputs[5].Status, 1);
            Assert.AreEqual(eps.Outputs[6].Status, 1);
            Assert.AreEqual(eps.Outputs[7].Status, 0);
            eps.SET_OUTPUT(output3);
            for (i = 0; i < 8; i++)
                Assert.AreEqual(eps.Outputs[i].Status, 1);
        }

        [TestMethod()]
        public void SET_SINGLE_OUTPUTTest()
        {
            EPS eps = new EPS();
            eps.SET_SINGLE_OUTPUT(5, 0, 1);
            Assert.AreEqual(eps.Outputs[5].Status, 0);
            eps.SET_SINGLE_OUTPUT(5, 1, 1);
            Assert.AreEqual(eps.Outputs[5].Status, 1);
        }

        [TestMethod()]
        public void SET_PV_VOLTTest()
        {
            EPS eps = new EPS();
            eps.SET_PV_VOLT(4000, 4500, 3300);
            Assert.AreEqual(eps.BoostConverters[0].Volt, 4000);
            Assert.AreEqual(eps.BoostConverters[1].Volt, 4500);
            Assert.AreEqual(eps.BoostConverters[2].Volt, 3300);
            eps.SET_PV_VOLT(2000, 4550, 4000);
            Assert.AreEqual(eps.BoostConverters[0].Volt, 2000);
            Assert.AreEqual(eps.BoostConverters[1].Volt, 4550);
            Assert.AreEqual(eps.BoostConverters[2].Volt, 4000);
        }

        [TestMethod()]
        public void SET_PV_AUTOTest()
        {
            EPS eps = new EPS();
            eps.SET_PV_AUTO(EPSConstants.HARDWARE);
            Assert.AreEqual(eps.CurrentConfig.PptMode, EPSConstants.HARDWARE);
            eps.SET_PV_AUTO(EPSConstants.MPPT);
            Assert.AreEqual(eps.CurrentConfig.PptMode, EPSConstants.MPPT);
            eps.SET_PV_AUTO(EPSConstants.FIXEDSWPPT);
            Assert.AreEqual(eps.CurrentConfig.PptMode, EPSConstants.FIXEDSWPPT);
        }

        [TestMethod()]
        public void SET_HEATERTest()
        {
            EPS eps = new EPS();
            Assert.AreEqual(eps.SET_HEATER(0, EPSConstants.BP4_HEATER, EPSConstants.ON), EPSConstants.ON + (eps.BatteryHeaters[EPSConstants.ONBOARD_HEATER].Status << 8));
            Assert.AreEqual(eps.BatteryHeaters[EPSConstants.BP4_HEATER].Status, EPSConstants.ON);

            Assert.AreEqual(eps.SET_HEATER(0, EPSConstants.BP4_HEATER, EPSConstants.OFF), EPSConstants.OFF + (eps.BatteryHeaters[EPSConstants.ONBOARD_HEATER].Status << 8));
            Assert.AreEqual(eps.BatteryHeaters[EPSConstants.BP4_HEATER].Status, EPSConstants.OFF);

            Assert.AreEqual(eps.SET_HEATER(0, EPSConstants.ONBOARD_HEATER, EPSConstants.ON), eps.BatteryHeaters[EPSConstants.BP4_HEATER].Status + (EPSConstants.ON << 8));
            Assert.AreEqual(eps.BatteryHeaters[EPSConstants.ONBOARD_HEATER].Status, EPSConstants.ON);

            Assert.AreEqual(eps.SET_HEATER(0, EPSConstants.ONBOARD_HEATER, EPSConstants.OFF), eps.BatteryHeaters[EPSConstants.BP4_HEATER].Status + (EPSConstants.OFF << 8));
            Assert.AreEqual(eps.BatteryHeaters[EPSConstants.ONBOARD_HEATER].Status, EPSConstants.OFF);

            Assert.AreEqual(eps.SET_HEATER(0, EPSConstants.BOTH_HEATER, EPSConstants.ON), EPSConstants.ON + (EPSConstants.ON << 8));
            Assert.AreEqual(eps.BatteryHeaters[EPSConstants.BP4_HEATER].Status, EPSConstants.ON);
            Assert.AreEqual(eps.BatteryHeaters[EPSConstants.ONBOARD_HEATER].Status, EPSConstants.ON);

            Assert.AreEqual(eps.SET_HEATER(0, EPSConstants.BOTH_HEATER, EPSConstants.OFF), EPSConstants.OFF + (EPSConstants.OFF << 8));
            Assert.AreEqual(eps.BatteryHeaters[EPSConstants.BP4_HEATER].Status, EPSConstants.OFF);
            Assert.AreEqual(eps.BatteryHeaters[EPSConstants.ONBOARD_HEATER].Status, EPSConstants.OFF);
        }

        [TestMethod()]
        public void RESET_COUNTERSTest()
        {
            EPS eps = new EPS();
            eps.RESET_COUNTERS(0x42);
            Assert.AreEqual(eps.RebootCount, (uint)0);
            Assert.AreEqual(eps.Wdts[(int)WdtType.CSP0].RebootCounter, (uint)0);
            Assert.AreEqual(eps.Wdts[(int)WdtType.GND].RebootCounter, (uint)0);
            Assert.AreEqual(eps.Wdts[(int)WdtType.I2C].RebootCounter, (uint)0);
            Assert.AreEqual(eps.Wdts[(int)WdtType.CSP1].RebootCounter, (uint)0);
        }

        [TestMethod()]
        public void RESET_WDTTest()
        {
            EPS eps = new EPS();
            eps.RESET_WDT(0x78);
            Assert.AreEqual(eps.Wdts[(int)WdtType.GND].TimeLeft, (uint)EPSConstants.WDT_GND_INIT_TIME);
        }

        [TestMethod()]
        public void CONFIG_CMDTest()
        {
            EPS eps = new EPS();
            eps.CONFIG_CMD(1);
            Assert.AreEqual(eps.CurrentConfig.PptMode, EPSConstants.FIXEDSWPPT);
            Assert.AreEqual(eps.CurrentConfig.BattheaterMode, EPSConstants.AUTO);
            Assert.AreEqual(eps.CurrentConfig.BattheaterHigh, 100); //to change
            Assert.AreEqual(eps.CurrentConfig.BattheaterLow, 0); //to change
            int i;
            for (i = 0; i < 3; i++)
                Assert.AreEqual(eps.CurrentConfig.Vboost[i], EPSConstants.SOFTWARE_PPT_DEFAULT_V);
            for (i = 0; i < 8; i++)
            {
                Assert.AreEqual(eps.CurrentConfig.OutputInitialOffDelay[i], 1);
                Assert.AreEqual(eps.CurrentConfig.OutputInitialOnDelay[i], 1);
                Assert.AreEqual(eps.CurrentConfig.OutputNormalValue[i], 0); //need to change
                Assert.AreEqual(eps.CurrentConfig.OutputSafeValue[i], 0); //need to change
            }
        }

        [TestMethod()]
        public void CONFIG_GETTest()
        {
            EPS eps = new EPS();
            EPS.eps_config_t config = eps.CONFIG_GET();
            Assert.AreEqual(eps.CurrentConfig.PptMode, config.ppt_mode);
            Assert.AreEqual(eps.CurrentConfig.BattheaterMode, config.battheater_mode);
            Assert.AreEqual(eps.CurrentConfig.BattheaterLow, config.battheater_low);
            Assert.AreEqual(eps.CurrentConfig.BattheaterHigh, config.battheater_high);
            Assert.AreEqual(eps.CurrentConfig.BattheaterMode, config.battheater_mode);
            int i;
            for (i = 0; i < 8; i++)
            {
                Assert.AreEqual(config.output_initial_off_delay[i], eps.CurrentConfig.OutputInitialOffDelay[i]);
                Assert.AreEqual(config.output_initial_on_delay[i], eps.CurrentConfig.OutputInitialOnDelay[i]);
                Assert.AreEqual(config.output_normal_value[i], eps.CurrentConfig.OutputNormalValue[i]);
                Assert.AreEqual(config.output_safe_value[i], eps.CurrentConfig.OutputSafeValue[i]);
            }
            for (i = 0; i < 3; i++)
                Assert.AreEqual(config.vboost[i], eps.CurrentConfig.Vboost[i]);
        }

        [TestMethod()]
        public void CONFIG_SETTest()
        {
            EPS eps = new EPS();
            EPS.eps_config_t config = new EPS.eps_config_t();
            config.ppt_mode = 8;
            config.battheater_mode = 8;
            config.battheater_low = 8;
            config.battheater_high = 8;
            config.battheater_mode = 8;
            config.output_initial_off_delay = new ushort[8];
            config.output_initial_on_delay = new ushort[8];
            config.output_normal_value = new byte[8];
            config.output_safe_value = new byte[8];
            config.vboost = new ushort[3];
            for (int i = 0; i < 8; i++)
            {
                config.output_initial_off_delay[i] = 8;
                config.output_initial_on_delay[i] = 8;
                config.output_normal_value[i] = 8;
                config.output_safe_value[i] = 8;
            }
            for (int i = 0; i < 3; i++)
                config.vboost[i] = 8;

            eps.CONFIG_SET(config);
            Assert.AreEqual(eps.CurrentConfig.PptMode, config.ppt_mode);
            Assert.AreEqual(eps.CurrentConfig.BattheaterMode, config.battheater_mode);
            Assert.AreEqual(eps.CurrentConfig.BattheaterLow, config.battheater_low);
            Assert.AreEqual(eps.CurrentConfig.BattheaterHigh, config.battheater_high);
            Assert.AreEqual(eps.CurrentConfig.BattheaterMode, config.battheater_mode);

            for (int i = 0; i < 8; i++)
            {
                Assert.AreEqual(config.output_initial_off_delay[i], eps.CurrentConfig.OutputInitialOffDelay[i]);
                Assert.AreEqual(config.output_initial_on_delay[i], eps.CurrentConfig.OutputInitialOnDelay[i]);
                Assert.AreEqual(config.output_normal_value[i], eps.CurrentConfig.OutputNormalValue[i]);
                Assert.AreEqual(config.output_safe_value[i], eps.CurrentConfig.OutputSafeValue[i]);
            }
            for (int i = 0; i < 3; i++)
                Assert.AreEqual(config.vboost[i], eps.CurrentConfig.Vboost[i]);
        }

        [TestMethod()]
        public void HARD_RESETTest()
        {
            EPS eps = new EPS();
            eps.HARD_RESET();
            Assert.AreEqual(eps.LastResetCause, EPSConstants.HARD_RESET_R);
        }

        [TestMethod()]
        public void CONFIG2_CMDTest()
        {
            EPS eps = new EPS();
            eps.CONFIG2_CMD(1);
            Assert.AreEqual(eps.CurrentConfig.BattSafeVoltage, EPSConstants.SAFE_VBAT);
            Assert.AreEqual(eps.CurrentConfig.BattNormalVoltage, EPSConstants.NORMAL_VBAT);
            Assert.AreEqual(eps.CurrentConfig.BattMaxVoltage, EPSConstants.MAX_VBAT);
            Assert.AreEqual(eps.CurrentConfig.BattCriticalVoltage, EPSConstants.CRITICAL_VBAT);
        }

        [TestMethod()]
        public void CONFIG2_GETTest()
        {
            EPS eps = new EPS();
            EPS.eps_config2_t config2 = eps.CONFIG2_GET();
            Assert.AreEqual(config2.batt_safevoltage, eps.CurrentConfig.BattSafeVoltage);
            Assert.AreEqual(config2.batt_normalvoltage, eps.CurrentConfig.BattNormalVoltage);
            Assert.AreEqual(config2.batt_maxvoltage, eps.CurrentConfig.BattMaxVoltage);
            Assert.AreEqual(config2.batt_criticalvoltage, eps.CurrentConfig.BattCriticalVoltage);

        }

        [TestMethod()]
        public void CONFIG2_SETTest()
        {
            EPS eps = new EPS();
            EPS.eps_config2_t config2 = new EPS.eps_config2_t();
            config2.batt_safevoltage = 8;
            config2.batt_normalvoltage = 8;
            config2.batt_maxvoltage = 8;
            config2.batt_criticalvoltage = 8;
            eps.CONFIG2_SET(config2);
            Assert.AreEqual(config2.batt_safevoltage, eps.CurrentConfig.BattSafeVoltage);
            Assert.AreEqual(config2.batt_normalvoltage, eps.CurrentConfig.BattNormalVoltage);
            Assert.AreEqual(config2.batt_maxvoltage, eps.CurrentConfig.BattMaxVoltage);
            Assert.AreEqual(config2.batt_criticalvoltage, eps.CurrentConfig.BattCriticalVoltage);

        }

        /*[TestMethod()]
        public void PINGTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void REBOOTTest()
        {
            Assert.Fail();
        }*/
    }
}