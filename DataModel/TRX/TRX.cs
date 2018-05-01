using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.TRX
{
    public class TRX
    {
        public int trxId { get; set; }
        public ISIStrxvuI2CAddress address { get; set; }
        public ISIStrxvuFrameLengths maxFrameLengths { get; set; }
        public ISIStrxvuBitrate default_bitrates { get; set; }
        public ISIStrxvuBeacon ISIStrxvuBeaconOn { get; set; }
        Transmitter transmitter;
        Receiver receiver;
        public AX25Frame Beacon { get; set; }
        ushort beaconInterval;

        public void OverflowReceiverBuffer()
        {

        }

        public void OverflowTransmitterBuffer()
        {

        }

        public TRX(int trxId, ISIStrxvuI2CAddress address, ISIStrxvuFrameLengths maxFrameLengths, ISIStrxvuBitrate default_bitrates)
        {
            this.trxId = trxId;
            this.address = address;
            this.maxFrameLengths = maxFrameLengths;
            this.default_bitrates = default_bitrates;
            this.ISIStrxvuBeaconOn = ISIStrxvuBeacon.trxvu_beacon_none;
            transmitter = new Transmitter();
            receiver = new Receiver();
        }

        /**
         *  @brief       Soft Reset the ISIS TRXVU Component.
         *  @param[in]   component TRXVU component, either VU_TC or VU_RC.
         *  @return      Void
         */
        public void IsisTrxvu_componentSoftReset(ISIStrxvuComponent component)
        {
            if(component == ISIStrxvuComponent.trxvu_tc)
            {
                transmitter.SofteareReset();
            }
            else
            {
                receiver.SoftwareReset();
            }
        }

        public void IsisTrxvu_componentHardReset(ISIStrxvuComponent component)
        {
            if (component == ISIStrxvuComponent.trxvu_tc)
            {
                transmitter.HardwareSystemReset();
            }
            else
            {
                receiver.HardwareSystemReset();
            }
        }

        public void IsisTrxvu_softReset()
        {
            transmitter.SofteareReset();
            receiver.SoftwareReset();
        }

        public void IsisTrxvu_hardReset()
        {
            transmitter.HardwareSystemReset();
            receiver.HardwareSystemReset();
        }

        public int IsisTrxvu_tcSendAX25DefClSign(byte[] data, byte length, Output<Byte> avail)
        {
            if(length > maxFrameLengths.maxAX25frameLengthTX)
            {
                return Constants.E_TRXUV_FRAME_LENGTH;
            }
            int res = transmitter.SendFrame(new AX25Frame(new byte[1], new byte[1], data));
            avail.output = Convert.ToByte(transmitter.getAvailbleSpace());
            return res;
        }

        public int IsisTrxvu_tcSendAX25OvrClSign(byte fromCallsign, byte toCallsign, byte[] data, byte length, Output<byte> avail)
        {
            if (length > maxFrameLengths.maxAX25frameLengthTX)
            {
                return Constants.E_TRXUV_FRAME_LENGTH;
            }
            int res = transmitter.SendFrame(new AX25Frame(new byte[]{ toCallsign }, new byte[] { fromCallsign }, data));
            avail.output = Convert.ToByte(transmitter.getAvailbleSpace());
            return res;
        }

        public int IsisTrxvu_tcSetAx25BeaconDefClSign(byte[] data, byte length, ushort interval)
        {
            if (length > maxFrameLengths.maxAX25frameLengthTX)
            {
                return Constants.E_TRXUV_FRAME_LENGTH;
            }
            ISIStrxvuBeaconOn = ISIStrxvuBeacon.trxvu_beacon_active;
            this.Beacon = new AX25Frame(new byte[1], new byte[1], data);
            this.beaconInterval = interval;
            return Constants.E_NO_SS_ERR;
        }

        public int IsisTrxvu_tcSetAx25BeaconOvrClSign(byte[] fromCallsign, byte[] toCallsign, byte[] data, byte length, ushort interval)
        {
            if (length > maxFrameLengths.maxAX25frameLengthTX)
            {
                return Constants.E_TRXUV_FRAME_LENGTH;
            }
            ISIStrxvuBeaconOn = ISIStrxvuBeacon.trxvu_beacon_active;
            this.Beacon = new AX25Frame(toCallsign, fromCallsign, data);
            this.beaconInterval = interval;
            return Constants.E_NO_SS_ERR;
        }

        public void IsisTrxvu_tcClearBeacon()
        {
            ISIStrxvuBeaconOn = ISIStrxvuBeacon.trxvu_beacon_none;
            this.Beacon = null;
            this.beaconInterval = 0;
        }

        public void IsisTrxvu_tcSetDefToClSign(string toCallsign)
        {
            throw new NotImplementedException();
        }

        public void IsisTrxvu_tcSetDefFromClSign(string fromCallsign)
        {
            throw new NotImplementedException();
        }

        public void IsisTrxvu_tcSetIdlestate(ISIStrxvuIdleState state)
        {
            transmitter.state = state;
        }

        public void IsisTrxvu_tcSetAx25Bitrate(ISIStrxvuBitrate bitrate)
        {
            this.default_bitrates = bitrate;
        }

        public void IsisTrxvu_tcGetUptime(Output<byte> uptime)
        {
            throw new NotImplementedException();
        }

        public ushort IsisTrxvu_tcEstimateTransmissionTime(byte length)
        {
            throw new NotImplementedException();
        }

        public void IsisTrxvu_rcGetCommandFrame(Output<ISIStrxvuRxFrame> rx_frame)
        {
            ISIStrxvuRxFrame outFrame = new ISIStrxvuRxFrame();
            Frame frame = receiver.ReceiveFrame();
            outFrame.rx_doppler = frame.rx_doppler;
            outFrame.rx_framedata = frame.rx_framedata;
            outFrame.rx_length = frame.rx_length;
            outFrame.rx_rssi = frame.rx_rssi;
            rx_frame.output = outFrame;
        }

        public void IsisTrxvu_rcGetUptime(Output<byte> uptime)
        {
            throw new NotImplementedException();
        }

        public void IsisTrxvu_rcGetFrameCount(Output<ushort> frameCount)
        {
            frameCount.output = (ushort) receiver.getFrameCount();
        }


        /*typedef struct _ISIStrxvuFrameLengths
        {
            unsigned int maxAX25frameLengthTX; ///< AX25 maximum frame size for transmission.
            unsigned int maxAX25frameLengthRX; ///< AX25 maximum frame size for reception.
        }
        ISIStrxvuFrameLengths;

            int availableFrames;

        void update_wod(gom_eps_hk_t EpsTelemetry_hk);
        void vurc_getRxTelemTest(isisRXtlm* converted);
        void vurc_getTxTelemTest(isisTXtlm* converted);
        void init_trxvu(void);
        int TRX_sendFrame(unsigned char* data, unsigned char length);
        void act_upon_comm(unsigned char* in, gom_eps_channelstates_t channels_state);
        void dump(void* arg);
        void Beacon(gom_eps_hk_t EpsTelemetry_hk);
        Boolean check_ants_deployed();
        void trxvu_logic(unsigned long* start_gs_time, unsigned long* time_now_unix, gom_eps_channelstates_t channels_state);
        void enter_gs_mode(unsigned long* start_gs_time);
        void end_gs_mode();

        extern unsigned char dumpparam[11];*/

    }
}
