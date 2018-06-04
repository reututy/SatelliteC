using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.TRX
{
    public class Receiver : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ISIStrxvuBitrateStatus bitrate;
        public ISIStrxvuBitrateStatus RxBitrate
        {
            get { return bitrate; }
            set
            {
                bitrate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RxBitrate"));
            }
        }

        public FrameBuffer rxFrameBuffer;
        private ushort tx_current; ///< Rx Telemetry transmitter current.
        public ushort Tx_current {
            get { return tx_current;}
            set { tx_current = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Tx_current"));}
        }

        private ushort rx_doppler; ///< Rx Telemetry receiver doppler.
        public ushort Rx_doppler
        {
            get { return rx_doppler; }
            set
            {
                rx_doppler = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Rx_doppler"));
            }
        }

        private ushort rx_current; ///< Rx Telemetry receiver current.
        public ushort Rx_current
        {
            get { return rx_current; }
            set
            {
                rx_current = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Rx_current"));
            }
        }
        private ushort bus_volt; ///< Rx Telemetry bus voltage.
        public ushort Bus_volt
        {
            get { return bus_volt; }
            set
            {
                bus_volt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Bus_volt"));
            }
        }
        private ushort board_temp; ///< Rx Telemetry board temperature.
        public ushort Board_temp
        {
            get { return board_temp; }
            set
            {
                board_temp = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Board_temp"));
            }
        }
        private ushort pa_temp; ///< Rx Telemetry power amplifier temperature.
        public ushort Pa_temp
        {
            get { return pa_temp; }
            set
            {
                pa_temp = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Pa_temp"));
            }
        }
        private ushort rx_rssi; ///< Rx Telemetry rssi measurement.
        public ushort Rx_rssi
        {
            get { return rx_rssi; }
            set
            {
                rx_rssi = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Rx_rssi"));
            }
        }

        private DateTime lastReset;

        public Receiver()
        {
            rxFrameBuffer = new FrameBuffer(40);
            lastReset = DateTime.Now;
        }

        /**
         * performs a reset of the I2C watchdog without performing any other operation 
         */
        public void WatchdogReset()
        {
            lastReset = DateTime.Now;
        }

        /**
         * performs a software reset of the receiver processor.
         */
        public void SoftwareReset()
        {
            lastReset = DateTime.Now;
        }

        /**
         * power cycle the full board (transmitter and receiver will be both reset).
         */
         public void HardwareSystemReset()
        {
            lastReset = DateTime.Now;
        }

        /**
         * retrieves the number of frames that are currently stored in the receiver buffer.
         * @ return: Number of frames in rcieve buffer. This number has a minimum value of 0 and a maximum value of
         * the maximum number of frames that can be in the buffer.
         */
         public int GetNumberOfFramesInReceiverBuffer()
        {
            return 0;
        }

        /**
         * retrieves the contents of the oldest frame in the receive buffer. The contents of the frame are preceded by
         * two bytes that indicate the frame size in number of bytes. This size can be used by the OBC to terminate the
         * transmission after all the relevant bytes have been received (when the actual size is less then the maximum size).
         * It can also be used for easier processing of the frame contents by the OBC's command processor. If there are 0
         * frames in the receive buffer the response is undefined.
         */
         public void GetFrameFromReceiveBuffer()
        {

        }

        /**
         * Removes oldest frame from the receive buffer. This is the same frame that can be retrieved from the receiver
         * buffer command. If there are 0 frames in the receive buffer this command has no effect.
         */
         public void RemoveFrameFromBuffer()
        {

        }

        /**
         * Measures all the available telemetry channels:
         *  -Total supply current
         *  -Power amplifier temperature
         *  -Local oscillator temperature
         *  -Instantaneous received signal Doppler offset at the receiver port
         *  -Instantaneous received signal strength at the receiver port
         *  -Supply voltage
         */
         public void MeasureAllTelemetryChannels()
        {

        }

        /**
         * Reports the amount of time the transmitter MCU has been active since last reset, also known as up-time.
         * The uptime is reported with a resolution of 1 second. The maximum supported uptime is 4294967295, after
         * which the uptime will overflow and the reported uptime will be 0 seconds.
         */
         public void ReportReceiverUptime()
        {

        }

        public Frame ReceiveFrame()
        {
            return rxFrameBuffer.removeFrame();
        }

        internal int getFrameCount()
        {
            return rxFrameBuffer.getFrameCount();
        }

        public void clear()
        {
            rxFrameBuffer.clear();
        }

        /*private byte[] buffer;
        private Interpreter interpreter;
        private int bitrate; // ????
        private byte I2Caddress;
        private Frame[] frames; 

        public void DecodeCommand()
        {
            
        }

        typedef struct __attribute__ ((__packed__)) _ISIStrxvuRxFrame
        {
            unsigned short rx_length; ///< Reception frame length.
        unsigned short rx_doppler; ///< Reception frame doppler measurement.
        unsigned short rx_rssi; ///< Reception frame rssi measurement.
        unsigned char* rx_framedata; ///< Reception frame data.
        }
    ISIStrxvuRxFrame;

        typedef union __attribute__((__packed__)) _ISIStrxvuRxTelemetry
{
	/** Raw value array with Rx Telemetry data*/
        /*unsigned char raw[TRXVU_ALL_RXTELEMETRY_SIZE];
        /** Telemetry values*/
        /*struct __attribute__ ((__packed__))
        {
            unsigned short tx_current; ///< Rx Telemetry transmitter current.
        unsigned short rx_doppler; ///< Rx Telemetry receiver doppler.
        unsigned short rx_current; ///< Rx Telemetry receiver current.
        unsigned short bus_volt; ///< Rx Telemetry bus voltage.
        unsigned short board_temp; ///< Rx Telemetry board temperature.
        unsigned short pa_temp; ///< Rx Telemetry power amplifier temperature.
        unsigned short rx_rssi; ///< Rx Telemetry rssi measurement.
    }
    fields;
    } ISIStrxvuRxTelemetry;*/


    }
}
