using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedBuf
{
    class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Goal> Goals { get; set; }
        public List<Feedback> Feedback { get; set; }
        public List<Action> Actions { get; set; }

        //constructor
        public Student(int id, string firstName, string lastName, string email, string password)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Goals = new List<Goal>();
            Feedback = new List<Feedback>();
            Actions = new List<Actions>();
        }
        //Add and remove goals
        public void AddGoal(Goal goal)
        {
            Goals.Add(goal);
        }
        public void RemoveGoal(Goal goal)
        {
            Goals.Remove(goal);
        }
        //add and remove actions
        public void AddAction(Action action)
        {
            Actions.Add(action);
        }
        public void RemoveAction(Action action)
        {
            Actions.Remove(action);
        }
        //add and remove feedback
        public void AddFeedback(Feedback feedback)
        {
            Feedback.Add(feedback);
        }
        public void RemoveFeedback(Feedback feedback)
        {
            Feedback.Remove(feedback);
        }
        //Give feedback
        public void GiveFeedback(Goal goal, Feedback feedback)
        {
            goal.AddFeedback(feedback);
        }
    }
}
