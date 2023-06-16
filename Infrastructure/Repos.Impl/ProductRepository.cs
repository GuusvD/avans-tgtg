namespace Infrastructure.Repos.Impl
{
    public class ProductRepository : IProductRepository
    {
        private ApplicationContext ApplicationContext;

        public ProductRepository(ApplicationContext ApplicationContext)
        {
            this.ApplicationContext = ApplicationContext;
        }

        public async Task<ICollection<Product>> GetAllAsync()
        {
            return await ApplicationContext.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int Id)
        {
            return await ApplicationContext.Products.FindAsync(Id);
        }
    }
}
