namespace Core.DomainServices.Services.Intf
{
    public interface ICanteenEmployeeService
    {
        CanteenEmployee? GetById(string Id);
        Task<bool> Add(CanteenEmployee CanteenEmployee);
    }
}
