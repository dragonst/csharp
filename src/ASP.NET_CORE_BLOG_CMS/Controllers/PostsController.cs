using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASP.NET_CORE_BLOG_CMS.Data;
using ASP.NET_CORE_BLOG_CMS.Models;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace ASP.NET_CORE_BLOG_CMS.Controllers
{

   
    [Authorize]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnv;
        private string cssFile { get; set; }



        public PostsController(ApplicationDbContext context, IHostingEnvironment hostingenv)
        {
            _context = context;
            _hostingEnv = hostingenv;
           
        }

        [HttpPost]
        public IActionResult DropzoneUpload()
        {
           
            var files = Request.Form.Files;
            var file = files.ElementAt(0);
            string filename = file.FileName;
           
            string filepath = _hostingEnv.WebRootPath + $@"\images\useruploads\{filename}";
            using (FileStream fs = System.IO.File.Create(filepath))
            {

                file.CopyTo(fs);
                
                fs.Flush();
            }
            /*  FOR MULTIPLE FILES foreach (var file in files)
               {
                   var filename = ContentDispositionHeaderValue
                                   .Parse(file.ContentDisposition)
                                   .FileName
                                   .Trim('"');
                   filename = _hostingEnv.WebRootPath + $@"\{filename}";
                   size += file.Length;
                   using (FileStream fs = System.IO.File.Create(filename))
                   {

                       file.CopyTo(fs);
                       fs.Flush();
                   }
               } */
            string message = $"../../images/useruploads/{filename}";
            return Json(message);
        }

        // GET: Posts
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            /*var blogs = _context.Post 
                        .FromSql("SELECT ID,Title,dateTimeAdded,headerImageThumbnailData,headerImageURl,LEFT(textContent, 300) as textContent FROM dbo.Post")
                           .ToListAsync(); */

            var blogs = _context.Posts.Select(x => new Post { ID = x.ID , Title = x.Title,dateTimeAdded = x.dateTimeAdded,headerImageURL = x.headerImageURL,
                textContent = x.textContent.Substring(0, 300)});
          int pageSize = 8;
    return View(await PaginatedList<Post>.CreateAsync(blogs.AsNoTracking(), page ?? 1, pageSize));
          
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.SingleOrDefaultAsync(m => m.ID == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost]
        public IActionResult generateCssFile()
        {

            string cssString = Request.Form.ElementAt(0).Value;
            string cssFileName = Request.Form.ElementAt(1).Value;
            cssFile = cssFileName;
            string filepath = _hostingEnv.WebRootPath + $@"\css\autogenerated\{cssFileName}";

            System.IO.File.WriteAllText(filepath, cssString);


            return Json(cssFile);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {

            var categories = _context.Categories.Select(x => new { Id = x.Name, Value = x.Name });

            var model = new PostCategoryviewModel();
            model.CategoryList = new SelectList(categories, "Id", "Value");
           
            return View(model);
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,headerImageThumbnailData,headerImageURL,textContent,cssFileUrl")] Post post, [Bind("Name")] Category category)
        {          
            if (ModelState.IsValid)
            {
                
                string CategoryName = category.Name;

                var existingCategoryRecord = _context.Categories.Where(w => w.Name == CategoryName);

                if (!existingCategoryRecord.Any())
                {
                    _context.Categories.Add(category);


                    category.Post.Add(post);
                                        
                }
                else
                {
                    post.Category = existingCategoryRecord.First();                                                      
                }
               
                _context.Add(post);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.SingleOrDefaultAsync(m => m.ID == id);
            if (post == null)
            {
                return NotFound();
            }
           
            var categories = _context.Categories.Select(x => new { Id = x.Name, Value = x.Name });

            var role = _context.Posts.Where(p => p.ID == id).Include(p => p.Category);
            


            var model = new PostCategoryviewModel();
            
            
            model.CategoryList = new SelectList(categories, "Id", "Value", role.First().Category.Name);
            model.Post = post;


           
           
            return View(model);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,headerImageURL,textContent,headerImageThumbnailData")] Post post, [Bind("Name")] Category category)
        {
            if (id != post.ID)
            {
                return NotFound();
            }
          
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {


                        string CategoryName = category.Name;

                        var existingCategoryRecord = _context.Categories.Where(w => w.Name == CategoryName);

                        if (!existingCategoryRecord.Any())
                        {
                            _context.Categories.Add(category);

                            if (!string.IsNullOrEmpty(cssFile))
                            {
                                post.cssFileUrl = cssFile;
                            }

                            category.Post.Add(post);

                        }
                        else
                        {
                            post.Category = existingCategoryRecord.First();

                            if (!string.IsNullOrEmpty(cssFile))
                            {
                                post.cssFileUrl = Request.Form.ElementAt(1).Value;
                             
                            }

                            _context.Add(post);
                        }

                        _context.Update(post);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.SingleOrDefaultAsync(m => m.ID == id);
            if (post == null)
            {
                return NotFound();
            }
     
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.SingleOrDefaultAsync(m => m.ID == id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return Json(id);
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.ID == id);
        }
    }
}
