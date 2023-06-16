namespace Core.DomainServices.Services.Intf
{
    public interface IStudentService
    {
        Student? GetById(string Id);
        Task<bool> Add(Student Student);
    }
}
