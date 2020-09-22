using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using KaymakNetwork;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Connections.TCP;
using NetworkCommsDotNet.DPSBase;
using NetworkCommsDotNet.DPSBase.SevenZipLZMACompressor;
using SunCommon;
using SunCommon.Entities;
using SunCommon.Packet.Agent.Character;

namespace AgentServer
{
    internal static class DBConnection
    {
        public static Connection connection;
        public static bool isConnected = false;
        public static void IniitializeConnection(string address, int port)
        {
            SendReceiveOptions optionsToUse = new SendReceiveOptions<ProtobufSerializer, LZMACompressor>();
            ConnectionInfo info = new ConnectionInfo(address.ToString(), port, ApplicationLayerProtocolStatus.Enabled);
            Connection conn = TCPConnection.GetConnection(info, optionsToUse, true);
            conn.AppendIncomingPacketHandler<string>("AgentDBConnectionACK", (header, connection, content) =>
                {
                    DBConnection.connection = conn;
                    isConnected = true;
                    Console.WriteLine("DBServer connected!");
                    RegisterDBPackets();
                }
            );
            conn.SendObject("AgentDBConnection");


        }

        public static void RegisterDBPackets()
        {
            connection.AppendIncomingPacketHandler<byte[]>("CharacterCreateSuccess",
                (header, connection, character) =>
                {
                    ByteBuffer buffer = new ByteBuffer(character);
                    var userID = BitConverter.ToInt32(buffer.ReadBlock(5),0);
                    var characterbytes = buffer.ReadBlock(buffer.Data.Length - 5);
                    var cc = CCM.GetClientConnection(userID);
                    //var charstruct = new PacketStructs.CharacterInfo(character);
                    var packet = new CharacterPackets.S2CAnsCreateCharacter(characterbytes,cc.AgentConnection);
                    packet.Send(cc.AgentConnection);
                });
            connection.AppendIncomingPacketHandler<byte[]>("CharacterList",((header, connection1, bytes) =>
            {
                ByteBuffer buffer = new ByteBuffer(bytes);
                var userID = BitConverter.ToInt32(buffer.ReadBlock(4),0);
                var cc = CCM.GetClientConnection(userID);
                var packet = new ConnectionPackets.S2CAnsEnterCharSelect(buffer.Data);
                packet.Send(cc.AgentConnection);
            }));
            connection.AppendIncomingPacketHandler<int>("CharacterDeleteSuccess",(
                (header, connection1, userID) =>
                {
                    var conn = CCM.GetClientConnection(userID).AgentConnection;
                    var packet = new CharacterPackets.S2CAnsDeleteCharacter(conn);
                    packet.Send(conn);
                }));
            connection.AppendIncomingPacketHandler<int[]>("CharacterDeleteFailed",(
                (header, connection1, incomingObject) =>
                {
                    var conn = CCM.GetClientConnection(incomingObject[1]).AgentConnection;
                    var errorCode = incomingObject[0];
                    var packet = new CharacterPackets.S2CErrCharacterPacket(errorCode,conn);
                    packet.Send(conn);
                }));
            connection.AppendIncomingPacketHandler<int>("CheckDuplicateNameSuccess",(
                (header, connection1, incomingObject) =>
                {

                    
                } ));
            connection.AppendIncomingPacketHandler<int>("CheckDuplicateNameFailed",(
                (header, connection1, incomingObject) =>
                {
                    var conn = CCM.GetClientConnection(incomingObject).AgentConnection;
                    var packet = new CharacterPackets.S2CAnsDuplicateNameCheck(0, connection); //0 oder 5
                    packet.Send(conn);
                }));
            connection.AppendIncomingPacketHandler<byte[]>("FullCharacterBytes",((header, connection1,
                fullCharacterBytes) =>
            {
                var userID = BitConverter.ToInt32(ByteUtils.SlicedBytes(fullCharacterBytes, 4, 8), 0);
                var conn = CCM.GetClientConnection(userID);
                var character = new Character(fullCharacterBytes);
                conn.Character = character;
                var packet = new ConnectionPackets.S2CAnsEnterGame(character.Id);
                packet.Send(conn.AgentConnection);
            }));
        }
    }
    }

