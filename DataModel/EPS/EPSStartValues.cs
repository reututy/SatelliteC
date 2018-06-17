using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.EPS
{
    public class EPSStartValues
    {
        //output channel 5V1 for example
        public static byte OutputState = EPSConstants.ON; //always ?
        public static ushort OutputVolt5V = EPSConstants.OUT_LATCHUP_PROTEC_5V_TYP; //configurable
        public static ushort OutputVolt3_3V = EPSConstants.OUT_LATCHUP_PROTEC_3_3V_TYP; //configurable
        public static ushort OutputCurrOut = EPSConstants.OUT_LATCHUP_PROTEC_I_MAX;
        public static byte OutputSafe = EPSConstants.DEFAULT_CONFIG_OUTPUT_SAFE;
        public static byte OutputNormal = EPSConstants.DEFAULT_CONFIG_OUTPUT_NORMAL;
        public static ushort OutputInitialOffDelay = EPSConstants.DEFAULT_CONFIG_OUTPUT_OFF_DELAY;
        public static ushort OutputInitialOnDelay = EPSConstants.DEFAULT_CONFIG_OUTPUT_ON_DELAY;

        //boost converter
        public static short PVTemp = EPSConstants.DEFAULT_TEMP; //configurable
        public static ushort PVVolt = EPSConstants.SOFTWARE_PPT_DEFAULT_V; //configurable
        public static ushort PVCurrentIn = EPSConstants.PV_IN_I_CHARGE_MIN; //configurable or depand on sunny/not
        public static ushort PVVboost = EPSConstants.DEFAULT_CONFIG_VBOOST;
        public static PPTMode PPTmode = PPTMode.FIXED;

        //battery
        public static short BattTemp = EPSConstants.DEFAULT_TEMP; //configurable
        public static ushort BattVoltBat = EPSConstants.BAT_CONNECT_V_TYP; //configurable
        //ushort BattCurrentInBat ; //configurable or depand on sunny/not
        public static ushort BattCurrentOutBat = EPSConstants.V_BAT_I_OUT_TYP; //calculated

        //battery heater 
        public static HeaterMode BattHeaterMode = HeaterMode.AUTO; //configurable 0 = Manual, 1 = Auto]
        public static byte BattHeaterStatus = EPSConstants.OFF; //always? off? 0 = OFF 1 = ON
        public static byte BattHeaterLow = EPSConstants.DEFAULT_CONFIG_BATTHEAT_LOW;
        public static byte BattHeaterHigh = EPSConstants.DEFAULT_CONFIG_BATTHEAT_HIGH;

        //WDT
        //I2C
        public static uint I2CTimeLeft = EPSConstants.WDT_I2C_INIT_TIME;
        public static uint I2CData = EPSConstants.I2C_WDT_RESET_0;  //I2C- type of reset
        //GND
        public static uint GNDTimeLeft = EPSConstants.WDT_GND_INIT_TIME;
        public static uint GNDData = EPSConstants.WDT_GND_INIT_TIME; //GND- last hour
        //CSP
        public static uint CSPTimeLeft = EPSConstants.WDT_CSP_INIT_TIME;
        public static uint CSPPingLeft = EPSConstants.WDT_CSP_INIT_PING;
        public static uint CSP0Data = (int)OutputType.T_5V1; //CSP- channel connected
        public static uint CSP1Data = (int)OutputType.T_3_3V1; //CSP- channel connected

       






    }
}






//        photo_current = EPSConstants.BAT_CONNECT_I_CHARGE_MAX;
//            system_current = EPSConstants.V_BAT_I_OUT_TYP;
//            reboot_count = 0;
//            sw_errors = 0;
//            last_reset_cause = EPSConstants.UNKNOWN_RESET_R;

            
           

//            ushort[] output_initial_off_delay = new ushort[8];
//ushort[] output_initial_on_delay = new ushort[8];
//byte[] output_normal_value = new byte[8];
//byte[] output_safe_value = new byte[8];

//            for (i = 0; i< 8; i++)
//            {
//                output_initial_off_delay[i] = EPSConstants.DEFAULT_CONFIG_OUTPUT_ON_DELAY;
//                output_initial_on_delay[i] = EPSConstants.DEFAULT_CONFIG_OUTPUT_OFF_DELAY;
//                output_normal_value[i] = EPSConstants.DEFAULT_CONFIG_OUTPUT_NORMAL; //need to change
//                output_safe_value[i] = EPSConstants.DEFAULT_CONFIG_OUTPUT_SAFE; //need to change
//            }

//            config = new EPSConfiguration(EPSConstants.DEFAULT_CONFIG_PPT_MODE, EPSConstants.DEFAULT_CONFIG_BATTHEAT_MODE,
//                EPSConstants.DEFAULT_CONFIG_BATTHEAT_LOW, EPSConstants.DEFAULT_CONFIG_BATTHEAT_HIGH, output_normal_value, output_safe_value,
//                output_initial_on_delay, output_initial_off_delay, vboost);

//config.batt_safevoltage = EPSConstants.SAFE_VBAT;
//            config.batt_normalvoltage = EPSConstants.NORMAL_VBAT;
//            config.batt_maxvoltage = EPSConstants.MAX_VBAT;
//            config.batt_criticalvoltage = EPSConstants.CRITICAL_VBAT;

//            curout = new ushort[6];
//            for (i = 0; i< 6; i++)
//                curout[i] = EPSConstants.OUT_LATCHUP_PROTEC_I_MIN;

//            kill_switch = EPSConstants.ON;

//    }

