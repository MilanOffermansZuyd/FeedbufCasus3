using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedBuf
{
    public class ZuydUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public List<Goal> Goals { get; set; }
        public List<Feedback> Feedbacks { get; set; }
        public List<UserAction> UserActions { get; set; }

        public ZuydUser(int id, string firstName, string lastName, string email, string password, int role)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Role = role;
        }


        public void AddGoal(Goal goal)
        {
            Goals.Add(goal);
        }

        public void GiveFeedback(Feedback feedback)
        {
            Feedbacks.Add(feedback);
        }

        public void Update()
        {
            // Update information
        }

        public void Delete()
        {
            // Delete information
        }
    }
}
