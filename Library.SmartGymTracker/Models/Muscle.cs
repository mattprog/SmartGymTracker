using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SmartGymTracker.Models
{
    public class Muscle
    {
        public int MuscleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Default Constructor
        public Muscle()
        {
            MuscleId = -1;
            Name = string.Empty;
            Description = string.Empty;
        }

        // Copy Constructor
        public Muscle(Muscle m)
        {
            MuscleId = m.MuscleId;
            Name = m.Name;
            Description = m.Description;
        }
    }
}
