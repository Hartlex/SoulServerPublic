using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunCommon;
using SunCommon.Entities;

namespace DatabaseProxy
{
    internal static class AuthPackets
    {
        public static void LoginPacket(string username,string pw, string connectionGUID)
        {
            Console.WriteLine("Login Request from User: "+username+" with GUID: "+connectionGUID);
            
            if (DatabaseFunctions.UserLogin(username, pw,out var userId))
            {
                AuthConnection.connection.SendObject("UserLoginSucces",new[]{userId.ToString(),connectionGUID});
            }
            else AuthConnection.connection.SendObject("UserLoginFailed");
        }

        
    }
}
