using PizzaStar.Models.Pages;
using PizzaStar.Models;

namespace PizzaStar.Interfaces
{
    public interface IProduct
    {
        PagedList<Product> GetAllProducts(QueryOptions options);
        PagedList<Product> GetAllProductsByCategory(QueryOptions options, int categoryId);

        Task<IEnumerable<Product>> GetEightRandomProductsAsync(int productId);
        Task<Product> GetProductAsync(int id);
        Task<Product> GetProductWithCategoryAsync(int id);
        Task AddProductAsync(Product product);
        Task DeleteProductAsync(Product product);
        Task EditProductAsync(Product product);
    }
}
