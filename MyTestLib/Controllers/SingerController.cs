using System.IO;
using System.Linq;
using System.Net;
using MyTestLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MyTestLib.Controllers
{
    public class SingerController : Controller
    {
        private readonly LibraryContext _db;
        private readonly IWebHostEnvironment _env;

        public SingerController(LibraryContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public IActionResult Index() => View(_db.Singer.Include(b => b.Id).ToList());

        public IActionResult Details(int? id)
        {
            if (id == null) return BadRequest();
            var singer = _db.Singer.Include(b => b.Id).FirstOrDefault(b => b.Id == id);
            if (singer == null) return NotFound();
            return View(singer);
        }
        public IActionResult Create()
        {
            ViewBag.DesignerId = new SelectList(_db.Singer, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Singer singer, IFormFile PictureFile)
        {
            if (ModelState.IsValid)
            {
                if (PictureFile != null && PictureFile.Length > 0)
                {
                    var fileName = Path.GetFileName(PictureFile.FileName);
                    var dir = Path.Combine(_env.WebRootPath, "Content", "Images", "Singer");
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    var path = Path.Combine(dir, fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        PictureFile.CopyTo(stream);
                    }
                    singer.ImageUrl = fileName;
                }
                _db.Singer.Add(singer);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DesignerId = new SelectList(_db.Singer, "Id", "Name", singer.Id);
            return View(singer);
        }

        private ActionResult HttpNotFound()
        {
            throw new NotImplementedException();
        }


        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var singer = _db.Singer.Find(id);
            if (singer == null) return HttpNotFound();
            ViewBag.DesignerId = new SelectList(_db.Singer, "Id", "Name", singer.Id);
            return View(singer);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var singer = _db.Singer.Include("Singer").FirstOrDefault(b => b.Id == id);
            if (singer == null) return HttpNotFound();
            return View(singer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var singer = _db.Singer.Find(id);
            if (singer == null) return HttpNotFound();
            _db.Singer.Remove(singer);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}