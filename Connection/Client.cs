using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using WholesomeDungeons.Helper;
using robotManager.Helpful;
using System.Threading;
using WholesomeDungeons.Connection.Data;
using wManager.Wow.ObjectManager;
using wManager.Wow.Helpers;

namespace WholesomeDungeons.Connection
{
    class Client
    {
        private static readonly Socket ClientSocket = new Socket
    (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private const int PORT = 2;
        public static void Main()
        {
            ConnectToServer();
            RequestLoop();
            Exit();
        }

        private static void ConnectToServer()
        {
            int attempts = 0;

            while (!ClientSocket.Connected)
            {
                try
                {
                    attempts++;
                    Logger.Log($"Connection  attept {attempts}");
                    ClientSocket.Connect(IPAddress.Loopback, PORT);
                }
                catch (SocketException e)
                {
                    Logger.Log("Catched exception: " + e);
                }
            }

            Logger.Log($"Connected");

        }

        private static void RequestLoop()
        {
            while (true)
            {
                Thread.Sleep(5000);
                SendRequest();

                ReceiveResponse();
            }
        }

        /// <summary>
        /// Close socket and exit program.
        /// </summary>
        public static void Exit()
        {
            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
            Environment.Exit(0);
        }

        private static void SendRequest()
        {
            Logger.Log($"Send a Request");
            string request = "getPosition";
            SendString("getPosition");

            if (request.ToLower() == "exit")
            {
                Exit();
            }
        }

        /// <summary>
        /// Sends a string to the server with ASCII encoding.
        /// </summary>
        private static void SendString(string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }

        private static void ReceiveResponse()
        {
            var buffer = new byte[2048];
            int received = ClientSocket.Receive(buffer, SocketFlags.None);
            if (received == 0) return;

            var data = new byte[received];
            Array.Copy(buffer, data, received);
            string text = Encoding.ASCII.GetString(data);
            TankData tank = Newtonsoft.Json.JsonConvert.DeserializeObject<TankData>(text);
            Logger.Log($"{tank.Position} and {tank.ContinentID}");
        }
    }
}
