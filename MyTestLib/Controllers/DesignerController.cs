using System.IO;
using System.Linq;
using System.Net;
using MyTestLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MyTestLib.Controllers
{
    public class DesignerController : Controller
    {
        private readonly LibraryContext _db;
        private readonly IWebHostEnvironment _env;

        public DesignerController(LibraryContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public IActionResult Index() => View(_db.Designer.Include(b => b.Id).ToList());

        public IActionResult Details(int? id)
        {
            if (id == null) return BadRequest();
            var designer = _db.Designer.Include(b => b.Id).FirstOrDefault(b => b.Id == id);
            if (designer == null) return NotFound();
            return View(designer);
        }
        public IActionResult Create()
        {
            ViewBag.DesignerId = new SelectList(_db.Designer, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Designer designer, IFormFile PictureFile)
        {
            if (ModelState.IsValid)
            {
                if (PictureFile != null && PictureFile.Length > 0)
                {
                    var fileName = Path.GetFileName(PictureFile.FileName);
                    var dir = Path.Combine(_env.WebRootPath, "Content", "Images", "Designer");
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    var path = Path.Combine(dir, fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        PictureFile.CopyTo(stream);
                    }
                    designer.ImageUrl = fileName;
                }
                _db.Designer.Add(designer);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DesignerId = new SelectList(_db.Designer, "Id", "Name", designer.Id);
            return View(designer);
        }

        private ActionResult HttpNotFound()
        {
            throw new NotImplementedException();
        }


        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var designer = _db.Designer.Find(id);
            if (designer == null) return HttpNotFound();
            ViewBag.DesignerId = new SelectList(_db.Designer, "Id", "Name", designer.Id);
            return View(designer);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var designer = _db.Designer.Include("Designer").FirstOrDefault(b => b.Id == id);
            if (designer == null) return HttpNotFound();
            return View(designer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var designer = _db.Designer.Find(id);
            if (designer == null) return HttpNotFound();
            _db.Designer.Remove(designer);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}