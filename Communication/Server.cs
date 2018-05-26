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

namespace DemoService
{
    class ServiceProgram
    {
        static void Main(string[] args)
        {
            try
            {
                int port = 4444;
                AsyncService service = new AsyncService(port);
                service.Run();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
    public class AsyncService
    {
        private Logic.GomEPS eps;
        private Logic.IsisTRXVU trx;
        private IPAddress ipAddress;
        private int port;
        public AsyncService(int port)
        {
            this.port = port;
            string hostName = Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.GetHostEntry(hostName);
            this.ipAddress = null;
            for (int i = 0; i < ipHostInfo.AddressList.Length; ++i)
            {
                if (ipHostInfo.AddressList[i].AddressFamily ==
                  AddressFamily.InterNetwork)
                {
                    this.ipAddress = ipHostInfo.AddressList[i];
                    break;
                }
            }
            if (this.ipAddress == null)
                throw new Exception("No IPv4 address for server");

            eps = new Logic.GomEPS();
            trx = new Logic.IsisTRXVU();
        }
        public async void Run()
        {
            TcpListener listener = new TcpListener(this.ipAddress, this.port);
            listener.Start();
            Console.Write("Array Min and Avg service is now running");
          
            Console.WriteLine(" on port " + this.port);
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

        private async Task Process(TcpClient tcpClient)
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
                        string response = Response(request);
                        Console.WriteLine("Computed response is: " + response + "\n");
                        await writer.WriteLineAsync(response);
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
        private string Response(string request)
        {
            string analyzedResponse = analyzeRequest(request);
            return analyzedResponse;
        }

        private string analyzeRequest(string request)
        {
            string response = "";
            string[] args = request.Split('&');
            string methodName = args[0];
            if (methodName.Contains("GomEps"))
                response = analyzeGomEps(request);
            else if (methodName.Contains("IsisTrxvu"))
                response = analyzeIsisTrxvu(request);
            return response;
        }

        private string analyzeIsisTrxvu(string request)
        {
            string response = "";
            string[] args = request.Split('&');
            string methodName = args[0];
            byte index = Encoding.ASCII.GetBytes(args[1])[0];
            switch (methodName)
            {
                case "IsisTrxvu_initialize":
                    {
                        //TODO: Finish!! 
                        ISIStrxvuI2CAddress[] address = convertToSturctISIStrxvuI2CAddressArr(args[1]);
                        byte number = Encoding.ASCII.GetBytes(args[2])[0];
                        break;
                    }
                case "IsisTrxvu_componentSoftReset":
                    {
                        ISIStrxvuComponent component = convertToSturctISIStrxvuComponent(args[2]);
                        response = convertErrorToString(trx.IsisTrxvu_componentSoftReset(index, component));
                        break;
                    }
                case "IsisTrxvu_componentHardReset":
                    {
                        ISIStrxvuComponent component = convertToSturctISIStrxvuComponent(args[2]);
                        response = convertErrorToString(trx.IsisTrxvu_componentHardReset(index, component));
                        break;
                    }
                case "IsisTrxvu_softReset":
                    {
                        response = convertErrorToString(trx.IsisTrxvu_softReset(index));
                        break;
                    }
                case "IsisTrxvu_hardReset":
                    {
                        response = convertErrorToString(trx.IsisTrxvu_hardReset(index));
                        break;
                    }
                case "IsisTrxvu_tcSendAX25DefClSign":
                    {
                        byte[] data = Encoding.ASCII.GetBytes(args[2]);
                        byte length = Encoding.ASCII.GetBytes(args[3])[0];
                        Output<Byte> avail = new Output<byte>();
                        response = String.Concat(convertErrorToString(trx.IsisTrxvu_tcSendAX25DefClSign(index, data, length, avail)), avail.output); //Maybe need to be different
                        break;
                    }
                case "IsisTrxvu_tcSendAX25OvrClSign":
                    {
                        //TODO : finish!! (need to see about the byte array)
                        byte[] data = Encoding.ASCII.GetBytes(args[2]);
                        byte length = Encoding.ASCII.GetBytes(args[3])[0];
                        Output<Byte> avail = new Output<byte>();
                        response = String.Concat(convertErrorToString(trx.IsisTrxvu_tcSendAX25DefClSign(index, data, length, avail)), avail.output); //Maybe need to be different
                        break;
                    }
                case "IsisTrxvu_tcSetAx25BeaconDefClSign":
                    {
                        byte[] data = Encoding.ASCII.GetBytes(args[2]);
                        byte length = Encoding.ASCII.GetBytes(args[3])[0];
                        ushort interval = Convert.ToUInt16(args[4]);
                        response = convertErrorToString(trx.IsisTrxvu_tcSetAx25BeaconDefClSign(index, data, length, interval));
                        break;
                    }
                case "IsisTrxvu_tcSetAx25BeaconOvrClSign":
                    {
                        byte[] fromCallsign = Encoding.ASCII.GetBytes(args[2]);
                        byte[] toCallsign = Encoding.ASCII.GetBytes(args[3]);
                        byte[] data = Encoding.ASCII.GetBytes(args[4]);
                        byte length = Encoding.ASCII.GetBytes(args[5])[0];
                        ushort interval = Convert.ToUInt16(args[6]);
                        response = convertErrorToString(trx.IsisTrxvu_tcSetAx25BeaconOvrClSign(index, fromCallsign, toCallsign, data, length, interval));
                        break;
                    }
                case "IsisTrxvu_tcClearBeacon":
                    {
                        response = convertErrorToString(trx.IsisTrxvu_tcClearBeacon(index));
                        break;
                    }
                case "IsisTrxvu_tcSetDefToClSign":
                    {
                        string toCallsign = args[2];
                        response = convertErrorToString(trx.IsisTrxvu_tcSetDefToClSign(index, toCallsign));
                        break;
                    }
                case "IsisTrxvu_tcSetDefFromClSign":
                    {
                        string toCallsign = args[2];
                        response = convertErrorToString(trx.IsisTrxvu_tcSetDefFromClSign(index, toCallsign));
                        break;
                    }
                case "IsisTrxvu_tcSetIdlestate":
                    {
                        ISIStrxvuIdleState state = (ISIStrxvuIdleState)Encoding.ASCII.GetBytes(args[2])[0];
                        response = convertErrorToString(trx.IsisTrxvu_tcSetIdlestate(index, state));
                        break;
                    }
                case "IsisTrxvu_tcSetAx25Bitrate":
                    {
                        ISIStrxvuBitrate bitrate = (ISIStrxvuBitrate)Encoding.ASCII.GetBytes(args[2])[0];
                        response = convertErrorToString(trx.IsisTrxvu_tcSetAx25Bitrate(index, bitrate));
                        break;
                    }
                case "IsisTrxvu_tcGetUptime":
                    {
                        Output<Byte> uptime = new Output<byte>();
                        response = String.Concat(convertErrorToString(trx.IsisTrxvu_tcGetUptime(index, uptime)), uptime.output); //Maybe need to be different
                        break;
                    }
                case "IsisTrxvu_tcGetState":
                    {
                        Output<ISIStrxvuTransmitterState> currentvutcState = new Output<ISIStrxvuTransmitterState>();
                        response = String.Concat(convertErrorToString(trx.IsisTrxvu_tcGetState(index, currentvutcState)), currentvutcState.output); //Maybe need to be different
                        break;
                    }
                case "IsisTrxvu_tcGetTelemetryAll":
                    {
                        Output<ISIStrxvuTxTelemetry> telemetry = new Output<ISIStrxvuTxTelemetry>();
                        response = String.Concat(convertErrorToString(trx.IsisTrxvu_tcGetTelemetryAll(index, telemetry)), telemetry.output); //Maybe need to be different
                        break;
                    }
                case "IsisTrxvu_tcGetLastTxTelemetry":
                    {
                        Output<ISIStrxvuTxTelemetry> last_telemetry = new Output<ISIStrxvuTxTelemetry>();
                        response = String.Concat(convertErrorToString(trx.IsisTrxvu_tcGetLastTxTelemetry(index, last_telemetry)), last_telemetry.output); //Maybe need to be different
                        break;
                    }
                case "IsisTrxvu_tcEstimateTransmissionTime":
                    {
                        byte length = Encoding.ASCII.GetBytes(args[2])[0];
                        response = convertErrorToString(trx.IsisTrxvu_tcEstimateTransmissionTime(index, length));
                        break;
                    }
                case "IsisTrxvu_rcGetFrameCount":
                    {
                        Output<ushort> frameCount = new Output<ushort>();
                        response = String.Concat(convertErrorToString(trx.IsisTrxvu_rcGetFrameCount(index, frameCount)), frameCount.output); //Maybe need to be different
                        break;
                    }
                case "IsisTrxvu_rcGetCommandFrame":
                    {
                        Output<ISIStrxvuRxFrame> rx_frame = new Output<ISIStrxvuRxFrame>();
                        response = String.Concat(convertErrorToString(trx.IsisTrxvu_rcGetCommandFrame(index, rx_frame)), rx_frame.output); //Maybe need to be different
                        break;
                    }
                case "IsisTrxvu_rcGetTelemetryAll":
                    {
                        Output<ISIStrxvuRxTelemetry> telemetry = new Output<ISIStrxvuRxTelemetry>();
                        response = String.Concat(convertErrorToString(trx.IsisTrxvu_rcGetTelemetryAll(index, telemetry)), telemetry.output); //Maybe need to be different
                        break;
                    }
                case "IsisTrxvu_rcGetUptime":
                    {
                        Output<Byte> uptime = new Output<byte>();
                        response = String.Concat(convertErrorToString(trx.IsisTrxvu_rcGetUptime(index, uptime)), uptime.output); //Maybe need to be different
                        break;
                    }
            }
            return response;
        }

        private string analyzeGomEps(string request)
        {
            string response = "";
            string[] args = request.Split('&');
            string methodName = args[0];
            byte index = Encoding.ASCII.GetBytes(args[1])[0];
            switch (methodName)
            {
                case "GomEpsInitialize":
                    {
                        byte i2c_address = Encoding.ASCII.GetBytes(args[1])[0];
                        byte number = Encoding.ASCII.GetBytes(args[2])[0];
                        response = convertErrorToString(eps.GomEpsInitialize(i2c_address, number));
                        break;
                    }
                case "GomEpsPing":
                    {
                        byte pingByte = Encoding.ASCII.GetBytes(args[2])[0];
                        Output<Byte> byteOut = new Output<byte>(); // How to use it??
                        response = String.Concat(convertErrorToString(eps.GomEpsPing(index, pingByte, byteOut)), byteOut.output); //Maybe need to be different
                        break;
                    }
                case "GomEpsSoftReset":
                    {
                        response = convertErrorToString(eps.GomEpsSoftReset(index));
                        break;
                    }
                case "GomEpsHardReset":
                    {
                        response = convertErrorToString(eps.GomEpsHardReset(index));
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
                        response = String.Concat(convertErrorToString(eps.GomEpsGetHkData_general(index, dataOut)), Encoding.UTF8.GetString(getBytes(dataOut.output))); //Maybe need to be different
                        break;
                    }
                case "GomEpsGetHkData_vi":
                    {
                        Output<EPS.eps_hk_vi_t> dataOut = new Output<EPS.eps_hk_vi_t>();
                        response = String.Concat(convertErrorToString(eps.GomEpsGetHkData_vi(index, dataOut)), Encoding.UTF8.GetString(getBytes(dataOut.output))); //Maybe need to be different
                        break;
                    }
                case "GomEpsGetHkData_out":
                    {
                        Output<EPS.eps_hk_out_t> dataOut = new Output<EPS.eps_hk_out_t>();
                        response = String.Concat(convertErrorToString(eps.GomEpsGetHkData_out(index, dataOut)), Encoding.UTF8.GetString(getBytes(dataOut.output))); //Maybe need to be different
                        break;
                    }
                case "GomEpsGetHkData_wdt":
                    {
                        Output<EPS.eps_hk_wdt_t> dataOut = new Output<EPS.eps_hk_wdt_t>();
                        response = String.Concat(convertErrorToString(eps.GomEpsGetHkData_wdt(index, dataOut)), Encoding.UTF8.GetString(getBytes(dataOut.output))); //Maybe need to be different
                        break;
                    }
                case "GomEpsGetHkData_basic":
                    {
                        Output<EPS.eps_hk_basic_t> dataOut = new Output<EPS.eps_hk_basic_t>();
                        response = String.Concat(convertErrorToString(eps.GomEpsGetHkData_basic(index, dataOut)), Encoding.UTF8.GetString(getBytes(dataOut.output))); //Maybe need to be different
                        break;
                    }
                case "GomEpsSetOutput":
                    {
                        byte output = Encoding.ASCII.GetBytes(args[2])[0];
                        response = convertErrorToString(eps.GomEpsSetOutput(index, output));
                        break;
                    }
                case "GomEpsSetSingleOutput":
                    {
                        byte channel_id = Encoding.ASCII.GetBytes(args[2])[0];
                        byte value = Encoding.ASCII.GetBytes(args[3])[0];
                        ushort delay = Convert.ToUInt16(args[4]);
                        response = convertErrorToString(eps.GomEpsSetSingleOutput(index, channel_id, value, delay));
                        break;
                    }
                case "GomEpsSetPhotovoltaicInputs":
                    {
                        ushort voltage1 = Convert.ToUInt16(args[2]);
                        ushort voltage2 = Convert.ToUInt16(args[3]);
                        ushort voltage3 = Convert.ToUInt16(args[4]);
                        response = convertErrorToString(eps.GomEpsSetPhotovoltaicInputs(index, voltage1, voltage2, voltage3));
                        break;
                    }
                case "GomEpsSetPptMode":
                    {
                        byte mode = Encoding.ASCII.GetBytes(args[2])[0];
                        response = convertErrorToString(eps.GomEpsSetPptMode(index, mode));
                        break;
                    }
                case "GomEpsSetHeaterAutoMode":
                    {
                        byte auto_mode = Encoding.ASCII.GetBytes(args[2])[0];
                        Output<ushort> auto_mode_return = new Output<ushort>();
                        response = String.Concat(convertErrorToString(eps.GomEpsSetHeaterAutoMode(index, auto_mode, auto_mode_return)), Encoding.UTF8.GetString(getBytes(auto_mode_return.output))); //Maybe need to be different
                        break;
                    }    
                case "GomEpsResetCounters":
                    {
                        response = convertErrorToString(eps.GomEpsResetCounters(index));
                        break;
                    }
                case "GomEpsResetWDT":
                    {
                        response = convertErrorToString(eps.GomEpsResetWDT(index));
                        break;
                    }
                case "GomEpsConfigCMD":
                    {
                        byte cmd = Encoding.ASCII.GetBytes(args[2])[0];
                        response = convertErrorToString(eps.GomEpsConfigCMD(index, cmd));
                        break;
                    }
                case "GomEpsConfigGet":
                    {
                        Output<EPS.eps_config_t> config_data = new Output<EPS.eps_config_t>();
                        response = String.Concat(convertErrorToString(eps.GomEpsConfigGet(index, config_data)), Encoding.UTF8.GetString(getBytes(config_data.output))); //Maybe need to be different
                        break;
                    }
                case "GomEpsConfigSet":
                    {
                        Output<EPS.eps_config_t> config_data = new Output<EPS.eps_config_t>();
                        config_data.output = convertToSturcteps_config_t(args[2]);
                        response = convertErrorToString(eps.GomEpsConfigSet(index, config_data));
                        break;
                    }
                case "GomEpsConfig2CMD":
                    {
                        byte cmd = Encoding.ASCII.GetBytes(args[2])[0];
                        response = convertErrorToString(eps.GomEpsConfig2CMD(index, cmd));
                        break;
                    }
                case "GomEpsConfig2Get":
                    {
                        Output<EPS.eps_config2_t> config_data = new Output<EPS.eps_config2_t>();
                        response = String.Concat(convertErrorToString(eps.GomEpsConfig2Get(index, config_data)), Encoding.UTF8.GetString(getBytes(config_data.output))); //Maybe need to be different
                        break;
                    }
                case "GomEpsConfig2Set":
                    {
                        Output<EPS.eps_config2_t> config_data = new Output<EPS.eps_config2_t>();
                        config_data.output = convertToSturcteps_config2_t(args[2]);
                        response = convertErrorToString(eps.GomEpsConfig2Set(index, config_data));
                        break;
                    }
            }
            return response;
        }

        private ISIStrxvuComponent convertToSturctISIStrxvuComponent(string str)
        {
            IntPtr pBuf = Marshal.StringToBSTR(str);
            ISIStrxvuComponent strct = (ISIStrxvuComponent)Marshal.PtrToStructure(pBuf, typeof(ISIStrxvuComponent));
            return strct;
        }

        private ISIStrxvuI2CAddress[] convertToSturctISIStrxvuI2CAddressArr(string str)
        {
            IntPtr pBuf = Marshal.StringToBSTR(str);
            ISIStrxvuI2CAddress strct = (ISIStrxvuI2CAddress)Marshal.PtrToStructure(pBuf, typeof(ISIStrxvuI2CAddress));
            ISIStrxvuI2CAddress[] ret = new ISIStrxvuI2CAddress[1];
            ret[0] = strct;
            return ret;
        }

        EPS.eps_config_t convertToSturcteps_config_t(string str)
        {
            IntPtr pBuf = Marshal.StringToBSTR(str);
            EPS.eps_config_t strct = (EPS.eps_config_t)Marshal.PtrToStructure(pBuf, typeof(EPS.eps_config_t));
            return strct;
        }

        EPS.eps_config2_t convertToSturcteps_config2_t(string str)
        {
            IntPtr pBuf = Marshal.StringToBSTR(str);
            EPS.eps_config2_t strct = (EPS.eps_config2_t)Marshal.PtrToStructure(pBuf, typeof(EPS.eps_config2_t));
            return strct;
        }

        string convertErrorToString(int err)
        {
            byte[] errByteArr = new byte[1];
            errByteArr[0] = Convert.ToByte(err);
            return Encoding.UTF8.GetString(errByteArr);
        }

        byte[] getBytes(EPS.eps_config2_t str)
        {
            int size = Marshal.SizeOf(str);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        byte[] getBytes(EPS.eps_config_t str)
        {
            int size = Marshal.SizeOf(str);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        byte[] getBytes(EPS.eps_hk_t str)
        {
            int size = Marshal.SizeOf(str);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        byte[] getBytes(EPS.eps_hk_vi_t str)
        {
            int size = Marshal.SizeOf(str);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        byte[] getBytes(EPS.eps_hk_out_t str)
        {
            int size = Marshal.SizeOf(str);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        byte[] getBytes(EPS.eps_hk_wdt_t str)
        {
            int size = Marshal.SizeOf(str);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        byte[] getBytes(EPS.eps_hk_basic_t str)
        {
            int size = Marshal.SizeOf(str);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        byte[] getBytes(ushort str)
        {
            int size = Marshal.SizeOf(str);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }
    }
}