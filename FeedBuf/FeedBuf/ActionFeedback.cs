using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedBuf
{
    class ActionFeedback
    {
        public int Id { get; set; }
        public UserAction UserAction { get; set; }
        public Feedback Feedback { get; set; }

        // Constructor
        public ActionFeedback(int id, UserAction userAction, Feedback feedback)
        {
            Id = id;
            UserAction = userAction;
            Feedback = feedback;
        }
    }
}
