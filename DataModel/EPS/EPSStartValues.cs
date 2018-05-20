using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.EPS
{
    class EPSStartValues
    {
        //output channel 5V1 for example
        byte state5V1; //always ?
        ushort volt5V1; //configurable
        ushort latchupNum5V1; //always 0

        //boost converter
        short tempPV; //configurable
        ushort voltPV; //configurable
        ushort currentInPV; //configurable or depand on sunny/not
        ushort currentOutPV; //calculated

        //boost converter
        short tempBat; //configurable
        ushort voltBat; //configurable
        ushort currentInBat; //configurable or depand on sunny/not
        ushort currentOutBat; //calculated

        //battery heater 
        byte modeHeater; //configurable 0 = Manual, 1 = Auto]
        byte typeHeater; //default - on board? 0 = BP4, 1= Onboard
        byte statusHeater; //always? off? 0 = OFF 1 = ON
        sbyte BattHeaterLow; //configurable
        sbyte BattHeaterHigh; //configurable

        //battery


        //wdt
        uint RebootCounter; //always 0
        uint TimePingLeft; //configurable
        uint Data; //I2C- type of reset, GND- last hour, CSP- channel connected







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

