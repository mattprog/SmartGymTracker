using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SmartGymTracker.Models
{
    public class ExerciseSet
    {
        public int ExerciseSetId { get; set; }
        public int WorkoutId { get; set; }
        public int ExerciseId { get; set; }

        public string Notes { get; set; }

        // Default Constructor
        public ExerciseSet()
        {
            ExerciseSetId = -1;
            WorkoutId = -1;
            ExerciseId = -1;
            Notes = string.Empty;
        }

        // Copy Constructor
        public ExerciseSet(ExerciseSet es)
        {
            ExerciseSetId = es.ExerciseSetId;
            WorkoutId = es.WorkoutId;
            ExerciseId = es.ExerciseId;
            Notes = es.Notes;
        }
    }
}
