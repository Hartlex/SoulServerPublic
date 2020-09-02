using KaymakNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer
{
    static class ServerPackets
    {
        
        public static byte[] ClientAsk()
        {
            List<byte> packet = new List<byte>();
            packet.AddRange(new byte[]{51,00});
            packet.AddRange(PacketStructs.stringBytes("", 64));
            packet.AddRange(new byte[] { 36, 72, 00, 00 });
            packet.InsertRange(0,PacketStructs.length(packet));
            return packet.ToArray();
        }
        public static byte[] ClientAckPacket()
        {
            return new byte[]
            {
                03,00, //Length
                51,02, //Packt ID and Protocol
                00     //00 true 01 false 
        };
        }
        public static byte[] LoginSuccessfull()
        {
            List<byte> packet = new List<byte>();
            packet.AddRange(new byte[] { 51, 14 });
            packet.Add(PacketStructs.sunBool(true));
            packet.AddRange(PacketStructs.stringBytes("This is a test Server", 64));
            packet.InsertRange(0, PacketStructs.length(packet));
            return packet.ToArray();
        }
        public static byte[] ServerInfo()
        {
            List<byte> packet = new List<byte>();
            packet.AddRange(new byte[] { 51, 17 });
            packet.Add(02); //Number of Servers
            packet.AddRange(PacketStructs.ServerInfo("Server 1", 1));
            packet.AddRange(PacketStructs.ServerInfo("Server 2", 2));
            packet.InsertRange(0, PacketStructs.length(packet));
            return packet.ToArray();
            //return new byte[]
            //{
            //    74,00,  //Length
            //    51,17,  //Packt ID and Protocol
            //    02,     //Number of Servers
            //    71, 108, 111, 98, 97, 108, 49, 00, 00, 00, //Server 1 Name 32Bytes
            //    00,00,00,00,00,00,00,00,00,00,
            //    00,00,00,00,00,00,00,00,00,00,
            //    00,00,
            //    00,     //unknown
            //    01,     //Server number
            //    00,     //unknown
            //    00,     //Separator 0x00
            //    71, 108, 111, 98, 97, 108, 50, 00, 00, 00, //Server 2 Name 32Bytes
            //    00,00,00,00,00,00,00,00,00,00,
            //    00,00,00,00,00,00,00,00,00,00,
            //    00,00,
            //    00,     //unknown
            //    02,     //Server number
            //    00      //unknown
            //
            //};
        }
        public static byte[] ChannelInfo()
        {
            List<byte> packet = new List<byte>();
            packet.AddRange(new byte[] { 51, 18 });
            packet.Add(03); //Number of Channels on Total
            packet.AddRange(PacketStructs.ChannelInfo("Channel 1", 1, 1));
            packet.AddRange(PacketStructs.ChannelInfo("Channel 2", 2, 1));
            packet.AddRange(PacketStructs.ChannelInfo("Channel 3", 3, 2));
            packet.InsertRange(0, PacketStructs.length(packet));
            return packet.ToArray();
            return new byte[]
            {
                114,00,  //Length
                51,18,  //Packt ID and Protocol
                03,     //Number of Channels
                67, 104, 97, 110, 110, 101, 108, 32, 49,00, 
                00,00,00,00,00,00,00,00,00,00,
                00,00,00,00,00,00,00,00,00,00,
                00,00,00,   //Channel name 33 Bytes why not 32???
                01,     //ServerNumber
                01,     //Channelnumber
                01,     //Terminator Byte cant be 00;
                00,     //Separator 00x0
                67, 104, 97, 110, 110, 101, 108, 32, 50,00,
                00,00,00,00,00,00,00,00,00,00,
                00,00,00,00,00,00,00,00,00,00,
                00,00,00,   //Channel name 33 Bytes why not 32???
                01,     //ServerNumber
                02,     //Channelnumber
                01,     //Terminator Byte cant be 00;
                00,     //Separator 00x0
                67, 104, 97, 110, 110, 101, 108, 32, 49,00,
                00,00,00,00,00,00,00,00,00,00,
                00,00,00,00,00,00,00,00,00,00,
                00,00,00,   //Channel name 33 Bytes why not 32???
                02,     //ServerNumber
                01,     //Channelnumber
                01,     //Terminator Byte cant be 00;
                00,     //Separator 00x0
            };
        }
        public static byte[] ConfirmServerSelect()
        {
            //List<byte> packet = new List<byte>();
            //packet.AddRange(new byte[] { 51, 26 });
            //packet.AddRange(new byte[] { 01, 00, 00, 00 }); //User ID
            //packet.AddRange(PacketStructs.stringBytes("this is send in S2CAnsSvrSelect", 32));
            //packet.AddRange(PacketStructs.stringBytes("127.0.0.1", 32));
            //packet.AddRange(PacketStructs.stringBytes("44405", 8)); //server Port as DWORD(8bytes)
            //packet.Add(00); //unk
            //packet.InsertRange(0, PacketStructs.length(packet));
            //return packet.ToArray();
            ByteBuffer buff = new ByteBuffer(4);
            
            List<byte> packet = new List<byte>();
            packet.AddRange(new byte[] { 51, 26 });
            packet.AddRange(PacketStructs.intBytes(new int[] { 33 },1)); //User ID
            packet.AddRange(new byte[] { 0x30, 0x00, 0x20, 0x00, 0x00, 0x20, 0x00, 0x00, 0x20, 0x81, 0x07, 0x20, 0x42, 0x00, 0x20, 0x0f, 0x00, 0x20, 0x00, 0x00, 0x20, 0x00, 0x00, 0x20, 0x00, 0x00, 0x20, 0x0e, 0x00, 0x20, 0x07, 0x08 });
            //packet.AddRange(PacketStructs.stringBytes("", 32));
            packet.AddRange(PacketStructs.stringBytes("127.0.0.1",32));
            packet.AddRange(PacketStructs.intBytes(new int[] { 44405},5));
            //packet.Add(00); //unk
            packet.InsertRange(0, PacketStructs.length(packet));
            return packet.ToArray();

            //return new byte[]
            //{
            //    32,00,  //Size
            //    0x30, 0x00, 0x20, 0x00, 0x00, 0x20, 0x00, 0x00, 0x20,
            //    0x81, 0x07, 0x20, 0x42, 0x00, 0x20, 0x0f, 0x00, 0x20,
            //    0x00, 0x00, 0x20, 0x00, 0x00, 0x20, 0x00, 0x00, 0x20,
            //    0x0e, 0x00, 0x20, 0x07, 0x08,

            //};
        }
    }
}
