using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataModel.TRX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.TRX.Tests
{
    [TestClass()]
    public class TRXTests
    {

        [TestMethod()]
        public void OverflowReceiverBufferTest()
        {
            TRX trx = new TRX(0, new ISIStrxvuI2CAddress(), new ISIStrxvuFrameLengths(), ISIStrxvuBitrate.trxvu_bitrate_1200);
            trx.OverflowReceiverBuffer();
            Assert.AreEqual(trx.receiver.GetNumberOfFramesInReceiverBuffer(), 40);
        }

        [TestMethod()]
        public void OverflowTransmitterBufferTest()
        {
            TRX trx = new TRX(0, new ISIStrxvuI2CAddress(), new ISIStrxvuFrameLengths(), ISIStrxvuBitrate.trxvu_bitrate_1200);
            trx.OverflowTransmitterBuffer();
            Assert.AreEqual(trx.transmitter.getAvailbleSpace(), 0);
        }

        [TestMethod()]
        public void clearReceiverBufferTest()
        {
            TRX trx = new TRX(0, new ISIStrxvuI2CAddress(), new ISIStrxvuFrameLengths(), ISIStrxvuBitrate.trxvu_bitrate_1200);
            trx.clearReceiverBuffer();
            Assert.AreEqual(trx.receiver.GetNumberOfFramesInReceiverBuffer(), 0);
        }

        [TestMethod()]
        public void clearTransmitterBufferTest()
        {
            TRX trx = new TRX(0, new ISIStrxvuI2CAddress(), new ISIStrxvuFrameLengths(), ISIStrxvuBitrate.trxvu_bitrate_1200);
            trx.clearTransmitterBuffer();
            Assert.AreEqual(trx.transmitter.getAvailbleSpace(), 40);
        }

        [TestMethod()]
        public void IsisTrxvu_tcSendAX25DefClSignTest()
        {
            ISIStrxvuFrameLengths len = new ISIStrxvuFrameLengths();
            len.maxAX25frameLengthRX = 20;
            len.maxAX25frameLengthTX = 20;
            TRX trx = new TRX(0, new ISIStrxvuI2CAddress(), len, ISIStrxvuBitrate.trxvu_bitrate_1200);
            int res = trx.IsisTrxvu_tcSendAX25DefClSign(new byte[30], 30, new Output<byte>());
            Assert.AreEqual(Constants.E_TRXUV_FRAME_LENGTH, res);

            TRX trx2 = new TRX(0, new ISIStrxvuI2CAddress(), len, ISIStrxvuBitrate.trxvu_bitrate_1200);
            Output<byte> avail = new Output<byte>();
            byte[] data = Encoding.ASCII.GetBytes("data");
            int res2 = trx2.IsisTrxvu_tcSendAX25DefClSign(data, 15, avail);
            Assert.AreEqual(Constants.E_NO_SS_ERR, res2);
            Assert.AreEqual(avail.output, 39);
            Assert.AreEqual(data, ((AX25Frame)trx2.transmitter.txFrameBuffer.frames.peek()).infoFeild);
        }

        [TestMethod()]
        public void IsisTrxvu_tcSendAX25OvrClSignTest()
        {
            ISIStrxvuFrameLengths len = new ISIStrxvuFrameLengths();
            len.maxAX25frameLengthRX = 20;
            len.maxAX25frameLengthTX = 20;
            TRX trx = new TRX(0, new ISIStrxvuI2CAddress(), len, ISIStrxvuBitrate.trxvu_bitrate_1200);
            int res = trx.IsisTrxvu_tcSendAX25OvrClSign(new char[7], new char[7], new byte[30], 30, new Output<byte>());
            Assert.AreEqual(Constants.E_TRXUV_FRAME_LENGTH, res);

            TRX trx2 = new TRX(0, new ISIStrxvuI2CAddress(), len, ISIStrxvuBitrate.trxvu_bitrate_1200);
            Output<byte> avail = new Output<byte>();
            byte[] data = Encoding.ASCII.GetBytes("data");
            char[] from = "fromSig".ToCharArray();
            char[] to = "toCalli".ToCharArray();
            int res2 = trx2.IsisTrxvu_tcSendAX25OvrClSign(from, to, data, 15, avail);
            Assert.AreEqual(Constants.E_NO_SS_ERR, res2);
            Assert.AreEqual(avail.output, 39);
            byte[] fromm = Encoding.UTF8.GetBytes(from);
            byte[] too = Encoding.UTF8.GetBytes(to);
            Assert.AreEqual(fromm.Length, ((AX25Frame)trx2.transmitter.txFrameBuffer.frames.peek()).Header.Src.Length);
            Assert.AreEqual(too.Length, ((AX25Frame)trx2.transmitter.txFrameBuffer.frames.peek()).Header.Dest.Length);
            for (int i=0; i<fromm.Length; i++)
            {
                Assert.AreEqual(fromm[i], ((AX25Frame)trx2.transmitter.txFrameBuffer.frames.peek()).Header.Src[i]);
            }
            for (int i = 0; i < too.Length; i++)
            {
                Assert.AreEqual(too[i], ((AX25Frame)trx2.transmitter.txFrameBuffer.frames.peek()).Header.Dest[i]);
            }
            
            Assert.AreEqual(data, ((AX25Frame)trx2.transmitter.txFrameBuffer.frames.peek()).infoFeild);
        }

        [TestMethod()]
        public void IsisTrxvu_tcSetAx25BeaconDefClSignTest()
        {
            ISIStrxvuFrameLengths len = new ISIStrxvuFrameLengths();
            len.maxAX25frameLengthRX = 20;
            len.maxAX25frameLengthTX = 20;
            TRX trx = new TRX(0, new ISIStrxvuI2CAddress(), len, ISIStrxvuBitrate.trxvu_bitrate_1200);
            int res = trx.IsisTrxvu_tcSetAx25BeaconDefClSign(new byte[30], 30, 100);
            Assert.AreEqual(Constants.E_TRXUV_FRAME_LENGTH, res);

            TRX trx2 = new TRX(0, new ISIStrxvuI2CAddress(), len, ISIStrxvuBitrate.trxvu_bitrate_1200);
            Output<byte> avail = new Output<byte>();
            byte[] data = Encoding.ASCII.GetBytes("data");
            int res2 = trx2.IsisTrxvu_tcSetAx25BeaconDefClSign(data, 15, 100);
            Assert.AreEqual(Constants.E_NO_SS_ERR, res2);
            Assert.AreEqual(trx2.beaconInterval, 100);
            Assert.AreEqual(trx2.Beacon.infoFeild, data);
        }

        [TestMethod()]
        public void IsisTrxvu_tcSetAx25BeaconOvrClSignTest()
        {
            ISIStrxvuFrameLengths len = new ISIStrxvuFrameLengths();
            len.maxAX25frameLengthRX = 20;
            len.maxAX25frameLengthTX = 20;
            TRX trx = new TRX(0, new ISIStrxvuI2CAddress(), len, ISIStrxvuBitrate.trxvu_bitrate_1200);
            int res = trx.IsisTrxvu_tcSetAx25BeaconOvrClSign(new char[7], new char[7], new byte[30], 30, 100);
            Assert.AreEqual(Constants.E_TRXUV_FRAME_LENGTH, res);

            TRX trx2 = new TRX(0, new ISIStrxvuI2CAddress(), len, ISIStrxvuBitrate.trxvu_bitrate_1200);
            byte[] data = Encoding.ASCII.GetBytes("data");
            char[] from = "fromSig".ToCharArray();
            char[] to = "toCalli".ToCharArray();
            int res2 = trx2.IsisTrxvu_tcSetAx25BeaconOvrClSign(from, to, data, 15, 100);
            Assert.AreEqual(Constants.E_NO_SS_ERR, res2);
            byte[] fromm = Encoding.UTF8.GetBytes(from);
            byte[] too = Encoding.UTF8.GetBytes(to);
            Assert.AreEqual(fromm.Length, trx2.Beacon.Header.Src.Length);
            Assert.AreEqual(too.Length, trx2.Beacon.Header.Dest.Length);
            for (int i = 0; i < fromm.Length; i++)
            {
                Assert.AreEqual(fromm[i], trx2.Beacon.Header.Src[i]);
            }
            for (int i = 0; i < too.Length; i++)
            {
                Assert.AreEqual(too[i], trx2.Beacon.Header.Dest[i]);
            }

            Assert.AreEqual(data, trx2.Beacon.infoFeild);
            Assert.AreEqual(trx2.beaconInterval, 100);
        }

        [TestMethod()]
        public void IsisTrxvu_tcClearBeaconTest()
        {
            ISIStrxvuFrameLengths len = new ISIStrxvuFrameLengths();
            len.maxAX25frameLengthRX = 20;
            len.maxAX25frameLengthTX = 20;
            TRX trx = new TRX(0, new ISIStrxvuI2CAddress(), len, ISIStrxvuBitrate.trxvu_bitrate_1200);
            
            byte[] data = Encoding.ASCII.GetBytes("data");
            int res2 = trx.IsisTrxvu_tcSetAx25BeaconDefClSign(data, 15, 100);

            trx.IsisTrxvu_tcClearBeacon();
            Assert.IsNull(trx.Beacon);
        }

        [TestMethod()]
        public void IsisTrxvu_tcSetDefToClSignTest()
        {
            ISIStrxvuFrameLengths len = new ISIStrxvuFrameLengths();
            len.maxAX25frameLengthRX = 20;
            len.maxAX25frameLengthTX = 20;
            TRX trx = new TRX(0, new ISIStrxvuI2CAddress(), len, ISIStrxvuBitrate.trxvu_bitrate_1200);

            char[] toCalli = "toCalli".ToCharArray();
            trx.IsisTrxvu_tcSetDefToClSign(toCalli);
            Assert.AreEqual(TRXConfiguration.ToDefClSign, toCalli);
        }

        [TestMethod()]
        public void IsisTrxvu_tcSetDefFromClSignTest()
        {
            ISIStrxvuFrameLengths len = new ISIStrxvuFrameLengths();
            len.maxAX25frameLengthRX = 20;
            len.maxAX25frameLengthTX = 20;
            TRX trx = new TRX(0, new ISIStrxvuI2CAddress(), len, ISIStrxvuBitrate.trxvu_bitrate_1200);

            char[] from = "fromCal".ToCharArray();
            trx.IsisTrxvu_tcSetDefFromClSign(from);
            Assert.AreEqual(TRXConfiguration.FromDefClSign, from);
        }

        [TestMethod()]
        public void IsisTrxvu_tcSetIdlestateTest()
        {
            ISIStrxvuFrameLengths len = new ISIStrxvuFrameLengths();
            len.maxAX25frameLengthRX = 20;
            len.maxAX25frameLengthTX = 20;
            TRX trx = new TRX(0, new ISIStrxvuI2CAddress(), len, ISIStrxvuBitrate.trxvu_bitrate_1200);
            
            trx.IsisTrxvu_tcSetIdlestate(ISIStrxvuIdleState.trxvu_idle_state_off);
            Assert.AreEqual(trx.transmitter.state, ISIStrxvuIdleState.trxvu_idle_state_off);

            trx.IsisTrxvu_tcSetIdlestate(ISIStrxvuIdleState.trxvu_idle_state_on);
            Assert.AreEqual(trx.transmitter.state, ISIStrxvuIdleState.trxvu_idle_state_on);
        }

        [TestMethod()]
        public void IsisTrxvu_tcSetAx25BitrateTest()
        {
            ISIStrxvuFrameLengths len = new ISIStrxvuFrameLengths();
            len.maxAX25frameLengthRX = 20;
            len.maxAX25frameLengthTX = 20;
            TRX trx = new TRX(0, new ISIStrxvuI2CAddress(), len, ISIStrxvuBitrate.trxvu_bitrate_1200);

            trx.IsisTrxvu_tcSetAx25Bitrate(ISIStrxvuBitrate.trxvu_bitrate_1200);
            Assert.AreEqual(trx.default_bitrates, ISIStrxvuBitrate.trxvu_bitrate_1200);
            Assert.AreEqual(trx.transmitter.bitrate, TRX.MapBitrateToState[ISIStrxvuBitrate.trxvu_bitrate_1200]);
            Assert.AreEqual(trx.receiver.bitrate, TRX.MapBitrateToState[ISIStrxvuBitrate.trxvu_bitrate_1200]);

            trx.IsisTrxvu_tcSetAx25Bitrate(ISIStrxvuBitrate.trxvu_bitrate_2400);
            Assert.AreEqual(trx.default_bitrates, ISIStrxvuBitrate.trxvu_bitrate_2400);
            Assert.AreEqual(trx.transmitter.bitrate, TRX.MapBitrateToState[ISIStrxvuBitrate.trxvu_bitrate_2400]);
            Assert.AreEqual(trx.receiver.bitrate, TRX.MapBitrateToState[ISIStrxvuBitrate.trxvu_bitrate_2400]);
        }
        
    }
}