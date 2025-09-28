using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SmartGymTracker.Models
{
    public class Exercise
    {
        public int ExerciseId { get; set; }
        public int MuscleId { get; set; }
        public string ExerciseName { get; set; }
        public string Description { get; set; }

        // Default Constructor
        public Exercise()
        {
            ExerciseId = -1;
            MuscleId = -1;
            ExerciseName = string.Empty;
            Description = string.Empty;
        }

        // Copy Constructor
        public Exercise(Exercise e)
        {
            ExerciseId = e.ExerciseId;
            MuscleId = e.MuscleId;
            ExerciseName = e.ExerciseName;
            Description = e.Description;
        }
    }
}
