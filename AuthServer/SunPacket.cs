using NetworkCommsDotNet.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer
{
    abstract class SunPacket
    {
        public Connection connection;
        public abstract void onReceive();
    }
}
