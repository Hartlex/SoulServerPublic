using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunCommon.Entities
{
    public class CharacterPosition
    {
        public int Id { get; set; }
        public int Region { get; set; }
        public int Angle { get; set; }
        public short LocationX { get; set; }
        public short LocationY { get; set; }
        public short LocationZ { get; set; }
    }
}

