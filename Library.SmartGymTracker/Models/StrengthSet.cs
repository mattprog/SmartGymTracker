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
        public int Reps { get; set; }

        public StrengthSet() : base()
        {
            SetNumber = -1;
            Weight = -1;
            Reps = -1;
        }

        public StrengthSet(StrengthSet cs) : base(cs)
        {
            SetNumber = cs.SetNumber;
            Weight = cs.Weight;
            Reps = cs.Reps;
        }
    }
}
