using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Tools;
using SunCommon.Entities;

namespace SunCommon
{
    public class ClientConnection
    {
        public ShortGuid ConnectionGuid;

        public ClientConnection(ShortGuid connectionGUID, Connection connection, sbyte[] encKey)
        {
            this.ConnectionGuid = connectionGUID;
            this.AuthConnection = connection;
        }

        public int UserID { get; set; }
        public Connection AuthConnection { get; set; }
        public Connection AgentConnection { get; set; }
        public Character Character { get; set; }
    }
}
