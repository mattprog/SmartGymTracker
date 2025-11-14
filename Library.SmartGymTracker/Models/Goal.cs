using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SmartGymTracker.Models
{
    public enum GoalStatus
    {
        Not_Started,
        In_Progress,
        Completed,
        Failed
    }
    public class Goal
    {
        public int GoalId { get; set; }
        public int UserId { get; set; }
        public DateTime TimeCreated { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly TargetEndDate { get; set; }
        public GoalStatus Status { get; set; }
        
        // Default Constructor
        public Goal()
        {
            GoalId = -1;
            UserId = -1;
            TimeCreated = DateTime.MinValue;
            Title = string.Empty;
            Description = string.Empty;
            StartDate = DateOnly.MinValue;
            TargetEndDate = DateOnly.MinValue;
            Status = GoalStatus.Not_Started;
        }
        
        // Copy Constructor
        public Goal(Goal g)
        {
            GoalId = g.GoalId;
            UserId = g.UserId;
            TimeCreated = g.TimeCreated;
            Title = g.Title;
            Description = g.Description;
            StartDate = g.StartDate;
            TargetEndDate = g.TargetEndDate;
            Status = g.Status;
        }
    }
}
