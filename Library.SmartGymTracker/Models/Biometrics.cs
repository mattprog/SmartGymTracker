using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SmartGymTracker.Models
{
    public class Biometrics
    {
        public int BiometricsId { get; set; }
        public int UserId { get; set; }
        public DateOnly DateEntered { get; set; }
        public double Weight { get; set; } // In lbs
        public double Height { get; set; } // In inches
        public double BodyFatPercentage { get; set; } // % 0 <= x <= 1
        public double BMI { get; set; } // Body Mass Index
        public int RestingHeartRate { get; set; } // In bpm

        // Default Constructor
        public Biometrics()
        {
            BiometricsId = -1;
            UserId = -1;
            DateEntered = DateOnly.MinValue;
            Weight = -1;
            Height = -1;
            BodyFatPercentage = -1;
            BMI = -1;
            RestingHeartRate = -1;
        }

        // Copy Constructor
        public Biometrics(Biometrics b)
        {
            BiometricsId = b.BiometricsId;
            UserId = b.UserId;
            DateEntered = b.DateEntered;
            Weight = b.Weight;
            Height = b.Height;
            BodyFatPercentage = b.BodyFatPercentage;
            BMI = b.BMI;
            RestingHeartRate = b.RestingHeartRate;
        }
    }
}
