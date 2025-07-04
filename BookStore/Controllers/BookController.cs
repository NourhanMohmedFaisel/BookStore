using BookStore.Helpers;
using BookStore.IRepository;
using BookStore.Models;
using BookStore.Repository;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Core.Types;
using System.Security.Claims;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepo _bookRepository;
        private readonly ICategoryRepo _categoryRepository;


        public BookController(IBookRepo bookRepository, ICategoryRepo categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index(string search, int? categoryId)
        {
            var books = _bookRepository.GetAll();

            if (!string.IsNullOrEmpty(search))
            {
                books = books.Where(b => b.Title.Contains(search) || b.Author.Contains(search)).ToList();
            }

            if (categoryId.HasValue && categoryId > 0)
            {
                books = books.Where(b => b.CategoryId == categoryId).ToList();
            }

            ViewBag.Categories = _categoryRepository.GetAll();
            return View(books);
        }


     
        public IActionResult SetDiscount(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost]
        public IActionResult SetDiscount(int id, double discount)
        {
            var book = _bookRepository.GetById(id);
            if (book == null) return NotFound();

            book.Discount = discount;
            _bookRepository.Update(book);
            _bookRepository.Save();
            return RedirectToAction("Index");
        }

       
        public IActionResult Add()
        {
            BookViewModel bookViewModel = new BookViewModel();
            bookViewModel.Categories = _categoryRepository.GetAll();

            return View("Add", bookViewModel);

        }

        [HttpPost]
        public IActionResult SaveAdd(BookViewModel bookViewModel)
        {
            Book newBook = new Book();
            bookViewModel.ImagePath = DocumentSettings.UploadFile(bookViewModel.Image, "images");
            if (ModelState.IsValid == true)
            {
                try
                {
                    newBook.Title = bookViewModel.Title;
                    newBook.Author = bookViewModel.Author;
                    newBook.Price = bookViewModel.Price;
                    newBook.Description = bookViewModel.Description;
                    newBook.ImagePath = bookViewModel.ImagePath; 
                    newBook.CategoryId = bookViewModel.CategoryId;
                    _bookRepository.Add(newBook);
                    _bookRepository.Save();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }
            }
            bookViewModel.Categories = _categoryRepository.GetAll();

            return View("Add", bookViewModel);
        }

      
        [HttpGet]
        public IActionResult Edit(int id)
        {
           
            Book book = _bookRepository.GetById(id);
            List<Category> categoryList = _categoryRepository.GetAll();
            if (book == null)
            {
                return NotFound();
            }

            BookViewModelWithId bookViewModel = new BookViewModelWithId();

            bookViewModel.Id = book.Id;
            bookViewModel.Title = book.Title;
            bookViewModel.Price = book.Price;
            bookViewModel.Author = book.Author;
            bookViewModel.ImagePath = book.ImagePath;
            bookViewModel.CategoryId = book.CategoryId;
            bookViewModel.Description = book.Description;
            bookViewModel.Categories = categoryList;




            return View("Edit", bookViewModel);
        }


        [HttpPost]
        public IActionResult SaveEdit(int id, BookViewModelWithId bookViewModel)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
                return NotFound();

            if (bookViewModel.Image != null)
            {
                bookViewModel.ImagePath = DocumentSettings.UploadFile(bookViewModel.Image, "images");
            }
            else
            {
                bookViewModel.ImagePath = book.ImagePath; 
            }

            if (ModelState.IsValid)
            {
                try
                {
                   
                    book.Title = bookViewModel.Title;
                    book.Price = bookViewModel.Price;
                    book.Author = bookViewModel.Author;
                    book.ImagePath = bookViewModel.ImagePath;
                    book.Description = bookViewModel.Description;
                    book.CategoryId = bookViewModel.CategoryId;

                    _bookRepository.Update(book);
                    _bookRepository.Save();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    var errorMessage = ex.InnerException?.Message ?? ex.Message;
                    ModelState.AddModelError(string.Empty, errorMessage);
                }
            }

            
            bookViewModel.Categories = _categoryRepository.GetAll();
            return View("Edit", bookViewModel);
        }


       
        public IActionResult ConfirmDelete(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book); 
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }

            book.IsDeleted = true;
            _bookRepository.Update(book);
            _bookRepository.Save();

            
            return RedirectToAction("Index");
        }

     
        public IActionResult Details(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        
        public IActionResult GetByCategory(int id)
        {
            ViewBag.IsAuthenticated = User.Identity.IsAuthenticated;
            var books = _bookRepository.GetAll()
                .Where(b => b.CategoryId == id && !b.IsDeleted) 
                .ToList();

            ViewBag.CategoryName = _categoryRepository.GetById(id)?.Name;
            return View("BooksByCategory", books);
        }



    }

}
