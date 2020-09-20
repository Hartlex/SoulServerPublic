using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;
using KaymakNetwork.Network.Client;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Tools;

namespace SunCommon
{
    public static class CCM // Client Connection Manager
    {
        private static Dictionary<ShortGuid, ClientConnection> ClientConnections = new Dictionary<ShortGuid, ClientConnection>();

        public static void AddCC(Connection connection, sbyte[] encKey)
        {
            var guid = connection.ConnectionInfo.NetworkIdentifier;
            if (ClientConnections.ContainsKey(guid))
            {
                ClientConnections[guid] = new ClientConnection(guid,connection,encKey);
            }
            else ClientConnections.Add(guid,new ClientConnection(guid,connection,encKey));
        }

        public static void PrintConnections()
        {
            
        }

        public static ClientConnection GetClientConnection(string s)
        {
            foreach (var conn in ClientConnections)
            {
                if (conn.Key.ToString() == s) return conn.Value;
            }

            return null;
        }

        public static ClientConnection GetClientConnection(ShortGuid guid)
        {
            return ClientConnections.TryGetValue(guid, out var Clientconnection) ? Clientconnection : null;
        }

        public static void AddCC(ClientConnection cc)
        {
            ClientConnections.Add(cc.ConnectionGuid,cc);
        }
        public static ClientConnection GetClientConnection(int userID)
        {
            foreach (var conn in ClientConnections)
            {
                if (conn.Value.UserID == userID) return conn.Value;
            }

            return null;
        }

        public static ClientConnection GetClientConnection(Connection conn)
        {
            foreach (var cc in ClientConnections)
            {
                if (cc.Value.AgentConnection == conn || cc.Value.AuthConnection == conn)
                {
                    return cc.Value;
                }
            }

            return null;
        }
    }
}
