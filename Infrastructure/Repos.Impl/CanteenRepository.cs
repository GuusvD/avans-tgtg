namespace Infrastructure.Repos.Impl
{
    public class CanteenRepository : ICanteenRepository
    {
        private ApplicationContext ApplicationContext;

        public CanteenRepository(ApplicationContext ApplicationContext)
        {
            this.ApplicationContext = ApplicationContext;
        }

        public async Task<ICollection<Canteen>> GetAllAsync()
        {
            return await ApplicationContext.Canteens.ToListAsync();
        }

        public async Task<Canteen?> GetByIdAsync(int Id)
        {
            return await ApplicationContext.Canteens.FindAsync(Id);
        }
    }
}
