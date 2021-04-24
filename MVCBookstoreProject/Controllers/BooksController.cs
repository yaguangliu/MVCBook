using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MVCBookstoreProject.Helpers;
using MVCBookstoreProject.Models;
using MVCBookstoreProject.ViewModels;

namespace MVCBookstoreProject.Controllers
{
    public class BooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = RoleName.CanManage)]
        // GET: Books
        public ActionResult Index()
        {
            var books = db.Books.Include(b => b.Category).Include(b => b.Language).Include(b => b.Publisher);
            return View(books.ToList());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }
        [Authorize(Roles = RoleName.CanManage)]
        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName");
            ViewBag.PublisherId = new SelectList(db.Publishers, "PublisherId", "PublisherName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult Create(BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                var book = new Book();
                book.PublisherId = bookViewModel.PublisherId;
                book.Author = bookViewModel.Author;
                book.PublishedDate = bookViewModel.PublishedDate;
                book.BookName = bookViewModel.BookName;
                book.CategoryId = bookViewModel.CategoryId;
                book.Description = bookViewModel.Description;
                book.LanguageId = bookViewModel.LanguageId;
                book.Price = bookViewModel.Price;
                book.Stock = bookViewModel.Stock;
                if (bookViewModel.Photo != null)
                {
                    book.Photo = ImageConverter.ByteArrayFromPostedFile(bookViewModel.Photo);
                }
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", book.CategoryId);
            //ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName", book.LanguageId);
            //ViewBag.PublisherId = new SelectList(db.Publishers, "PublisherId", "PublisherName", book.PublisherId);
            return View(bookViewModel);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Include(c => c.Category).Include(l => l.Language).Include(p => p.Publisher)
                .Where(b => b.BookId == id).FirstOrDefault();
            if (book == null)
            {
                return HttpNotFound();
            }

            Mapper.CreateMap<Book, BookViewModel>().ForMember(x => x.Photo, opt => opt.Ignore());
            var bookViewModel = Mapper.Map<BookViewModel>(book);
            bookViewModel.PhotoDb = book.Photo;
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", book.CategoryId);
            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName", book.LanguageId);
            ViewBag.PublisherId = new SelectList(db.Publishers, "PublisherId", "PublisherName", book.PublisherId);
            return View(bookViewModel);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult Edit(BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                Book book = db.Books.Find(bookViewModel.BookId);
                if (bookViewModel != null && bookViewModel.Photo != null)
                {
                    book.Photo = ImageConverter.ByteArrayFromPostedFile(bookViewModel.Photo);
                }

                book.PublisherId = bookViewModel.PublisherId;
                book.CategoryId = bookViewModel.CategoryId;
                book.LanguageId = bookViewModel.LanguageId;
                book.Author = bookViewModel.Author;
                book.Description = bookViewModel.Description;
                book.BookName = bookViewModel.BookName;
                book.Price = bookViewModel.Price;
                book.PublishedDate = bookViewModel.PublishedDate;
                book.Stock = bookViewModel.Stock;
                book.isAvailable = bookViewModel.isAvailable;

                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", bookViewModel.CategoryId);
            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName", bookViewModel.LanguageId);
            ViewBag.PublisherId = new SelectList(db.Publishers, "PublisherId", "PublisherName", bookViewModel.PublisherId);
            return View(bookViewModel);
        }

        // GET: Books/Delete/5
        
        public ActionResult Delete(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            book.isAvailable = false;
            db.SaveChanges();
            var books = db.Books.ToList();
            //return View(book);
            return RedirectToAction("Index");
        }

        //// POST: Books/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Book book = db.Books.Find(id);
        //    db.Books.Remove(book);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
