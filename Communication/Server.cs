using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading.Tasks;
using DataModel;
using DataModel.EPS;
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
            throw new NotImplementedException();
        }

        private string analyzeGomEps(string request)
        {
            string response = "";
            string[] args = request.Split('&');
            string methodName = args[0];
            switch (methodName)
            {
                case "GomEpsInitialize":
                    {
                        byte i2c_address = Convert.ToByte(args[1]);
                        byte number = Convert.ToByte(args[2]);
                        response = convertErrorToString(eps.GomEpsInitialize(i2c_address, number));
                        break;
                    }
                case "GomEpsPing":
                    {
                        byte index = Convert.ToByte(args[1]);
                        byte pingByte = Convert.ToByte(args[2]);
                        Output<Byte> byteOut = new Output<byte>(); // How to use it??
                        response = String.Concat(convertErrorToString(eps.GomEpsPing(index, pingByte, byteOut)), byteOut.output); //Maybe need to be different
                        break;
                    }
                case "GomEpsSoftReset":
                    {
                        byte index = Convert.ToByte(args[1]);
                        response = convertErrorToString(eps.GomEpsSoftReset(index));
                        break;
                    }
                case "GomEpsHardReset":
                    {
                        byte index = Convert.ToByte(args[1]);
                        response = convertErrorToString(eps.GomEpsHardReset(index));
                        break;
                    }
                case "GomEpsGetHkData_param":
                    {
                        break;
                    }
                case "GomEpsGetHkData_general":
                    {
                        byte index = Convert.ToByte(args[1]);
                        Output<EPS.eps_hk_t> dataOut = new Output<EPS.eps_hk_t>();
                        response = String.Concat(convertErrorToString(eps.GomEpsGetHkData_general(index, dataOut)), Encoding.UTF8.GetString(getBytes(dataOut.output))); //Maybe need to be different
                        break;
                    }
                case "GomEpsGetHkData_vi":
                    {
                        byte index = Convert.ToByte(args[1]);
                        Output<EPS.eps_hk_vi_t> dataOut = new Output<EPS.eps_hk_vi_t>();
                        response = String.Concat(convertErrorToString(eps.GomEpsGetHkData_vi(index, dataOut)), Encoding.UTF8.GetString(getBytes(dataOut.output))); //Maybe need to be different
                        break;
                    }
                case "GomEpsGetHkData_out":
                    {
                        byte index = Convert.ToByte(args[1]);
                        Output<EPS.eps_hk_out_t> dataOut = new Output<EPS.eps_hk_out_t>();
                        response = String.Concat(convertErrorToString(eps.GomEpsGetHkData_out(index, dataOut)), Encoding.UTF8.GetString(getBytes(dataOut.output))); //Maybe need to be different
                        break;
                    }
                case "GomEpsGetHkData_wdt":
                    {
                        byte index = Convert.ToByte(args[1]);
                        Output<EPS.eps_hk_wdt_t> dataOut = new Output<EPS.eps_hk_wdt_t>();
                        response = String.Concat(convertErrorToString(eps.GomEpsGetHkData_wdt(index, dataOut)), Encoding.UTF8.GetString(getBytes(dataOut.output))); //Maybe need to be different
                        break;
                    }
                case "GomEpsGetHkData_basic":
                    {
                        byte index = Convert.ToByte(args[1]);
                        Output<EPS.eps_hk_basic_t> dataOut = new Output<EPS.eps_hk_basic_t>();
                        response = String.Concat(convertErrorToString(eps.GomEpsGetHkData_basic(index, dataOut)), Encoding.UTF8.GetString(getBytes(dataOut.output))); //Maybe need to be different
                        break;
                    }
                case "GomEpsSetOutput":
                    {
                        byte index = Convert.ToByte(args[1]);
                        byte output = Convert.ToByte(args[2]);
                        response = convertErrorToString(eps.GomEpsSetOutput(index, output));
                        break;
                    }
                case "GomEpsSetSingleOutput":
                    {
                        byte index = Convert.ToByte(args[1]);
                        byte channel_id = Convert.ToByte(args[2]);
                        byte value = Convert.ToByte(args[3]);
                        ushort delay = Convert.ToUInt16(args[4]);
                        response = convertErrorToString(eps.GomEpsSetSingleOutput(index, channel_id, value, delay));
                        break;
                    }
                case "GomEpsSetPhotovoltaicInputs":
                    {
                        byte index = Convert.ToByte(args[1]);
                        ushort voltage1 = Convert.ToUInt16(args[2]);
                        ushort voltage2 = Convert.ToUInt16(args[3]);
                        ushort voltage3 = Convert.ToUInt16(args[4]);
                        response = convertErrorToString(eps.GomEpsSetPhotovoltaicInputs(index, voltage1, voltage2, voltage3));
                        break;
                    }
                case "GomEpsSetPptMode":
                    {
                        byte index = Convert.ToByte(args[1]);
                        byte mode = Convert.ToByte(args[2]);
                        response = convertErrorToString(eps.GomEpsSetPptMode(index, mode));
                        break;
                    }
                case "GomEpsSetHeaterAutoMode":
                    {
                        byte index = Convert.ToByte(args[1]);
                        byte auto_mode = Convert.ToByte(args[2]);
                        Output<ushort> auto_mode_return = new Output<ushort>();
                        response = String.Concat(convertErrorToString(eps.GomEpsSetHeaterAutoMode(index, auto_mode, auto_mode_return)), Encoding.UTF8.GetString(getBytes(auto_mode_return.output))); //Maybe need to be different
                        break;
                    }    
                case "GomEpsResetCounters":
                    {
                        byte index = Convert.ToByte(args[1]);
                        response = convertErrorToString(eps.GomEpsResetCounters(index));
                        break;
                    }
                case "GomEpsResetWDT":
                    {
                        byte index = Convert.ToByte(args[1]);
                        response = convertErrorToString(eps.GomEpsResetWDT(index));
                        break;
                    }
                case "GomEpsConfigCMD":
                    {
                        byte index = Convert.ToByte(args[1]);
                        byte cmd = Convert.ToByte(args[2]);
                        response = convertErrorToString(eps.GomEpsConfigCMD(index, cmd));
                        break;
                    }
                case "GomEpsConfigGet":
                    {
                        byte index = Convert.ToByte(args[1]);
                        Output<EPS.eps_config_t> config_data = new Output<EPS.eps_config_t>();
                        response = String.Concat(convertErrorToString(eps.GomEpsConfigGet(index, config_data)), Encoding.UTF8.GetString(getBytes(config_data.output))); //Maybe need to be different
                        break;
                    }
                case "GomEpsConfigSet":
                    {
                        byte index = Convert.ToByte(args[1]);
                        Output<EPS.eps_config_t> config_data = new Output<EPS.eps_config_t>();
                        //config_data.output = Convert.ToByte(args[2]);
                        break;
                    }
                case "GomEpsConfig2CMD":
                    break;
                case "GomEpsConfig2Get":
                    break;
                case "GomEpsConfig2Set":
                    break;
            }
            return response;
        }

        string convertErrorToString(int err)
        {
            byte[] errByteArr = new byte[1];
            errByteArr[0] = Convert.ToByte(err);
            return Encoding.UTF8.GetString(errByteArr);
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