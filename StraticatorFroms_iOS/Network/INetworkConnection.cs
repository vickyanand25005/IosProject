using System;
using System.Collections.Generic;
using System.Text;

namespace StraticatorFroms_iOS.Network
{
    public interface INetworkConnection
    {
        bool IsConnected { get; }
        void CheckNetworkConnection();
    }
}
