namespace MultiTenancy.Services
{
    public interface IProductService
    {
        Task<Product?> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product product);
        Task<IReadOnlyList<Product>> GetAllAsync();
    }
}
