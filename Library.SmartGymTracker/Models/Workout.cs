using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SmartGymTracker.Models
{
    public class Workout
    {
        public int WorkoutId { get; set; }
        public int UserId { get; set; }
        public int WorkoutTypeId { get; set; }
        public DateTime WorkoutStart { get; set; }
        public int Duration { get; set; } // In seconds
        public string Notes { get; set; }

        // Default Constructor
        public Workout()
        {
            WorkoutId = -1;
            UserId = -1;
            WorkoutTypeId = -1;
            WorkoutStart = DateTime.MinValue;
            Duration = -1;
            Notes = string.Empty;
        }

        // Copy Constructor
        public Workout(Workout w)
        {
            WorkoutId = w.WorkoutId;
            UserId = w.UserId;
            WorkoutTypeId = w.WorkoutTypeId;
            WorkoutStart = w.WorkoutStart;
            Duration = w.Duration;
            Notes = w.Notes;
        }
    }
}
