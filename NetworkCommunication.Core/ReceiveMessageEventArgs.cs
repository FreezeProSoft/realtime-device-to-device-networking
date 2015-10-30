using System;

namespace NetworkCommunication.Core
{
    /// <summary>
    /// Receive message event arguments.
    /// </summary>
    public class MessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the information about host connection.
        /// </summary>
        /// <value>The information.</value>
        public RemoteHost Host { get; private set; }

        /// <summary>
        /// Gets the message byte array.
        /// </summary>
        /// <value>The message byte array.</value>
        public byte[] Message { get; private set; }

        /// <summary>
        /// Initializes a new instance of the receive message event arguments/> class.
        /// </summary>
        /// <param name="host">The information about host connection.</param>
        /// <param name="message">The message byte array.</param>
        public MessageReceivedEventArgs(RemoteHost host, byte[] message)
        {
            Host = host;
            
            Message = message;
        }
    }
}
