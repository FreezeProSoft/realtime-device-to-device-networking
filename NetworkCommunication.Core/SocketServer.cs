using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace NetworkCommunication.Core
{
    /// <summary>
    /// Socket server.
    /// </summary>
    public class SocketServer
    {
        /// <summary>
        /// Occurs when the server state changes.
        /// </summary>
        public event EventHandler<SocketServerState> StateChanged;

        /// <summary>
        /// Occurs when the server receives message.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs> ReceivedMessage;

        /// <summary>
        /// Occurs when the server has accepted the connection host.
        /// </summary>
        public event EventHandler<RemoteHost> HostAcceptConnection;

        /// <summary>
        /// Occurs when a host connection is closed.
        /// </summary>
        public event EventHandler<RemoteHost> HostClosedConnection;

        /// <summary>
        /// Gets the server current port.
        /// </summary>
        /// <value>The port.</value>
        public int Port
        {
            get
            {
                return port;
            }
        }

        /// <summary>
        /// Gets the server current state.
        /// </summary>
        /// <value>The state.</value>
        public SocketServerState State 
        {
            get
            {
                return state;
            }
        }

        /// <summary>
        /// Initializes a new instance of the server class.
        /// </summary>
        public SocketServer()
        {
            this.state = SocketServerState.Stopped;
        }

        /// <summary>
        /// Run the server by specified port.
        /// </summary>
        /// <param name="port">The port for the server to listen to.</param>
        public void Run(int port)
        {
            if (state == SocketServerState.Stopped)
            {
                this.port = port;

                OnStateChanged(SocketServerState.Starting);

                var thread = new Thread(Listening);

                thread.IsBackground = true;

                thread.Start();
            }
        }

        /// <summary>
        /// Stop this server instance.
        /// </summary>
        public void Stop()
        {
            if (mainSocket != null)
            {
                mainSocket.Close();
            
                mainSocket = null;
            }
        }

        /// <summary>
        /// Starts listening for the incomming connections on specified port.
        /// </summary>
        private void Listening()
        {
            var hosts = new List<RemoteHost>();
        
            try
            {  
                var endPoint = new IPEndPoint(IPAddress.Any, (int)port);
                        
                mainSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        
                mainSocket.Bind(endPoint);
        
                mainSocket.Listen((int)(SocketOptionName.MaxConnections));
        
                OnStateChanged(SocketServerState.Running);
        
                while (state == SocketServerState.Running)
                {
                    var host = new RemoteHost(mainSocket.Accept());

                    OnHostAcceptConnection(host);

                    hosts.Add(host);
        
                    var thread = new Thread(HostConnection);
        
                    thread.IsBackground = true;
        
                    thread.Start(host);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                OnStateChanged(SocketServerState.Stopped);
        
                for (int index = 0; index < hosts.Count; index++)
                {
                    var host = hosts[index];
        
                    if (host.Connection.Connected)
                    {
                        host.Connection.Close();
                    }
                }
        
                hosts.Clear();
            }
        }

        /// <summary>
        /// Handles the connection and messaging with single client.
        /// </summary>
        /// <param name="obj">Information about host connection.</param>
        private void HostConnection(object obj)
        {
            var host = (RemoteHost)obj;

            try
            {
                while (host.Connection.Connected && state == SocketServerState.Running)
                {
                    var buffer = new byte[1024];
        
                    var count = host.Connection.Receive(buffer);
        
                    if (count == 0 || state == SocketServerState.Stopped)
                    {
                        break;
                    }
        
                    OnReceivedMessage(host, buffer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                OnHostClosedConnection(host);
            }
        }

        /// <summary>
        /// Raises the server state changed event.
        /// </summary>
        /// <param name="state">Server state.</param>
        protected void OnStateChanged(SocketServerState state)
        {
            this.state = state;
            
            var handler = StateChanged;

            if (handler != null)
            {
                handler(this, state);
            }
        }

        /// <summary>
        /// Raises the server received message event.
        /// </summary>
        /// <param name="host">Information about host connection who sent the message.</param>
        /// <param name="message">Message byte array.</param>
        protected void OnReceivedMessage(RemoteHost host, byte[] message)
        {    
            var handler = ReceivedMessage;
        
            if (handler != null)
            {
                handler(this, new MessageReceivedEventArgs(host, message));
            }
        }

        /// <summary>
        /// Raises the event whenever server accepts connection.
        /// </summary>
        /// <param name="host">Information about remote host connection from which is accepted</param>
        protected void OnHostAcceptConnection(RemoteHost host)
        {
            var handler = HostAcceptConnection;

            if (handler != null)
            {
                handler(this, host);
            }
        }

        /// <summary>
        /// Raises a host closed connection event.
        /// </summary>
        /// <param name="host">Information about host connection.</param>
        protected void OnHostClosedConnection(RemoteHost host)
        {    
            var handler = HostClosedConnection;

            if (handler != null)
            {
                handler(this, host);
            }
        }
 
        /// <summary>
        /// The main server socket.
        /// </summary>
        private Socket mainSocket;

        /// <summary>
        /// The current server state.
        /// </summary>
        private SocketServerState state;

        /// <summary>
        /// The current server port.
        /// </summary>
        private int port;
    }
}

