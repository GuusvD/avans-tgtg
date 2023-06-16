namespace Core.DomainServices.Services.Intf
{
    public interface IPackageService
    {
        Task<ICollection<Package>> GetAll();
        Task<bool> Add(Package Package, ICollection<int> SelectedProducts, string UserId);
        Package? GetById(int Id);
        Package UpdateGetValidate(int PackageId, string UserId);
        bool Update(Package NewData, ICollection<int> SelectedProducts, int PackageId, string UserId);
        bool Delete(int PackageId, string UserId);
        bool AssignStudent(int PackageId, string UserId);
        ICollection<Package> GetAllPackagesByStudent(string Id);
        ICollection<Package> GetAllPackagesByCanteen(LocationCanteenEnum LocationCanteenEnum);
        ICollection<Package> GetAllAvailablePackages();
        ICollection<Package> GetAllNonExpiredPackages(LocationCanteenEnum LocationCanteenEnum);
        ICollection<Product> SelectedProductsToProducts(ICollection<int> SelectedProducts);
        ICollection<Product> ProductLinkPackage(ICollection<Product> Products, Package Package);
        void ValidatePickupTime(DateTime PickupTime);
        void ValidateLastPickupTime(DateTime LastPickupTime, DateTime PickupTime);
        void ValidateWarmMealsAllowed(bool CanteenWarmMeals, MealTypeEnum PackageMealType);
    }
}
