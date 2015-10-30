using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace NetworkCommunication.Core
{
    public static class NetworkClient
    {
        public static event EventHandler Connected;

        public static event EventHandler Disconnected;

        public static void TryConnect(string ip, string port)
        {
            var thread = new Thread(ConnectionWork);

            thread.IsBackground = true;

            thread.Start(new IPEndPoint(IPAddress.Parse(ip), int.Parse(port)));
        }

        private static void ConnectionWork(object obj)
        {
            try
            {
                var endPoint = (IPEndPoint)obj;
                
                clientSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                clientSocket.Connect(endPoint);

                Start();

                while (isConnected)
                {
                    var buffer = new byte[1024];

                    var count = clientSocket.Receive(buffer);

                    if(count == 0 || !isConnected)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                if (isConnected)
                {
                    Stop();
                }
            }
        }

        public static void TrySendMessage(string message)
        {
            if (messagesThread == null)
            {
                messagesThread = new Thread(MessagesThreadWork);

                messagesThread.IsBackground = true;

                messageQueue = new QueueWithBlock<string>();

                messagesThread.Start();
            }

            messageQueue.Enqueue(message);
        }

        private static void MessagesThreadWork()
        {
            while(isConnected)
            {
                var message = messageQueue.Dequeue();

                if (message != null)
                {
                    SendMessageWork(message);
                }
            }
        }

        private static void SendMessageWork(object obj)
        {
            try
            {
                var message = (string)obj;
                
                if (clientSocket != null && clientSocket.Connected && !string.IsNullOrEmpty(message) && isConnected)
                {
                    clientSocket.Send(Encoding.ASCII.GetBytes(message));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);

                if (isConnected)
                {
                    Stop();
                }
            }
        }

        public static void TryDisconnect()
        {
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    clientSocket.Close();

                    clientSocket = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private static void Start()
        {
            isConnected = true;

            OnConnected();
        }

        private static void Stop()
        {
            isConnected = false;

            if (messageQueue != null)
            {
                messageQueue.Release();

                messageQueue = null;
            }

            messagesThread = null;

            OnDisconnected();
        }

        private static void OnConnected()
        {
            isConnected = true;

            var handler = Connected;

            if (handler != null)
            {
                handler(null, EventArgs.Empty);
            }
        }
            
        private static void OnDisconnected()
        {
            var handler = Disconnected;

            if (handler != null)
            {
                handler(null, EventArgs.Empty);
            }
        }

        private static QueueWithBlock<string> messageQueue;

        private static Thread messagesThread;

        private static Socket clientSocket;

        public static bool isConnected;
    }

}

