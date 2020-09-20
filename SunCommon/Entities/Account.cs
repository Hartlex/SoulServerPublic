using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunCommon.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public List<Character> Characters { get; set; }

        public Account(int id)
        {
            Id = id;
        }

        public void AddCharacter(Character character)
        {
            Characters.Add(character);
        }
    }
}
