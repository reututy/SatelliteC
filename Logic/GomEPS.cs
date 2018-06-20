using DataModel;
using DataModel.EPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class GomEPS
    {
        public EPS[] eps_table;
        int eps_num; //number of EPSs
        public static List<String> logs = new List<String>();

        /**
         * 	Initialize the GOMSpace EPS with the corresponding i2cAddress. This function can only be called once.
         *
         * 	@param[in] i2c_address array of GOMSpace EPS I2C bus address
         * 	@param[in] number number of attached EPS in the system to be initialized
         * 	@return Error code according to <hal/errors.h>
         */

        public int GomEpsInitialize(byte i2c_address, byte number)
        {
            logs.Add(DateTime.Now + " IsisTrxvu_initialize");
            logs.Add("address: " + i2c_address + ", number: " + number);
            if (eps_table == null)
            {
                if (number > 0)
                {
                    eps_num = number;
                    eps_table = new EPS[eps_num];
                    for (int i = 0; i < eps_num; i++)
                    {
                        eps_table[i] = new EPS();
                    }
                    logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR]);
                    return Constants.E_NO_SS_ERR;
                }
                else
                {
                    logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INDEX_ERROR]);
                    return Constants.E_INDEX_ERROR;
                }
            }
            else
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_IS_INITIALIZED]);
                return Constants.E_IS_INITIALIZED;
            }
        }

        /**
         *	Send a ping to the GOMSpace EPS
         *
         *	@param[in] index index of GOMSpace EPS I2C bus address
         *	@param[in] ping_byte a byte used to ping the GOMSpace EPS
         *	@param[out] ping_byte_out byte returned from GOMSpace EPS as a response
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsPing(byte index, byte ping_byte, Output<Byte> byte_out)
        {
            logs.Add(DateTime.Now + " GomEpsPing");
            logs.Add("index: " + index + ", ping_byt: " + ping_byte);
            if (eps_table == null)
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NOT_INITIALIZED]);
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                byte_out.output = eps_table[index].PING(ping_byte);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR] + ", output: " + byte_out.output);
                return Constants.E_NO_SS_ERR;
            }
            else
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INDEX_ERROR]);
                return Constants.E_INDEX_ERROR;
            }
        }

        /**
         * 	Software Reset on the GOMSpace EPS based on the index.
         *
         * 	@param[in] index index of GOMSpace EPS I2C bus address
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsSoftReset(byte index)
        {
            logs.Add(DateTime.Now + " GomEpsSoftReset");
            logs.Add("index: " + index);
            if (eps_table == null)
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NOT_INITIALIZED]);
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                /// TODO - where is the function soft reset.
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR]);
                return Constants.E_NO_SS_ERR;
            }
            else
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INDEX_ERROR]);
                return Constants.E_INDEX_ERROR;
            }
        }

        /**
         * 	Hardware Reset on the GOMSpace EPS based on the index.
         *
         * 	@param[in] index index of GOMSpace EPS I2C bus address
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsHardReset(byte index)
        {
            logs.Add(DateTime.Now + " GomEpsHardReset");
            logs.Add("index: " + index);
            if (eps_table == null)
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NOT_INITIALIZED]);
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                eps_table[index].HARD_RESET();
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR]);
                return Constants.E_NO_SS_ERR;
            }
            else
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INDEX_ERROR]);
                return Constants.E_INDEX_ERROR;
            }
        }

        /**
         *	Read back the current housekeeping data from GOMSpace EPS
         *
         *	@param[in] index index of GOMSpace EPS I2C bus address
         *	@param[out] data_out housekeeping output of GOMSpace EPS
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsGetHkData_param(byte index, Output<EPS.hkparam_t> data_out)
        {
            logs.Add(DateTime.Now + " GomEpsGetHkData_param");
            logs.Add("index: " + index);
            if (eps_table == null)
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NOT_INITIALIZED]);
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                data_out.output = eps_table[index].GET_HK_1();
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR] + ", output: " + data_out.output);
                return Constants.E_NO_SS_ERR;
            }
            else
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INDEX_ERROR]);
                return Constants.E_INDEX_ERROR;
            }
        }

        


        /**
         *	Read back the current housekeeping data from GOMSpace EPS
         *
         *	@param[in] index index of GOMSpace EPS I2C bus address
         *	@param[out] data_out housekeeping output of GOMSpace EPS. p31u-8 format.
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsGetHkData_general(byte index, Output<EPS.eps_hk_t> data_out)
        {
            logs.Add(DateTime.Now + " GomEpsGetHkData_general");
            logs.Add("index: " + index);
            if (eps_table == null)
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NOT_INITIALIZED]);
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                //struct eps_hk_t data =
                // TODO - raw?
                data_out.output = eps_table[index].GET_HK_2(0);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR] + ", output: " + data_out.output);
                return Constants.E_NO_SS_ERR;
            }
            else
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INDEX_ERROR]);
                return Constants.E_INDEX_ERROR;
            }
        }

        /**
         *	Read back the current housekeeping data from GOMSpace EPS
         *
         * @param[in] index index of GOMSpace EPS I2C bus address
         *	@param[out] data_out housekeeping output of GOMSpace EPS.Voltage and Current subset.
         * 	@return Error code according to<hal/errors.h>
         */
        public int GomEpsGetHkData_vi(byte index, Output<EPS.eps_hk_vi_t> data_out)
        {
            logs.Add(DateTime.Now + " GomEpsGetHkData_vi");
            logs.Add("index: " + index);
            if (eps_table == null)
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NOT_INITIALIZED]);
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                //struct eps_hk_t data =
                // TODO - raw? , type = 1.
                data_out.output = eps_table[index].GET_HK_2_VI(1);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR] + ", output: " + data_out.output);
                return Constants.E_NO_SS_ERR;
            }
            else
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INDEX_ERROR]);
                return Constants.E_INDEX_ERROR;
            }
        }

        /**
         *	Read back the current housekeeping data from GOMSpace EPS
         *
         *	@param[in] index index of GOMSpace EPS I2C bus address
         *	@param[out] data_out housekeeping output of GOMSpace EPS. Output switch data subset.
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsGetHkData_out(byte index, Output<EPS.eps_hk_out_t> data_out)
        {
            logs.Add(DateTime.Now + " GomEpsGetHkData_out");
            logs.Add("index: " + index);
            if (eps_table == null)
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NOT_INITIALIZED]);
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                //struct eps_hk_t data =
                // TODO - raw? , type = 2.
                data_out.output = eps_table[index].GET_HK_2_OUT(2);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR] + ", output: " + data_out.output);
                return Constants.E_NO_SS_ERR;
            }
            else
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INDEX_ERROR]);
                return Constants.E_INDEX_ERROR;
            }
        }

        /**
         *	Read back the current housekeeping data from GOMSpace EPS
         *
         *	@param[in] index index of GOMSpace EPS I2C bus address
         *	@param[out] data_out housekeeping output of GOMSpace EPS. WDT data subset.
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsGetHkData_wdt(byte index, Output<EPS.eps_hk_wdt_t> data_out)
        {
            logs.Add(DateTime.Now + " GomEpsGetHkData_wdt");
            logs.Add("index: " + index);
            if (eps_table == null)
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NOT_INITIALIZED]);
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                //struct eps_hk_t data =
                // TODO - raw? , type = 3.
                data_out.output = eps_table[index].GET_HK_2_WDT(3);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR] + ", output: " + data_out.output);
                return Constants.E_NO_SS_ERR;
            }
            else
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INDEX_ERROR]);
                return Constants.E_INDEX_ERROR;
            }
        }

        /**
         *	Read back the current housekeeping data from GOMSpace EPS
         *
         *	@param[in] index index of GOMSpace EPS I2C bus address
         *	@param[out] data_out housekeeping output of GOMSpace EPS. Basic data subset.
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsGetHkData_basic(byte index, Output<EPS.eps_hk_basic_t> data_out)
        {
            logs.Add(DateTime.Now + " GomEpsGetHkData_basic");
            logs.Add("index: " + index);
            if (eps_table == null)
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NOT_INITIALIZED]);
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                //struct eps_hk_t data =
                // TODO - raw? , type = 4.
                data_out.output = eps_table[index].GET_HK_2_BASIC(4);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR] + ", output: " + data_out.output);
                return Constants.E_NO_SS_ERR;
            }
            else
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INDEX_ERROR]);
                return Constants.E_INDEX_ERROR;
            }
        }


        /**
         *	Set the GOMSpace EPS Output channel
         *
         *	@param[in] index index of GOMSpace EPS I2C bus address
         *	@param[in] output output channel mask selection, 1 = on, 0 = off
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsSetOutput(byte index, byte output)
        {
            logs.Add(DateTime.Now + " GomEpsSetOutput");
            logs.Add("index: " + index + ", output: " + output);
            if (eps_table == null)
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NOT_INITIALIZED]);
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR]);
                eps_table[index].SET_OUTPUT(output);
                return Constants.E_NO_SS_ERR;
            }
            else
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INDEX_ERROR]);
                return Constants.E_INDEX_ERROR;
            }
        }

        /**
         * 	Command the GOMSpace EPS to turn on / off a single switched channel after a delay
         *
         * 	@param[in] index index of GOMSpace EPS I2C bus address
         *	@param[in] channel_id channel number to be turned on / off
         *	@param[in] out value of the output channel defined in the output channel mask selection
         *	@param[in] delay scheduled delay in seconds before the output is executed
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsSetSingleOutput(byte index, byte channel_id, byte value, ushort delay)
        {
            logs.Add(DateTime.Now + " GomEpsSetSingleOutput");
            logs.Add("index: " + index + ", channel_id: " + channel_id + ", value: " + value + ", delay: " + delay);
            if (eps_table == null)
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NOT_INITIALIZED]);
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                if (value != EPSConstants.ON && value != EPSConstants.OFF)
                {
                    logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INVALID_INPUT]);
                    return Constants.E_INVALID_INPUT;
                }
                //TODO - change 0 and 7 to constants
                else if (!(channel_id >= 0 && channel_id <= 7))
                {
                    logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INVALID_INPUT]);
                    return Constants.E_INVALID_INPUT;
                }
                else
                {
                    eps_table[index].SET_SINGLE_OUTPUT(channel_id, value, delay);
                    logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR]);
                    return Constants.E_NO_SS_ERR;
		        }
	        }
            else
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INDEX_ERROR]);
                return Constants.E_INDEX_ERROR;
            }   
        }

        /**
         *	Set the GOMSpace EPS photovoltaic input voltage into a specific value
         *
         *	@param[in] index index of GOMSpace EPS I2C bus address
         *	@param[in] voltage1 photovoltaic1 voltage
         *	@param[in] voltage2 photovoltaic2 voltage
         *	@param[in] voltage3 photovoltaic3 voltage
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsSetPhotovoltaicInputs(byte index, ushort voltage1, ushort voltage2, ushort voltage3)
        {
            logs.Add(DateTime.Now + " GomEpsSetPhotovoltaicInputs");
            logs.Add("index: " + index + ", voltage1: " + voltage1 + ", voltage2: " + voltage2 + ", voltage3: " + voltage3);
            if (eps_table == null)
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NOT_INITIALIZED]);
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                if (eps_table[index].CurrentConfig.PptMode != PPTMode.FIXED)
                {
                    logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INVALID_ACTION]);
                    return Constants.E_INVALID_ACTION;
                }
                else
                {
                    eps_table[index].SET_PV_VOLT(voltage1, voltage2, voltage3);
                    logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR]);
                    return Constants.E_NO_SS_ERR;
                }

            }
            else
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INDEX_ERROR]);
                return Constants.E_INDEX_ERROR;
            }
        }

        /**
         *	Set the GOMSpace EPS power point mode
         *
         *	@param[in] index index of GOMSpace EPS I2C bus address
         *	@param[in] mode power point mode of the eps
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsSetPptMode(byte index, byte mode)
        {
            logs.Add(DateTime.Now + " GomEpsSetPptMode");
            logs.Add("index: " + index + ", mode: " + mode);
            if (eps_table == null)
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NOT_INITIALIZED]);
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                if (mode == (byte)PPTMode.HARDWARE || mode == (byte)PPTMode.MPPT || mode == (byte)PPTMode.FIXED)
                {
                    eps_table[index].SET_PV_AUTO(mode);
                    logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR]);
                    return Constants.E_NO_SS_ERR;
                }
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INVALID_INPUT]);
                return Constants.E_INVALID_INPUT;
            }
            else
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INDEX_ERROR]);
                return Constants.E_INDEX_ERROR;
            }
        }


        /**
         *	Set the GOMSpace EPS Heater Auto Mode
         *
         *	@param[in] index index of GOMSpace EPS I2C bus address
         *	@param[in] auto_mode desired heater auto mode to the GOMSpace EPS
         *	@param[out] auto_mode_return current heater auto mode
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsSetHeaterAutoMode(byte index, byte auto_mode, Output<ushort> auto_mode_return)
        {
            logs.Add(DateTime.Now + " GomEpsSetHeaterAutoMode");
            logs.Add("index: " + index + ", auto_mode: " + auto_mode);
            if (eps_table == null)
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NOT_INITIALIZED]);
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                auto_mode_return.output = eps_table[index].SET_HEATER(0, EPSConstants.ONBOARD_HEATER,auto_mode);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR] + ", output: " + auto_mode_return.output);
                return Constants.E_NO_SS_ERR;
            }
            else
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INDEX_ERROR]);
                return Constants.E_INDEX_ERROR;
            }
        }

        /**
         *	Reset the GOMSpace EPS counters
         *
         *	@param[in] index index of GOMSpace EPS I2C bus address
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsResetCounters(byte index)
        {
            logs.Add(DateTime.Now + " GomEpsResetCounters");
            logs.Add("index: " + index);
            if (eps_table == null)
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NOT_INITIALIZED]);
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                // TODO - magic number.
                eps_table[index].RESET_COUNTERS(0x42);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR]);
                return Constants.E_NO_SS_ERR;
            }
            else
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INDEX_ERROR]);
                return Constants.E_INDEX_ERROR;
            }
        }

        /**
         *	Reset WDT in the GOMSpace EPS
         *
         *	@param[in] index index of GOMSpace EPS I2C bus address
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsResetWDT(byte index)
        {
            logs.Add(DateTime.Now + " GomEpsResetWDT");
            logs.Add("index: " + index);
            if (eps_table == null)
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NOT_INITIALIZED]);
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                // TODO - magic number.
                eps_table[index].RESET_WDT(0x78);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR]);
                return Constants.E_NO_SS_ERR;
            }
            else
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INDEX_ERROR]);
                return Constants.E_INDEX_ERROR;
            }
        }

        /**
         *	Configuration command for the GOMSpace EPS version
         *
         *	@param[in] index index of GOMSpace EPS I2C bus address
         *	@param[in] cmd configuration control command.
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsConfigCMD(byte index, byte cmd)
        {
            logs.Add(DateTime.Now + " GomEpsConfigCMD");
            logs.Add("index: " + index + ", cmd: " + cmd);
            if (eps_table == null)
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NOT_INITIALIZED]);
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                if (cmd == 1)
                {
                    eps_table[index].CONFIG_CMD(cmd);
                    logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR]);
                    return Constants.E_NO_SS_ERR;
                }
                else
                {
                    logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INVALID_INPUT]);
                    return Constants.E_INVALID_INPUT;
                }
            }
            else
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INDEX_ERROR]);
                return Constants.E_INDEX_ERROR;
            }
        }

        /**
         *	Get configuration data for the GOMSpace EPS version
         *
         *	@param[in] index index of GOMSpace EPS I2C bus address
         *	@param[out] config_data configuration 1 data.
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsConfigGet(byte index, Output<EPS.eps_config_t> config_data)
        {
            logs.Add(DateTime.Now + " GomEpsConfigGet");
            logs.Add("index: " + index);
            if (eps_table == null)
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NOT_INITIALIZED]);
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                config_data.output = eps_table[index].CONFIG_GET();
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR] + ", output: " + config_data.output);
                return Constants.E_NO_SS_ERR;
            }
            else
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INDEX_ERROR]);
                return Constants.E_INDEX_ERROR;
            }
        }

        /**
         *	Set configuration data for the GOMSpace EPS version
         *
         *	@param[in] index index of GOMSpace EPS I2C bus address
         *	@param[in] config_data configuration 1 data.
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsConfigSet(byte index, EPS.eps_config_t config_data)
        {
            logs.Add(DateTime.Now + " GomEpsConfigSet");
            logs.Add("index: " + index);
            if (eps_table == null)
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NOT_INITIALIZED]);
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                eps_table[index].CONFIG_SET(config_data);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR]);
                return Constants.E_NO_SS_ERR;
            }
            else
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INDEX_ERROR]);
                return Constants.E_INDEX_ERROR;
            }
        }

        /**
         *	Configuration 2 command for the GOMSpace EPS version
         *
         *	@param[in] index index of GOMSpace EPS I2C bus address
         *	@param[in] cmd configuration control command.
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsConfig2CMD(byte index, byte cmd)
        {
            logs.Add(DateTime.Now + " GomEpsConfig2CMD");
            logs.Add("index: " + index + ", cmd: " + cmd);
            if (eps_table == null)
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NOT_INITIALIZED]);
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                if (cmd == 1 || cmd == 2)
                {
                    eps_table[index].CONFIG2_CMD(cmd);
                    logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR]);
                    return Constants.E_NO_SS_ERR;
                }
                else
                {
                    logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INVALID_INPUT]);
                    return Constants.E_INVALID_INPUT;
                }
            }
            else
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INDEX_ERROR]);
                return Constants.E_INDEX_ERROR;
            }
        }

        /**
         *	Get configuration 2 data for the GOMSpace EPS version
         *
         *	@param[in] index index of GOMSpace EPS I2C bus address
         *	@param[out] config_data configuration 1 data.
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsConfig2Get(byte index, Output<EPS.eps_config2_t> config_data)
        {
            logs.Add(DateTime.Now + " GomEpsConfig2Get");
            logs.Add("index: " + index);
            if (eps_table == null)
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NOT_INITIALIZED]);
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                config_data.output = eps_table[index].CONFIG2_GET();
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR] + ", output: " + config_data.output);
                return Constants.E_NO_SS_ERR;
            }
            else
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INDEX_ERROR]);
                return Constants.E_INDEX_ERROR;
            }
        }

        /**
         *	Set configuration 2 data for the GOMSpace EPS version
         *
         *	@param[in] index index of GOMSpace EPS I2C bus address
         *	@param[in] config_data configuration 1 data.
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsConfig2Set(byte index, EPS.eps_config2_t config_data)
        {
            logs.Add(DateTime.Now + " GomEpsConfig2Set");
            logs.Add("index: " + index);
            if (eps_table == null)
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NOT_INITIALIZED]);
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                eps_table[index].CONFIG2_SET(config_data);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR]);
                return Constants.E_NO_SS_ERR;
            }
            else
            {
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_INDEX_ERROR]);
                return Constants.E_INDEX_ERROR;
            }
        }
    }
}


