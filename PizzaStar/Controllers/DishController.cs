using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PizzaStar.Interfaces;
using PizzaStar.Models;
using PizzaStar.Models.Pages;
using PizzaStar.ViewModels;

namespace PizzaStar.Controllers
{
    [Authorize(Roles = "Admin,Editor")]
    public class DishController : Controller
    {
        private readonly IProduct _products;
        private readonly ICategory _categories;
        private readonly IWebHostEnvironment _appEnvironment;

        public DishController(IProduct products, ICategory categories, IWebHostEnvironment appEnvironment)
        {
            _products = products;
            _appEnvironment = appEnvironment;
            _categories = categories;
        }

        [Route("/panel/dishes")]
        [HttpGet]
        public IActionResult Dishes(QueryOptions options)
        {
            return View(_products.GetAllProducts(options));
        }

        [Route("/panel/delete-product")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int productId)
        {
            var currentProduct = await _products.GetProductAsync(productId);
            if (currentProduct != null)
            {
                if (currentProduct.Image != null)
                {
                    if (System.IO.File.Exists(_appEnvironment.WebRootPath + currentProduct.Image))
                    {
                        System.IO.File.Delete(_appEnvironment.WebRootPath + currentProduct.Image);
                    }
                }
                await _products.DeleteProductAsync(currentProduct);
            }
            return Ok();
        }

        [Route("/panel/create-product")]
        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            var categories = await _categories.GetAllCategoriesAsync();
            return View(new ProductViewModel
            {
                AllCategories = new SelectList(categories, "Id", "Name")
            });
        }

        [Route("/panel/create-product")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateProduct(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                productViewModel.DateOfPublication = DateTime.Now;
                var currentProduct = new Product
                {
                    Name = productViewModel.Name!,
                    Description = productViewModel.Description!,
                    Type = productViewModel.Type,
                    Brand = productViewModel.Brand,
                    Calories = productViewModel.Calories ?? 0,
                    Price = productViewModel.Price ?? 0,
                    Weight = productViewModel.Weight ?? 0,
                    CategoryId = productViewModel.CategoryId,
                    DateOfPublication = DateTime.Now
                };
                if (productViewModel.File != null)
                {
                    string fileName = productViewModel.File.FileName;
                    if (fileName.Contains("\\"))
                    {
                        fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);
                    }
                    string filePath = "/productFiles/" + Guid.NewGuid().ToString() + fileName;
                    currentProduct.Image = filePath;

                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + filePath, FileMode.Create))
                    {
                        await productViewModel.File.CopyToAsync(fileStream);
                    }
                }
                await _products.AddProductAsync(currentProduct);
                return RedirectToAction(nameof(Dishes));
            }
            return View(productViewModel);
        }

        [Route("/panel/edit-product")]
        [HttpGet]
        public async Task<IActionResult> EditProduct(int productId)
        {
            var currentProduct = await _products.GetProductAsync(productId);
            if (currentProduct != null)
            {
                var categories = await _categories.GetAllCategoriesAsync();
                ProductViewModel productViewModel = new ProductViewModel
                {
                    Id = currentProduct.Id,
                    Name = currentProduct.Name,
                    Image = currentProduct.Image,
                    Description = currentProduct.Description,
                    Brand = currentProduct.Brand,
                    Calories = currentProduct.Calories,
                    Price = currentProduct.Price,
                    Type = currentProduct.Type,
                    Weight = currentProduct.Weight,
                    CategoryId = currentProduct.CategoryId,
                    AllCategories = new SelectList(categories, "Id", "Name")
                };
                return View(productViewModel);
            }
            return NotFound();
        }

        [Route("/panel/edit-product")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditProduct(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var selectedProduct = await _products.GetProductAsync(productViewModel.Id ?? 0);
                if (selectedProduct == null)
                {
                    return NotFound();
                }
                var currentProduct = new Product
                {
                    Id = productViewModel.Id ?? 0,
                    Name = productViewModel.Name!,
                    Description = productViewModel.Description!,
                    Image = selectedProduct.Image,
                    Brand = productViewModel.Brand,
                    Calories = productViewModel.Calories ?? 0,
                    Price = productViewModel.Price ?? 0,
                    Type = productViewModel.Type,
                    Weight = productViewModel.Weight ?? 0,
                    CategoryId = productViewModel.CategoryId
                };
                if (productViewModel.File != null)
                {
                    if (selectedProduct.Image != null)
                    {
                        if (System.IO.File.Exists(_appEnvironment.WebRootPath + selectedProduct.Image))
                        {
                            System.IO.File.Delete(_appEnvironment.WebRootPath + selectedProduct.Image);
                        }
                    }
                    string fileName = productViewModel.File.FileName;
                    if (fileName.Contains("\\"))
                    {
                        fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);
                    }
                    string filePath = "/categoryFiles/" + Guid.NewGuid().ToString() + fileName;
                    currentProduct.Image = filePath;

                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + filePath, FileMode.Create))
                    {
                        await productViewModel.File.CopyToAsync(fileStream);
                    }
                }
                await _products.EditProductAsync(currentProduct);
                return RedirectToAction(nameof(Dishes));
            }
            return View(productViewModel);
        }
    }
}
