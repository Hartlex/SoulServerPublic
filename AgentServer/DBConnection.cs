using System;
using System.Collections.Generic;
using System.Linq;
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
                var userID = BitConverter.ToInt32(buffer.ReadBlock(5),0);
                int charCount = buffer.ReadByte();
                var characterbytes = buffer.ReadBlock(buffer.Data.Length - 6);
                var cc = CCM.GetClientConnection(userID);
                var packet = new ConnectionPackets.S2CAnsEnterCharSelect(userID,charCount,characterbytes);
                packet.Send(cc.AgentConnection);
            }));
        }
    }
    }

