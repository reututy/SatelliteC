using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading.Tasks;
using DataModel;
using DataModel.EPS;
using DataModel.TRX;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace DemoService
{
    public class AsyncService
    {
        public static Logic.GomEPS eps;
        public static Logic.IsisTRXVU trx;
        public static Logic.FRAMLogic fram;
        public static IPAddress ipAddress;
        public static int port = 4444;

        public static async void Run()
        {
            string hostName = Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.GetHostEntry(hostName);
            ipAddress = null;
            for (int i = 0; i < ipHostInfo.AddressList.Length; ++i)
            {
                if (ipHostInfo.AddressList[i].AddressFamily ==
                  AddressFamily.InterNetwork)
                {
                    ipAddress = ipHostInfo.AddressList[i];
                    break;
                }
            }
            if (ipAddress == null)
                throw new Exception("No IPv4 address for server");

            TcpListener listener = new TcpListener(IPAddress.Loopback, port);
            listener.Start();
            Console.Write("Array Min and Avg service is now running");
          
            Console.WriteLine(" on port " + port);
            Console.WriteLine("Hit <enter> to stop service\n");
            while (true)
            {
                try
                {
                    TcpClient tcpClient = await listener.AcceptTcpClientAsync();
                    Task t = Process(tcpClient);
                    await t;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public static async Task Process(TcpClient tcpClient)
        {
            string clientEndPoint =
              tcpClient.Client.RemoteEndPoint.ToString();
            Console.WriteLine("Received connection request from "
              + clientEndPoint);
            try
            {
                NetworkStream networkStream = tcpClient.GetStream();
                StreamReader reader = new StreamReader(networkStream);
                StreamWriter writer = new StreamWriter(networkStream);
                writer.AutoFlush = true;
                while (true)
                {
                    string request = await reader.ReadLineAsync();
                    if (request != null)
                    {
                        Console.WriteLine("Received service request: " + request);
                        byte[] response = Response(request);
                        Console.WriteLine("Computed response is: " + response + "\n");
                        await writer.BaseStream.WriteAsync(response, 0, response.Length);
                    }
                    else
                        break; // Client closed connection
                }
                tcpClient.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (tcpClient.Connected)
                    tcpClient.Close();
            }
        }
        public static byte[] Response(string request)
        {
            byte[] analyzedResponse = analyzeRequest(request);
            return analyzedResponse;
        }

        public static byte[] analyzeRequest(string request)
        {
            byte[] response = { 0 };
            string[] args = request.Split('&');
            string methodName = args[0];
            if (methodName.Contains("GomEps"))
                response = analyzeGomEps(request);
            else if (methodName.Contains("IsisTrxvu"))
                response = analyzeIsisTrxvu(request);
            else if (methodName.Contains("FRAM"))
                response = analyzeFRAM(request);
            return response;
        }

        private static byte[] analyzeFRAM(string request)
        {
            byte[] response = { 0 };
            string[] args = request.Split('&');
            string methodName = args[0];
            switch (methodName)
            {
                case "FRAM_start":
                    {
                        response = convertErrorToByteArr(fram.FRAM_start());
                        break;
                    }
                case "FRAM_write":
                    {
                        byte[] data = Encoding.ASCII.GetBytes(args[1]);
                        uint address = Convert.ToUInt32(args[2]);
                        uint size = BitConverter.ToUInt32(Encoding.ASCII.GetBytes(args[3]), 0);
                        response = convertErrorToByteArr(fram.FRAM_write(data, address, size));
                        break;
                    }
                case "FRAM_read":
                    {
                        uint address = Convert.ToUInt32(args[1]);
                        uint size = BitConverter.ToUInt32(Encoding.ASCII.GetBytes(args[2]), 0);
                        byte[] data = new byte[size];
                        byte[] err = convertErrorToByteArr(fram.FRAM_read(data, address, size));
                        response = concatBytesArr(err, data);
                        break;
                    }
            }
            return response;
        }

        public static byte[] analyzeIsisTrxvu(string request)
        {
            byte[] response = { 0 };
            string[] args = request.Split('&');
            string methodName = args[0];
            byte index = Encoding.ASCII.GetBytes(args[1])[0];
            switch (methodName)
            {
                case "IsisTrxvu_initialize":
                    {
                        byte number = Encoding.ASCII.GetBytes(args[4])[0];
                        ISIStrxvuI2CAddress[] address = convertToSturctISIStrxvuI2CAddressArr(args[1], number);
                        ISIStrxvuFrameLengths[] maxFrameLengths = convertToStructISIStrxvuFrameLengths(args[2], number);
                        ISIStrxvuBitrate default_bitrates = convertToStructISIStrxvuBitrate(args[3]);
                        response = convertErrorToByteArr(trx.IsisTrxvu_initialize(address, maxFrameLengths, default_bitrates, number));
                        break;
                    }
                case "IsisTrxvu_componentSoftReset":
                    {
                        ISIStrxvuComponent component = convertToSturctISIStrxvuComponent(args[2]);
                        response = convertErrorToByteArr(trx.IsisTrxvu_componentSoftReset(index, component));
                        break;
                    }
                case "IsisTrxvu_componentHardReset":
                    {
                        ISIStrxvuComponent component = convertToSturctISIStrxvuComponent(args[2]);
                        response = convertErrorToByteArr(trx.IsisTrxvu_componentHardReset(index, component));
                        break;
                    }
                case "IsisTrxvu_softReset":
                    {
                        response = convertErrorToByteArr(trx.IsisTrxvu_softReset(index));
                        break;
                    }
                case "IsisTrxvu_hardReset":
                    {
                        response = convertErrorToByteArr(trx.IsisTrxvu_hardReset(index));
                        break;
                    }
                case "IsisTrxvu_tcSendAX25DefClSign":
                    {
                        byte[] data = Encoding.ASCII.GetBytes(args[2]);
                        byte length = Encoding.ASCII.GetBytes(args[3])[0];
                        Output<Byte> avail = new Output<byte>();
                        byte[] err = convertErrorToByteArr(trx.IsisTrxvu_tcSendAX25DefClSign(index, data, length, avail));
                        byte[] output = new byte[] { avail.output };
                        response = concatBytesArr(err, output);
                        break;
                    }
                case "IsisTrxvu_tcSendAX25OvrClSign":
                    {
                        char[] fromCallsign = args[2].ToCharArray(0, args[2].Length);
                        char[] toCallsign = args[3].ToCharArray(0, args[3].Length);
                        byte[] data = Encoding.ASCII.GetBytes(args[4]);
                        byte length = Encoding.ASCII.GetBytes(args[5])[0];
                        Output<Byte> avail = new Output<byte>();
                        byte[] err = convertErrorToByteArr(trx.IsisTrxvu_tcSendAX25OvrClSign(index, fromCallsign, toCallsign, data, length, avail));
                        byte[] output = new byte[] { avail.output };
                        response = concatBytesArr(err, output);
                        break;
                    }
                case "IsisTrxvu_tcSetAx25BeaconDefClSign":
                    {
                        byte[] data = Encoding.ASCII.GetBytes(args[2]);
                        byte length = Encoding.ASCII.GetBytes(args[3])[0];
                        ushort interval = BitConverter.ToUInt16(Encoding.ASCII.GetBytes(args[4]), 0);
                        response = convertErrorToByteArr(trx.IsisTrxvu_tcSetAx25BeaconDefClSign(index, data, length, interval));
                        break;
                    }
                case "IsisTrxvu_tcSetAx25BeaconOvrClSign":
                    {
                        char[] fromCallsign = args[2].ToCharArray(0, args[2].Length);
                        char[] toCallsign = args[3].ToCharArray(0, args[3].Length);
                        byte[] data = Encoding.ASCII.GetBytes(args[4]);
                        byte length = Encoding.ASCII.GetBytes(args[5])[0];
                        ushort interval = BitConverter.ToUInt16(Encoding.ASCII.GetBytes(args[6]), 0);
                        response = convertErrorToByteArr(trx.IsisTrxvu_tcSetAx25BeaconOvrClSign(index, fromCallsign, toCallsign, data, length, interval));
                        break;
                    }
                case "IsisTrxvu_tcClearBeacon":
                    {
                        response = convertErrorToByteArr(trx.IsisTrxvu_tcClearBeacon(index));
                        break;
                    }
                case "IsisTrxvu_tcSetDefToClSign":
                    {
                        char[] toCallsign = args[2].ToCharArray(0, args[2].Length);
                        response = convertErrorToByteArr(trx.IsisTrxvu_tcSetDefToClSign(index, toCallsign));
                        break;
                    }
                case "IsisTrxvu_tcSetDefFromClSign":
                    {
                        char[] toCallsign = args[2].ToCharArray(0, args[2].Length);
                        response = convertErrorToByteArr(trx.IsisTrxvu_tcSetDefFromClSign(index, toCallsign));
                        break;
                    }
                case "IsisTrxvu_tcSetIdlestate":
                    {
                        ISIStrxvuIdleState state = (ISIStrxvuIdleState)Encoding.ASCII.GetBytes(args[2])[0];
                        response = convertErrorToByteArr(trx.IsisTrxvu_tcSetIdlestate(index, state));
                        break;
                    }
                case "IsisTrxvu_tcSetAx25Bitrate":
                    {
                        ISIStrxvuBitrate bitrate = (ISIStrxvuBitrate)Encoding.ASCII.GetBytes(args[2])[0];
                        response = convertErrorToByteArr(trx.IsisTrxvu_tcSetAx25Bitrate(index, bitrate));
                        break;
                    }
                case "IsisTrxvu_tcGetUptime":
                    {
                        /*Output<Byte> uptime = new Output<byte>();
                        byte[] err = convertErrorToByteArr(trx.IsisTrxvu_tcGetUptime(index, uptime));
                        byte[] output = { uptime.output };
                        response = concatBytesArr(err, output);*/
                        break;
                    }
                case "IsisTrxvu_tcGetState":
                    {
                        Output<ISIStrxvuTransmitterState> currentvutcState = new Output<ISIStrxvuTransmitterState>();
                        ISIStrxvuTransmitterState currOutput = new ISIStrxvuTransmitterState();
                        currOutput.transmitter_bitrate = ISIStrxvuBitrateStatus.trxvu_bitratestatus_4800;
                        currOutput.transmitter_idle_state = ISIStrxvuIdleState.trxvu_idle_state_off;
                        currentvutcState.output = currOutput;
                        byte[] err = convertErrorToByteArr(trx.IsisTrxvu_tcGetState(index, currentvutcState));
                        byte[] output = getBytes(currentvutcState.output);
                        response = concatBytesArr(err, output);
                        break;
                    }
                case "IsisTrxvu_tcGetTelemetryAll":
                    {
                        Output<ISIStrxvuTxTelemetry> telemetry = new Output<ISIStrxvuTxTelemetry>();
                        byte[] err = convertErrorToByteArr(trx.IsisTrxvu_tcGetTelemetryAll(index, telemetry));
                        byte[] output = getBytes(telemetry.output);

                        response = concatBytesArr(err, output);
                        break;
                    }
                case "IsisTrxvu_tcGetLastTxTelemetry":
                    {
                        Output<ISIStrxvuTxTelemetry> telemetry = new Output<ISIStrxvuTxTelemetry>();
                        byte[] err = convertErrorToByteArr(trx.IsisTrxvu_tcGetLastTxTelemetry(index, telemetry));
                        //byte[] err = convertErrorToByteArr(0);
                        //ISIStrxvuTxTelemetry telemetryoutput = new ISIStrxvuTxTelemetry();
                        //telemetryoutput.tx_reflpwr = 1;
                        //telemetryoutput.pa_temp = 2;
                        //telemetryoutput.tx_fwrdpwr = 3;
                        //telemetryoutput.tx_current = 4;
                        //telemetry.output = telemetryoutput;

                        byte[] output = getBytes(telemetry.output);

                        response = concatBytesArr(err, output);
                        break;
                    }
                case "IsisTrxvu_tcEstimateTransmissionTime":
                    {
                        byte length = Encoding.ASCII.GetBytes(args[2])[0];
                        response = convertErrorToByteArr(trx.IsisTrxvu_tcEstimateTransmissionTime(index, length));
                        break;
                    }
                case "IsisTrxvu_rcGetFrameCount":
                    {
                        Output<ushort> frameCount = new Output<ushort>();
                        byte[] err = convertErrorToByteArr(trx.IsisTrxvu_rcGetFrameCount(index, frameCount));
                        //byte[] err = convertErrorToByteArr(0);
                        //frameCount.output = 5;
                        byte[] output = BitConverter.GetBytes(frameCount.output);
                        response = concatBytesArr(err, output);
                        break;
                    }
                case "IsisTrxvu_rcGetCommandFrame":
                    {
                        Output<ISIStrxvuRxFrame> rx_frame = new Output<ISIStrxvuRxFrame>();
                        byte[] err = convertErrorToByteArr(trx.IsisTrxvu_rcGetCommandFrame(index, rx_frame));
                        //byte[] err = convertErrorToByteArr(0);
                        //ISIStrxvuRxFrame rxFrameOut = new ISIStrxvuRxFrame();
                        //rxFrameOut.rx_doppler = 1;
                        //rxFrameOut.rx_framedata = new byte[]{ 2,3,4,5};
                        //rxFrameOut.rx_length = 4;
                        //rxFrameOut.rx_rssi = 8;
                        //rx_frame.output = rxFrameOut;
                        byte[] output = getBytes(rx_frame.output);
                        response = concatBytesArr(err, output);
                        break;
                    }
                case "IsisTrxvu_rcGetTelemetryAll":
                    {
                        Output<ISIStrxvuRxTelemetry> telemetry = new Output<ISIStrxvuRxTelemetry>();
                        //ISIStrxvuRxTelemetry telOutput = new ISIStrxvuRxTelemetry();
                        //telOutput.tx_current = 1;
                        //telOutput.rx_doppler = 2;
                        //telOutput.rx_current = 3;
                        //telOutput.bus_volt = 4;
                        //telOutput.board_temp = 5;
                        //telOutput.pa_temp = 6;
                        //telOutput.rx_rssi = 7;
                        //telemetry.output = telOutput;
                        byte[] err = convertErrorToByteArr(trx.IsisTrxvu_rcGetTelemetryAll(index, telemetry));
                        //byte[] err = convertErrorToByteArr(0);
                        byte[] output = getBytes(telemetry.output);
                        response = concatBytesArr(err, output);
                        break;
                    }
                case "IsisTrxvu_rcGetUptime":
                    {
                        Output<Byte[]> uptime = new Output<byte[]>();
                        byte[] err = convertErrorToByteArr(trx.IsisTrxvu_rcGetUptime(index, uptime));
                        //byte[] err = convertErrorToByteArr(0);
                        //uptime.output = new byte[]{ 1,2,3,4 };
                        response = concatBytesArr(err, uptime.output);
                        break;
                    }
            }
            return response;
        }

        public static byte[] getBytes(ISIStrxvuTransmitterState str)
        {
            byte[] ans = new byte[255];
            int index = 0;

            copyToByteArr(ans, (uint)str.transmitter_idle_state, ref index);
            copyToByteArr(ans, (uint)str.transmitter_beacon, ref index);
            copyToByteArr(ans, (uint)str.transmitter_bitrate, ref index);

            return ans;
        }

        public static byte[] getBytes(ISIStrxvuRxTelemetry str)
        {
            int size = Marshal.SizeOf(str);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        public static byte[] getBytes(ISIStrxvuRxFrame str)
        {
            byte[] ans = new byte[255];
            int index = 0;

            copyToByteArr(ans, str.rx_length, ref index);
            copyToByteArr(ans, str.rx_doppler, ref index);
            copyToByteArr(ans, str.rx_rssi, ref index);
            copyToByteArr(ans, str.rx_framedata, ref index);

            return ans;
        }

        public static byte[] getBytes(ISIStrxvuTxTelemetry str)
        {
            int size = Marshal.SizeOf(str);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        public static ISIStrxvuBitrate convertToStructISIStrxvuBitrate(string v)
        {
            byte bitrate = Encoding.ASCII.GetBytes(v)[0];
            switch (bitrate)
            {
                case 1:
                    return ISIStrxvuBitrate.trxvu_bitrate_1200;
                case 2:
                    return ISIStrxvuBitrate.trxvu_bitrate_2400;
                case 4:
                    return ISIStrxvuBitrate.trxvu_bitrate_4800;
                case 8:
                    return ISIStrxvuBitrate.trxvu_bitrate_9600;
                default:
                    throw new Exception("Illegal bitrate");
            }
        }

        public static ISIStrxvuFrameLengths[] convertToStructISIStrxvuFrameLengths(string v, byte number)
        {
            string[] framesLengths = split(v, number);
            ISIStrxvuFrameLengths[] ans = new ISIStrxvuFrameLengths[number];
            for (int i = 0; i < framesLengths.Length; i++)
            {
                string[] fields = split(framesLengths[i], 2);
                ans[i].maxAX25frameLengthTX = BitConverter.ToUInt32(Encoding.ASCII.GetBytes(fields[0]), 0);
                ans[i].maxAX25frameLengthRX = BitConverter.ToUInt32(Encoding.ASCII.GetBytes(fields[1]), 0);
            }
            return ans;
        }

        public static ISIStrxvuI2CAddress[] convertToSturctISIStrxvuI2CAddressArr(string v, byte number)
        {
            string[] addresses = split(v, number);
            ISIStrxvuI2CAddress[] ans = new ISIStrxvuI2CAddress[number];
            for (int i = 0; i < addresses.Length; i++)
            {
                ans[i].addressVu_rc = BitConverter.GetBytes(addresses[i][0])[0];
                ans[i].addressVu_tc = BitConverter.GetBytes(addresses[i][1])[0];
            }
            return ans;
        }

        public static string[] split(string value, int numberOfItems)
        {

            if (value.Length == 0) { return new string[0]; }

            int desiredLength = value.Length / numberOfItems;

            int remaining = (value.Length > numberOfItems * desiredLength) ? 1 : 0;

            List<string> splitted = new List<string>(numberOfItems + remaining);

            for (int i = 0; i < numberOfItems; i++)
            {
                splitted.Add(value.Substring(i * desiredLength, desiredLength));
            }

            if (remaining != 0)
            {
                splitted.Add(value.Substring(numberOfItems * desiredLength));
            }

            return splitted.ToArray();
        }

        public static byte[] concatBytesArr(byte[] a1, byte[] a2)
        {
            byte[] rv = new byte[a1.Length + a2.Length];
            System.Buffer.BlockCopy(a1, 0, rv, 0, a1.Length);
            System.Buffer.BlockCopy(a2, 0, rv, a1.Length, a2.Length);
            return rv;
        }

        public static byte[] analyzeGomEps(string request)
        {
            byte[] response = { 0 };
            string[] args = request.Split('&');
            string methodName = args[0];
            byte index = Encoding.ASCII.GetBytes(args[1])[0];
            switch (methodName)
            {
                case "GomEpsInitialize":
                    {
                        byte i2c_address = Encoding.ASCII.GetBytes(args[1])[0];
                        byte number = Encoding.ASCII.GetBytes(args[2])[0];
                        response = convertErrorToByteArr(eps.GomEpsInitialize(i2c_address, number));
                        break;
                    }
                case "GomEpsPing":
                    {
                        byte pingByte = Encoding.ASCII.GetBytes(args[2])[0];
                        Output<Byte> byteOut = new Output<byte>(); // How to use it??
                        byte[] err = convertErrorToByteArr(eps.GomEpsPing(index, pingByte, byteOut));
                        byte[] output = new byte[] { byteOut.output };
                        response = concatBytesArr(err, output);
                        break;
                    }
                case "GomEpsSoftReset":
                    {
                        response = convertErrorToByteArr(eps.GomEpsSoftReset(index));
                        break;
                    }
                case "GomEpsHardReset":
                    {
                        response = convertErrorToByteArr(eps.GomEpsHardReset(index));
                        break;
                    }
                case "GomEpsGetHkData_param":
                    {
                        //TODO: Take care when mor implement function
                        break;
                    }
                case "GomEpsGetHkData_general":
                    {
                        Output<EPS.eps_hk_t> dataOut = new Output<EPS.eps_hk_t>();
                        byte[] err = convertErrorToByteArr(eps.GomEpsGetHkData_general(index, dataOut));
                        byte[] output = getBytes(dataOut.output);
                        response = concatBytesArr(err, output);
                        break;
                    }
                case "GomEpsGetHkData_vi":
                    {
                        Output<EPS.eps_hk_vi_t> dataOut = new Output<EPS.eps_hk_vi_t>();
                        byte[] err = convertErrorToByteArr(eps.GomEpsGetHkData_vi(index, dataOut));
                        byte[] output = getBytes(dataOut.output);
                        response = concatBytesArr(err, output);
                        break;
                    }
                case "GomEpsGetHkData_out":
                    {
                        Output<EPS.eps_hk_out_t> dataOut = new Output<EPS.eps_hk_out_t>();
                        byte[] err = convertErrorToByteArr(eps.GomEpsGetHkData_out(index, dataOut));
                        byte[] output = getBytes(dataOut.output);
                        response = concatBytesArr(err, output);
                        break;
                    }
                case "GomEpsGetHkData_wdt":
                    {
                        Output<EPS.eps_hk_wdt_t> dataOut = new Output<EPS.eps_hk_wdt_t>();
                        byte[] err = convertErrorToByteArr(eps.GomEpsGetHkData_wdt(index, dataOut));
                        byte[] output = getBytes(dataOut.output);
                        response = concatBytesArr(err, output);
                        break;
                    }
                case "GomEpsGetHkData_basic":
                    {
                        Output<EPS.eps_hk_basic_t> dataOut = new Output<EPS.eps_hk_basic_t>();
                        byte[] err = convertErrorToByteArr(eps.GomEpsGetHkData_basic(index, dataOut));
                        byte[] output = getBytes(dataOut.output);
                        response = concatBytesArr(err, output);
                        break;
                    }
                case "GomEpsSetOutput":
                    {
                        byte output = Encoding.ASCII.GetBytes(args[2])[0];
                        response = convertErrorToByteArr(eps.GomEpsSetOutput(index, output));
                        break;
                    }
                case "GomEpsSetSingleOutput":
                    {
                        byte channel_id = Encoding.ASCII.GetBytes(args[2])[0];
                        byte value = Encoding.ASCII.GetBytes(args[3])[0];
                        ushort delay = BitConverter.ToUInt16(Encoding.ASCII.GetBytes(args[4]), 0);
                        response = convertErrorToByteArr(eps.GomEpsSetSingleOutput(index, channel_id, value, delay));
                        break;
                    }
                case "GomEpsSetPhotovoltaicInputs":
                    {
                        ushort voltage1 = BitConverter.ToUInt16(Encoding.ASCII.GetBytes(args[2]), 0);
                        ushort voltage2 = BitConverter.ToUInt16(Encoding.ASCII.GetBytes(args[3]), 0);
                        ushort voltage3 = BitConverter.ToUInt16(Encoding.ASCII.GetBytes(args[4]), 0);
                        response = convertErrorToByteArr(eps.GomEpsSetPhotovoltaicInputs(index, voltage1, voltage2, voltage3));
                        break;
                    }
                case "GomEpsSetPptMode":
                    {
                        byte mode = Encoding.ASCII.GetBytes(args[2])[0];
                        response = convertErrorToByteArr(eps.GomEpsSetPptMode(index, mode));
                        break;
                    }
                case "GomEpsSetHeaterAutoMode": //TODO: need to check after mor will fix the function
                    {
                        byte auto_mode = Encoding.ASCII.GetBytes(args[2])[0];
                        Output<ushort> auto_mode_return = new Output<ushort>();
                        //response = String.Concat(convertErrorToByteArr(eps.GomEpsSetHeaterAutoMode(index, auto_mode, auto_mode_return)), Encoding.UTF8.GetString(getBytes(auto_mode_return.output))); //Maybe need to be different
                        byte[] err = convertErrorToByteArr(eps.GomEpsSetHeaterAutoMode(index, auto_mode, auto_mode_return));
                        byte[] output = BitConverter.GetBytes(Convert.ToInt32(auto_mode_return.output));
                        response = concatBytesArr(err, output);
                        break;
                    }
                case "GomEpsResetCounters":
                    {
                        response = convertErrorToByteArr(eps.GomEpsResetCounters(index));
                        break;
                    }
                case "GomEpsResetWDT":
                    {
                        response = convertErrorToByteArr(eps.GomEpsResetWDT(index));
                        break;
                    }
                case "GomEpsConfigCMD":
                    {
                        byte cmd = Encoding.ASCII.GetBytes(args[2])[0];
                        response = convertErrorToByteArr(eps.GomEpsConfigCMD(index, cmd));
                        break;
                    }
                case "GomEpsConfigGet":
                    {
                        Output<EPS.eps_config_t> config_data = new Output<EPS.eps_config_t>();
                        byte[] err = convertErrorToByteArr(eps.GomEpsConfigGet(index, config_data));
                        byte[] output = getBytes(config_data.output);
                        response = concatBytesArr(err, output);
                        break;
                    }
                case "GomEpsConfigSet":
                    {
                        Output<EPS.eps_config_t> config_data = new Output<EPS.eps_config_t>();
                        config_data.output = convertToSturcteps_config_t(args[2]);
                        response = convertErrorToByteArr(eps.GomEpsConfigSet(index, config_data));
                        break;
                    }
                case "GomEpsConfig2CMD":
                    {
                        byte cmd = Encoding.ASCII.GetBytes(args[2])[0];
                        response = convertErrorToByteArr(eps.GomEpsConfig2CMD(index, cmd));
                        break;
                    }
                case "GomEpsConfig2Get": //TODO: check after mor fix the function
                    {
                        Output<EPS.eps_config2_t> config_data = new Output<EPS.eps_config2_t>();
                        //response = String.Concat(convertErrorToByteArr(eps.GomEpsConfig2Get(index, config_data)), Encoding.UTF8.GetString(getBytes(config_data.output))); //Maybe need to be different
                        byte[] err = convertErrorToByteArr(eps.GomEpsConfig2Get(index, config_data));
                        byte[] output = getBytes(config_data.output);
                        response = concatBytesArr(err, output);
                        break;
                    }
                case "GomEpsConfig2Set":
                    {
                        Output<EPS.eps_config2_t> config_data = new Output<EPS.eps_config2_t>();
                        config_data.output = convertToSturcteps_config2_t(args[2]);
                        response = convertErrorToByteArr(eps.GomEpsConfig2Set(index, config_data));
                        break;
                    }
            }
            return response;
        }

        public static ISIStrxvuComponent convertToSturctISIStrxvuComponent(string str)
        {
            byte component = Encoding.ASCII.GetBytes(str)[0];
            switch (component)
            {
                case 0:
                    return ISIStrxvuComponent.trxvu_rc;
                case 1:
                    return ISIStrxvuComponent.trxvu_tc;
                default:
                    throw new Exception("Illegal ISIStrxvuComponent");
            }
        }

        public static ISIStrxvuI2CAddress[] convertToSturctISIStrxvuI2CAddressArr(string str)
        {
            IntPtr pBuf = Marshal.StringToBSTR(str);
            ISIStrxvuI2CAddress strct = (ISIStrxvuI2CAddress)Marshal.PtrToStructure(pBuf, typeof(ISIStrxvuI2CAddress));
            ISIStrxvuI2CAddress[] ret = new ISIStrxvuI2CAddress[1];
            ret[0] = strct;
            return ret;
        }

        public static EPS.eps_config_t convertToSturcteps_config_t(string str)
        {
            IntPtr pBuf = Marshal.StringToBSTR(str);
            EPS.eps_config_t strct = (EPS.eps_config_t)Marshal.PtrToStructure(pBuf, typeof(EPS.eps_config_t));
            return strct;
        }

        public static EPS.eps_config2_t convertToSturcteps_config2_t(string str)
        {
            IntPtr pBuf = Marshal.StringToBSTR(str);
            EPS.eps_config2_t strct = (EPS.eps_config2_t)Marshal.PtrToStructure(pBuf, typeof(EPS.eps_config2_t));
            return strct;
        }

        public static byte[] convertErrorToByteArr(int err)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] errByteArr = BitConverter.GetBytes(0-err);
            return errByteArr;
        }

        public static byte[] getBytes(EPS.eps_config2_t str)
        {
            byte[] ans = new byte[133]; //TODO: determine size
            int index = 0;

            copyToByteArr(ans, str.commandReply, ref index);
            copyToByteArr(ans, str.batt_maxvoltage, ref index);
            copyToByteArr(ans, str.batt_safevoltage, ref index);
            copyToByteArr(ans, str.batt_criticalvoltage, ref index);
            copyToByteArr(ans, str.batt_normalvoltage, ref index);

            ushort roundUpToIntSize1 = 0;
            copyToByteArr(ans, roundUpToIntSize1, ref index);

            copyToByteArr(ans, str.reserved1, ref index);
            copyToByteArr(ans, str.reserved2, ref index);

            return ans;
        }

        public static byte[] getBytes(EPS.eps_config_t str)
        {
            byte[] ans = new byte[133]; //TODO: determine size
            int index = 0;

            copyToByteArr(ans, str.commandReply, ref index);

            ans[index] = str.ppt_mode;
            index += 1;

            ans[index] = str.battheater_mode;
            index += 1;

            ans[index] = str.battheater_low;
            index += 1;

            ans[index] = str.battheater_high;
            index += 1;

            copyToByteArr(ans, str.output_normal_value, ref index);
            copyToByteArr(ans, str.output_safe_value, ref index);
            copyToByteArr(ans, str.output_initial_on_delay, ref index);
            copyToByteArr(ans, str.output_initial_off_delay, ref index);
            copyToByteArr(ans, str.vboost, ref index);

            return ans;
        }

        public static byte[] getBytes(EPS.eps_hk_t str)
        {
            byte[] ans = new byte[140]; //TODO: determine size
            int index = 0;

            copyToByteArr(ans, str.commandReply, ref index);
            copyToByteArr(ans, str.vboost, ref index);
            copyToByteArr(ans, str.vbatt, ref index);
            copyToByteArr(ans, str.curin, ref index);
            copyToByteArr(ans, str.cursun, ref index);
            copyToByteArr(ans, str.cursys, ref index);
            copyToByteArr(ans, str.reserved1, ref index);
            copyToByteArr(ans, str.curout, ref index);
            copyToByteArr(ans, str.output, ref index);
            copyToByteArr(ans, str.output_on_delta, ref index);
            copyToByteArr(ans, str.output_off_delta, ref index);
            copyToByteArr(ans, str.latchup, ref index);
            ushort roundUpToIntSize1 = 0;
            copyToByteArr(ans, roundUpToIntSize1, ref index);
            copyToByteArr(ans, str.wdt_i2c_time_left, ref index);
            copyToByteArr(ans, str.wdt_gnd_time_left, ref index);
            copyToByteArr(ans, str.wdt_csp_pings_left, ref index);
            copyToByteArr(ans, str.counter_wdt_i2c, ref index);
            copyToByteArr(ans, str.counter_wdt_gnd, ref index);
            copyToByteArr(ans, str.counter_wdt_csp, ref index);
            byte roundUpToIntSize2 = 0;
            copyToByteArr(ans, roundUpToIntSize2, ref index);
            copyToByteArr(ans, str.counter_boot, ref index);
            copyToByteArr(ans, str.temp, ref index);
            ans[index] = str.bootcause;
            index += 1;
            ans[index] = str.battmode;
            index += 1;
            ans[index] = str.pptmode;
            index += 1;
            copyToByteArr(ans, str.reserved2, ref index);
            return ans;
        }

        public static void copyToByteArr(byte[] target, byte[] source, ref int index)
        {
            Buffer.BlockCopy(source, 0, target, index, source.Length * sizeof(byte));
            index += source.Length * sizeof(byte);
        }

        public static void copyToByteArr(byte[] target, short[] source, ref int index)
        {
            Buffer.BlockCopy(source, 0, target, index, source.Length * sizeof(short));
            index += source.Length * sizeof(short);
        }

        public static void copyToByteArr(byte[] target, uint[] source, ref int index)
        {
            Buffer.BlockCopy(source, 0, target, index, source.Length * sizeof(uint));
            index += source.Length * sizeof(uint);
        }

        public static void copyToByteArr(byte[] target, ushort[] source, ref int index)
        {
            Buffer.BlockCopy(source, 0, target, index, source.Length * sizeof(ushort));
            index += source.Length * sizeof(ushort);
        }

        public static void copyToByteArr(byte[] target, ushort source, ref int index)
        {
            byte[] arr = BitConverter.GetBytes(source);
            Buffer.BlockCopy(arr, 0, target, index, arr.Length);
            index += arr.Length;
        }

        public static void copyToByteArr(byte[] target, uint source, ref int index)
        {
            byte[] arr = BitConverter.GetBytes(source);
            Buffer.BlockCopy(arr, 0, target, index, arr.Length);
            index += arr.Length;
        }

        public static byte[] getBytes(EPS.eps_hk_vi_t str)
        {
            byte[] ans = new byte[133]; //TODO: determine size
            int index = 0;

            copyToByteArr(ans, str.commandReply, ref index);
            copyToByteArr(ans, str.vboost, ref index);
            copyToByteArr(ans, str.vbatt, ref index);
            copyToByteArr(ans, str.curin, ref index);
            copyToByteArr(ans, str.cursun, ref index);
            copyToByteArr(ans, str.cursys, ref index);
            copyToByteArr(ans, str.reserved1, ref index);

            return ans;
        }

        public static byte[] getBytes(EPS.eps_hk_out_t str)
        {
            byte[] ans = new byte[66]; //TODO: determine size
            int index = 0;

            copyToByteArr(ans, str.commandReply, ref index);
            copyToByteArr(ans, str.curout, ref index);
            copyToByteArr(ans, str.output, ref index);
            copyToByteArr(ans, str.output_on_delta, ref index);
            copyToByteArr(ans, str.output_off_delta, ref index);
            copyToByteArr(ans, str.latchup, ref index);

            return ans;
        }

        public static byte[] getBytes(EPS.eps_hk_wdt_t str)
        {
            byte[] ans = new byte[32];
            int index = 0;

            copyToByteArr(ans, str.commandReply, ref index);

            ushort roundUpToIntSize1 = 0;
            copyToByteArr(ans, roundUpToIntSize1, ref index);

            copyToByteArr(ans, str.wdt_i2c_time_left, ref index);
            copyToByteArr(ans, str.wdt_gnd_time_left, ref index);
            copyToByteArr(ans, str.wdt_csp_pings_left, ref index);

            copyToByteArr(ans, roundUpToIntSize1, ref index);

            copyToByteArr(ans, str.counter_wdt_i2c, ref index);
            copyToByteArr(ans, str.counter_wdt_gnd, ref index);
            copyToByteArr(ans, str.counter_wdt_csp, ref index);

            return ans;
        }

        public static byte[] getBytes(EPS.eps_hk_basic_t str)
        {
            byte[] ans = new byte[133]; //TODO: determine size
            int index = 0;

            copyToByteArr(ans, str.commandReply, ref index);

            ushort roundUpToIntSize1 = 0;
            copyToByteArr(ans, roundUpToIntSize1, ref index);

            copyToByteArr(ans, str.counter_boot, ref index);
            copyToByteArr(ans, str.temp, ref index);
            //copyToByteArr(ans, str.bootcause, ref index); //20
            ans[index] = str.bootcause;
            index += 1;

            ans[index] = str.battmode;
            index += 1;

            ans[index] = str.pptmode;
            index += 1;
            //copyToByteArr(ans, str.battmode, ref index); //21


            //copyToByteArr(ans, str.pptmode, ref index); //22
            copyToByteArr(ans, str.reserved2, ref index);

            return ans;
        }

        public static byte[] getBytes(ushort str)
        {
            int size = Marshal.SizeOf(str);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(str, ptr, false);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }
    }
}