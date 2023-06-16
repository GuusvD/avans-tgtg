namespace Core.DomainServices.Repos.Intf
{
    public interface IProductRepository
    {
        Task<ICollection<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int Id);
    }
}
