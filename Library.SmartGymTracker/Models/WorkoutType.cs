using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SmartGymTracker.Models
{
    public class WorkoutType
    {
        public int WorkoutTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }

        // Default Constructor
        public WorkoutType()
        {
            WorkoutTypeId = -1;
            Name = string.Empty;
            Description = string.Empty;
            Difficulty = string.Empty;
        }

        // Copy Constructor
        public WorkoutType(WorkoutType wt)
        {
            WorkoutTypeId = wt.WorkoutTypeId;
            Name = wt.Name;
            Description = wt.Description;
            Difficulty = wt.Difficulty;
        }
    }
}
