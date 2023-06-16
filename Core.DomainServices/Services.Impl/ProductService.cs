namespace Core.DomainServices.Services.Impl
{
    public class ProductService : IProductService
    {
        private IProductRepository IProductRepository;

        public ProductService(IProductRepository IProductRepository)
        {
            this.IProductRepository = IProductRepository;
        }

        public async Task<ICollection<Product>> GetAll()
        {
            return await IProductRepository.GetAllAsync();
        }

        public async Task<Product?> GetById(int Id)
        {
            Product? Product = await IProductRepository.GetByIdAsync(Id);

            if (Product != null)
            {
                return Product;
            } else
            {
                throw new Exception("Het gegeven Product Id word niet gebruikt.");
            }
        }
    }
}
