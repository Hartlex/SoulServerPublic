using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterServer.Network.Encryption;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Tools;
using SunCommon.Entities;

namespace MasterServer.Clients
{
    public class Client
    {
        public Connection AuthConnection;
        public Connection ServerConnection;
        public Connection ChannelConnection;
        public int EncryptionKey { get; }
        private ShortGuid clientId;
        private Character selectedCharacter;
        public int UserId { get; set; }
        private SunServer connectedServer;
        private SunChannel connectedChannel;
        private atZone atZone;
        private Character[] charslots = new Character[6];
        public uint objectKey;
        public uint objectKey2;

        public Client(Connection authConnection, ShortGuid clientId)
        {
            this.AuthConnection = authConnection;
            this.clientId = clientId;
            this.EncryptionKey = EncryptionManager.generateEncKey();
            atZone = atZone.undefined;

        }

        public void setAtZone(atZone atZone)
        {
            this.atZone = atZone;
        }
        public atZone getAtZone()
        {
            return atZone;
        }
        public void ConnectToServer(SunServer server)
        {
            connectedServer = server;
        }

        public void ConnectToChannel(SunChannel channel)
        {
            connectedChannel = channel;
        }

        public SunChannel GetChannel()
        {
            return connectedChannel;
        }

        public SunServer getServer()
        {
            return connectedServer;
        }

        public void SelectCharacter(Character character)
        {
            selectedCharacter = character;
        }

        public Character GetSelectedCharacter()
        {
            return selectedCharacter;
        }
    }

    public enum atZone
    {
        undefined=-1,
        connectedToServer=0,
        afterLogin =1,
        atCharSelect=2,
        atVillage=3,
        atZone=4,
        atMission=5,
        atPvP=6
    }
}
