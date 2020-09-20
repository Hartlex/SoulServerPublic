using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunCommon;

namespace DatabaseProxy
{
    public static class AgentPackets
    {
        public static void CreateCharacterPacket(string userIdStr, string charName, string classCodeStr, string heightCodeStr, string faceCodeStr, string hairCodeStr)
        {
            int userID = Int32.Parse(userIdStr);
            int classCode = Int32.Parse(classCodeStr);
            int heightCode = Int32.Parse(heightCodeStr);
            int faceCode = Int32.Parse(faceCodeStr);
            int hairCode = Int32.Parse(hairCodeStr);
            if (!DatabaseFunctions.CreateCharacter(userID, charName, classCode, heightCode, faceCode, hairCode,
                out var character)) return;
            if (!DatabaseFunctions.AddCharacterToDB(character, out var charId)) return;
            character.Id = charId;
            var sendbytes2 = new PacketStructs.CharacterInfo(character).ToBytes();
            var sendbytes1 = ByteUtils.ToByteArray(userID, 5);
            byte[] sendbytes = new byte[sendbytes2.Length + sendbytes1.Length];
            Buffer.BlockCopy(sendbytes1, 0, sendbytes, 0, sendbytes1.Length);
            Buffer.BlockCopy(sendbytes2, 0, sendbytes, sendbytes1.Length, sendbytes2.Length);
            AgentConnection.connection.SendObject("CharacterCreateSuccess", sendbytes);


        }

        public static void GetAllCharacters(int userID)
        {
            if (!DatabaseFunctions.getAllCharacters(userID,out var characterList)) return;
            var returnbytes = new List<byte>();
            returnbytes.AddRange(ByteUtils.ToByteArray(userID,5));
            returnbytes.Add((byte)characterList.Count);
            foreach (var characterInfo in characterList)
            {
                returnbytes.AddRange(characterInfo.ToBytes());
            }

            AgentConnection.connection.SendObject("CharacterList",returnbytes.ToArray());
        }
    }
}
