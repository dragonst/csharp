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

namespace ASP.NET_CORE_BLOG_CMS.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment hostingEnv;

        public PostsController(ApplicationDbContext context, IHostingEnvironment env)
        {
            _context = context;
            hostingEnv = env;
        }

        [HttpPost]
        public IActionResult DropzoneUpload([FromForm]string filename)
        {
          
            long size = 0;
            var files = Request.Form.Files;
            var file = files.ElementAt(0);
            filename = file.FileName;
            string filepath = hostingEnv.WebRootPath + $@"\images\useruploads\{filename}";
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
                   filename = hostingEnv.WebRootPath + $@"\{filename}";
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
        [Authorize]
        public async Task<IActionResult> Index()
        {
            /*var blogs = _context.Post 
                        .FromSql("SELECT ID,Title,dateTimeAdded,headerImageThumbnailData,headerImageURl,LEFT(textContent, 300) as textContent FROM dbo.Post")
                           .ToListAsync(); */

            var blogs = _context.Post.Select(x => new Post { ID = x.ID , Title = x.Title,dateTimeAdded = x.dateTimeAdded,headerImageURL = x.headerImageURL,
                textContent = x.textContent.Substring(0, 300)}).ToListAsync();
           
            return View(await blogs);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post.SingleOrDefaultAsync(m => m.ID == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,headerImageURL,textContent,headerImageThumbnailData")] Post post)
        {
            if (ModelState.IsValid)
            {
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

            var post = await _context.Post.SingleOrDefaultAsync(m => m.ID == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,headerImageURL,textContent,headerImageThumbnailData")] Post post)
        {
            if (id != post.ID)
            {
                return NotFound();
            }
          
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
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

            var post = await _context.Post.SingleOrDefaultAsync(m => m.ID == id);
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
            var post = await _context.Post.SingleOrDefaultAsync(m => m.ID == id);
            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
            return Json(id);
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.ID == id);
        }
    }
}
