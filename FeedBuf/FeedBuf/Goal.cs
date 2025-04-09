using FeedBuf.Catagory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedBuf
{
    public class Goal : Message
    {
        public DateTime SoftDeadline { get; set; }
        public DateTime HardDeadline { get; set; }
        public bool IsFinished { get; set; }
        public Category Category { get; set; }
        public List<Feedback> Feedbacks { get; set; }
        public List<UserAction> Actions { get; set; }

        // Constructor
        public Goal(int id , DateTime softDeadline, DateTime hardDeadline, bool isFinished, Category category, string text, ZuydUser student, ZuydUser author)
            : base(id, text, student, author)
        {
            Id = id;
            SoftDeadline = softDeadline;
            HardDeadline = hardDeadline;
            IsFinished = isFinished;
            Category = category;
            Feedbacks = new List<Feedback>();
            Actions = new List<UserAction>();
        }

        
    }
}
