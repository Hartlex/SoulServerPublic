using NetworkCommsDotNet.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseProxy
{
    static class AuthConnection
    {
        public static Connection connection;
        public static void RegisterPackets()
        {
            connection.AppendIncomingPacketHandler<string[]>("UserLogin", (header, connection, content) => 
            {
                AuthPackets.LoginPacket(content[0], content[1],content[2]);
            });
         
        }
    }
}
