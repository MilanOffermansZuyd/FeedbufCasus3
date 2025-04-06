using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace FeedBuf
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public Author ZuydUser { get; set; }
        public Student Student { get; set; }
        public Goal Goal { get; set; }
        public Actions List<Actions> { get; set; }

        public Feedback(int id, string text, Author zuydUser, Student student, Goal goal, Actions List<Actions>)
        {
            Id = id;
            Text = text;
            ZuydUser = zuydUser;
            Student = student;
            Goal = goal;
            Actions = List<Actions>;
        }
    }
}
