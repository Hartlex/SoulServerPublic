using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCommsDotNet.Connections;

namespace DatabaseProxy
{
    static class AgentConnection
    {
        public static Connection connection;

        public static void RegisterPackets()
        {
            connection.AppendIncomingPacketHandler<string[]>("CreateCharacter", (header, connection, content) =>
            {
                AgentPackets.CreateCharacterPacket(content[0], content[1], content[2], content[3], content[4],
                    content[5]);
            });

            connection.AppendIncomingPacketHandler<int>("AskForCharacterList",((header, connection1, userID) =>
            {
                AgentPackets.GetAllCharacters(userID);
            } ));
            connection.AppendIncomingPacketHandler<int[]>("DeleteCharacter",((header, connection1, incomingObject) =>
            {
                var userID = incomingObject[0];
                var charSlot = incomingObject[1];
                AgentPackets.DeleteCharacter(userID, charSlot);
            }));
            connection.AppendIncomingPacketHandler<string[]>("AskDuplicateName",(
                (header, connection1, incomingObject) =>
                {
                    var name = incomingObject[0];
                    var userID = Int32.Parse(incomingObject[1]);
                    AgentPackets.CheckDuplicateName(name, userID);
                }));
            connection.AppendIncomingPacketHandler<int[]>("GetFullCharacter",((header, connection1, incomingObject) =>
            {
                var userID = incomingObject[0];
                var charSlot = incomingObject[1];
                AgentPackets.GetFullCharacter(userID, charSlot);
            }));
        }
    }
}
