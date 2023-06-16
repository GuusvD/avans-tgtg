namespace Core.DomainServices.Services.Impl
{
    public class CanteenEmployeeService : ICanteenEmployeeService
    {
        private ICanteenEmployeeRepository ICanteenEmployeeRepository;

        public CanteenEmployeeService(ICanteenEmployeeRepository ICanteenEmployeeRepository)
        {
            this.ICanteenEmployeeRepository = ICanteenEmployeeRepository;
        }

        public async Task<bool> Add(CanteenEmployee CanteenEmployee)
        {
            if (CanteenEmployee != null)
            {
                await ICanteenEmployeeRepository.AddAsync(CanteenEmployee);
                return true;
            } else
            {
                throw new Exception("Het CanteenEmployee object is null.");
            }
        }

        public CanteenEmployee? GetById(string Id)
        {
            return ICanteenEmployeeRepository.GetAllAsync().Result.Where(p => p.EmployeeId == Id).FirstOrDefault();
        }
    }
}
