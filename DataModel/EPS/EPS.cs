using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.EPS
{
    public class EPS
    {
        private Channel[] channels;
	    private BoostConvertor[] boost_convertors;
	    private Battery onboard_battery;
	    private BatteryHeaters[] battery_heaters;
	    private ushort photo_current; //Total photo current [mA]
        private ushort system_current; //Total system current [mA]
        private ushort reboot_count; //Number of EPS reboots
        private ushort sw_errors; //Number of errors in the eps software
        private byte last_reset_cause; //Cause of last EPS reset
        private WDT[] wdts;
	    eps_config_t config;
        eps_config2_t config2;
        private ushort[] curout; //! Current out (switchable outputs) [mA]
        private byte kill_switch; //ON or OFF
        private byte charging; //ON or OFF

        public EPS()
        {

        }

        public struct hkparam_t
        {

            public ushort[] pv; //Photo-voltaic input voltage [mV]
            public ushort pc; //Total photo current [mA]
            public ushort bv; //Battery voltage [mV]
            public ushort sc; //Total system current [mA]
            public short[] temp; //Temp. of boost converters (1,2,3) and onboard battery [degC]
            public short[] batt_temp; //External board battery temperatures [degC];
            public ushort[] latchup; //Number of latch-ups on each output 5V and +3V3 channel

            //Order[5V1 5V2 5V3 3.3V1 3.3V2 3.3V3]
            //Transmit as 5V1 first and 3.3V3 last

            public byte reset; //Cause of last EPS reset
            public ushort bootcount; //Number of EPS reboots
            public ushort sw_errors; //Number of errors in the eps software
            public byte ppt_mode; //0 = Hardware, 1 = MPPT, 2 = Fixed SW PPT.
            public byte channel_status; //Mask of output channel status, 1=on, 0=off
            //MSB - [QH QS 3.3V3 3.3V2 3.3V1 5V3 5V2 5V1] - LSB
            // QH = Quadbat heater, QS = Quadbat switch
        };


        /**
         * Union for storing the block of telemetry values coming from the EPS. HK version 2.
         */
         public struct eps_hk_t
        {
            public ushort commandReply; //!< reply of the last command
            public ushort[] vboost; //!< Voltage of boost converters [mV] [PV1, PV2, PV3]
            public ushort vbatt; //!< Voltage of battery [mV]
            public ushort[] curin; //!< Current in [mA]
            public ushort cursun; //!< Current from boost converters
            public ushort cursys; //!< Current out of battery
            public ushort reserved1; //!< Reserved for future use
            public ushort[] curout; //!< Current out [mA]
            public byte[] output; //!< Status of outputs
            public ushort[] output_on_delta; //!< Time till power on for each channel
            public ushort[] output_off_delta; //!< Time till power off for each channel
            public ushort[] latchup; //!< Number of latch-ups for each channel
            public uint wdt_i2c_time_left; //!< Time left on I2C wdt
            public uint wdt_gnd_time_left; //!< Time left on I2C wdt
            public byte[] wdt_csp_pings_left; //!< Pings left on CSP wdt
            public uint counter_wdt_i2c; //!< Number of WDT I2C reboots
            public uint counter_wdt_gnd; //!< Number of WDT GND reboots
            public uint[] counter_wdt_csp; //!< Number of WDT CSP reboots
            public uint counter_boot; //!< Number of EPS reboots
            public short[] temp; //!< Temperature sensors [0 = TEMP1, TEMP2, TEMP3, TEMP4, BATT0, BATT1]
            public byte bootcause; //!< Cause of last EPS reset
            public byte battmode; //!< Mode for battery [0 = normal, 1 = undervoltage, 2 = overvoltage]
            public byte pptmode; //!< Mode of PPT tracker
            public ushort reserved2;
        }

        /**
         * Union for storing the block of telemetry values coming from the EPS. HK version 3.
         */
        public struct eps_hk_vi_t
        {
            public ushort commandReply;
            public ushort[] vboost; //! Voltage of boost converters [mV] [PV1, PV2, PV3]
            public ushort vbatt; //! Voltage of battery [mV]
            public ushort[] curin; //! Current in [mA]
            public ushort cursun; //! Current from boost converters [mA]
            public ushort cursys; //! Current out of battery [mA]
            public ushort reserved1; //! Reserved for future use
        };

        /**
         * Union for storing the block of telemetry values coming from the EPS. HK version 4.
         */
        public struct eps_hk_out_t
        {
            public ushort commandReply; //!< reply of the last command
            public ushort[] curout; //! Current out (switchable outputs) [mA]
            public byte[] output; //! Status of outputs**
            public ushort[] output_on_delta; //! Time till power on** [s]
            public ushort[] output_off_delta; //! Time till power off** [s]
            public ushort[] latchup; //! Number of latch-ups
        };

        /**
         * Union for storing the block of telemetry values coming from the EPS. HK version 5.
         */
        public struct eps_hk_wdt_t
        {
            public ushort commandReply; //!< reply of the last command
            public uint wdt_i2c_time_left; //! Time left on I2C wdt [s]
            public uint wdt_gnd_time_left; //! Time left on I2C wdt [s]
            public byte[] wdt_csp_pings_left;// = new int[2]; //! Pings left on CSP wdt
            public uint counter_wdt_i2c; //! Number of WDT I2C reboots
            public uint counter_wdt_gnd; //! Number of WDT GND reboots
            public uint[] counter_wdt_csp;// = new int[2]; //! Number of WDT CSP reboots
        };

        /**
         * Union for storing the block of telemetry values coming from the EPS. HK version 6.
         */
        public struct eps_hk_basic_t
        {
            public ushort commandReply; //!< reply of the last command
            public uint counter_boot; //! Number of EPS reboots
            public short[] temp; //! Temperatures [degC] [0 = TEMP1, TEMP2, TEMP3, TEMP4, BATT0, BATT1]
            public byte bootcause; //! Cause of last EPS reset
            public byte battmode; //! Mode for battery [0 = initial, 1 = undervoltage,2 = safemode, 3 = nominal, 4=full]

            public byte pptmode; //! Mode of PPT tracker [1=MPPT, 2=FIXED]
            public ushort reserved2;
        };

        /**
         * Union for storing the block of configuration values coming from the EPS.
         */
        public struct eps_config_t
        {
            public ushort commandReply; //!< reply of the last command
            public byte ppt_mode; //!< Mode for PPT [1 = AUTO, 2 = FIXED]
            public byte battheater_mode; //!< Mode for battheater [0 = Manual, 1 = Auto]
            public char battheater_low; //!< Turn heater on at [degC]
            public char battheater_high; //!< Turn heater off at [degC]
            public byte[] output_normal_value; //!< Nominal mode output value
            public byte[] output_safe_value; //!< Safe mode output value
            public ushort[] output_initial_on_delay; //!< Output switches: init with these on delays [s]
            public ushort[] output_initial_off_delay;//!< Output switches: init with these off delays [s]
            public ushort[] vboost; //!< Fixed PPT point for boost converters [mV]
        };

        /**
         * Union for storing the block of configuration values 2 coming from the EPS.
         */
        public struct eps_config2_t
        {
            public ushort commandReply; //!< reply of the last command
            public ushort batt_maxvoltage; //!< Maximum battery voltage
            public ushort batt_safevoltage; //!< Battery voltage for safe mode
            public ushort batt_criticalvoltage; //!< Battery voltage for critical mode
            public ushort batt_normalvoltage; //!< Battery voltage for normal mode
            public uint[] reserved1;
            public byte[] reserved2;
        }

        /*Send empty packet to request backwards compatible housekeeping struct. */
        public hkparam_t GET_HK_1()
        {
            hkparam_t ans = new hkparam_t();
            ans.temp = new short[4];
            ans.batt_temp = new short[2];
            ans.latchup = new ushort[6];
            ans.pc = photo_current;
            ans.bv = onboard_battery.vbat;
            ans.sc = system_current;
            int i;
            for (i = 0; i < 3; i++)
            {
                ans.temp[i] = boost_convertors[i].temperture;
            }  
            ans.temp[i] = onboard_battery.temperture;
            ans.batt_temp[0] = onboard_battery.temperture; // external - need to change
            ans.batt_temp[1] = onboard_battery.temperture; // external - need to change
            for (i = 0; i < 6; i++)
                ans.latchup[i] = channels[i].latchup;
            ans.reset = last_reset_cause;
            ans.bootcount = reboot_count;
            ans.sw_errors = sw_errors; //Number of errors in the eps software
            ans.ppt_mode = config.ppt_mode; //0 = Hardware, 1 = MPPT, 2 = Fixed SW PPT.
            byte channel_status = 0; //Mask of output channel status, 1=on, 0=off
            for (i = 0; i < 8; i++)
            {
                if (channels[i].status == 1) //need to be changed to 'ON'
                {
                    channel_status = (byte)(channel_status | (1 << i));
                }
            }
            ans.channel_status = channel_status;
            return ans;
        }

        public eps_hk_t GET_HK_2(byte type)
        {
            eps_hk_t ans = new eps_hk_t();
            return ans;
        }

        public eps_hk_vi_t GET_HK_2_VI(byte type)
        {
            eps_hk_vi_t ans = new eps_hk_vi_t();
            ans.vboost = new ushort[3];
            ans.curin = new ushort[3];
            int i;
            for (i = 0; i < 3; i++)
            {
                ans.vboost[i] = boost_convertors[i].volt;
                ans.curin[i] = boost_convertors[i].current_in;
            }
            ans.vbatt = onboard_battery.vbat;
            ans.cursys = system_current;
            ans.cursun = photo_current;
            return ans;
        }

        public eps_hk_out_t GET_HK_2_OUT(byte type)
        {
            eps_hk_out_t ans = new eps_hk_out_t();
            ans.curout = new ushort[6];
            ans.output = new byte[8];
            ans.output_on_delta = new ushort[8];
            ans.output_off_delta = new ushort[8];
            ans.latchup = new ushort[6];
            int i;
            for (i = 0; i < 8; i++)
            {
                ans.output[i] = channels[i].status;
                ans.output_off_delta[i] = config.output_initial_off_delay[i];
                ans.output_on_delta[i] = config.output_initial_on_delay[i];
            }
            for (i = 0; i < 6; i++)
            {
                ans.latchup[i] = channels[i].latchup;
                ans.curout[i] = curout[i];
            }
            System.Threading.Thread.Sleep(2000);
            return ans;
        }
     


    }
}
