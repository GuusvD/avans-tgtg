namespace Core.DomainServices.Services.Impl
{
    public class StudentService : IStudentService
    {
        private IStudentRepository IStudentRepository;

        public StudentService(IStudentRepository IStudentRepository)
        {
            this.IStudentRepository = IStudentRepository;
        }

        public async Task<bool> Add(Student Student)
        {
            if (Student != null)
            {
                await IStudentRepository.AddAsync(Student);
                return true;
            } else
            {
                throw new Exception("Het Student object is null.");
            }
        }

        public Student? GetById(string Id)
        {
            return IStudentRepository.GetAllAsync().Result.Where(p => p.StudentId == Id).FirstOrDefault();
        }
    }
}
