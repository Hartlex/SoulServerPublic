using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunCommon.Entities.Item
{
    public class ExerciseEffect
    {
        public int exerciseEffect;
        public int optionKind;
        public int optionType;
        public int optionValue;

        public ExerciseEffect(string eff, string oK, string oT, string oV)
        {
            exerciseEffect = Int32.Parse(eff);
            optionKind = Int32.Parse(oK);
            optionType = Int32.Parse(oT);
            optionValue = Int32.Parse(oV);
        }
    }
}