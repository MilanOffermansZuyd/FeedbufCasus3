using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FeedBuf
{
    public class UserAction : Message
    {
        public int Id { get; set; }
        public Goal Goal { get; set; }
        public bool IsFinished { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? SoftDeadline { get; set; }
        public DateTime HardDeadline { get; set; }
        public List<Feedback> Feedbacks { get; set; }
        public bool OpenForFeedback { get; set; }


        // Constructor
        public UserAction(int id, Goal goal, DateTime createdOn, DateTime? softDeadline, DateTime hardDeadline, bool isFinished, string text, ZuydUser student, ZuydUser author, string shortDescription, bool openForFeedback) 
            : base(id, text, student, author, shortDescription)
        {
            Id = id;
            Goal = goal;
            CreatedOn = createdOn;
            SoftDeadline = softDeadline;
            HardDeadline = hardDeadline;
            IsFinished = isFinished;
            OpenForFeedback = openForFeedback;
        }

        // Mark as finished
        public void MarkAsFinished()
        {
            IsFinished = true;
        }

        // Add feedback
        public void AddFeedback(Feedback feedback)
        {
            Feedbacks.Add(feedback);
        }

        // Remove Feedback
        public void RemoveFeedback(Feedback feedback)
        {
            Feedbacks.Remove(feedback);
        }
    }
}
