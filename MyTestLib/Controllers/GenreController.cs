using System.IO;
using System.Linq;
using System.Net;
using MyTestLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MyTestLib.Controllers
{
    public class GenreController : Controller
    {
        private readonly LibraryContext _db;
        private readonly IWebHostEnvironment _env;

        public GenreController(LibraryContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public IActionResult Index() => View(_db.Genre.Include(b => b.Id).ToList());

        public IActionResult Details(int? id)
        {
            if (id == null) return BadRequest();
            var genre = _db.Genre.Include(b => b.Id).FirstOrDefault(b => b.Id == id);
            if (genre == null) return NotFound();
            return View(genre);
        }
        public IActionResult Create()
        {
            ViewBag.DesignerId = new SelectList(_db.Designer, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Genre genre, IFormFile PictureFile)
        {
            if (ModelState.IsValid)
            {
                if (PictureFile != null && PictureFile.Length > 0)
                {
                    var fileName = Path.GetFileName(PictureFile.FileName);
                    var dir = Path.Combine(_env.WebRootPath, "Content", "Images", "Genre");
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    var path = Path.Combine(dir, fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        PictureFile.CopyTo(stream);
                    }
                    genre.ImageUrl = fileName;
                }
                _db.Genre.Add(genre);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DesignerId = new SelectList(_db.Designer, "Id", "Name", genre.Id);
            return View(genre);
        }

        private ActionResult HttpNotFound()
        {
            throw new NotImplementedException();
        }


        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var genre = _db.Genre.Find(id);
            if (genre == null) return HttpNotFound();
            ViewBag.DesignerId = new SelectList(_db.Genre, "Id", "Name", genre.Id);
            return View(genre);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var genre = _db.Genre.Include("Id").FirstOrDefault(b => b.Id == id);
            if (genre == null) return HttpNotFound();
            return View(genre);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var genre = _db.Genre.Find(id);
            if (genre == null) return HttpNotFound();
            _db.Genre.Remove(genre);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}