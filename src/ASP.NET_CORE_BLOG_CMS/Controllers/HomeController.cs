using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ASP.NET_CORE_BLOG_CMS.Data;
using Microsoft.Extensions.Logging;

namespace ASP.NET_CORE_BLOG_CMS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
           ViewBag.PostsCount =  _context.Posts.Count();
           ViewBag.CategoriesCount = _context.Categories.Count();
            ViewBag.UsersCount = _context.Users.Count();
                      
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
