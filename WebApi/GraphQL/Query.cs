namespace WebApi.GraphQL
{
    public class Query
    {
        public async Task<IEnumerable<Package>> Packages(IPackageService IPackageService) => await IPackageService.GetAll();
    }
}
