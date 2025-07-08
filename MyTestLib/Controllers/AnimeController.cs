using System.IO;
using System.Linq;
using System.Net;
using MyTestLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MyTestLib.Controllers
{
    public class AnimeController : Controller
    {
        private readonly LibraryContext _db;
        private readonly IWebHostEnvironment _env;

        public AnimeController(LibraryContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public IActionResult Index() => View(_db.Anime.Include(b => b.Id).ToList());

        public IActionResult Details(int? id)
        {
            if (id == null) return BadRequest();
            var anime = _db.Anime.Include(b => b.Id).FirstOrDefault(b => b.Id == id);
            if (anime == null) return NotFound();
            return View(anime);
        }
        public IActionResult Create()
        {
            ViewBag.DesignerId = new SelectList(_db.Designer, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Anime anime, IFormFile PictureFile)
        {
            if (ModelState.IsValid)
            {
                if (PictureFile != null && PictureFile.Length > 0)
                {
                    var fileName = Path.GetFileName(PictureFile.FileName);
                    var dir = Path.Combine(_env.WebRootPath, "Content", "Images", "Anime");
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    var path = Path.Combine(dir, fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        PictureFile.CopyTo(stream);
                    }
                    anime.ImageUrl = fileName;
                }
                _db.Anime.Add(anime);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID = new SelectList(_db.Anime, "Id", "Name", anime.Id);
            return View(anime);
        }

        private ActionResult HttpNotFound()
        {
            throw new NotImplementedException();
        }


        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var anime = _db.Anime.Find(id);
            if (anime == null) return HttpNotFound();
            ViewBag.DesignerId = new SelectList(_db.Anime, "Id", "Name", anime.Id);
            return View(anime);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var anime = _db.Anime.Include("Id").FirstOrDefault(b => b.Id == id);
            if (anime == null) return HttpNotFound();
            return View(anime);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var anime = _db.Anime.Find(id);
            if (anime == null) return HttpNotFound();
            _db.Anime.Remove(anime);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}