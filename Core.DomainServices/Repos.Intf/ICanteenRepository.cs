namespace Core.DomainServices.Repos.Intf
{
    public interface ICanteenRepository
    {
        Task<ICollection<Canteen>> GetAllAsync();
        Task<Canteen?> GetByIdAsync(int Id);
    }
}
