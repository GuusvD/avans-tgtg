namespace Core.DomainServices.Repos.Intf
{
    public interface IPackageRepository
    {
        Task<ICollection<Package>> GetAllAsync();
        Task AddAsync(Package Package);
        void Delete(int Id);
        void Update(Package Package);
    }
}
