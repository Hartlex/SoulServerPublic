using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using KaymakNetwork;
using MasterServer.Clients;
using MasterServer.Database;
using NetworkCommsDotNet.Connections;
using SunCommon;
using SunCommon.Packet.Agent.Character;

namespace MasterServer.Network.Packets.Processors
{
    internal static class CharacterProcessors
    {
        internal static void OnC2SAskSelectPlayer(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new CharacterPackets.C2SAskSelectPlayer(buffer);
            var client = ClientManager.GetClient(connection);
            var character = client.GetSelectedCharacter();
            client.objectKey = incPacket.objectKey;
            var outPacket = new CharacterPackets.S2CAnsSelectPlayer(
                incPacket.objectKey,
                character.Hp,
                character.MaxHp,
                character.Level
            );
            outPacket.Send(connection);
        }
        internal static void OnC2SAskCreateCharacter(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new CharacterPackets.C2SAskCreateCharacter(buffer, connection);
            var client = ClientManager.GetClient(connection);
            if (!DatabaseFunctions.CreateCharacter(
                client.UserId, incPacket.CharName,
                (byte) incPacket.ClassCode,
                (byte) incPacket.HeightCode,
                (byte) incPacket.FaceCode,
                (byte) incPacket.HairCode,
                out var character)) return;
            if (!DatabaseFunctions.AddCharacterToDB(character, out var charID)) return;
            character.Id = charID;
            var charInfoForPacket = new PacketStructs.CharacterInfo(character);
            var outPacket = new CharacterPackets.S2CAnsCreateCharacter(charInfoForPacket.ToBytes(),connection);
            outPacket.Send(connection);
        }
        internal static void OnC2SAskDuplicateName(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new CharacterPackets.C2SAskDublicateNameCheck(buffer, connection);
            if (DatabaseFunctions.IsNameFree(incPacket.name))
            {
                var outPacket = new CharacterPackets.S2CAnsDuplicateNameCheck(0,connection);
                outPacket.Send(connection);
            }
        }
        internal static void OnC2SAskDeleteCharacter(ByteBuffer buffer, Connection connection)
        {
            var packet = new CharacterPackets.C2SAskDeleteCharacter(buffer, connection);
            var client = ClientManager.GetClient(connection);
            if(!DatabaseFunctions.DeleteCharacter(client.UserId, packet.charSlot)) return;
            var outPacket = new CharacterPackets.S2CAnsDeleteCharacter(connection);
            outPacket.Send(connection);
        }
    }
}
