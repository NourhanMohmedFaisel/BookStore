using BookStore.Helpers;
using BookStore.IRepository;
using BookStore.Models;
using BookStore.Repository;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepo _categoryRepository;
        public CategoryController(ICategoryRepo categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            var categories = _categoryRepository.GetAll();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Add()
        {
         
            return View();

        }

        [HttpPost]
        public IActionResult SaveAdd(CategoryViewModel categoryViewModel)
        {
            Category newCategory = new Category();
            categoryViewModel.ImagePath = DocumentSettings.UploadFile(categoryViewModel.Image, "images");
            if (ModelState.IsValid == true)
            {
                try
                {
                    newCategory.Name = categoryViewModel.Name;
                   
                    newCategory.ImagePath = categoryViewModel.ImagePath; 
                    
                    _categoryRepository.Add(newCategory);
                    _categoryRepository.Save();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }
            }
           

            return View("Add", categoryViewModel);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {

            Category category = _categoryRepository.GetById(id);
            
            if (category == null)
            {
                return NotFound();
            }

            CategoryViewModelWithId categoryViewModel = new CategoryViewModelWithId();

            categoryViewModel.Id = category.Id;
            categoryViewModel.Name = category.Name;
            categoryViewModel.ImagePath = category.ImagePath;
           
            return View("Edit", categoryViewModel);
        }


        [HttpPost]
        public IActionResult SaveEdit(int id, CategoryViewModelWithId categoryViewModel)
        {
            
            var category = _categoryRepository.GetById(id);
            if (category == null)
                return NotFound();

        
            if (categoryViewModel.Image != null)
            {
                categoryViewModel.ImagePath = DocumentSettings.UploadFile(categoryViewModel.Image, "images");
            }
            else
            {
                categoryViewModel.ImagePath = category.ImagePath;
            }

            if (ModelState.IsValid)
            {
                try
                {
                   
                    category.Name = categoryViewModel.Name;
                   
                    category.ImagePath = categoryViewModel.ImagePath;
                   

                    _categoryRepository.Update(category);
                    _categoryRepository.Save();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    var errorMessage = ex.InnerException?.Message ?? ex.Message;
                    ModelState.AddModelError(string.Empty, errorMessage);
                }
            }

            
            return View("Edit", categoryViewModel);
        }


        public IActionResult ConfirmDelete(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category); 
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            category.IsDeleted = true;
            _categoryRepository.Update(category);
            _categoryRepository.Save();

           
            return RedirectToAction("Index");
        }




    }
}
