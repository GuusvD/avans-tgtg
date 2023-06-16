namespace Core.DomainServices.Services.Impl
{
    public class CanteenService : ICanteenService
    {
        private ICanteenRepository ICanteenRepository;

        public CanteenService(ICanteenRepository ICanteenRepository)
        {
            this.ICanteenRepository = ICanteenRepository;
        }

        public async Task<Canteen> GetByLocation(LocationCanteenEnum? LocationCanteenEnum)
        {
            ICollection<Canteen> Canteens = await ICanteenRepository.GetAllAsync();

            foreach(Canteen Canteen in Canteens)
            {
                if (Canteen.Location == LocationCanteenEnum)
                {
                    return Canteen;
                }
            }

            return null!;
        }
    }
}
