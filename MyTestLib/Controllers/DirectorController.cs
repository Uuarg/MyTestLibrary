using System.IO;
using System.Linq;
using System.Net;
using MyTestLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MyTestLib.Controllers
{
    public class DirectorController : Controller
    {
        private readonly LibraryContext _db;
        private readonly IWebHostEnvironment _env;

        public DirectorController(LibraryContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public IActionResult Index() => View(_db.Director.Include(b => b.Id).ToList());

        public IActionResult Details(int? id)
        {
            if (id == null) return BadRequest();
            var director = _db.Director.Include(b => b.Id).FirstOrDefault(b => b.Id == id);
            if (director == null) return NotFound();
            return View(director);
        }
        public IActionResult Create()
        {
            ViewBag.DesignerId = new SelectList(_db.Designer, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Director director, IFormFile PictureFile)
        {
            if (ModelState.IsValid)
            {
                if (PictureFile != null && PictureFile.Length > 0)
                {
                    var fileName = Path.GetFileName(PictureFile.FileName);
                    var dir = Path.Combine(_env.WebRootPath, "Content", "Images", "Director");
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    var path = Path.Combine(dir, fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        PictureFile.CopyTo(stream);
                    }
                    director.ImageUrl = fileName;
                }
                _db.Director.Add(director);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DesignerId = new SelectList(_db.Designer, "Id", "Name", director.Id);
            return View(director);
        }

        private ActionResult HttpNotFound()
        {
            throw new NotImplementedException();
        }


        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var director = _db.Director.Find(id);
            if (director == null) return HttpNotFound();
            ViewBag.DesignerId = new SelectList(_db.Designer, "Id", "Name", director.Id);
            return View(director);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var director = _db.Director.Include("Designer").FirstOrDefault(b => b.Id == id);
            if (director == null) return HttpNotFound();
            return View(director);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var director = _db.Director.Find(id);
            if (director == null) return HttpNotFound();
            _db.Director.Remove(director);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}