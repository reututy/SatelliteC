using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataModel.TRX
{
    public class TRX : INotifyPropertyChanged
    {
        private int trxId;
        private String name;
        public String Name
        {
            get { return this.name; }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.NotifyPropertyChanged("Name");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public ISIStrxvuI2CAddress address { get; set; }
        public ISIStrxvuFrameLengths maxFrameLengths { get; set; }
        public ISIStrxvuBitrate default_bitrates { get; set; }
        public ISIStrxvuBeacon _ISIStrxvuBeaconOn { get; set; }
        public ISIStrxvuBeacon ISIStrxvuBeaconOn
        {
            get { return this._ISIStrxvuBeaconOn; }
            set
            {
                if (this._ISIStrxvuBeaconOn != value)
                {
                    this._ISIStrxvuBeaconOn = value;
                    this.NotifyPropertyChanged("ISIStrxvuBeaconOn");
                }
            }
        }
        public Transmitter transmitter;
        public Receiver receiver;
        public AX25Frame Beacon { get; set; }
        private Thread beaconThread;
        ushort _beaconInterval;
        public ushort beaconInterval
        {
            get { return this._beaconInterval; }
            set
            {
                if (this._beaconInterval != value)
                {
                    this._beaconInterval = value;
                    this.NotifyPropertyChanged("beaconInterval");
                }
            }
        }

        public void OverflowReceiverBuffer()
        {
            for (int i = 0; i < 40; i++)
            {
                receiver.rxFrameBuffer.addFrame(new AX25Frame(TRXConfiguration.FromDefClSign, TRXConfiguration.ToDefClSign, Encoding.UTF8.GetBytes("defult")));
            }
        }

        public void OverflowTransmitterBuffer()
        {
            for(int i=0; i<40; i++)
            {
                transmitter.SendFrame(new AX25Frame(TRXConfiguration.FromDefClSign, TRXConfiguration.ToDefClSign, Encoding.UTF8.GetBytes("defult")));
            }
        }

        public void clearReceiverBuffer()
        {
            receiver.clear();
        }

        public void clearTransmitterBuffer()
        {
            transmitter.clear();
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
            int res = transmitter.SendFrame(new AX25Frame(TRXConfiguration.FromDefClSign, TRXConfiguration.ToDefClSign, data));
            avail.output = Convert.ToByte(transmitter.getAvailbleSpace());
            return res;
        }

        public int IsisTrxvu_tcSendAX25OvrClSign(char[] fromCallsign, char[] toCallsign, byte[] data, byte length, Output<byte> avail)
        {
            if (length > maxFrameLengths.maxAX25frameLengthTX)
            {
                return Constants.E_TRXUV_FRAME_LENGTH;
            }
            int res = transmitter.SendFrame(new AX25Frame(toCallsign, fromCallsign, data));
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
            this.Beacon = new AX25Frame(TRXConfiguration.FromDefClSign, TRXConfiguration.ToDefClSign, data);
            this.beaconInterval = interval;
            if (beaconThread != null)
            {
                beaconThread.Abort();
            }
            beaconThread = new Thread(() =>
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(interval);
                    transmitter.SendFrame(this.Beacon);
                }
            });
            return Constants.E_NO_SS_ERR;
        }

        public int IsisTrxvu_tcSetAx25BeaconOvrClSign(char[] fromCallsign, char[] toCallsign, byte[] data, byte length, ushort interval)
        {
            if (length > maxFrameLengths.maxAX25frameLengthTX)
            {
                return Constants.E_TRXUV_FRAME_LENGTH;
            }
            ISIStrxvuBeaconOn = ISIStrxvuBeacon.trxvu_beacon_active;
            this.Beacon = new AX25Frame(toCallsign, fromCallsign, data);
            this.beaconInterval = interval;
            if(beaconThread != null)
            {
                beaconThread.Abort();
            }
            beaconThread = new Thread(() =>
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(interval);
                    transmitter.SendFrame(this.Beacon);
                }
            });
            return Constants.E_NO_SS_ERR;
        }

        public void IsisTrxvu_tcClearBeacon()
        {
            ISIStrxvuBeaconOn = ISIStrxvuBeacon.trxvu_beacon_none;
            if (beaconThread != null)
            {
                beaconThread.Abort();
                beaconThread = null;
            }
            this.Beacon = null;
            this.beaconInterval = 0;
        }

        public void IsisTrxvu_tcSetDefToClSign(char[] toCallsign)
        {
            TRXConfiguration.ToDefClSign = toCallsign;
        }

        public void IsisTrxvu_tcSetDefFromClSign(char[] fromCallsign)
        {
            TRXConfiguration.FromDefClSign = fromCallsign;
        }

        public void IsisTrxvu_tcSetIdlestate(ISIStrxvuIdleState state)
        {
            transmitter.IdleState = state;
        }

        public void IsisTrxvu_tcSetAx25Bitrate(ISIStrxvuBitrate bitrate)
        {
            this.default_bitrates = bitrate;
            if(bitrate == ISIStrxvuBitrate.trxvu_bitrate_1200)
            {
                transmitter.TxBitrate = ISIStrxvuBitrateStatus.trxvu_bitratestatus_1200;
                receiver.RxBitrate = ISIStrxvuBitrateStatus.trxvu_bitratestatus_1200;
            }
            if (bitrate == ISIStrxvuBitrate.trxvu_bitrate_2400)
            {
                transmitter.TxBitrate = ISIStrxvuBitrateStatus.trxvu_bitratestatus_2400;
                receiver.RxBitrate = ISIStrxvuBitrateStatus.trxvu_bitratestatus_2400;
            }
            if (bitrate == ISIStrxvuBitrate.trxvu_bitrate_4800)
            {
                transmitter.TxBitrate = ISIStrxvuBitrateStatus.trxvu_bitratestatus_4800;
                receiver.RxBitrate = ISIStrxvuBitrateStatus.trxvu_bitratestatus_4800;
            }
            if (bitrate == ISIStrxvuBitrate.trxvu_bitrate_9600)
            {
                transmitter.TxBitrate = ISIStrxvuBitrateStatus.trxvu_bitratestatus_9600;
                receiver.RxBitrate = ISIStrxvuBitrateStatus.trxvu_bitratestatus_9600;
            }
            

        }

        public void IsisTrxvu_tcGetUptime(Output<byte[]> uptime)
        {
            throw new NotImplementedException();
        }

        public ushort IsisTrxvu_tcEstimateTransmissionTime(byte length)
        {
            throw new NotImplementedException();
        }

        public void IsisTrxvu_rcGetCommandFrame(Output<ISIStrxvuRxFrame> rx_frame)
        {
            Frame frame = receiver.ReceiveFrame();
            rx_frame.output = frame.frame;
        }

        public void IsisTrxvu_rcGetUptime(Output<byte[]> uptime)
        {
            throw new NotImplementedException();
        }

        public void IsisTrxvu_rcGetFrameCount(Output<ushort> frameCount)
        {
            frameCount.output = (ushort) receiver.getFrameCount();
        }

        public void IsisTrxvu_tcGetState(Output<ISIStrxvuTransmitterState> currentvutcState)
        {
            ISIStrxvuTransmitterState state = new ISIStrxvuTransmitterState();
            state.transmitter_beacon = this.ISIStrxvuBeaconOn;
            state.transmitter_bitrate = transmitter.bitrate;
            state.transmitter_idle_state = transmitter.state;
            currentvutcState.output = state;
        }

        public void IsisTrxvu_tcGetLastTxTelemetry(Output<ISIStrxvuTxTelemetry> last_telemetry)
        {
            ISIStrxvuTxTelemetry tl = new ISIStrxvuTxTelemetry();
            tl.pa_temp = transmitter.Pa_temp;
            tl.tx_current = transmitter.Tx_current;
            tl.tx_fwrdpwr = transmitter.Tx_fwrdpwr;
            tl.tx_reflpwr = transmitter.Tx_reflpwr;
            last_telemetry.output = tl;
        }

        public void IsisTrxvu_rcGetTelemetryAll(Output<ISIStrxvuRxTelemetry> telemetry)
        {
            ISIStrxvuRxTelemetry tl = new ISIStrxvuRxTelemetry();
            tl.tx_current = receiver.Tx_current;
            tl.pa_temp = receiver.Pa_temp;
            tl.rx_current = receiver.Rx_current;
            tl.rx_doppler = receiver.Rx_doppler;
            tl.rx_rssi = receiver.Rx_rssi;
            tl.board_temp = receiver.Board_temp;
            tl.bus_volt = receiver.Bus_volt;
            telemetry.output = tl;
        }

    }
}
