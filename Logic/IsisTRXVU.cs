using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using DataModel.TRX;
using static System.Net.Mime.MediaTypeNames;

namespace Logic
{

    public class IsisTRXVU
    {
        

        private TRX[] tRXes;
        public static List<String> logs = new List<String>();
        public ObservableCollection<TRX> tRXesCollection = new ObservableCollection<TRX>();

        public IsisTRXVU()
        {
        }
        
        /**
         *  @brief      Initialize the ISIS TRXVU with the corresponding i2cAddress from the array of TRXVU I2C Address structure.
         *  @note       This function can only be called once.
         *  @param[in]  address array of TRXVU I2C Address structure.
         *  @param[in]  maxFrameLengths array of maximum frame length structures for TRXVU.
         *  @param[in]	default_bitrates initial default bitrate.
         *  @param[in]  number of attached TRXVU in the system to be initialized.
         *  @return     Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_initialize(ISIStrxvuI2CAddress[] address, ISIStrxvuFrameLengths[] maxFrameLengths, ISIStrxvuBitrate default_bitrates, byte number)
        {
            logs.Add(DateTime.Now + " IsisTrxvu_initialize");
            if (tRXes == null)
            {
                tRXes = new TRX[number];
                for (int i = 0; i < number; i++)
                {
                    logs.Add("address: " + address[i].ToString() + ", maxFrameLength: " + maxFrameLengths[i].ToString() + " ,default bitrates: " + default_bitrates + " ,index: " + i);
                    TRX trx = new TRX(i, address[i], maxFrameLengths[i], default_bitrates) { Name = i.ToString() };
                    tRXes[i] = trx;
                    tRXesCollection.Add(trx);
                }
                logs.Add(DateTime.Now + " Exit Status: No Error");
                return Constants.E_NO_SS_ERR;
            }
            logs.Add(DateTime.Now + " ERROR: IsisTRXVU allready initiallized");
            return Constants.E_IS_INITIALIZED;
        }

        /**
         *  @brief       Soft Reset the ISIS TRXVU Component.
         *  @param[in]   index of ISIS TRXVU I2C bus address.
         *  @param[in]   component TRXVU component, either VU_TC or VU_RC.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_componentSoftReset(byte index, ISIStrxvuComponent component)
        {
            logs.Add(DateTime.Now + " IsisTrxvu_componentSoftReset");
            logs.Add("index: " + index + " ,component" + component);
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_componentSoftReset(component);
                logs.Add(DateTime.Now + " Exit Status: No Error");
                return Constants.E_NO_SS_ERR;
            }
            logs.Add(DateTime.Now + " ERROR: E_INDEX_ERROR");
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Hard Reset the ISIS TRXVU Component.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[in]   component TRXVU component, either VU_TC or VU_RC.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_componentHardReset(byte index, ISIStrxvuComponent component)
        {
            logs.Add(DateTime.Now + " IsisTrxvu_componentHardReset");
            logs.Add("index: " + index + " ,component" + component);
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_componentHardReset(component);
                logs.Add(DateTime.Now + " Exit Status: No Error");
                return Constants.E_NO_SS_ERR;
            }
            logs.Add(DateTime.Now + " ERROR: E_INDEX_ERROR");
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Soft Reset the ISIS TRXVU VU_RC and VU_TC.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_softReset(byte index)
        {
            logs.Add(DateTime.Now + " IsisTrxvu_softReset");
            logs.Add("index: " + index);
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_softReset();
                logs.Add(DateTime.Now + " Exit Status: No Error");
                return Constants.E_NO_SS_ERR;
            }
            logs.Add(DateTime.Now + " ERROR: E_INDEX_ERROR");
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Hard Reset the ISIS TRXVU VU_RC and VU_TC.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_hardReset(byte index)
        {
            logs.Add(DateTime.Now + " IsisTrxvu_hardReset");
            logs.Add("index: " + index);
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_hardReset();
                logs.Add(DateTime.Now + " Exit Status: No Error");
                return Constants.E_NO_SS_ERR;
            }
            logs.Add(DateTime.Now + " ERROR: E_INDEX_ERROR");
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Tell the TRXVU to transmit an AX.25 message with default callsigns and specified content.
         *  @param[in]   index of ISIS TRXVU I2C bus address.
         *  @param[in]   data Pointer to the array containing the data to put in the AX.25 message.
         *  @param[in]   length Length of the data to be put in the AX.25 message.
         *  @param[out]  avail Number of the available slots in the transmission buffer of the VU_TC after the frame has been added.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_tcSendAX25DefClSign(byte index, byte[] data, byte length, Output<Byte> avail) 
        {
            logs.Add(DateTime.Now + " IsisTrxvu_tcSendAX25DefClSign");
            logs.Add("index: " + index + " ,data: " + data + " ,length: " + length);
            if (index < tRXes.Length)
            {
                int result = tRXes[index].IsisTrxvu_tcSendAX25DefClSign(data, length, avail);
                logs.Add("output : " + avail.output);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[result]);
                return result;
            }
            logs.Add(DateTime.Now + " ERROR: E_INDEX_ERROR");
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Tell the TRXVU to transmit an AX.25 message with override callsigns and specified content.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[in]	 fromCallsign This variable will define the new 7 characters from callsign.
         *  @param[in]	 toCallsign This variable will define the new 7 characters to callsign.
         *  @param[in]   data Pointer to the array containing the data to put in the AX.25 message.
         *  @param[in]   length Length of the data to be put in the AX.25 message.
         *  @param[out]  avail Number of the available slots in the transmission buffer of the VU_TC after the frame has been added.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_tcSendAX25OvrClSign(byte index, char[] fromCallsign, char[] toCallsign, byte[] data, byte length, Output<Byte> avail)
        {
            logs.Add(DateTime.Now + " IsisTrxvu_tcSendAX25OvrClSign");
            logs.Add("index: " + index + " ,fromCallsign: "+ fromCallsign + " ,toCallsign: " + toCallsign + ", data: " + data + " ,length: " + length);
            if (index < tRXes.Length)
            {
                int result = tRXes[index].IsisTrxvu_tcSendAX25OvrClSign(fromCallsign, toCallsign, data, length, avail);
                logs.Add("output : " + avail.output);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[result]);
                return result;
            }
            logs.Add(DateTime.Now + " ERROR: E_INDEX_ERROR");
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Tell the TRXVU to set the parameters for the AX25 Beacon with default callsigns.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[in]   data Pointer to the array containing the message to be transmitted.
         *  @param[in]   length Length of the message.
         *  @param[in]   interval Interval of beacon transmission.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_tcSetAx25BeaconDefClSign(byte index, byte[] data, byte length, ushort interval)
        {
            logs.Add(DateTime.Now + " IsisTrxvu_tcSendAX25OvrClSign");
            logs.Add("index: " + index +", data: " + data + " ,length: " + length + " ,interval: " + interval);
            if (index < tRXes.Length)
            {
                int result = tRXes[index].IsisTrxvu_tcSetAx25BeaconDefClSign(data, length, interval);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[result]);
                return result;
            }
            logs.Add(DateTime.Now + " ERROR: E_INDEX_ERROR");
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Tell the TRXVU to set the parameters for the AX25 Beacon with override callsigns.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[in]	 fromCallsign This variable will define the new 7 characters from callsign.
         *  @param[in]	 toCallsign This variable will define the new 7 characters to callsign.
         *  @param[in]   data Pointer to the array containing the message to be transmitted.
         *  @param[in]   length Length of the message.
         *  @param[in]   interval Interval of beacon transmission.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_tcSetAx25BeaconOvrClSign(byte index, char[] fromCallsign, char[] toCallsign, byte[] data, byte length, ushort interval)
        {
            logs.Add(DateTime.Now + " IsisTrxvu_tcSetAx25BeaconOvrClSign");
            logs.Add("index: " + index + ", fromCallsign" + fromCallsign + ", toCallsign" + toCallsign  + ", data: " + data + " ,length: " + length + " ,interval: " + interval);
            if (index < tRXes.Length)
            {
                int result = tRXes[index].IsisTrxvu_tcSetAx25BeaconOvrClSign(fromCallsign, toCallsign, data, length, interval);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[result]);
                return result;
            }
            logs.Add(DateTime.Now + " ERROR: E_INDEX_ERROR");
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Tell the TRXVU to clear the current beacon.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_tcClearBeacon(byte index)
        {
            logs.Add(DateTime.Now + " IsisTrxvu_tcClearBeacon");
            logs.Add("index: " + index);
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_tcClearBeacon();
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR]);
                return Constants.E_NO_SS_ERR;
            }
            logs.Add(DateTime.Now + " ERROR: E_INDEX_ERROR");
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Tell the TRXVU to set a new default to callsign name.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[in]   toCallsign This variable will define the new 7 characters default to callsign.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_tcSetDefToClSign(byte index, char[] toCallsign)
        {
            logs.Add(DateTime.Now + " IsisTrxvu_tcClearBeacon");
            logs.Add("index: " + index + ", toCallsign: " + toCallsign);
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_tcSetDefToClSign(toCallsign);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR]);
                return Constants.E_NO_SS_ERR;
            }
            logs.Add(DateTime.Now + " ERROR: E_INDEX_ERROR");
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Tell the TRXVU to set a new default from callsign name.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[in]   fromCallsign This variable will define the new 7 characters default from callsign.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_tcSetDefFromClSign(byte index, char[] fromCallsign)
        {
            logs.Add(DateTime.Now + " IsisTrxvu_tcClearBeacon");
            logs.Add("index: " + index + ", fromCallsign: " + fromCallsign);
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_tcSetDefFromClSign(fromCallsign);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR]);
                return Constants.E_NO_SS_ERR;
            }
            logs.Add(DateTime.Now + " ERROR: E_INDEX_ERROR");
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Set the idle state of the TRXVU transmitter, i.e. the state in between transmission.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[in]   state The desired idle state of the TRXVU.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_tcSetIdlestate(byte index, ISIStrxvuIdleState state)
        {
            logs.Add(DateTime.Now + " IsisTrxvu_tcSetIdlestate");
            logs.Add("index: " + index + ", state: " + state);
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_tcSetIdlestate(state);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR]);
                return Constants.E_NO_SS_ERR;
            }
            logs.Add(DateTime.Now + " ERROR: E_INDEX_ERROR");
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Set the AX.25 bitrate of the TRXVU transmitter.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[in]   bitrate The desired AX.25 bitrate of the TRXVU.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_tcSetAx25Bitrate(byte index, ISIStrxvuBitrate bitrate)
        {
            logs.Add(DateTime.Now + " IsisTrxvu_tcSetAx25Bitrate");
            logs.Add("index: " + index + " ,bitrate: " + bitrate);
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_tcSetAx25Bitrate(bitrate);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR]);
                return Constants.E_NO_SS_ERR;
            }
            logs.Add(DateTime.Now + " ERROR: E_INDEX_ERROR");
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Retrieve the current time of operation of the TRXVU transmitter.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[out]  uptime This array of 4 characters contains the operation time of the transmitter (Seconds, Minutes, Hours and Days, in that order).
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_tcGetUptime(byte index, Output<byte[]> uptime)
        {
            logs.Add(DateTime.Now + " IsisTrxvu_tcSetIdlestate");
            logs.Add("index: " + index);
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_tcGetUptime(uptime);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR] + ", result: " + uptime.output);
                return Constants.E_NO_SS_ERR;
            }
            logs.Add(DateTime.Now + " ERROR: E_INDEX_ERROR");
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Retrieve the current transmitter status.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[out]  currentvutcState Pointer to the union where the current status should be stored.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_tcGetState(byte index, Output<ISIStrxvuTransmitterState> currentvutcState)
        {
            logs.Add(DateTime.Now + " IsisTrxvu_tcGetState");
            logs.Add("index: " + index + " ,currentvutcState: " + currentvutcState);
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_tcGetState(currentvutcState);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR]);
                return Constants.E_NO_SS_ERR;
            }
            logs.Add(DateTime.Now + " ERROR: E_INDEX_ERROR");
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Retrieve a block of telemetry from the TRXVU transmitter.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[out]  telemetry Pointer to the union where the telemetry should be stored.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_tcGetTelemetryAll(byte index, Output<ISIStrxvuTxTelemetry> telemetry)
        {
            logs.Add(DateTime.Now + " IsisTrxvu_tcGetTelemetryAll");
            logs.Add("index: " + index);
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_tcGetTelemetryAll(telemetry);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR] + ", Output: " + telemetry.output);
                return Constants.E_NO_SS_ERR;
            }
            logs.Add(DateTime.Now + " ERROR: E_INDEX_ERROR");
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Retrieve a block of telemetry from the TRXVU transmitter sotre on the last transmission.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[out]  last_telemetry Pointer to the union where the telemetry should be stored.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_tcGetLastTxTelemetry(byte index, Output<ISIStrxvuTxTelemetry> last_telemetry)
        {
            logs.Add(DateTime.Now + " IsisTrxvu_tcGetTelemetryAll");
            logs.Add("index: " + index);
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_tcGetLastTxTelemetry(last_telemetry);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR] + ", Output: " + last_telemetry.output);
                return Constants.E_NO_SS_ERR;
            }
            logs.Add(DateTime.Now + " ERROR: E_INDEX_ERROR");
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Calculates the approximate time it will take for a certain transmission to complete.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[in]   length The length of the transmission in bytes.
         *  @return      The time estimate in milliseconds.
         */
        public ushort IsisTrxvu_tcEstimateTransmissionTime(byte index, byte length)
        {
            logs.Add(DateTime.Now + " IsisTrxvu_tcEstimateTransmissionTime");
            logs.Add("index: " + index + ", length: " + length);
            if (index < tRXes.Length)
            {
                ushort result = tRXes[index].IsisTrxvu_tcEstimateTransmissionTime(length);
                logs.Add(DateTime.Now + "Exit with result: " + result);
                return result;
            }
            logs.Add(DateTime.Now + " ERROR: E_INDEX_ERROR");
            return 0;
        }

