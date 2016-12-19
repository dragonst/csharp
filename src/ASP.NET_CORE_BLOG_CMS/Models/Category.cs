using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace ASP.NET_CORE_BLOG_CMS.Models
{
    public class Category
    {
        public Category()
        {
            Post = new List<Post>();
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public int numberOfPosts { get; set; }
        public virtual ICollection<Post> Post {get; set;}
    }
}
