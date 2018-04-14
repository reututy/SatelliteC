using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataModel.EPS
{
    public class EPS
    {
        public Channel[] channels { get; set; }
        public BoostConvertor[] boost_convertors { get; set; }
        public Battery onboard_battery { get; set; }
        public BatteryHeaters[] battery_heaters { get; set; }
        public ushort photo_current { get; set; } //Total photo current [mA]
        public ushort system_current { get; set; } //Total system current [mA]
        public ushort reboot_count { get; set; } //Number of EPS reboots
        public ushort sw_errors { get; set; } //Number of errors in the eps software
        public byte last_reset_cause { get; set; } //Cause of last EPS reset
        public WDT[] wdts { get; set; }
        public EPSConfiguration config { get; set; }
        public ushort[] curout { get; set; } //! Current out (switchable outputs) [mA]
        public byte kill_switch { get; set; } //ON or OFF
        public byte charging { get; set; } //ON or OFF

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
            public byte battheater_low; //!< Turn heater on at [degC]
            public byte battheater_high; //!< Turn heater off at [degC]
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

        /*Send packet of length = 1 with type = 0 to request p31u-8 housekeeping struct.*/
        public eps_hk_t GET_HK_2(byte type)
        {
            eps_hk_t ans = new eps_hk_t();
            //TODO
            return ans;
        }

        /*Send packet of length = 1 with type = 1 to request voltage and current subset of HK_2 */
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

        /*Send packet of length = 1 with type = 2 to request output switch data subset of HK_2 */
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
            return ans;
        }

        /*Send packet of length = 1 with type = 3 to request wdt data subset of HK_2 */
        public eps_hk_wdt_t GET_HK_2_WDT(byte type)
        {
            eps_hk_wdt_t ans = new eps_hk_wdt_t();
            ans.counter_wdt_csp[0] = wdts[(int)wdt_type.CSP0].reboot_counter;
	        ans.counter_wdt_csp[1] = wdts[(int)wdt_type.CSP1].reboot_counter;
	        ans.counter_wdt_gnd = wdts[(int)wdt_type.GND].reboot_counter;
	        ans.counter_wdt_i2c = wdts[(int)wdt_type.I2C].reboot_counter;
	        ans.wdt_csp_pings_left[0] = (byte)wdts[(int)wdt_type.CSP0].time_ping_left;
	        ans.wdt_csp_pings_left[1] = (byte)wdts[(int)wdt_type.CSP1].time_ping_left;
	        ans.wdt_gnd_time_left = wdts[(int)wdt_type.GND].time_ping_left;
	        ans.wdt_i2c_time_left = wdts[(int)wdt_type.I2C].time_ping_left;
	        return ans;
        }

        /*Send packet of length = 1 with type = 4 to request the basic data subset of HK_2 */
        public eps_hk_basic_t GET_HK_2_BASIC(byte type)
        {
	        eps_hk_basic_t ans = new eps_hk_basic_t();
            ans.counter_boot = reboot_count;
	        ans.bootcause = last_reset_cause;
	        ans.pptmode = config.ppt_mode;
	        ans.battmode = (byte)onboard_battery.batt_mode;
	        for (int i = 0; i< 3; i++)
            {
		        ans.temp[i] = boost_convertors[i].temperture;
	        }
            ans.temp[3] = onboard_battery.temperture;
	        ans.temp[4] = onboard_battery.temperture; // external - need to change
	        ans.temp[5] = onboard_battery.temperture; // external - need to change
	        return ans;
        }

        /*Set output switch states by a bitmask where "1" means the channel
         * is switched on and "0" means it is switched off. LSB is channel 1,
         * next bit is channel 2 etc. (Quadbat switch and heater cannot be
         * controlled through this command) [NC NC 3.3V3 3.3V2 3.3V1 5V3 5V2 5V1] */
        public void SET_OUTPUT(byte output_byte) 
        {
	        for (int i = 0; i< 8; i++)
            {
                if ((output_byte & (1 << i)) != 0)
                    channels[i].status = EPSConstants.ON;
                else
                    channels[i].status = EPSConstants.OFF;
            }
        }

         /* Set output %channel% to value %value% with delay %dela%,
         * Channel (0-5), Quadbat  heater (6), Quadbat switch (7) Value 0 = Off, 1 = On Delay in seconds.*/
        public void SET_SINGLE_OUTPUT(byte channel, byte value, ushort delay)
        {
            Task.Factory.StartNew(() => Thread.Sleep(delay * 1000))
            .ContinueWith((t) =>
            {
                channels[channel].status = value;
            }, TaskScheduler.FromCurrentSynchronizationContext());
            
        }

        /*Set the voltage on the photo-voltaic inputs V1, V2, V3 in mV.
        * Takes effect when MODE = 2, See SET_PV_AUTO.  Transmit voltage1 first and voltage3 last.  */
        public void SET_PV_VOLT(ushort voltage1, ushort voltage2, ushort voltage3)
        {
            boost_convertors[0].volt = voltage1;
	        boost_convertors[1].volt = voltage2;
	        boost_convertors[2].volt = voltage3;
        }

        /*Sets the solar cell power tracking mode:
            * MODE = 0: Hardware default power point
            * MODE = 1: Maximum power point tracking
            * MODE = 2: Fixed software powerpoint,
            * value set with SET_PV_VOLT, default 4V*/
        public void SET_PV_AUTO(byte mode)
        {
        if (mode == EPSConstants.HARDWARE || mode == EPSConstants.MPPT || mode == EPSConstants.FIXEDSWPPT)
	        config.ppt_mode = mode;
        else
            throw new System.Exception("ERROR: mode param is not valid in SET_PV_AUTO\n");
        }

        /* Cmd = 0: Set heater on/off
         * Heater: 0 = BP4, 1= Onboard, 2 = Both
         * Mode: 0 = OFF, 1 = ON
         * Command replies with heater modes. 0=OFF, 1= ON.
         * To do only query, simple send an empty message.  */
        public ushort SET_HEATER(byte cmd, byte heater, byte mode)
        {
	        if (cmd != 0)
            {
                throw new System.Exception("ERROR: cmd param is not 0 in SET_HEATER\n");
            }
	        if (heater != EPSConstants.BP4_HEATER && heater != EPSConstants.ONBOARD_HEATER && heater != EPSConstants.BOTH_HEATER)
            {
                throw new System.Exception("ERROR: heater param is not 0/1/2 in SET_HEATER\n");
            }
        	if (mode != EPSConstants.ON && mode != EPSConstants.OFF)
            {
                throw new System.Exception("ERROR: mode param is not 0/1 in SET_HEATER\n");
            }
	        switch (heater)
            {
	            case EPSConstants.BP4_HEATER:
		            battery_heaters[EPSConstants.BP4_HEATER].status = mode;
		            break;
	            case EPSConstants.ONBOARD_HEATER:
		            battery_heaters[EPSConstants.ONBOARD_HEATER].status = mode;
		            break;
	            case EPSConstants.BOTH_HEATER:
		            battery_heaters[EPSConstants.BP4_HEATER].status = mode;
		            battery_heaters[EPSConstants.ONBOARD_HEATER].status = mode;
		            break;
	        }
            ushort ans = BitConverter.ToUInt16(new byte[2] { battery_heaters[EPSConstants.BP4_HEATER].status, battery_heaters[EPSConstants.ONBOARD_HEATER].status }, 0);	        
	        return ans;
        }

        /*Send this command to reset boot counter and WDT counters.  magic = 0x42 */
        public void RESET_COUNTERS(byte magic)
        {
	        if (magic != 0x42)
            {
                throw new System.Exception("ERROR: magic param is not 0x42 in RESET_COUNTERS\n");
            }
	        else
            {
		        reboot_count = 0;
		        wdts[(int)wdt_type.CSP0].reboot_counter = 0;
		        wdts[(int)wdt_type.CSP1].reboot_counter = 0;
		        wdts[(int)wdt_type.GND].reboot_counter = 0;
		        wdts[(int)wdt_type.I2C].reboot_counter = 0;
	        }
        }

        /*Send this command to reset (kick) dedicated WDT.  magic = 0x78*/
        public void RESET_WDT(byte magic) 
        {
	        if (magic != 0x78)
            {
                throw new System.Exception("ERROR: magic param is not 0x78 in RESET_WDT\n");
            }
            else
            {
                wdts[(int)wdt_type.GND].time_ping_left = EPSConstants.WDT_GND_INIT_TIME; //need to change;
            }
		        
        }

        /*Use this command to control the config system.  cmd=1: Restore default config*/
        public void CONFIG_CMD(byte cmd) 
        {
	        if (cmd != EPSConstants.RESTORE_DEFAULT_CONFIG)
            {
                throw new System.Exception("ERROR: cmd param is not 1(RESTORE_DEFAULT_CONFIG) in CONFIG_CMD\n");
            }
	        else
            {
		        config.ppt_mode = EPSConstants.DEFAULT_CONFIG_PPT_MODE;
		        config.battheater_mode = EPSConstants.DEFAULT_CONFIG_BATTHEAT_MODE;
		        config.battheater_high = EPSConstants.DEFAULT_CONFIG_BATTHEAT_HIGH;
		        config.battheater_low = EPSConstants.DEFAULT_CONFIG_BATTHEAT_LOW;
		        for (int i = 0; i< 3; i++)
                {
                    config.vboost[i] = EPSConstants.DEFAULT_CONFIG_VBOOST;
                }    
		        for (int i = 0; i< 8; i++)
                {
			        config.output_initial_off_delay[i] = EPSConstants.DEFAULT_CONFIG_OUTPUT_OFF_DELAY;
			        config.output_initial_on_delay[i] = EPSConstants.DEFAULT_CONFIG_OUTPUT_ON_DELAY;
			        config.output_normal_value[i] = EPSConstants.DEFAULT_CONFIG_OUTPUT_NORMAL;
			        config.output_safe_value[i] = EPSConstants.DEFAULT_CONFIG_OUTPUT_SAFE;
		        }
	        }

        }

        /*Use this command to request the NanoPower config*/
        eps_config_t CONFIG_GET()
        {
            eps_config_t ans = new eps_config_t();
            ans.ppt_mode = config.ppt_mode;
	        ans.battheater_mode = config.battheater_mode;
	        ans.battheater_low = config.battheater_low;
	        ans.battheater_high = config.battheater_high;	        
	        for (int i = 0; i< 8; i++)
            {
		        ans.output_initial_off_delay[i] = config.output_initial_off_delay[i];
		        ans.output_initial_on_delay[i] = config.output_initial_on_delay[i];
		        ans.output_normal_value[i] = config.output_normal_value[i];
		        ans.output_safe_value[i] = config.output_safe_value[i];
	        }
	        for (int i = 0; i< 3; i++)
            {
                ans.vboost[i] = config.vboost[i];
            }
	        return ans;
        }

        /*Use this command to send a config to the NanoPower and save it. */
        public void CONFIG_SET(eps_config_t eps_config) 
        {
	        config.ppt_mode = eps_config.ppt_mode;
	        config.battheater_mode = eps_config.battheater_mode;
	        config.battheater_low = eps_config.battheater_low;
	        config.battheater_high = eps_config.battheater_high;
	        int i;
	        for (i = 0; i< 8; i++)
            {
		        config.output_initial_off_delay[i] = eps_config.output_initial_off_delay[i];
		        config.output_initial_on_delay[i] = eps_config.output_initial_on_delay[i];
		        config.output_normal_value[i] = eps_config.output_normal_value[i];
		        config.output_safe_value[i] = eps_config.output_safe_value[i];
	        }
	        for (i = 0; i< 3; i++)
            {
                config.vboost[i] = eps_config.vboost[i];
            }
        }

        /*Send this command to perform a hard reset of NanoPower, including cycling permanent 5V and 3.3V and battery outputs.*/
        public void HARD_RESET() 
        {
	        /*It is possible to command the Nanopower to perform a hard-reset.
	         * This will switch off the kill-switch for 100ms and then on again.
	         * This will switch off power to all systems, both internal and external for 100ms.*/
	        kill_switch = EPSConstants.OFF;

            SET_OUTPUT(0);

            //delay_sec(0.1);
            Thread.Sleep(100);
            kill_switch = EPSConstants.ON;
	        last_reset_cause = EPSConstants.HARD_RESET_R;
	        //should run on different thread

	        //maybe I need to add something more to that
        }

        /*Use this command to control the config 2 system.
         * cmd=1: Restore default config
         * cmd=2: Confirm current config */
        void CONFIG2_CMD(byte cmd) 
        {
	        if (cmd != 1 && cmd != 2)
            {
                throw new System.Exception("ERROR: cmd param is not 1 or 2 in CONFIG2_CMD\n");
            }
	        else if (cmd == 1)
            {
		        config.batt_safevoltage = EPSConstants.SAFE_VBAT;
		        config.batt_normalvoltage = EPSConstants.NORMAL_VBAT;
		        config.batt_maxvoltage = EPSConstants.MAX_VBAT;
		        config.batt_criticalvoltage = EPSConstants.CRITICAL_VBAT;
	        }
            else //cmd == 2
            { 
			            //?????????????????????????????????????
	        }
        }

        /*Use this command to request the NanoPower config 2. */
        public eps_config2_t CONFIG2_GET()
        {
	        eps_config2_t ans = new eps_config2_t();
            ans.batt_safevoltage = config.batt_safevoltage;
	        ans.batt_normalvoltage = config.batt_normalvoltage;
	        ans.batt_maxvoltage = config.batt_maxvoltage;
	        ans.batt_criticalvoltage = config.batt_criticalvoltage;
	        return ans;
        }

        /*Use this command to send config 2 to the NanoPower and save it (remember to also confirm it)*/
        public void CONFIG2_SET(eps_config2_t eps_config2)
        {
	        config.batt_safevoltage = eps_config2.batt_safevoltage;
	        config.batt_normalvoltage = eps_config2.batt_normalvoltage;
	        config.batt_maxvoltage = eps_config2.batt_maxvoltage;
	        config.batt_criticalvoltage = eps_config2.batt_criticalvoltage;
        }

        /*The NanoPower replies with the same value as in the ping request. */
        public byte PING(byte value) {
	        return value;
        }

        /*The NanoPower is rebooted, so no reply is generated. A stop condition should be sent after the request, instead of the repeated START. */
        public void REBOOT(byte[] magic) {
	        if (magic.Length!=4 || magic[0] != 0x80 || magic[1] != 0x07 || magic[2] != 0x80 || magic[3] != 0x07)
            {
                throw new System.Exception("ERROR: magic param is not 0x80,0x07,0x80,0x07 in REBOOT\n");
            }
	        else
            {
		        //reboot
		        //TODO
		        if (wdts[(int)wdt_type.GND].data > EPSConstants.WDT_GND_HOUR)
                {
                    wdts[(int)wdt_type.GND].data -= EPSConstants.WDT_GND_HOUR;
                }
                else
                {
                    wdts[(int)wdt_type.GND].data = 0;
                }
	        }
}




}
}
