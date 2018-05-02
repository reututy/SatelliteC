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
            Assert.AreEqual(ans.pc, eps.photo_current);
            Assert.AreEqual(ans.bv, eps.onboard_battery.Vbat);
            Assert.AreEqual(ans.sc, eps.system_current);
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(ans.temp[i], eps.boost_convertors[i].temperture);
            }
            Assert.AreEqual(ans.temp[3], eps.onboard_battery.temperture);
            Assert.AreEqual(ans.batt_temp[0], eps.onboard_battery.temperture);
            Assert.AreEqual(ans.batt_temp[1], eps.onboard_battery.temperture);
            for (int i = 0; i < 6; i++)
            {
                Assert.AreEqual(ans.latchup[i], eps.channels[i].latchup);
            }
            Assert.AreEqual(ans.reset, eps.last_reset_cause);
            Assert.AreEqual(ans.bootcount, eps.reboot_count);
            Assert.AreEqual(ans.sw_errors, eps.sw_errors);
            Assert.AreEqual(ans.ppt_mode, eps.config.ppt_mode);
            Assert.AreEqual(ans.channel_status, 255);
        }

        /*[TestMethod()]
        public void GET_HK_2Test()
        {
            Assert.Fail();
        }*/

        [TestMethod()]
        public void GET_HK_2_VITest()
        {
            EPS eps = new EPS();
            EPS.eps_hk_vi_t ans = eps.GET_HK_2_VI(1);
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(ans.vboost[i], eps.boost_convertors[i].volt);
                Assert.AreEqual(ans.curin[i], eps.boost_convertors[i].current_in);
            }
            Assert.AreEqual(ans.vbatt, eps.onboard_battery.Vbat);
            Assert.AreEqual(ans.cursys, eps.system_current);
            Assert.AreEqual(ans.cursun, eps.photo_current);

        }

        [TestMethod()]
        public void GET_HK_2_OUTTest()
        {
            EPS eps = new EPS();
            EPS.eps_hk_out_t ans = eps.GET_HK_2_OUT(2);
            for (int i = 0; i < 8; i++)
            {
                Assert.AreEqual(ans.output[i], eps.channels[i].status);
                Assert.AreEqual(ans.output_off_delta[i], eps.config.output_initial_off_delay[i]);
                Assert.AreEqual(ans.output_on_delta[i], eps.config.output_initial_on_delay[i]);
            }
            for (int i = 0; i < 6; i++)
            {
                Assert.AreEqual(ans.latchup[i], eps.channels[i].latchup);
                Assert.AreEqual(ans.curout[i], eps.curout[i]);
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

        /*[TestMethod()]
        public void GET_HK_2_WDTTest()
        {
            EPS eps = new EPS();
            EPS.eps_hk_wdt_t ans = eps.GET_HK_2_WDT(3);
            Assert.AreEqual(ans.counter_wdt_csp[0], eps.wdts[(int)wdt_type.CSP0].reboot_counter);
            Assert.AreEqual(ans.counter_wdt_csp[1], eps.wdts[(int)wdt_type.CSP1].reboot_counter);
            Assert.AreEqual(ans.counter_wdt_gnd, eps.wdts[(int)wdt_type.GND].reboot_counter);
            Assert.AreEqual(ans.counter_wdt_i2c, eps.wdts[(int)wdt_type.I2C].reboot_counter);
            Assert.AreEqual(ans.wdt_csp_pings_left[0], eps.wdts[(int)wdt_type.CSP0].time_ping_left);
            Assert.AreEqual(ans.wdt_csp_pings_left[1], eps.wdts[(int)wdt_type.CSP1].time_ping_left);
            Assert.AreEqual(ans.wdt_gnd_time_left, eps.wdts[(int)wdt_type.GND].time_ping_left);
            Assert.AreEqual(ans.wdt_i2c_time_left, eps.wdts[(int)wdt_type.I2C].time_ping_left);

        }*/

        /*[TestMethod()]
        public void GET_HK_2_BASICTest()
        {
            EPS eps = new EPS();
            EPS.eps_hk_basic_t ans = eps.GET_HK_2_BASIC(4);
            Assert.AreEqual(ans.counter_boot, eps.reboot_count);
            Assert.AreEqual(ans.bootcause, eps.last_reset_cause);
            Assert.AreEqual(ans.pptmode, eps.config.ppt_mode);
            Assert.AreEqual(ans.battmode, eps.onboard_battery.batt_state);
            int i;
            for (i = 0; i < 3; i++)
            {
                Assert.AreEqual(ans.temp[i], eps.boost_convertors[i].temperture);
            }
            Assert.AreEqual(ans.temp[3], eps.onboard_battery.temperture);
            Assert.AreEqual(ans.temp[4], eps.onboard_battery.temperture);
            Assert.AreEqual(ans.temp[5], eps.onboard_battery.temperture);
        }*/

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
                Assert.AreEqual(eps.channels[i].status, 0);
            eps.SET_OUTPUT(output2);
            Assert.AreEqual(eps.channels[0].status, 0);
            Assert.AreEqual(eps.channels[1].status, 0);
            Assert.AreEqual(eps.channels[2].status, 0);
            Assert.AreEqual(eps.channels[3].status, 1);
            Assert.AreEqual(eps.channels[4].status, 1);
            Assert.AreEqual(eps.channels[5].status, 1);
            Assert.AreEqual(eps.channels[6].status, 1);
            Assert.AreEqual(eps.channels[7].status, 0);
            eps.SET_OUTPUT(output3);
            for (i = 0; i < 8; i++)
                Assert.AreEqual(eps.channels[i].status, 1);
        }

        /*[TestMethod()]
        public void SET_SINGLE_OUTPUTTest()
        {
            EPS eps = new EPS();
            eps.SET_SINGLE_OUTPUT(5, 0, 1);
            Assert.AreEqual(eps.channels[5].status, 0);
            eps.SET_SINGLE_OUTPUT(5, 1, 1);
            Assert.AreEqual(eps.channels[5].status, 1);
        }*/

        [TestMethod()]
        public void SET_PV_VOLTTest()
        {
            EPS eps = new EPS();
            eps.SET_PV_VOLT(4000, 4500, 3300);
            Assert.AreEqual(eps.boost_convertors[0].volt, 4000);
            Assert.AreEqual(eps.boost_convertors[1].volt, 4500);
            Assert.AreEqual(eps.boost_convertors[2].volt, 3300);
            eps.SET_PV_VOLT(2000, 4550, 4000);
            Assert.AreEqual(eps.boost_convertors[0].volt, 2000);
            Assert.AreEqual(eps.boost_convertors[1].volt, 4550);
            Assert.AreEqual(eps.boost_convertors[2].volt, 4000);
        }

        [TestMethod()]
        public void SET_PV_AUTOTest()
        {
            EPS eps = new EPS();
            eps.SET_PV_AUTO(EPSConstants.HARDWARE);
            Assert.AreEqual(eps.config.ppt_mode, EPSConstants.HARDWARE);
            eps.SET_PV_AUTO(EPSConstants.MPPT);
            Assert.AreEqual(eps.config.ppt_mode, EPSConstants.MPPT);
            eps.SET_PV_AUTO(EPSConstants.FIXEDSWPPT);
            Assert.AreEqual(eps.config.ppt_mode, EPSConstants.FIXEDSWPPT);
        }

        [TestMethod()]
        public void SET_HEATERTest()
        {
            EPS eps = new EPS();
            Assert.AreEqual(eps.SET_HEATER(0, EPSConstants.BP4_HEATER, EPSConstants.ON), EPSConstants.ON + (eps.battery_heaters[EPSConstants.ONBOARD_HEATER].status << 8));
            Assert.AreEqual(eps.battery_heaters[EPSConstants.BP4_HEATER].status, EPSConstants.ON);

            Assert.AreEqual(eps.SET_HEATER(0, EPSConstants.BP4_HEATER, EPSConstants.OFF), EPSConstants.OFF + (eps.battery_heaters[EPSConstants.ONBOARD_HEATER].status << 8));
            Assert.AreEqual(eps.battery_heaters[EPSConstants.BP4_HEATER].status, EPSConstants.OFF);

            Assert.AreEqual(eps.SET_HEATER(0, EPSConstants.ONBOARD_HEATER, EPSConstants.ON), eps.battery_heaters[EPSConstants.BP4_HEATER].status + (EPSConstants.ON << 8));
            Assert.AreEqual(eps.battery_heaters[EPSConstants.ONBOARD_HEATER].status, EPSConstants.ON);

            Assert.AreEqual(eps.SET_HEATER(0, EPSConstants.ONBOARD_HEATER, EPSConstants.OFF), eps.battery_heaters[EPSConstants.BP4_HEATER].status + (EPSConstants.OFF << 8));
            Assert.AreEqual(eps.battery_heaters[EPSConstants.ONBOARD_HEATER].status, EPSConstants.OFF);

            Assert.AreEqual(eps.SET_HEATER(0, EPSConstants.BOTH_HEATER, EPSConstants.ON), EPSConstants.ON + (EPSConstants.ON << 8));
            Assert.AreEqual(eps.battery_heaters[EPSConstants.BP4_HEATER].status, EPSConstants.ON);
            Assert.AreEqual(eps.battery_heaters[EPSConstants.ONBOARD_HEATER].status, EPSConstants.ON);

            Assert.AreEqual(eps.SET_HEATER(0, EPSConstants.BOTH_HEATER, EPSConstants.OFF), EPSConstants.OFF + (EPSConstants.OFF << 8));
            Assert.AreEqual(eps.battery_heaters[EPSConstants.BP4_HEATER].status, EPSConstants.OFF);
            Assert.AreEqual(eps.battery_heaters[EPSConstants.ONBOARD_HEATER].status, EPSConstants.OFF);
        }

        [TestMethod()]
        public void RESET_COUNTERSTest()
        {
            EPS eps = new EPS();
            eps.RESET_COUNTERS(0x42);
            Assert.AreEqual(eps.reboot_count, (uint)0);
            Assert.AreEqual(eps.wdts[(int)wdt_type.CSP0].reboot_counter, (uint)0);
            Assert.AreEqual(eps.wdts[(int)wdt_type.GND].reboot_counter, (uint)0);
            Assert.AreEqual(eps.wdts[(int)wdt_type.I2C].reboot_counter, (uint)0);
            Assert.AreEqual(eps.wdts[(int)wdt_type.CSP1].reboot_counter, (uint)0);
        }

        [TestMethod()]
        public void RESET_WDTTest()
        {
            EPS eps = new EPS();
            eps.RESET_WDT(0x78);
            Assert.AreEqual(eps.wdts[(int)wdt_type.GND].time_ping_left, (uint)EPSConstants.WDT_GND_INIT_TIME);
        }

        [TestMethod()]
        public void CONFIG_CMDTest()
        {
            EPS eps = new EPS();
            eps.CONFIG_CMD(1);
            Assert.AreEqual(eps.config.ppt_mode, EPSConstants.FIXEDSWPPT);
            Assert.AreEqual(eps.config.battheater_mode, EPSConstants.AUTO);
            Assert.AreEqual(eps.config.battheater_high, 100); //to change
            Assert.AreEqual(eps.config.battheater_low, 0); //to change
            int i;
            for (i = 0; i < 3; i++)
                Assert.AreEqual(eps.config.vboost[i], EPSConstants.SOFTWARE_PPT_DEFAULT_V);
            for (i = 0; i < 8; i++)
            {
                Assert.AreEqual(eps.config.output_initial_off_delay[i], 1);
                Assert.AreEqual(eps.config.output_initial_on_delay[i], 1);
                Assert.AreEqual(eps.config.output_normal_value[i], 0); //need to change
                Assert.AreEqual(eps.config.output_safe_value[i], 0); //need to change
            }
        }

        /*[TestMethod()]
        public void CONFIG_GETTest()
        {
            EPS eps = new EPS();
            EPS.eps_config_t config = eps.CONFIG_GET();
            Assert.AreEqual(eps.config.ppt_mode, config.ppt_mode);
            Assert.AreEqual(eps.config.battheater_mode, config.battheater_mode);
            Assert.AreEqual(eps.config.battheater_low, config.battheater_low);
            Assert.AreEqual(eps.config.battheater_high, config.battheater_high);
            Assert.AreEqual(eps.config.battheater_mode, config.battheater_mode);
            int i;
            for (i = 0; i < 8; i++)
            {
                Assert.AreEqual(config.output_initial_off_delay[i], eps.config.output_initial_off_delay[i]);
                Assert.AreEqual(config.output_initial_on_delay[i], eps.config.output_initial_on_delay[i]);
                Assert.AreEqual(config.output_normal_value[i], eps.config.output_normal_value[i]);
                Assert.AreEqual(config.output_safe_value[i], eps.config.output_safe_value[i]);
            }
            for (i = 0; i < 3; i++)
                Assert.AreEqual(config.vboost[i], eps.config.vboost[i]);
        }*/

        /*[TestMethod()]
        public void CONFIG_SETTest()
        {
            EPS eps = new EPS();
            EPS.eps_config_t config = new EPS.eps_config_t();
            config.ppt_mode = 8;
            config.battheater_mode = 8;
            config.battheater_low = 8;
            config.battheater_high = 8;
            config.battheater_mode = 8;
            int i;
            for (i = 0; i < 8; i++)
            {
                config.output_initial_off_delay[i] = 8;
                config.output_initial_on_delay[i] = 8;
                config.output_normal_value[i] = 8;
                config.output_safe_value[i] = 8;
            }
            for (i = 0; i < 3; i++)
                config.vboost[i] = 8;

            eps.CONFIG_SET(config);
            Assert.AreEqual(eps.config.ppt_mode, config.ppt_mode);
            Assert.AreEqual(eps.config.battheater_mode, config.battheater_mode);
            Assert.AreEqual(eps.config.battheater_low, config.battheater_low);
            Assert.AreEqual(eps.config.battheater_high, config.battheater_high);
            Assert.AreEqual(eps.config.battheater_mode, config.battheater_mode);

            for (i = 0; i < 8; i++)
            {
                Assert.AreEqual(config.output_initial_off_delay[i], eps.config.output_initial_off_delay[i]);
                Assert.AreEqual(config.output_initial_on_delay[i], eps.config.output_initial_on_delay[i]);
                Assert.AreEqual(config.output_normal_value[i], eps.config.output_normal_value[i]);
                Assert.AreEqual(config.output_safe_value[i], eps.config.output_safe_value[i]);
            }
            for (i = 0; i < 3; i++)
                Assert.AreEqual(config.vboost[i], eps.config.vboost[i]);
        }*/

        [TestMethod()]
        public void HARD_RESETTest()
        {
            EPS eps = new EPS();
            eps.HARD_RESET();
            Assert.AreEqual(eps.last_reset_cause, EPSConstants.HARD_RESET_R);
        }

        /*[TestMethod()]
        public void CONFIG2_CMDTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CONFIG2_GETTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CONFIG2_SETTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
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