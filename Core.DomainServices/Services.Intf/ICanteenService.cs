namespace Core.DomainServices.Services.Intf
{
    public interface ICanteenService
    {
        Task<Canteen> GetByLocation(LocationCanteenEnum? LocationCanteenEnum);
    }
}
