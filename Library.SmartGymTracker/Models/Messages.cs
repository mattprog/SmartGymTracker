using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SmartGymTracker.Models
{
    public enum MessageType
    {
        System,
        Milestone,
        Tip,
        Trend,
        Goal,
        Specific
    }
    public class Messages
    {
        public int MessageId { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime TimeCreated { get; set; }
        public MessageType Type { get; set; }

        // Default Constructor
        public Messages()
        {
            MessageId = -1;
            Title = string.Empty;
            Details = string.Empty;
            TimeCreated = DateTime.MinValue;
            Type = MessageType.System;
        }

        // Copy Constructor
        public Messages(Messages m)
        {
            MessageId = m.MessageId;
            Title = m.Title;
            Details = m.Details;
            TimeCreated = m.TimeCreated;
            Type = m.Type;
        }
    }
}
