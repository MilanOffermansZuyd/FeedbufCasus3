using FeedBuf.Catagory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedBuf
{
    class Goal
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public ZuydUser Author { get; set; } // Eventueel pleite nog niet over eens
        public Student Student { get; set; }
        public DateTime SoftDeadline { get; set; }
        public DateTime HardDeadline { get; set; }
        public bool IsFinished { get; set; }
        public Category Category { get; set; }
        public List<Feedback> Feedbacks { get; set; }
        public List<UserAction> Actions { get; set; }

        // Constructor
        public Goal(int id, string text, ZuydUser author, Student student, DateTime softDeadline, DateTime hardDeadline, bool isFinished, Category category)
        {
            Id = id;
            Text = text;
            Author = author;
            Student = student;
            SoftDeadline = softDeadline;
            HardDeadline = hardDeadline;
            IsFinished = isFinished;
            Category = category;
            Feedbacks = new List<Feedback>();
            Actions = new List<UserAction>();
        }

        // Add Feedback
        public void AddFeedback(Feedback feedback)
        {
            Feedbacks.Add(feedback);
        }

        // Remove Feedback
        public void RemoveFeedback(Feedback feedback)
        {
            Feedbacks.Remove(feedback);
        }

        // Add Action
        public void AddAction(UserAction action)
        {
            Actions.Add(action);
        }

        // Remove Action
        public void RemoveAction(UserAction action)
        {
            Actions.Remove(action);
        }

    }
}
