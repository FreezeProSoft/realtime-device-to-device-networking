using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace NetworkCommunication.Core
{
    /// <summary>
    /// The client states.
    /// </summary>
    public enum SocketClientState : int
    {
        Connected = 0,
        Disconnected = 1,
    }
}
