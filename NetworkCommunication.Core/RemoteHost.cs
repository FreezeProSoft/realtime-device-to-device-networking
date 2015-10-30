using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace NetworkCommunication.Core
{
    /// <summary>
    /// Information about host connection.
    /// </summary>
    public class RemoteHost
    {
        /// <summary>
        /// Gets the host address.
        /// </summary>
        /// <value>The host ip address.</value>
        public string Address { get; private set; }

        /// <summary>
        /// Gets the host socket connection.
        /// </summary>
        /// <value>The connection.</value>
        public Socket Connection { get; private set; }

        /// <summary>
        /// Initializes a new instance of the host connection class.
        /// </summary>
        /// <param name="socket">Socket connection.</param>
        public RemoteHost(Socket socket)
        {
            var connection = socket;

            Connection = connection;

            if (connection.RemoteEndPoint != null)
            {
                Address = Connection.RemoteEndPoint.ToString();
            }
            else
            {
                Address = "none";
            }
        }
    }
}
