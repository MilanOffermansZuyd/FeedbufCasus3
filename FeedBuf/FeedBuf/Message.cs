using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedBuf
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public ZuydUser Student { get; set; }
        public ZuydUser Author { get; set; }

        public Message(int id, string text, ZuydUser student, ZuydUser author)
        {
            Id = id;
            Text = text;
            Student = student;
            Author = author;
        }

    }
}
