using System.IO;
using System.Linq;
using System.Net;
using MyTestLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MyTestLib.Controllers
{
    public class BookController : Controller
    {
        private readonly LibraryContext _db;
        private readonly IWebHostEnvironment _env;

        public BookController(LibraryContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public IActionResult Index() => View(_db.Book.Include(b => b.AuthorId).ToList());

        public IActionResult Details(int? id)
        {
            if (id == null) return BadRequest();
            var book = _db.Book.Include(b => b.AuthorId).FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();
            return View(book);
        }
        public IActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(_db.Designer, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book boardbook, IFormFile PictureFile)
        {
            if (ModelState.IsValid)
            {
                if (PictureFile != null && PictureFile.Length > 0)
                {
                    var fileName = Path.GetFileName(PictureFile.FileName);
                    var dir = Path.Combine(_env.WebRootPath, "Content", "Images", "Book");
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    var path = Path.Combine(dir, fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        PictureFile.CopyTo(stream);
                    }
                    boardbook.ImageUrl = fileName;
                }
                _db.Book.Add(boardbook);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(_db.Designer, "Id", "Name", boardbook.AuthorId);
            return View(boardbook);
        }

        private ActionResult HttpNotFound()
        {
            throw new NotImplementedException();
        }


        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var boardbook = _db.Book.Find(id);
            if (boardbook == null) return HttpNotFound();
            ViewBag.AuthorId = new SelectList(_db.Designer, "Id", "Name", boardbook.AuthorId);
            return View(boardbook);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var book = _db.Book.Include("Designer").FirstOrDefault(b => b.Id == id);
            if (book == null) return HttpNotFound();
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var book = _db.Book.Find(id);
            if (book == null) return HttpNotFound();
            _db.Book.Remove(book);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}