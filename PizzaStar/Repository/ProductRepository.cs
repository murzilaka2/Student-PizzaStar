using Microsoft.EntityFrameworkCore;
using PizzaStar.Data;
using PizzaStar.Interfaces;
using PizzaStar.Models;
using PizzaStar.Models.Pages;

namespace PizzaStar.Repository
{
    public class ProductRepository : IProduct
    {
        private readonly ApplicationContext _applicationContext;

        public ProductRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public PagedList<Product> GetAllProducts(QueryOptions options)
        {
            return new PagedList<Product>(_applicationContext.Products.Include(e => e.Category), options);
        }

        public async Task AddProductAsync(Product product)
        {
            await _applicationContext.Products.AddAsync(product);
            await _applicationContext.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Product product)
        {
            _applicationContext.Products.Remove(product);
            await _applicationContext.SaveChangesAsync();
        }

        public async Task EditProductAsync(Product product)
        {
            _applicationContext.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _applicationContext.Entry(product).Property(e => e.DateOfPublication).IsModified = false;
            await _applicationContext.SaveChangesAsync();
        }

        public async Task<Product> GetProductAsync(int id)
        {
            return await _applicationContext.Products.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public PagedList<Product> GetAllProductsByCategory(QueryOptions options, int categoryId)
        {
            return new PagedList<Product>(_applicationContext.Products.Include(e => e.Category).Where(e=>e.CategoryId == categoryId), options);
        }

        public async Task<Product> GetProductWithCategoryAsync(int id)
        {
            return await _applicationContext.Products.Include(e=>e.Category).AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Product>> GetEightRandomProductsAsync(int productId)
        {
            return await _applicationContext.Products.Where(e => e.Id != productId).OrderBy(e => Guid.NewGuid()).Take(8).ToListAsync();
        }
    }
}
