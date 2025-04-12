using FeedBuf.Catagory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedBuf
{
    public class SubGoal : Goal
    {
        public int SubId { get; set; }
        public Goal Goal { get; set; }

        public SubGoal(int id, DateTime softDeadline, DateTime hardDeadline, bool isFinished, Category category, string text, ZuydUser student, ZuydUser author, bool openForFeedback, int subId, Goal goal)
            : base(id, softDeadline, hardDeadline, isFinished, category, text, student, author, openForFeedback)
        {
            Id = id;
            Goal = goal;
            SubId = subId;
        }
    }
}
