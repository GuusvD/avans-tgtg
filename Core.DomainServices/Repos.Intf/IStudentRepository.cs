namespace Core.DomainServices.Repos.Intf
{
    public interface IStudentRepository
    {
        Task<ICollection<Student>> GetAllAsync();
        Task AddAsync(Student Student);
    }
}
