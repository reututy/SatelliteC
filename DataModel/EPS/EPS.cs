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
        public Output[] Outputs { get; set; }
        public BoostConverter[] BoostConverters { get; set; }
        public Battery OnboardBattery { get; set; }
        public BatteryHeater BatteryHeater { get; set; }
        //public ushort photo_current { get; set; } //Total photo current [mA]
        //public ushort system_current { get; set; } //Total system current [mA]
        public ushort RebootCount { get; set; } //Number of EPS reboots
        public ushort SwErrors { get; set; } //Number of errors in the eps software
        public byte LastResetCause { get; set; } //Cause of last EPS reset
        public WDT[] Wdts { get; set; }
        public EPSConfiguration CurrentConfig { get; set; }
        public EPSConfiguration DefaultConfig { get; set; }
        //public ushort[] curout { get; set; } //! Current out (switchable outputs) [mA]
        public byte KillSwitchStatus { get; set; } //ON or OFF
        public bool IsCharging { get; set; } //ON or OFF

        public EPS()
        {
            InitEps();
        }

       
        /// ///////////////////////////

        public void ChargingFlow()
        {
            if (IsCharging)
            {
                ushort totalCurrent = 0;
                for (int i = 0; i < 3; i++)
                {
                    totalCurrent += BoostConverters[i].CurrentIn;
                    OnboardBattery.CurrentIn = totalCurrent;
                }
            }
        }

        private void HardwareHighVoltProtection()
        {
            for (int i = 0; i < 3; i++)
            {
                BoostConverters[i].Volt = EPSConstants.PV_IN_V_MIN;
                BoostConverters[i].CurrentIn = EPSConstants.PV_IN_I_CHARGE_MIN;
            }
            IsCharging = false;
        }

        private void CheckBatteryState()
        {
            switch (OnboardBattery.BattState)
            {
                case batt_state.INITIAL:
                    if (OnboardBattery.Vbat < EPSConstants.CRITICAL_VBAT)
                    {
                        OnboardBattery.BattState = batt_state.CRITICAL;
                    }
                    else if (OnboardBattery.Vbat < EPSConstants.SAFE_VBAT)
                    {
                        OnboardBattery.BattState = batt_state.SAFE;
                    }
                    else if (OnboardBattery.Vbat < EPSConstants.MAX_VBAT)
                    {
                        OnboardBattery.BattState = batt_state.NORMAL;
                    }
                    else
                    {
                        OnboardBattery.BattState = batt_state.FULL;
                        HardwareHighVoltProtection();
                    }
                    break;
                case batt_state.CRITICAL:
                    //hardware LOW voltage protection will switch off the kill-switch
                    if (OnboardBattery.Vbat <= EPSConstants.SWITCH_ON_V)
                    {
                        KillSwitchStatus = EPSConstants.OFF;
                        SET_OUTPUT(0);
                    }
                    else
                    {
                        KillSwitchStatus = EPSConstants.ON;
                        SET_OUTPUT(54);
                    }
                    if (OnboardBattery.Vbat > EPSConstants.SAFE_VBAT)
                    {
                        OnboardBattery.BattState = batt_state.SAFE;
                    }
                    break;
                case batt_state.SAFE:
                    if (OnboardBattery.Vbat < EPSConstants.CRITICAL_VBAT)
                    {
                        OnboardBattery.BattState = batt_state.CRITICAL;
                    }
                    else if (OnboardBattery.Vbat > EPSConstants.NORMAL_VBAT)
                    {
                        OnboardBattery.BattState = batt_state.NORMAL;
                    }
                    break;
                case batt_state.NORMAL:
                    if (OnboardBattery.Vbat < EPSConstants.SAFE_VBAT)
                    {
                        OnboardBattery.BattState = batt_state.SAFE;
                    }
                    else if (OnboardBattery.Vbat > EPSConstants.MAX_VBAT)
                    {
                        OnboardBattery.BattState = batt_state.FULL;
                        HardwareHighVoltProtection();
                    }
                    break;
                case batt_state.FULL:
                    if (IsCharging)
                        HardwareHighVoltProtection();
                    if (OnboardBattery.Vbat < EPSConstants.MAX_VBAT)
                    {
                        OnboardBattery.BattState = batt_state.NORMAL;
                        IsCharging = true;
                    }
                    break;
            }
        }

        public void BatteryDrop()
        {
            OnboardBattery.Vbat -= 10; //need to be changed
            OnboardBattery.CurrentOut -= 10; //need to be changed
            CheckBatteryState();
        }

        private void i2c_wdt_timeout()
        {
	        //only cycle the 6 switchable outputs.
	        if (Wdts[(int)WdtType.I2C].Data == EPSConstants.I2C_WDT_RESET_0)
            {
                SET_OUTPUT(0);
            }
	        //do a hard-reset and thereby reset the NanoPower itself as well as all outputs including the permanent outputs
	        else if (Wdts[(int)WdtType.I2C].Data == EPSConstants.I2C_WDT_RESET_1)
            {
                HARD_RESET();
            }
        }

        //Any valid I2C communication to the EPS will kick (reset) this WDT.
        private void i2c_wdt_reset()
        {
	        Wdts[(int)WdtType.I2C].TimePingLeft = EPSConstants.WDT_I2C_INIT_TIME;
        }

        public void i2c_wdt_work()
        {
	        //while (true)
            //{
	            //The I2C watchdog does not run when NanoPower is in critical power mode.
		        if (OnboardBattery.BattState != batt_state.CRITICAL)
                {
			        //every 1 sec
			        Wdts[(int)WdtType.I2C].TimePingLeft -= 1;
			        if (Wdts[(int)WdtType.I2C].TimePingLeft == 0)

                        i2c_wdt_timeout();
		        }
                //Thread.Sleep(30000); //30 sec
	        //}
        }

        private void gnd_wdt_timeout()
        {
            // If no communication has been received for a long period of time (configurable by customer), NanoPower will switch off all outputs and do a reset
            HARD_RESET();
            //Note that if the dedicated WDT times out, the default config is restored on NanoPower
            CONFIG_CMD(EPSConstants.RESTORE_DEFAULT_CONFIG);
        }

        public void gnd_wdt_work()
        {
	        //while (true)
            //{
		        Wdts[(int)WdtType.GND].TimePingLeft -= 1;
		        //every integer hour it stores its hour value in persistent storage
		        if (Wdts[(int)WdtType.GND].TimePingLeft % EPSConstants.WDT_GND_HOUR == 0)
                {
                    Wdts[(int)WdtType.GND].Data = Wdts[(int)WdtType.GND].TimePingLeft;
                }
                if (Wdts[(int)WdtType.GND].TimePingLeft == 0)
                {
                    gnd_wdt_timeout();
                }
                //Thread.Sleep(1000); //1 sec
	        //}
        }

        //num of csp is 0 or 1
        public void csp_wdt_work(uint num_of_csp){
	        //while (true)
            //{
		        int i;
		        if (num_of_csp == 0)
                {
                    i = (int)WdtType.CSP0;
                }
                else
                {
                    i = (int)WdtType.CSP1;
                }
		        Wdts[i].TimePingLeft -= 1;
                if (Wdts[i].TimePingLeft == 0)
                {
                    SET_SINGLE_OUTPUT((byte)Wdts[i].Data, EPSConstants.OFF, 5);
                }
                //Thread.Sleep(1000); //1 sec
            //}
        }
        /// ///////////////////////////////


        private void InitEps()
        {
            Outputs = new Output[8];
            int i = 0;
            for (i = 0; i < 8; i++)
            {
                switch (i)
                {
                    case (int)OutputType.T_5V1:
                        Outputs[i] = new Channel(EPSConstants.ON, OutputType.T_5V1, EPSConstants.OUT_LATCHUP_PROTEC_5V_TYP, EPSConstants.OUT_LATCHUP_PROTEC_I_MAX, 0);
                        break;
                    case (int)OutputType.T_5V2:
                        Outputs[i] = new Channel(EPSConstants.ON, OutputType.T_5V2, EPSConstants.OUT_LATCHUP_PROTEC_5V_TYP, EPSConstants.OUT_LATCHUP_PROTEC_I_MAX, 0);
                        break;
                    case (int)OutputType.T_5V3:
                        Outputs[i] = new Channel(EPSConstants.ON, OutputType.T_5V3, EPSConstants.OUT_LATCHUP_PROTEC_5V_TYP, EPSConstants.OUT_LATCHUP_PROTEC_I_MAX, 0);
                        break;
                    case (int)OutputType.T_3_3V1:
                        Outputs[i] = new Channel(EPSConstants.ON, OutputType.T_3_3V1, EPSConstants.OUT_LATCHUP_PROTEC_3_3V_TYP, EPSConstants.OUT_LATCHUP_PROTEC_I_MAX, 0);
                        break;
                    case (int)OutputType.T_3_3V2:
                        Outputs[i] = new Channel(EPSConstants.ON, OutputType.T_3_3V2, EPSConstants.OUT_LATCHUP_PROTEC_3_3V_TYP, EPSConstants.OUT_LATCHUP_PROTEC_I_MAX, 0);
                        break;
                    case (int)OutputType.T_3_3V3:
                        Outputs[i] = new Channel(EPSConstants.ON, OutputType.T_3_3V3, EPSConstants.OUT_LATCHUP_PROTEC_3_3V_TYP, EPSConstants.OUT_LATCHUP_PROTEC_I_MAX, 0);
                        break;
                    case (int)OutputType.T_QS:
                        Outputs[i] = new Output(EPSConstants.ON, OutputType.T_QS, 0);
                        break;
                    case (int)OutputType.T_QH:
                        Outputs[i] = new Output(EPSConstants.ON, OutputType.T_QH, 0);
                        break;
                }
            }
            BoostConverters = new BoostConverter[3];
            for (i = 0; i < 3; i++)
            {
                BoostConverters[i] = new BoostConverter(EPSConstants.DEFAULT_TEMP, EPSConstants.SOFTWARE_PPT_DEFAULT_V, EPSConstants.PV_IN_I_CHARGE_MIN);
            }

            OnboardBattery = new Battery(EPSConstants.ONBOARD_BATT, EPSConstants.BAT_CONNECT_V_TYP, 0, EPSConstants.V_BAT_I_OUT_TYP, EPSConstants.DEFAULT_TEMP, batt_state.INITIAL, batt_mode.NORMAL);

            
            BatteryHeater = new BatteryHeater(EPSConstants.MANUAL, EPSConstants.ONBOARD_HEATER, EPSConstants.OFF, EPSConstants.DEFAULT_CONFIG_BATTHEAT_LOW, EPSConstants.DEFAULT_CONFIG_BATTHEAT_HIGH);
            

            //photo_current = EPSConstants.BAT_CONNECT_I_CHARGE_MAX;
            //system_current = EPSConstants.V_BAT_I_OUT_TYP;
            RebootCount = 0;
            SwErrors = 0;
            LastResetCause = EPSConstants.UNKNOWN_RESET_R;

            Wdts = new WDT[4];
            for (i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case (int)WdtType.I2C:
                        Wdts[i] = new WDT(WdtType.I2C,0, EPSConstants.WDT_I2C_INIT_TIME, EPSConstants.I2C_WDT_RESET_0);
                        break;
                    case (int)WdtType.GND:
                        Wdts[i] = new WDT(WdtType.GND, 0, EPSConstants.WDT_GND_INIT_TIME, EPSConstants.WDT_GND_INIT_TIME);
                        break;
                    case (int)WdtType.CSP0:
                        Wdts[i] = new WDT(WdtType.CSP0, 0, EPSConstants.WDT_CSP_INIT_PING, (int)OutputType.T_5V1);
                        break;
                    case (int)WdtType.CSP1:
                        Wdts[i] = new WDT(WdtType.CSP1, 0, EPSConstants.WDT_CSP_INIT_PING, (int)OutputType.T_3_3V1);
                        break;
                }
            }
            

            ushort[] vboost = new ushort[3];
            for (i = 0; i < 3; i++)
                vboost[i] = EPSConstants.DEFAULT_CONFIG_VBOOST;

            ushort[] output_initial_off_delay = new ushort[8];
            ushort[] output_initial_on_delay = new ushort[8];
            byte[] output_normal_value = new byte[8];
            byte[] output_safe_value = new byte[8];

            for (i = 0; i < 8; i++)
            {
                output_initial_off_delay[i] = EPSConstants.DEFAULT_CONFIG_OUTPUT_ON_DELAY;
                output_initial_on_delay[i] = EPSConstants.DEFAULT_CONFIG_OUTPUT_OFF_DELAY;
                output_normal_value[i] = EPSConstants.DEFAULT_CONFIG_OUTPUT_NORMAL; //need to change
                output_safe_value[i] = EPSConstants.DEFAULT_CONFIG_OUTPUT_SAFE; //need to change
            }

            CurrentConfig = new EPSConfiguration(EPSConstants.DEFAULT_CONFIG_PPT_MODE, EPSConstants.DEFAULT_CONFIG_BATTHEAT_MODE,
                EPSConstants.DEFAULT_CONFIG_BATTHEAT_LOW, EPSConstants.DEFAULT_CONFIG_BATTHEAT_HIGH, output_normal_value, output_safe_value,
                output_initial_on_delay, output_initial_off_delay, vboost);

            CurrentConfig.BattSafeVoltage = EPSConstants.SAFE_VBAT;
            CurrentConfig.BattNormalVoltage = EPSConstants.NORMAL_VBAT;
            CurrentConfig.BattMaxVoltage = EPSConstants.MAX_VBAT;
            CurrentConfig.BattCriticalVoltage = EPSConstants.CRITICAL_VBAT;

            //curout = new ushort[6];
            //for (i = 0; i < 6; i++)
            //    curout[i] = EPSConstants.OUT_LATCHUP_PROTEC_I_MIN;

            KillSwitchStatus = EPSConstants.ON;
            IsCharging = false;

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
            ushort photoCurrent = 0;
            
            ans.bv = OnboardBattery.Vbat;
            ans.sc = OnboardBattery.CurrentOut;
            int i;
            for (i = 0; i < 3; i++)
            {
                ans.temp[i] = BoostConverters[i].Temperture;
                photoCurrent += BoostConverters[i].CurrentOut;
            }
            ans.pc = photoCurrent;
            ans.temp[i] = OnboardBattery.Temperture;
            ans.batt_temp[0] = OnboardBattery.Temperture; // external - need to change
            ans.batt_temp[1] = OnboardBattery.Temperture; // external - need to change
            for (i = 0; i < 6; i++)
                ans.latchup[i] = ((Channel)Outputs[i]).LatchupNum;
            ans.reset = LastResetCause;
            ans.bootcount = RebootCount;
            ans.sw_errors = SwErrors; //Number of errors in the eps software
            ans.ppt_mode = CurrentConfig.PptMode; //0 = Hardware, 1 = MPPT, 2 = Fixed SW PPT.
            byte channel_status = 0; //Mask of output channel status, 1=on, 0=off
            for (i = 0; i < 8; i++)
            {
                if (Outputs[i].Status == 1) //need to be changed to 'ON'
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
            ans.vboost = new ushort[3];
            ans.curin = new ushort[3];
            ans.curout = new ushort[6];
            ans.output = new byte[8];
            ans.output_off_delta = new ushort[8];
            ans.output_on_delta = new ushort[8];
            ans.latchup = new ushort[6];
            ans.wdt_csp_pings_left = new byte[2];
            ans.counter_wdt_csp = new uint[2];
            ans.temp = new short[6];
            ushort photoCurrent = 0;
            int i;
            for (i = 0; i < 3; i++)
            {
                ans.vboost[i] = BoostConverters[i].Volt;
                ans.curin[i] = BoostConverters[i].CurrentIn;
                photoCurrent += BoostConverters[i].CurrentOut;
            }
            ans.vbatt = OnboardBattery.Vbat;
            ans.cursun = photoCurrent;
            ans.cursys = OnboardBattery.CurrentOut;
            for (i = 0; i < 6; i++)
            {
                ans.curout[i] = ((Channel)Outputs[i]).CurrentOut;
                ans.latchup[i] = ((Channel)Outputs[i]).LatchupNum;
            }
            for (i = 0; i < 8; i++)
            {
                ans.output[i] =  Outputs[i].Status;
                ans.output_on_delta[i] =  CurrentConfig.OutputInitialOnDelay[i];
                ans.output_off_delta[i] =  CurrentConfig.OutputInitialOffDelay[i];
            }   
            ans.wdt_csp_pings_left[0] =  (byte)Wdts[(int)WdtType.CSP0].TimePingLeft;
            ans.wdt_csp_pings_left[1] =  (byte)Wdts[(int)WdtType.CSP1].TimePingLeft;
            ans.wdt_gnd_time_left =  Wdts[(int)WdtType.GND].TimePingLeft;
            ans.wdt_i2c_time_left =  Wdts[(int)WdtType.I2C].TimePingLeft;
            ans.counter_boot =  RebootCount;
            ans.counter_wdt_csp[0] =  Wdts[(int)WdtType.CSP0].RebootCounter;
            ans.counter_wdt_csp[1] =  Wdts[(int)WdtType.CSP1].RebootCounter;
            ans.counter_wdt_gnd =  Wdts[(int)WdtType.GND].RebootCounter;
            ans.counter_wdt_i2c =  Wdts[(int)WdtType.I2C].RebootCounter;
            for (i = 0; i < 3; i++)
                ans.temp[i] =  BoostConverters[i].Temperture;
            ans.temp[3] =  OnboardBattery.Temperture;
            ans.temp[4] =  OnboardBattery.Temperture; // external - need to change
            ans.temp[5] =  OnboardBattery.Temperture; // external - need to change
            ans.bootcause =  LastResetCause;
            ans.battmode =  (byte)OnboardBattery.BattMode;
            ans.pptmode =  CurrentConfig.PptMode;
            return ans;
        }

        /*Send packet of length = 1 with type = 1 to request voltage and current subset of HK_2 */
        public eps_hk_vi_t GET_HK_2_VI(byte type)
        {
            eps_hk_vi_t ans = new eps_hk_vi_t();
            ans.vboost = new ushort[3];
            ans.curin = new ushort[3];
            ushort photoCurrent = 0;
            int i;
            for (i = 0; i < 3; i++)
            {
                ans.vboost[i] = BoostConverters[i].Volt;
                ans.curin[i] = BoostConverters[i].CurrentIn;
                photoCurrent += BoostConverters[i].CurrentOut;
            }
            ans.vbatt = OnboardBattery.Vbat;
            ans.cursys = OnboardBattery.CurrentOut;
            ans.cursun = photoCurrent;
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
                ans.output[i] = Outputs[i].Status;
                ans.output_off_delta[i] = CurrentConfig.OutputInitialOffDelay[i];
                ans.output_on_delta[i] = CurrentConfig.OutputInitialOnDelay[i];
            }
            for (i = 0; i < 6; i++)
            {
                ans.latchup[i] = ((Channel)Outputs[i]).LatchupNum;
                ans.curout[i] = ((Channel)Outputs[i]).CurrentOut;
            }
            return ans;
        }

        /*Send packet of length = 1 with type = 3 to request wdt data subset of HK_2 */
        public eps_hk_wdt_t GET_HK_2_WDT(byte type)
        {
            eps_hk_wdt_t ans = new eps_hk_wdt_t();
            ans.counter_wdt_csp = new uint[2];
            ans.wdt_csp_pings_left = new byte[2];
            ans.counter_wdt_csp[0] = Wdts[(int)WdtType.CSP0].RebootCounter;
	        ans.counter_wdt_csp[1] = Wdts[(int)WdtType.CSP1].RebootCounter;
	        ans.counter_wdt_gnd = Wdts[(int)WdtType.GND].RebootCounter;
	        ans.counter_wdt_i2c = Wdts[(int)WdtType.I2C].RebootCounter;
	        ans.wdt_csp_pings_left[0] = (byte)Wdts[(int)WdtType.CSP0].TimePingLeft;
	        ans.wdt_csp_pings_left[1] = (byte)Wdts[(int)WdtType.CSP1].TimePingLeft;
	        ans.wdt_gnd_time_left = Wdts[(int)WdtType.GND].TimePingLeft;
	        ans.wdt_i2c_time_left = Wdts[(int)WdtType.I2C].TimePingLeft;
	        return ans;
        }

        /*Send packet of length = 1 with type = 4 to request the basic data subset of HK_2 */
        public eps_hk_basic_t GET_HK_2_BASIC(byte type)
        {
	        eps_hk_basic_t ans = new eps_hk_basic_t();
            ans.counter_boot = RebootCount;
	        ans.bootcause = LastResetCause;
	        ans.pptmode = CurrentConfig.PptMode;
	        ans.battmode = (byte)OnboardBattery.BattMode;
            ans.temp = new short[6];
	        for (int i = 0; i< 3; i++)
            {   
		        ans.temp[i] = BoostConverters[i].Temperture;
	        }
            ans.temp[3] = OnboardBattery.Temperture;
	        ans.temp[4] = OnboardBattery.Temperture; // external - need to change
	        ans.temp[5] = OnboardBattery.Temperture; // external - need to change
	        return ans;
        }

        /*Set output switch states by a bitmask where "1" means the channel
         * is switched on and "0" means it is switched off. LSB is channel 1,
         * next bit is channel 2 etc. (Quadbat switch and heater cannot be
         * controlled through this command) [NC NC 3.3V3 3.3V2 3.3V1 5V3 5V2 5V1] */
        public void SET_OUTPUT(byte output_byte) 
        {
            //?? if less than 2 channels - invalid action
            for (int i = 0; i< 8; i++)
            {
                if ((output_byte & (1 << i)) != 0)
                    Outputs[i].Status = EPSConstants.ON;
                else
                    Outputs[i].Status = EPSConstants.OFF;
            }
        }

         /* Set output %channel% to value %value% with delay %dela%,
         * Channel (0-5), Quadbat  heater (6), Quadbat switch (7) Value 0 = Off, 1 = On Delay in seconds.*/
        public void SET_SINGLE_OUTPUT(byte channel, byte value, ushort delay)
        {
            //?? if less than 2 channels - invalid action
            //Task.Factory.StartNew(() => Thread.Sleep(delay * 1000))
            //.ContinueWith((t) =>
            //{
                Outputs[channel].Status = value;
            //}, TaskScheduler.FromCurrentSynchronizationContext());
            
        }

        /*Set the voltage on the photo-voltaic inputs V1, V2, V3 in mV.
        * Takes effect when MODE = 2, See SET_PV_AUTO.  Transmit voltage1 first and voltage3 last.  */
        public void SET_PV_VOLT(ushort voltage1, ushort voltage2, ushort voltage3)
        {
            BoostConverters[0].Volt = voltage1;
	        BoostConverters[1].Volt = voltage2;
	        BoostConverters[2].Volt = voltage3;
        }

        /*Sets the solar cell power tracking mode:
            * MODE = 0: Hardware default power point
            * MODE = 1: Maximum power point tracking
            * MODE = 2: Fixed software powerpoint,
            * value set with SET_PV_VOLT, default 4V*/
        public void SET_PV_AUTO(byte mode)
        {
        if (mode == EPSConstants.HARDWARE || mode == EPSConstants.MPPT || mode == EPSConstants.FIXEDSWPPT)
	        CurrentConfig.PptMode = mode;
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
	        /*switch (heater)
            {
	            case EPSConstants.BP4_HEATER:
		            BatteryHeater[EPSConstants.BP4_HEATER].Status = mode;
		            break;
	            case EPSConstants.ONBOARD_HEATER:
		            BatteryHeater[EPSConstants.ONBOARD_HEATER].Status = mode;
		            break;
	            case EPSConstants.BOTH_HEATER:
		            BatteryHeater[EPSConstants.BP4_HEATER].Status = mode;
		            BatteryHeater[EPSConstants.ONBOARD_HEATER].Status = mode;
		            break;
	        }*/
            if (heater == EPSConstants.ONBOARD_HEATER)
            {
                BatteryHeater.Status = mode;
            }
            //ushort ans = BitConverter.ToUInt16(new byte[2] { BatteryHeater[EPSConstants.BP4_HEATER].Status, BatteryHeater[EPSConstants.ONBOARD_HEATER].Status }, 0);	        
            ushort ans = BitConverter.ToUInt16(new byte[2] { (byte)EPSConstants.OFF, BatteryHeater.Status }, 0);
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
		        RebootCount = 0;
		        Wdts[(int)WdtType.CSP0].RebootCounter = 0;
		        Wdts[(int)WdtType.CSP1].RebootCounter = 0;
		        Wdts[(int)WdtType.GND].RebootCounter = 0;
		        Wdts[(int)WdtType.I2C].RebootCounter = 0;
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
                Wdts[(int)WdtType.GND].TimePingLeft = EPSConstants.WDT_GND_INIT_TIME; //need to change;
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
		        CurrentConfig.PptMode = EPSConstants.DEFAULT_CONFIG_PPT_MODE;
		        CurrentConfig.BattheaterMode = EPSConstants.DEFAULT_CONFIG_BATTHEAT_MODE;
		        CurrentConfig.BattheaterHigh = EPSConstants.DEFAULT_CONFIG_BATTHEAT_HIGH;
		        CurrentConfig.BattheaterLow = EPSConstants.DEFAULT_CONFIG_BATTHEAT_LOW;
		        for (int i = 0; i< 3; i++)
                {
                    CurrentConfig.Vboost[i] = EPSConstants.DEFAULT_CONFIG_VBOOST;
                }    
		        for (int i = 0; i< 8; i++)
                {
			        CurrentConfig.OutputInitialOffDelay[i] = EPSConstants.DEFAULT_CONFIG_OUTPUT_OFF_DELAY;
			        CurrentConfig.OutputInitialOnDelay[i] = EPSConstants.DEFAULT_CONFIG_OUTPUT_ON_DELAY;
			        CurrentConfig.OutputNormalValue[i] = EPSConstants.DEFAULT_CONFIG_OUTPUT_NORMAL;
			        CurrentConfig.OutputSafeValue[i] = EPSConstants.DEFAULT_CONFIG_OUTPUT_SAFE;
		        }
	        }

        }

        /*Use this command to request the NanoPower config*/
        public eps_config_t CONFIG_GET()
        {
            eps_config_t ans = new eps_config_t();
            ans.ppt_mode = CurrentConfig.PptMode;
	        ans.battheater_mode = CurrentConfig.BattheaterMode;
	        ans.battheater_low = CurrentConfig.BattheaterLow;
	        ans.battheater_high = CurrentConfig.BattheaterHigh;
            ans.output_initial_off_delay = new ushort[8];
            ans.output_initial_on_delay = new ushort[8];
            ans.output_normal_value = new byte[8];
            ans.output_safe_value = new byte[8];
	        for (int i = 0; i< 8; i++)
            {
		        ans.output_initial_off_delay[i] = CurrentConfig.OutputInitialOffDelay[i];
		        ans.output_initial_on_delay[i] = CurrentConfig.OutputInitialOnDelay[i];
		        ans.output_normal_value[i] = CurrentConfig.OutputNormalValue[i];
		        ans.output_safe_value[i] = CurrentConfig.OutputSafeValue[i];
	        }
            ans.vboost = new ushort[3];
	        for (int i = 0; i< 3; i++)
            {
                ans.vboost[i] = CurrentConfig.Vboost[i];
            }
	        return ans;
        }

        /*Use this command to send a config to the NanoPower and save it. */
        public void CONFIG_SET(eps_config_t eps_config) 
        {
	        CurrentConfig.PptMode = eps_config.ppt_mode;
	        CurrentConfig.BattheaterMode = eps_config.battheater_mode;
	        CurrentConfig.BattheaterLow = eps_config.battheater_low;
	        CurrentConfig.BattheaterHigh = eps_config.battheater_high;
	        int i;
	        for (i = 0; i< 8; i++)
            {
		        CurrentConfig.OutputInitialOffDelay[i] = eps_config.output_initial_off_delay[i];
		        CurrentConfig.OutputInitialOnDelay[i] = eps_config.output_initial_on_delay[i];
		        CurrentConfig.OutputNormalValue[i] = eps_config.output_normal_value[i];
		        CurrentConfig.OutputSafeValue[i] = eps_config.output_safe_value[i];
	        }
	        for (i = 0; i< 3; i++)
            {
                CurrentConfig.Vboost[i] = eps_config.vboost[i];
            }
        }

        /*Send this command to perform a hard reset of NanoPower, including cycling permanent 5V and 3.3V and battery outputs.*/
        public void HARD_RESET() 
        {
	        /*It is possible to command the Nanopower to perform a hard-reset.
	         * This will switch off the kill-switch for 100ms and then on again.
	         * This will switch off power to all systems, both internal and external for 100ms.*/
	        KillSwitchStatus = EPSConstants.OFF;

            SET_OUTPUT(0);

            //delay_sec(0.1);
            Thread.Sleep(100);
            KillSwitchStatus = EPSConstants.ON;
	        LastResetCause = EPSConstants.HARD_RESET_R;
	        //should run on different thread

	        //maybe I need to add something more to that
        }

        /*Use this command to control the config 2 system.
         * cmd=1: Restore default config
         * cmd=2: Confirm current config */
        public void CONFIG2_CMD(byte cmd) 
        {
	        if (cmd != 1 && cmd != 2)
            {
                throw new System.Exception("ERROR: cmd param is not 1 or 2 in CONFIG2_CMD\n");
            }
	        else if (cmd == 1)
            {
		        CurrentConfig.BattSafeVoltage = EPSConstants.SAFE_VBAT;
		        CurrentConfig.BattNormalVoltage = EPSConstants.NORMAL_VBAT;
		        CurrentConfig.BattMaxVoltage = EPSConstants.MAX_VBAT;
		        CurrentConfig.BattCriticalVoltage = EPSConstants.CRITICAL_VBAT;
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
            ans.batt_safevoltage = CurrentConfig.BattSafeVoltage;
	        ans.batt_normalvoltage = CurrentConfig.BattNormalVoltage;
	        ans.batt_maxvoltage = CurrentConfig.BattMaxVoltage;
	        ans.batt_criticalvoltage = CurrentConfig.BattCriticalVoltage;
	        return ans;
        }

        /*Use this command to send config 2 to the NanoPower and save it (remember to also confirm it)*/
        public void CONFIG2_SET(eps_config2_t eps_config2)
        {
	        CurrentConfig.BattSafeVoltage = eps_config2.batt_safevoltage;
	        CurrentConfig.BattNormalVoltage = eps_config2.batt_normalvoltage;
	        CurrentConfig.BattMaxVoltage = eps_config2.batt_maxvoltage;
	        CurrentConfig.BattCriticalVoltage = eps_config2.batt_criticalvoltage;
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
		        if (Wdts[(int)WdtType.GND].Data > EPSConstants.WDT_GND_HOUR)
                {
                    Wdts[(int)WdtType.GND].Data -= EPSConstants.WDT_GND_HOUR;
                }
                else
                {
                    Wdts[(int)WdtType.GND].Data = 0;
                }
	        }
        }



    }
}
