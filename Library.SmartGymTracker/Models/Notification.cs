using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.SmartGymTracker.Models
{
    public class Notification
    {
        public int UserId { get; set; }
        public int MessageId { get; set; }
        public DateTime TimeSent { get; set; }
        public bool IsRead { get; set; }

        // Default Constructor
        public Notification()
        {
            UserId = -1;
            MessageId = -1;
            TimeSent = DateTime.MinValue;
            IsRead = false;
        }

        // Copy Constructor
        public Notification(Notification n)
        {
            UserId = n.UserId;
            MessageId = n.MessageId;
            TimeSent = n.TimeSent;
            IsRead = n.IsRead;
        }
    }
}
