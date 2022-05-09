using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using WholesomeDungeons.Connection.Data;
using WholesomeDungeons.Helper;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;

namespace WholesomeDungeons.Connection
{
    class Server
    {
        private static readonly Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static readonly List<Socket> clientSockets = new List<Socket>();
        private const int BUFFER_SIZE = 2048;
        private const int PORT = 2;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];
        private static Socket _current;
        private static bool alreadyrunning = false;
        public static void Main()
        {
            SetupServer();
            //CloseAllSockets();
        }

        private static void SetupServer()
        {
            if(!alreadyrunning)
            {
                Logger.Log("Setting up  Server");
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT));
                serverSocket.Listen(0);
                serverSocket.BeginAccept(AcceptCallback, null);
                Logger.Log("Server Setup complete");
            }
            alreadyrunning = true;
        }

        public static void StopServer()
        {
            Logger.Log("Stopping Server");
            foreach (Socket socket in clientSockets)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            clientSockets.Clear();
            //serverSocket.Disconnect(true);
            try
            {
                serverSocket.Close();
                Logger.Log("Server Terminated");
            }
            catch(Exception)
            {
                return;
            }
            
        }

        private static void AcceptCallback(IAsyncResult AR)
        {
            Socket socket;

            try
            {
                socket = serverSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException) // I cannot seem to avoid this (on exit when properly closing sockets)
            {
                return;
            }

            clientSockets.Add(socket);
            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, socket);
            Logger.Log("Client connected, waiting for request...");
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        private static void ReceiveCallback(IAsyncResult AR)
        {
            Socket current = (Socket)AR.AsyncState;
            _current = current;
            int received;

            try
            {
                received = current.EndReceive(AR);
            }
            catch (SocketException)
            {
                Logger.Log("Client forcefully disconnected");
                // Don't shutdown because the socket may be disposed and its disconnected anyway.
                current.Close();
                clientSockets.Remove(current);
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            string text = Encoding.ASCII.GetString(recBuf);
            Logger.Log($"Received Text: {text}");

            decoding(text);

            current.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, current);
        }

        private static void decoding(string text)
        {
            Logger.Log($"Reiceived Message: " + text);
            switch (text)
            {
                case "getPosition":
                {
                        //byte[] data = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(new TankData(ObjectManager.Me.Position, Usefuls.ContinentId)));
                        _current.Send(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(new TankData(ObjectManager.Me.Position, Usefuls.ContinentId))));
                        break;
                }
                default:
                    Logger.Log("Invalid Textrequest to Server");
                    byte[] data = Encoding.ASCII.GetBytes("Invalid request");
                    _current.Send(data);
                    Logger.Log("Warning to client sent");
                    break;
            }
        }
    }
}