        /**
         *  @brief       Retrieve the number of telecommand frames present in the receive buffer of the TRXVU.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[out]  frameCount The number of telecommand frames in the buffer.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_rcGetFrameCount(byte index, Output<ushort> frameCount)
        {
            logs.Add(DateTime.Now + " IsisTrxvu_rcGetFrameCount");
            logs.Add("index: " + index);
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_rcGetFrameCount(frameCount);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR] + ", Output: " + frameCount.output);
                return Constants.E_NO_SS_ERR;
            }
            logs.Add(DateTime.Now + " ERROR: E_INDEX_ERROR");
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Retrieve and delete a telecommand frame from the TRXVU.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[out]  rx_frame Pointer to the struct where the telecommand frame should be store.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_rcGetCommandFrame(byte index, Output<ISIStrxvuRxFrame> rx_frame)
        {
            logs.Add(DateTime.Now + " IsisTrxvu_rcGetCommandFrame");
            logs.Add("index: " + index);
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_rcGetCommandFrame(rx_frame);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR] + ", Output: " + rx_frame.output);
                return Constants.E_NO_SS_ERR;
            }
            logs.Add(DateTime.Now + " ERROR: E_INDEX_ERROR");
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Retrieve a block of telemetry from the TRXVU receiver.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[out]  telemetry Pointer to the union where the telemetry should be stored.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_rcGetTelemetryAll(byte index, Output<ISIStrxvuRxTelemetry> telemetry)
        {
            logs.Add(DateTime.Now + " IsisTrxvu_rcGetTelemetryAll");
            logs.Add("index: " + index);
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_rcGetTelemetryAll(telemetry);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR] + ", Output: " + telemetry.output);
                return Constants.E_NO_SS_ERR;
            }
            logs.Add(DateTime.Now + " ERROR: E_INDEX_ERROR");
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Retrieve the current time of operation of the TRXVU receiver.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[out]  uptime This array of 3 characters contains the operation time of the receiver (Minutes, Hours and Days, in that order)..
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_rcGetUptime(byte index, Output<byte[]> uptime)
        {
            logs.Add(DateTime.Now + " IsisTrxvu_rcGetUptime");
            logs.Add("index: " + index);
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_rcGetUptime(uptime);
                logs.Add(DateTime.Now + "Exit Status: " + Constants.MapIdToError[Constants.E_NO_SS_ERR] + ", Output: " + uptime.output);
                return Constants.E_NO_SS_ERR;
            }
            logs.Add(DateTime.Now + " ERROR: E_INDEX_ERROR");
            return Constants.E_INDEX_ERROR;
        }
    }
}
