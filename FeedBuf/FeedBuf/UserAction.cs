using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedBuf
{
    class UserAction
    {
        public int Id { get; set; }
        public string Text { get; set; } // Eventueel updaten naar Title aangezien dit in de ERD ook zo is (Evt ook aanpassen in klassendiagram)
        public Student Student { get; set; }
        public Goal Goal { get; set; }
        public bool IsFinished { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime SoftDeadline { get; set; }
        public DateTime HardDeadline { get; set; }
        public List<Feedback> Feedbacks { get; set; }


        // Constructor
        public UserAction(int id, string text, Student student, Goal goal, DateTime createdOn, DateTime softDeadline, DateTime hardDeadline)
        {
            Id = id;
            Text = text;
            Student = student;
            Goal = goal;
            CreatedOn = createdOn;
            SoftDeadline = softDeadline;
            HardDeadline = hardDeadline;
            IsFinished = false;
            Feedbacks = new List<Feedback>();
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
