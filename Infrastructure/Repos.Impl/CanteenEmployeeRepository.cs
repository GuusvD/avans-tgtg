namespace Infrastructure.Repos.Impl
{
    public class CanteenEmployeeRepository : ICanteenEmployeeRepository
    {
        private ApplicationContext ApplicationContext;

        public CanteenEmployeeRepository(ApplicationContext ApplicationContext)
        {
            this.ApplicationContext = ApplicationContext;
        }

        public async Task AddAsync(CanteenEmployee CanteenEmployee)
        {
            await ApplicationContext.CanteenEmployees.AddAsync(CanteenEmployee);
            ApplicationContext.SaveChanges();
        }

        public async Task<ICollection<CanteenEmployee>> GetAllAsync()
        {
            return await ApplicationContext.CanteenEmployees.ToListAsync();
        }
    }
}
