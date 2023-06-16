namespace Infrastructure.Repos.Impl
{
    public class StudentRepository : IStudentRepository
    {
        private ApplicationContext ApplicationContext;

        public StudentRepository(ApplicationContext ApplicationContext)
        {
            this.ApplicationContext = ApplicationContext;
        }

        public async Task AddAsync(Student Student)
        {
            await ApplicationContext.Students.AddAsync(Student);
            ApplicationContext.SaveChanges();
        }

        public async Task<ICollection<Student>> GetAllAsync()
        {
            return await ApplicationContext.Students.ToListAsync();
        }
    }
}
