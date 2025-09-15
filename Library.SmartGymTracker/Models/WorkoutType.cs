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
        public string DifficultyLevel { get; set; }

        // Default Constructor
        public WorkoutType()
        {
            WorkoutTypeId = -1;
            Name = string.Empty;
            Description = string.Empty;
            DifficultyLevel = string.Empty;
        }

        // Copy Constructor
        public WorkoutType(WorkoutType wt)
        {
            WorkoutTypeId = wt.WorkoutTypeId;
            Name = wt.Name;
            Description = wt.Description;
            DifficultyLevel = wt.DifficultyLevel;
        }
    }
}
