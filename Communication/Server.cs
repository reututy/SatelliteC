﻿using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading.Tasks;

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
            //string[] pairs = request.Split('&');
            //string methodName = pairs[0].Split('=')[1];
            //string valueString = pairs[1].Split('=')[1];
            //string[] values = valueString.Split(' ');
            //double[] vals = new double[values.Length];
            //for (int i = 0; i < values.Length; ++i)
            //    vals[i] = double.Parse(values[i]);
            //string response = "";
            //if (methodName == "average") response += Average(vals);
            //else if (methodName == "minimum") response += Minimum(vals);
            //else response += "BAD methodName: " + methodName;
            //int delay = ((int)vals[0]) * 1000; // Dummy delay
            //System.Threading.Thread.Sleep(delay);
            //return response;
            return analyzedResponse;
        }

        private string analyzeRequest(string request)
        {
            string response = "";
            string[] args = request.Split('&');
            string methodName = args[0];
            if (methodName.Contains("GomEps"))
                response = analyzeGomEps(request);
            return response;
        }

        private string analyzeGomEps(string request)
        {
            string response = "";
            string[] args = request.Split('&');
            string methodName = args[0];
            switch (methodName)
            {
                case "GomEpsInitialize":
                    byte i2c_address = Convert.ToByte(args[1]);
                    byte number = Convert.ToByte(args[2]);
                    response = Convert.ToString(eps.GomEpsInitialize(i2c_address, number));
                    break;
            }
            return response;
        }
    }
}