using System.IO;
using System.Linq;
using System.Net;
using MyTestLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MyTestLib.Controllers
{
    public class MovieController : Controller
    {
        private readonly LibraryContext _db;
        private readonly IWebHostEnvironment _env;

        public MovieController(LibraryContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public IActionResult Index() => View(_db.Movie.Include(b => b.Id).ToList());

        public IActionResult Details(int? id)
        {
            if (id == null) return BadRequest();
            var movie = _db.Movie.Include(b => b.Id).FirstOrDefault(b => b.Id == id);
            if (movie == null) return NotFound();
            return View(movie);
        }
        public IActionResult Create()
        {
            ViewBag.Id = new SelectList(_db.Movie, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Movie movie, IFormFile PictureFile)
        {
            if (ModelState.IsValid)
            {
                if (PictureFile != null && PictureFile.Length > 0)
                {
                    var fileName = Path.GetFileName(PictureFile.FileName);
                    var dir = Path.Combine(_env.WebRootPath, "Content", "Images", "Movie");
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    var path = Path.Combine(dir, fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        PictureFile.CopyTo(stream);
                    }
                    movie.ImageUrl = fileName;
                }
                _db.Movie.Add(movie);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DesignerId = new SelectList(_db.Movie, "Id", "Name", movie.Id);
            return View(movie);
        }

        private ActionResult HttpNotFound()
        {
            throw new NotImplementedException();
        }


        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var movie = _db.Movie.Find(id);
            if (movie == null) return HttpNotFound();
            ViewBag.DesignerId = new SelectList(_db.Movie, "Id", "Name", movie.Id);
            return View(movie);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var movie = _db.Movie.Include("Movie").FirstOrDefault(b => b.Id == id);
            if (movie == null) return HttpNotFound();
            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var movie = _db.Movie.Find(id);
            if (movie == null) return HttpNotFound();
            _db.Movie.Remove(movie);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}