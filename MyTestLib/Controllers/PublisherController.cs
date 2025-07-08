using System.IO;
using System.Linq;
using System.Net;
using MyTestLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MyTestLib.Controllers
{
    public class PublisherController : Controller
    {
        private readonly LibraryContext _db;
        private readonly IWebHostEnvironment _env;

        public PublisherController(LibraryContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public IActionResult Index() => View(_db.Publisher.Include(b => b.Id).ToList());

        public IActionResult Details(int? id)
        {
            if (id == null) return BadRequest();
            var publisher = _db.Publisher.Include(b => b.Id).FirstOrDefault(b => b.Id == id);
            if (publisher == null) return NotFound();
            return View(publisher);
        }
        public IActionResult Create()
        {
            ViewBag.DesignerId = new SelectList(_db.Designer, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Publisher publisher, IFormFile PictureFile)
        {
            if (ModelState.IsValid)
            {
                if (PictureFile != null && PictureFile.Length > 0)
                {
                    var fileName = Path.GetFileName(PictureFile.FileName);
                    var dir = Path.Combine(_env.WebRootPath, "Content", "Images", "Publisher");
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    var path = Path.Combine(dir, fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        PictureFile.CopyTo(stream);
                    }
                    publisher.ImageUrl = fileName;
                }
                _db.Publisher.Add(publisher);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DesignerId = new SelectList(_db.Publisher, "Id", "Name", publisher.Id);
            return View(publisher);
        }

        private ActionResult HttpNotFound()
        {
            throw new NotImplementedException();
        }


        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var publisher = _db.Publisher.Find(id);
            if (publisher == null) return HttpNotFound();
            ViewBag.DesignerId = new SelectList(_db.Designer, "Id", "Name", publisher.Id);
            return View(publisher);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var publisher = _db.Publisher.Include("Publisher").FirstOrDefault(b => b.Id == id);
            if (publisher == null) return HttpNotFound();
            return View(publisher);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var publisher = _db.Publisher.Find(id);
            if (publisher == null) return HttpNotFound();
            _db.Publisher.Remove(publisher);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}