using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SmartGymTracker.Models
{
    public class WorkoutBiometrics
    {
        public int WorkoutId { get; set; }
        public int Steps { get; set; }
        public int AverageHeartRate { get; set; }
        public int MaxHeartRate { get; set; }
        public int CaloriesBurned { get; set; }
        public string Feeling { get; set; }
        public int SleepScore { get; set; }

        // Default constructor
        public WorkoutBiometrics()
        {
            WorkoutId = -1;
            Steps = -1;
            AverageHeartRate = -1;
            MaxHeartRate = -1;
            CaloriesBurned = -1;
            Feeling = string.Empty;
            SleepScore = -1;
        }

        // Copy Constructor
        public WorkoutBiometrics(WorkoutBiometrics eb)
        {
            WorkoutId = eb.WorkoutId;
            Steps = eb.Steps;
            AverageHeartRate = eb.AverageHeartRate;
            MaxHeartRate = eb.MaxHeartRate;
            CaloriesBurned = eb.CaloriesBurned;
            Feeling = eb.Feeling;
            SleepScore = eb.SleepScore;
        }
    }
}
