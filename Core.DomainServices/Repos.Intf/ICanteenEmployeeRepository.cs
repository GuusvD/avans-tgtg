namespace Core.DomainServices.Repos.Intf
{
    public interface ICanteenEmployeeRepository
    {
        Task<ICollection<CanteenEmployee>> GetAllAsync();
        Task AddAsync(CanteenEmployee CanteenEmployee);
    }
}
