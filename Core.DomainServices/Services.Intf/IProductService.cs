namespace Core.DomainServices.Services.Intf
{
    public interface IProductService
    {
        Task<ICollection<Product>> GetAll();
        Task<Product?> GetById(int Id);
    }
}
