using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.TRX
{
    public class Transmitter : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public FrameBuffer txFrameBuffer;
        public ISIStrxvuIdleState state { get; set; }
        public ISIStrxvuIdleState IdleState
        {
            get { return state; }
            set
            {
                state = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IdleState"));
            }
        }

        public ISIStrxvuBitrateStatus bitrate;
        public ISIStrxvuBitrateStatus TxBitrate
        {
            get { return bitrate; }
            set
            {
                bitrate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TxBitrate"));
            }
        }

        private ushort tx_reflpwr; ///< Tx Telemetry reflected power.
        public ushort Tx_reflpwr
        {
            get { return tx_reflpwr; }
            set
            {
                tx_reflpwr = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Tx_reflpwr"));
            }
        }
        private ushort pa_temp; ///< Tx Telemetry power amplifier temperature.
        public ushort Pa_temp
        {
            get { return pa_temp; }
            set
            {
                pa_temp = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Pa_temp"));
            }
        }
        private ushort tx_fwrdpwr; ///< Tx Telemetry forward power.
        public ushort Tx_fwrdpwr
        {
            get { return tx_fwrdpwr; }
            set
            {
                tx_fwrdpwr = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Tx_fwrdpwr"));
            }
        }
        private ushort tx_current; ///< Tx Telemetry transmitter current.
        public ushort Tx_current
        {
            get { return tx_current; }
            set
            {
                tx_current = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Tx_current"));
            }
        }

        private DateTime lastReset;

        public Transmitter()
        {
            txFrameBuffer = new FrameBuffer(40); // max frames = 40
            state = ISIStrxvuIdleState.trxvu_idle_state_off;
            lastReset = DateTime.Now;
        }

        public int getAvailbleSpace()
        {
            return txFrameBuffer.getAvailbleSpace();
        }
        
        /**
         * Performs a reset of the I2C watchdog withou performing any other operation.
         */
         public void WatchdogReset()
        {
            lastReset = DateTime.Now;
        }

        /**
         * Performs a software reset of the transmitter processor.
         */
         public void SofteareReset()
        {
            lastReset = DateTime.Now;
        }

        /**
         * Power cycles the full board (transmitter and receiver will be both reset).
         */
         public void HardwareSystemReset()
        {
            lastReset = DateTime.Now;
        }

        /**
         * Adds a frame (AX.25 UI or HDLC frame, according to [AD01]) to the frame buffer of the transmitter. If the radio
         * mode is AX.25 (as specified in [AD01]), the AX.25 frame will contain the default callsigns as they are set in the
         * controller at the time this command is received. This command will disable any beacon thar is currently being
         * transmitted by the transceiver. The frame will not be added to the frame buffer if:
         *  -The frame buffer is full
         *  -The content size is 0 bytes
         *  -The content size is larger than the maximum size (specified in [AD01])
         */
         public int SendFrame(Frame frame)
        {
            return txFrameBuffer.addFrame(frame);
        }



       /* typedef union __attribute__((__packed__)) _ISIStrxvuTxTelemetry
{
	/** Raw value array with Tx Telemetry data*/
    /*unsigned char raw[TRXVU_ALL_TXTELEMETRY_SIZE];
        /** Telemetry values*/
      /*  struct __attribute__ ((__packed__))
    {
        unsigned short tx_reflpwr; ///< Tx Telemetry reflected power.
        unsigned short pa_temp; ///< Tx Telemetry power amplifier temperature.
        unsigned short tx_fwrdpwr; ///< Tx Telemetry forward power.
        unsigned short tx_current; ///< Tx Telemetry transmitter current.
    }
    fields;
}
ISIStrxvuTxTelemetry;

    typedef union __attribute__((__packed__)) _ISIStrxvuTransmitterState
{
	/** Raw value that contains the current transmitter state*/
   /* unsigned char raw;
struct __attribute__ ((__packed__))
    {
        ISIStrxvuIdleState transmitter_idle_state : 1; ///< Transmitter current idle state.
        ISIStrxvuBeacon transmitter_beacon : 1; ///< Transmitter beacon mode status.
        ISIStrxvuBitrateStatus transmitter_bitrate : 2; ///< Transmitter current bitrate.
    }fields;
} ISIStrxvuTransmitterState;*/
    }
}
