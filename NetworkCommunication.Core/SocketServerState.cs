using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace NetworkCommunication.Core
{
    /// <summary>
    /// The server states.
    /// </summary>
    public enum SocketServerState : int
    {
        Starting = 0,
        Running = 1,
        Stopped = 2,
    }
}
