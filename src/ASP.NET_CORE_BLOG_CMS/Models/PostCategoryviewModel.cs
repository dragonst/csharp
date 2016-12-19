using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_BLOG_CMS.Models
{
    public class PostCategoryviewModel
    {
        public Post Post { get; set; }
        public Category Category { get; set; }
        public SelectList CategoryList { get; set; }
     
    }
}
