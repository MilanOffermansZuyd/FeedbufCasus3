﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace FeedBuf
{
    public class Feedback : Message
    {
        public int Id { get; set; }
        public Goal Goal { get; set; }
        public List<UserAction> UserActions { get; set; }


        // Constructor
        public Feedback(int id, Goal goal, string text, ZuydUser student, ZuydUser author, string shortDescription)
            : base(id, text, student, author, shortDescription)
        {
            Id = id;
            Goal = goal;
        }
    }
}
