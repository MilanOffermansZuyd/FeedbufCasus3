using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedBuf.Catagory
{
    public class Category
    {
        public Category(int id ,string type)
        {
            Id = id;
            Type = type;
        }
        public Category(string type)
        {
            Type = type;
        }
        public int Id { get; set; } 
        public string Type { get; set; }

        public void GetCategoryTypes()
        {

        }

        public void AddCategoryType() 
        {

        }

    }
}
