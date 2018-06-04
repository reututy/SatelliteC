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
        EPS[] eps_table;
        int eps_num; //number of EPSs

        /**
         * 	Initialize the GOMSpace EPS with the corresponding i2cAddress. This function can only be called once.
         *
         * 	@param[in] i2c_address array of GOMSpace EPS I2C bus address
         * 	@param[in] number number of attached EPS in the system to be initialized
         * 	@return Error code according to <hal/errors.h>
         */
        
        public int GomEpsInitialize(byte i2c_address, byte number)
        {
            // TODO - what to do with the address?
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
                    return Constants.E_NO_SS_ERR;
                }
                else
                    return Constants.E_INDEX_ERROR;
            }
            else
                return Constants.E_IS_INITIALIZED;
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
            if (eps_table == null)
            {
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                byte_out.output = eps_table[index].PING(ping_byte);
                return Constants.E_NO_SS_ERR;
            }
            else
                return Constants.E_INDEX_ERROR;
        }

        /**
         * 	Software Reset on the GOMSpace EPS based on the index.
         *
         * 	@param[in] index index of GOMSpace EPS I2C bus address
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsSoftReset(byte index)
        {
            if (eps_table == null)
            {
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                /// TODO - where is the function soft reset.
                return Constants.E_NO_SS_ERR;
            }
            else
                return Constants.E_INDEX_ERROR;
        }

        /**
         * 	Hardware Reset on the GOMSpace EPS based on the index.
         *
         * 	@param[in] index index of GOMSpace EPS I2C bus address
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsHardReset(byte index)
        {
            if (eps_table == null)
            {
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                eps_table[index].HARD_RESET();
                return Constants.E_NO_SS_ERR;
            }
            else
                return Constants.E_INDEX_ERROR;
        }

        //TODO GomEpsGetHkData_param

        /**
         *	Read back the current housekeeping data from GOMSpace EPS
         *
         *	@param[in] index index of GOMSpace EPS I2C bus address
         *	@param[out] data_out housekeeping output of GOMSpace EPS. p31u-8 format.
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsGetHkData_general(byte index, Output<EPS.eps_hk_t> data_out)
        {
            if (eps_table == null)
            {
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                //struct eps_hk_t data =
                // TODO - raw?
                data_out.output = eps_table[index].GET_HK_2(0);
                return Constants.E_NO_SS_ERR;
            }
            else
                return Constants.E_INDEX_ERROR;
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
            if (eps_table == null)
            {
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                //struct eps_hk_t data =
                // TODO - raw? , type = 1.
                data_out.output = eps_table[index].GET_HK_2_VI(1);
                return Constants.E_NO_SS_ERR;
            }
            else
                return Constants.E_INDEX_ERROR;
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
            if (eps_table == null)
            {
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                //struct eps_hk_t data =
                // TODO - raw? , type = 2.
                data_out.output = eps_table[index].GET_HK_2_OUT(2);
                return Constants.E_NO_SS_ERR;
            }
            else
                return Constants.E_INDEX_ERROR;
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
            if (eps_table == null)
            {
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                //struct eps_hk_t data =
                // TODO - raw? , type = 3.
                data_out.output = eps_table[index].GET_HK_2_WDT(3);
                return Constants.E_NO_SS_ERR;
            }
            else
                return Constants.E_INDEX_ERROR;
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
            if (eps_table == null)
            {
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                //struct eps_hk_t data =
                // TODO - raw? , type = 4.
                data_out.output = eps_table[index].GET_HK_2_BASIC(4);
                return Constants.E_NO_SS_ERR;
            }
            else
                return Constants.E_INDEX_ERROR;
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
            if (eps_table == null)
            {
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                eps_table[index].SET_OUTPUT(output);
                return Constants.E_NO_SS_ERR;
            }
            else
                return Constants.E_INDEX_ERROR;
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
            if (eps_table == null)
            {
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                if (value != EPSConstants.ON && value != EPSConstants.OFF)
                {
                    return Constants.E_INVALID_INPUT;
                }
                //TODO - change 0 and 7 to constants
                else if (!(channel_id >= 0 && channel_id <= 7))
                {
                    return Constants.E_INVALID_INPUT;
                }
                else
                {
                    eps_table[index].SET_SINGLE_OUTPUT(channel_id, value, delay);
			        return Constants.E_NO_SS_ERR;
		        }
	        }
            else
            {
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
            if (eps_table == null)
            {
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
		        //if(eps_table[index].ppt_mode != FIXEDSWPPT)
                //{
			    //    return Constants.E_INVALID_ACTION;
		        //}
		        //else
                //{
                    eps_table[index].SET_PV_VOLT(voltage1, voltage2, voltage3);
			        return Constants.E_NO_SS_ERR;
		        //}

	        }
	        else
		        return Constants.E_INDEX_ERROR;
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
            if (eps_table == null)
            {
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                eps_table[index].SET_PV_AUTO(mode);
                return Constants.E_NO_SS_ERR;
            }
            else
                return Constants.E_INDEX_ERROR;
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
            if (eps_table == null)
            {
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                auto_mode_return.output = eps_table[index].SET_HEATER(0, EPSConstants.ONBOARD_HEATER,auto_mode);
                return Constants.E_NO_SS_ERR;
            }
            else
            {
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
            if (eps_table == null)
            {
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                // TODO - magic number.
                eps_table[index].RESET_COUNTERS(0x42);
                return Constants.E_NO_SS_ERR;
            }
            else
                return Constants.E_INDEX_ERROR;
        }

        /**
         *	Reset WDT in the GOMSpace EPS
         *
         *	@param[in] index index of GOMSpace EPS I2C bus address
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsResetWDT(byte index)
        {
            if (eps_table == null)
            {
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                // TODO - magic number.
                eps_table[index].RESET_WDT(0x78);
                return Constants.E_NO_SS_ERR;
            }
            else
                return Constants.E_INDEX_ERROR;
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
            if (eps_table == null)
            {
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                if (cmd == 1)
                {
                    eps_table[index].CONFIG_CMD(cmd);
                    return Constants.E_NO_SS_ERR;
                }
                else
                    return Constants.E_INVALID_INPUT;
            }
            else
                return Constants.E_INDEX_ERROR;
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
            if (eps_table == null)
            {
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                config_data.output = eps_table[index].CONFIG_GET();
                return Constants.E_NO_SS_ERR;
            }
            else
                return Constants.E_INDEX_ERROR;
        }

        /**
         *	Set configuration data for the GOMSpace EPS version
         *
         *	@param[in] index index of GOMSpace EPS I2C bus address
         *	@param[in] config_data configuration 1 data.
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsConfigSet(byte index, Output<EPS.eps_config_t> config_data)
        {
            if (eps_table == null)
            {
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                eps_table[index].CONFIG_SET(config_data.output);
                return Constants.E_NO_SS_ERR;
            }
            else
                return Constants.E_INDEX_ERROR;
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
            if (eps_table == null)
            {
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                if (cmd == 1 || cmd == 2)
                {
                    eps_table[index].CONFIG2_CMD(cmd);
                    return Constants.E_NO_SS_ERR;
                }
                else
                    return Constants.E_INVALID_INPUT;
            }
            else
                return Constants.E_INDEX_ERROR;
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
            if (eps_table == null)
            {
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                config_data.output = eps_table[index].CONFIG2_GET();
                return Constants.E_NO_SS_ERR;
            }
            else
                return Constants.E_INDEX_ERROR;
        }

        /**
         *	Set configuration 2 data for the GOMSpace EPS version
         *
         *	@param[in] index index of GOMSpace EPS I2C bus address
         *	@param[in] config_data configuration 1 data.
         * 	@return Error code according to <hal/errors.h>
         */
        public int GomEpsConfig2Set(byte index, Output<EPS.eps_config2_t> config_data)
        {
            if (eps_table == null)
            {
                return Constants.E_NOT_INITIALIZED;
            }
            if (index < eps_num && index >= 0)
            {
                eps_table[index].CONFIG2_SET(config_data.output);
                return Constants.E_NO_SS_ERR;
            }
            else
                return Constants.E_INDEX_ERROR;
        }
    }
}


