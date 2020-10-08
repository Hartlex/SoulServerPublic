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
            byte classCode = Byte.Parse(classCodeStr);
            byte heightCode = Byte.Parse(heightCodeStr);
            byte faceCode = Byte.Parse(faceCodeStr);
            byte hairCode = Byte.Parse(hairCodeStr);
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
            GetAllCharacters(userID);

        }

        public static void GetAllCharacters(int userID)
        {
            if (!DatabaseFunctions.getAllCharacters(userID,out var characterList)) return;
            var returnbytes = new List<byte>();
            returnbytes.AddRange(ByteUtils.ToByteArray(userID,4));
            returnbytes.Add((byte)characterList.Count);
            returnbytes.Add((byte)characterList.Count);
            foreach (var characterInfo in characterList)
            {
                returnbytes.AddRange(characterInfo.ToBytes());
            }

            AgentConnection.connection.SendObject("CharacterList",returnbytes.ToArray());
        }

        public static void DeleteCharacter(int userId, int charSlot)
        {
            if (DatabaseFunctions.DeleteCharacter(userId, charSlot))
            {
                AgentConnection.connection.SendObject("CharacterDeleteSuccess",userId);
            }
            else
            {
                var errorCode = 0; //TODO implement Errocodes
                AgentConnection.connection.SendObject("CharacterDeleteFailed",new []{errorCode,userId});
            }
        }

        public static void CheckDuplicateName(string name, int userId)
        {
            AgentConnection.connection.SendObject(
                DatabaseFunctions.IsNameFree(name) ? "CheckDuplicateNameSuccess" : "CheckDuplicateNameFailed", userId);
        }

        public static void GetFullCharacter(int userId, int charSlot)
        {
            if (DatabaseFunctions.GetFullCharacter(userId, charSlot,out var fullCharBytes))
            {

                AgentConnection.connection.SendObject("FullCharacterBytes",fullCharBytes);
            }
        }
    }
}
