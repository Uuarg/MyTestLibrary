using System.IO;
using System.Linq;
using System.Net;
using MyTestLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MyTestLib.Controllers
{
    public class AuthorController : Controller
    {
        private readonly LibraryContext _db;
        private readonly IWebHostEnvironment _env;

        public AuthorController(LibraryContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public IActionResult Index() => View(_db.Authors.ToList());

        public IActionResult Details(int? id)
        {
            if (id == null) return BadRequest();
            var author = _db.Authors.FirstOrDefault(b => b.Id == id);
            if (author == null) return NotFound();
            return View(author);
        }
        public IActionResult Create()
        {
            ViewBag.Id = new SelectList(_db.Authors, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Author author, IFormFile PictureFile)
        {
            if (ModelState.IsValid)
            {
                if (PictureFile != null && PictureFile.Length > 0)
                {
                    var fileName = Path.GetFileName(PictureFile.FileName);
                    var dir = Path.Combine(_env.WebRootPath, "Content", "Images", "Author");
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    var path = Path.Combine(dir, fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        PictureFile.CopyTo(stream);
                    }
                    author.ImageUrl = fileName;
                }
                _db.Authors.Add(author);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(_db.Authors, "Id", "Name", author.Id);
            return View(author);
        }

        private ActionResult HttpNotFound()
        {
            throw new NotImplementedException();
        }


        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var author = _db.Authors.Find(id);
            if (author == null) return HttpNotFound();
            ViewBag.DesignerId = new SelectList(_db.Authors, "Id", "Name", author.Id);
            return View(author);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var author = _db.Authors.Include("Author").FirstOrDefault(b => b.Id == id);
            if (author == null) return HttpNotFound();
            return View(author);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var author = _db.Authors.Find(id);
            if (author == null) return HttpNotFound();
            _db.Authors.Remove(author);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}