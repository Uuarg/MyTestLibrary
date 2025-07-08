using System.IO;
using System.Linq;
using System.Net;
using MyTestLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MyTestLib.Controllers
{
    public class RecordController : Controller
    {
        private readonly LibraryContext _db;
        private readonly IWebHostEnvironment _env;

        public RecordController(LibraryContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public IActionResult Index() => View(_db.Record.Include(b => b.Id).ToList());

        public IActionResult Details(int? id)
        {
            if (id == null) return BadRequest();
            var record = _db.Record.Include(b => b.Id).FirstOrDefault(b => b.Id == id);
            if (record == null) return NotFound();
            return View(record);
        }
        public IActionResult Create()
        {
            ViewBag.DesignerId = new SelectList(_db.Record, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Record record, IFormFile PictureFile)
        {
            if (ModelState.IsValid)
            {
                if (PictureFile != null && PictureFile.Length > 0)
                {
                    var fileName = Path.GetFileName(PictureFile.FileName);
                    var dir = Path.Combine(_env.WebRootPath, "Content", "Images", "Record");
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    var path = Path.Combine(dir, fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        PictureFile.CopyTo(stream);
                    }
                    record.ImageUrl = fileName;
                }
                _db.Record.Add(record);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DesignerId = new SelectList(_db.Record, "Id", "Name", record.Id);
            return View(record);
        }

        private ActionResult HttpNotFound()
        {
            throw new NotImplementedException();
        }


        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var record = _db.Record.Find(id);
            if (record == null) return HttpNotFound();
            ViewBag.DesignerId = new SelectList(_db.Record, "Id", "Name", record.Id);
            return View(record);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var record = _db.Record.Include("Record").FirstOrDefault(b => b.Id == id);
            if (record == null) return HttpNotFound();
            return View(record);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var record = _db.Record.Find(id);
            if (record == null) return HttpNotFound();
            _db.Record.Remove(record);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}