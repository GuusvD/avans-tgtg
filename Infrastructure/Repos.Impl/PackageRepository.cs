namespace Infrastructure.Repos.Impl
{
    public class PackageRepository : IPackageRepository
    {
        private ApplicationContext ApplicationContext;

        public PackageRepository(ApplicationContext ApplicationContext)
        {
            this.ApplicationContext = ApplicationContext;
        }

        public async Task AddAsync(Package Package)
        {
            await ApplicationContext.Packages.AddAsync(Package);
            ApplicationContext.SaveChanges();
        }

        public void Delete(int Id)
        {
            var Package = ApplicationContext.Packages.Find(Id);

            if (Package != null)
            {
                ApplicationContext.Packages.Remove(Package);
                ApplicationContext.SaveChanges();
            }
        }

        public async Task<ICollection<Package>> GetAllAsync()
        {
            return await ApplicationContext.Packages.Include(p => p.Products).Include(p => p.Student).Include(p => p.Canteen).ToListAsync();
        }

        public void Update(Package Package)
        {
            ApplicationContext.Update(Package);
            ApplicationContext.SaveChanges();
        }
    }
}
