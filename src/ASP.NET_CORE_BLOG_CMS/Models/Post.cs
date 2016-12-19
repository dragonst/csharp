using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_CORE_BLOG_CMS.Models
{
    public class Post
    {
        public int ID { get; set; }
        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }
        [Display(Name = "Header Image")]
        public string headerImageURL { get; set; }
        [Display(Name = "Thumbnail")]
        public string headerImageThumbnailData { get; set; }
        [Display(Name = "Text Content")]
        public string textContent { get; set; }
        [Display(Name = "Last Modified")]
        public DateTime dateTimeAdded { get; set; } = DateTime.Now;
        public string cssFileUrl { get; set; }

        public virtual Category Category { get; set; }
    }
}
