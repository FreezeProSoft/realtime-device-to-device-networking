using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace NetworkCommunication.Core
{
    public static class NetworkServer
    {
        public static event EventHandler<string> ReceiveMessage;

        public static event EventHandler Started;

        public static event EventHandler Stop;


        public static void TryStart(string port)
        {
            var thread = new Thread(ListenerWork);

            thread.IsBackground = true;

            thread.Start(port);
        }

        public static void TryStop()
        {
            if (serverSocket != null)
            {
                serverSocket.Close();

                serverSocket = null;
            }
        }

        private static void ListenerWork(object port)
        {
            var sockets = new List<Socket>();

            try
            {  
                var endPoint = new IPEndPoint(IPAddress.Any, int.Parse((string)port));
                
                serverSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                serverSocket.Bind(endPoint);

                serverSocket.Listen((int)(SocketOptionName.MaxConnections));

                OnStarted();

                while (isRunning)
                {
                    var connectionSocket = serverSocket.Accept();

                    sockets.Add(connectionSocket);

                    var thread =  new Thread(ListenConnection);

                    thread.IsBackground = true;

                    thread.Start(connectionSocket);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                OnStop();

                for (int index = 0; index < sockets.Count; index++) 
                {
                    var socket = sockets[index];

                    if(socket.Connected)
                    {
                        socket.Close();
                    }
                }

                sockets.Clear();
            }
        }
            
        private static void ListenConnection(object obj)
        {
            try
            {
                var socket = (Socket)obj;

                while (socket.Connected && isRunning)
                {
                    var buffer = new byte[1024];

                    var count = socket.Receive(buffer);

                    if(count == 0 || !isRunning)
                    {
                        break;
                    }

                    var result = Encoding.ASCII.GetString(buffer, 0, count);

                    OnReceiveMessage(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private static void OnReceiveMessage(string message)
        {    
            var handler = ReceiveMessage;

            if (handler != null)
            {
                handler(null, message);
            }
        }

        private static void OnStarted()
        {
            isRunning = true;

            var handler = Started;

            if (handler != null)
            {
                handler(null, EventArgs.Empty);
            }
        }

        private static void OnStop()
        {
            isRunning = false;

            var handler = Stop;

            if (handler != null)
            {
                handler(null, EventArgs.Empty);
            }
        }
            
        private static Socket serverSocket;

        public static bool isRunning;
    }
}

