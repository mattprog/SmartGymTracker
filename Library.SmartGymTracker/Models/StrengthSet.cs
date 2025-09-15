using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SmartGymTracker.Models
{
    public class StrengthSet : ExerciseSet
    {
        public int SetNumber { get; set; }
        public double Weight { get; set; }
        public int Repetitions { get; set; }

        public StrengthSet()
        {
            SetNumber = -1;
            Weight = -1;
            Repetitions = -1;
        }

        public StrengthSet(StrengthSet cs) : base(cs)
        {
            SetNumber = cs.SetNumber;
            Weight = cs.Weight;
            Repetitions = cs.Repetitions;
        }
    }
}
