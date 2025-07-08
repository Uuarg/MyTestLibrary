using System.IO;
using System.Linq;
using System.Net;
using MyTestLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MyTestLib.Controllers
{
    public class ShelfController : Controller
    {
        private readonly LibraryContext _db;
        private readonly IWebHostEnvironment _env;

        public ShelfController(LibraryContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public IActionResult Index() => View(_db.Shelf.Include(b => b.Id).ToList());

        public IActionResult Details(int? id)
        {
            if (id == null) return BadRequest();
            var shelf = _db.Shelf.Include(b => b.Id).FirstOrDefault(b => b.Id == id);
            if (shelf == null) return NotFound();
            return View(shelf);
        }
        public IActionResult Create()
        {
            ViewBag.DesignerId = new SelectList(_db.Shelf, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Shelf shelf, IFormFile PictureFile)
        {
            if (ModelState.IsValid)
            {
                if (PictureFile != null && PictureFile.Length > 0)
                {
                    var fileName = Path.GetFileName(PictureFile.FileName);
                    var dir = Path.Combine(_env.WebRootPath, "Content", "Images", "Shelf");
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    var path = Path.Combine(dir, fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        PictureFile.CopyTo(stream);
                    }
                    shelf.ImageUrl = fileName;
                }
                _db.Shelf.Add(shelf);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DesignerId = new SelectList(_db.Shelf, "Id", "Name", shelf.Id);
            return View(shelf);
        }

        private ActionResult HttpNotFound()
        {
            throw new NotImplementedException();
        }


        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var shelf = _db.Shelf.Find(id);
            if (shelf == null) return HttpNotFound();
            ViewBag.DesignerId = new SelectList(_db.Shelf, "Id", "Name", shelf.Id);
            return View(shelf);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var shelf = _db.Shelf.Include("Shelf").FirstOrDefault(b => b.Id == id);
            if (shelf == null) return HttpNotFound();
            return View(shelf);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var shelf = _db.Shelf.Find(id);
            if (shelf == null) return HttpNotFound();
            _db.Shelf.Remove(shelf);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}