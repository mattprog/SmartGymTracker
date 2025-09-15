using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SmartGymTracker.Models
{
    public class CardioSet : ExerciseSet
    {
        public int Duration { get; set; } // In Seconds
        public double Distance { get; set; } // In Miles

        // Default Constructor
        public CardioSet() : base()
        {
            Duration = -1;
            Distance = -1;
        }

        // Copy Constructor
        public CardioSet(CardioSet cs) : base(cs)
        {
            Duration = cs.Duration;
            Distance = cs.Distance;
        }
    }
}
