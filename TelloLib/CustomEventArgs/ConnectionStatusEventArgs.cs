using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelloLib.CustomEventArgs
{
    public class ConnectionStatusEventArgs : EventArgs
    {
        public ConnectionStatus ConnectionStatus { get; }

        public ConnectionStatusEventArgs(ConnectionStatus status)
        {
            ConnectionStatus = status;
        }
    }
}
