using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.EPS
{
    public class EPSConstants
    {
        /* battery software level set points */
        public const int MAX_VBAT = 8300;
        public const int NORMAL_VBAT = 7400;
        public const int SAFE_VBAT = 7200;
        public const int CRITICAL_VBAT = 6500;

        /* ppt mode */
        public const int HARDWARE = 0;
        public const int MPPT = 1;
        public const int FIXEDSWPPT = 2;

        /* channels status */
        public const int ON = 1;
        public const int OFF = 0;

        /* battery type */
        public const int ONBOARD_BATT = 0;
        public const int EXTERNAL_BATT = 1;

        /* battery heater mode */
        public const int MANUAL = 0;
        public const int AUTO = 1;

        /* heater type */
        public const int BP4_HEATER = 0;
        public const int ONBOARD_HEATER = 1;
        public const int BOTH_HEATER = 2;

        /* causes of reset */
        public const int UNKNOWN_RESET_R = 0;
        public const int DEDICATED_WDT_RESET_R = 1;
        public const int I2C_WDT_RESET_R = 2;
        public const int HARD_RESET_R = 3;
        public const int SOFT_RESET_R = 4;
        public const int STACK_OVERFLOW_RESET_R = 5;
        public const int TIMER_OVERFLOW_RESET_R = 6;
        public const int BROWNOUT_RESET_R = 7;
        public const int INTERNAL_WDT_RESET_R = 8;


        public const int SOFTWARE_PPT_DEFAULT_V = 4000;

        public const int SWITCH_OFF_V = 6000;
        public const int SWITCH_ON_V = 6400;

        /*config default values*/
        public const int DEFAULT_CONFIG_PPT_MODE = FIXEDSWPPT;
        public const int DEFAULT_CONFIG_BATTHEAT_MODE = AUTO;
        public const int DEFAULT_CONFIG_BATTHEAT_LOW = 0;       //need to be changed
        public const int DEFAULT_CONFIG_BATTHEAT_HIGH = 100;    //need to be changed
        public const int DEFAULT_CONFIG_OUTPUT_NORMAL = 0;    //need to be changed
        public const int DEFAULT_CONFIG_OUTPUT_SAFE = 0;      //need to be changed
        public const int DEFAULT_CONFIG_OUTPUT_ON_DELAY = 1;      //need to be changed
        public const int DEFAULT_CONFIG_OUTPUT_OFF_DELAY = 1;       //need to be changed
        public const int DEFAULT_CONFIG_VBOOST = SOFTWARE_PPT_DEFAULT_V;

        /*parameters - min, max and typ values*/

        /*battery*/
        public const int BAT_CONNECT_V_MIN = 6000;
        public const int BAT_CONNECT_V_TYP = 7400;
        public const int BAT_CONNECT_V_MAX = 8400;
        public const int BAT_CONNECT_I_CHARGE_MAX = 6000;
        public const int BAT_CONNECT_I_DISCHARGE_THRESHOLD = 6800;

        /*pv inputs*/
        public const int PV_IN_V_MIN = 0;
        public const int PV_IN_V_TYP = 4200;
        public const int PV_IN_V_MAX = 8500;
        public const int PV_IN_I_CHARGE_MIN = 0;
        public const int PV_IN_I_CHARGE_MAX = 2000;

        /*5v in*/
        public const int IN_5V_V_MIN = 4100;
        public const int IN_5V_V_TYP = 5000;
        public const int IN_5V_V_MAX = 5000;
        public const int IN_5V_I_MIN = 0;
        public const int IN_5V_I_TYP = 900;
        public const int IN_5V_I_MAX = 1100;

        /*out 1-6 latchup protected*/
        public const int OUT_LATCHUP_PROTEC_5V_TYP = 4980;
        public const int OUT_LATCHUP_PROTEC_3_3V_TYP = 3300;
        public const int OUT_LATCHUP_PROTEC_I_MIN = 500;
        public const int OUT_LATCHUP_PROTEC_I_MAX = 3000;

        /*+5v*/
        public const int REG_OUT_5V_V_MIN = 4890;
        public const int REG_OUT_5V_V_TYP = 4980;
        public const int REG_OUT_5V_V_MAX = 5050;
        public const int REG_OUT_5V_I_MIN = 5;
        public const int REG_OUT_5V_I_MAX = 4000;

        /*+3.3v*/
        public const int REG_OUT_3V_V_MIN = 3290;
        public const int REG_OUT_3V_V_TYP = 3340;
        public const int REG_OUT_3V_V_MAX = 3390;
        public const int REG_OUT_3V_I_MIN = 0;
        public const int REG_OUT_3V_I_MAX = 5000;

        /*v_bat raw battery*/
        public const int V_BAT_V_MIN = 6000;
        public const int V_BAT_V_MAX = 8400;
        public const int V_BAT_I_OUT_TYP = 12000;

        /*power cosumption mW*/
        public const int POWER_CONSUMPTION_TYP = 115;

        /*Current consumed with separation switch OFF uA - micro???*/
        public const int SEPARATION_SWITCH_OFF_I_TYP = 35;
        public const int SEPARATION_SWITCH_OFF_I_MAX = 60;

        public const int SHELF_LIFE_MIN = 700;
        public const int SHELF_LIFE_TYP = 1400;


        /*batteries*/
        public const int EOCV = 4150; //end of charge voltage
        public const int DOD = 20; // Depth-Of-Discharge % -OURS


        /*OURS defualt values*/
        public const int DEFAULT_TEMP = 1;


        /*CMD*/
        public const int RESTORE_DEFAULT_CONFIG = 1;


        /*WDT*/
        /*I2C reset mode*/
        public const int I2C_WDT_RESET_0 = 0;
        public const int I2C_WDT_RESET_1 = 1;

        public const int WDT_I2C_INIT_TIME = 10; //sec
        public const int WDT_GND_INIT_TIME = 480; //sec - should be configurable to  48 hours in the real system
        public const int WDT_GND_HOUR = 300; //in sec - should be 1 hour
        public const int WDT_CSP_INIT_TIME = 60; //in sec, every 60 sec- one ping is send
        public const int WDT_CSP_INIT_PING = 5; //num of pings


    }
}
