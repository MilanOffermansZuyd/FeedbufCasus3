using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FeedBuf
{
    class Teacher
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Goal> Goals { get; set; }

        public Teacher(int id, string firstName, string lastName, string email, string password)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Goals = new List<Goal>();
        }

        public void AddGoal(Goal goal)
        {
            Goals.Add(goal);
        }

        public void RemoveGoal(Goal goal)
        {
            Goals.Remove(goal);
        }

        public void GiveFeedback(Goal goal, Feedback feedback)
        {
            goal.AddFeedback(feedback);
        }
    }
}
