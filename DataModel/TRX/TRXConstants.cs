﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.TRX
{
    /**
    * Enumeration list of the Idle states of the TRXVU.
    */
    public enum ISIStrxvuIdleState  {
        trxvu_idle_state_off = 0x00,
        trxvu_idle_state_on = 0x01
    }

    /**
     * Enumeration list of bitrate options for setting the bitrate in the TRXVU.
     */
     public enum ISIStrxvuBitrate
    {
        trxvu_bitrate_1200 = 0x01, ///< Transmission Bitrate 1200 bps.
        trxvu_bitrate_2400 = 0x02, ///< Transmission Bitrate 2400 bps.
        trxvu_bitrate_4800 = 0x04, ///< Transmission Bitrate 4800 bps.
        trxvu_bitrate_9600 = 0x08 ///< Transmission Bitrate 9600 bps.
    }

    /**
     * Enumeration list of bitrate options of the TRXVU when reporting the status
     */
     public enum ISIStrxvuBitrateStatus
    {
        trxvu_bitratestatus_1200 = 0x00, ///< Transmission Bitrate 1200 bps.
        trxvu_bitratestatus_2400 = 0x01, ///< Transmission Bitrate 2400 bps.
        trxvu_bitratestatus_4800 = 0x02, ///< Transmission Bitrate 4800 bps.
        trxvu_bitratestatus_9600 = 0x03 ///< Transmission Bitrate 9600 bps.
    }
    

    /**
     * Enumeration list of TRXVU Components.
     */
    public enum ISIStrxvuComponent
    {
        trxvu_rc = 0x00, ///< TRXVU receiver component.
        trxvu_tc = 0x01 ///< TRXVU transmitter component.
    }

    /**
     * Enumeration list of TRXVU beacon status.
     */
     public enum ISIStrxvuBeacon
    {
        trxvu_beacon_none = 0x00,
        trxvu_beacon_active = 0x01
    }

    /**
     *  Struct for defining ISIS TRXVU I2C Address.
     */
     public struct ISIStrxvuI2CAddress
    {
        public byte addressVu_rc; ///< I2C address of the VU_RC.
        public byte addressVu_tc; ///< I2C address of the VU_TC.
        public override string ToString()
        {
            return "ISIStrxvuI2CAddress {addressVu_rc: " + addressVu_rc + ", addressVu_tc: " + addressVu_tc + "}";
        }
    }

    /**
     *  Struct for defining ISIS TRXVU buffers length.
     */
    public struct ISIStrxvuFrameLengths : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public uint _maxAX25frameLengthTX; ///< AX25 maximum frame size for transmission.
        public uint maxAX25frameLengthTX
        {
            get { return _maxAX25frameLengthTX; }
            set
            {
                _maxAX25frameLengthTX = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("maxAX25frameLengthTX"));
                }
            }
        }
        public uint _maxAX25frameLengthRX; ///< AX25 maximum frame size for reception.
        public uint maxAX25frameLengthRX
        {
            get { return _maxAX25frameLengthRX; }
            set
            {
                _maxAX25frameLengthRX = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("maxAX25frameLengthRX"));
                }
            }
        }
        public override string ToString()
        {
            return "ISIStrxvuFrameLengths {maxAX25frameLengthRX: " + maxAX25frameLengthRX + ", maxAX25frameLengthTX: " + maxAX25frameLengthTX + "}";
        }

    }

    /**
     *  Struct for the TRXVU reception frame.
     */
    public struct ISIStrxvuRxFrame
    {
        public ushort rx_length { get; set; } ///< Reception frame length.
        public ushort rx_doppler { get; set; } ///< Reception frame doppler measurement.
        public ushort rx_rssi { get; set; } ///< Reception frame rssi measurement.
        public byte[] rx_framedata { get; set; } ///< Reception frame data. //unsigned char*
        public override string ToString()
        {
            return "ISIStrxvuRxFrame {rx_length: " + rx_length + ", rx_doppler: " + rx_doppler + ", rx_rssi: " + rx_rssi + ", rx_framedata: " + rx_framedata + "}";
        }
    }

    public struct ISIStrxvuRxTelemetry
    {
        public ushort tx_current; ///< Rx Telemetry transmitter current.
        public ushort rx_doppler; ///< Rx Telemetry receiver doppler.
        public ushort rx_current; ///< Rx Telemetry receiver current.
        public ushort bus_volt; ///< Rx Telemetry bus voltage.
        public ushort board_temp; ///< Rx Telemetry board temperature.
        public ushort pa_temp; ///< Rx Telemetry power amplifier temperature.
        public ushort rx_rssi; ///< Rx Telemetry rssi measurement.
        public override string ToString()
        {
            return "ISIStrxvuRxTelemetry {tx_current: " + tx_current + ", rx_doppler: " + rx_doppler + ", rx_current: " + rx_current + ", bus_volt: " + bus_volt + ", board_temp: " + board_temp + ", pa_temp: " + pa_temp + ", rx_rssi: " + rx_rssi + "}";
        }
    }

    public struct ISIStrxvuTxTelemetry
    {
        public ushort tx_reflpwr; ///< Tx Telemetry reflected power.
        public ushort pa_temp; ///< Tx Telemetry power amplifier temperature.
        public ushort tx_fwrdpwr; ///< Tx Telemetry forward power.
        public ushort tx_current; ///< Tx Telemetry transmitter current.
        public override string ToString()
        {
            return "ISIStrxvuTxTelemetry {tx_reflpwr: " + tx_reflpwr + ", pa_temp: " + pa_temp + ", tx_fwrdpwr: " + tx_fwrdpwr + ", tx_current: " + tx_current + "}";
        }
    }

    public class ISIStrxvuTransmitterState
    {
        public ISIStrxvuIdleState transmitter_idle_state = ISIStrxvuIdleState.trxvu_idle_state_on; ///< Transmitter current idle state.
        public ISIStrxvuBeacon transmitter_beacon  = ISIStrxvuBeacon.trxvu_beacon_active; ///< Transmitter beacon mode status.
        public ISIStrxvuBitrateStatus transmitter_bitrate  = ISIStrxvuBitrateStatus.trxvu_bitratestatus_2400; ///< Transmitter current bitrate.
        public override string ToString()
        {
            return "ISIStrxvuTransmitterState {transmitter_idle_state: " + transmitter_idle_state + ", transmitter_beacon: " + transmitter_beacon + ", transmitter_bitrate: " + transmitter_bitrate + "}";
        }
    }

    public class TRXConstants
    {
        public static int TRXVU_UPTIME_SIZE = 4;  ///< Size for the up time buffer.
        public static int TRXVU_ALL_RXTELEMETRY_SIZE = 14;  ///< Size of the buffer for the complete telemetry read out from the receiver component.
        public static int TRXVU_ALL_TXTELEMETRY_SIZE = 8;  ///< Size of the buffer for the complete telemetry read out from the transmitter component.
        public override string ToString()
        {
            return "TRXConstants {TRXVU_UPTIME_SIZE: " + TRXVU_UPTIME_SIZE + ", TRXVU_ALL_RXTELEMETRY_SIZE: " + TRXVU_ALL_RXTELEMETRY_SIZE + ", TRXVU_ALL_TXTELEMETRY_SIZE: " + TRXVU_ALL_TXTELEMETRY_SIZE + "}";
        }
    }
}
