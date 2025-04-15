using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedBuf
{
    public class Notification : Message
    {
        public int Id { get; set; }
        public string  Title { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsRead { get; set; }

        public Notification(int id, ZuydUser student, ZuydUser author, string title, string text, DateTime createdOn, bool isRead)
            : base(id, text, student, author)
        {
            Id = id;
            Title = title;
            CreatedOn = createdOn;
            IsRead = isRead;
        }
    }
}
