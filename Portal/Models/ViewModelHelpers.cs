namespace Portal.Models
{
    public static class ViewModelHelpers
    {
        public static PackageModel ToPackageModel(Package Package, ICollection<int> SelectedProducts)
        {
            return new PackageModel()
            {
                Id = Package.Id,
                Name = Package.Name,
                Products = Package.Products,
                PickupTime = Package.PickupTime,
                LastPickupTime = Package.LastPickupTime,
                ForAdults = Package.ForAdults,
                Price = Package.Price,
                MealType = Package.MealType,
                Student = Package.Student,
                SelectedProducts = SelectedProducts.ToList()
            };
        }

        public static ProductPackageModel ToProductPackageModel(ICollection<Product>? Products, PackageModel? PackageModel)
        {
            ProductPackageModel ProductPackageModel = new ProductPackageModel();

            if (Products != null)
            {
                ProductPackageModel.Products = Products;
            }

            if (PackageModel != null)
            {
                ProductPackageModel.Package = PackageModel;
            }

            return ProductPackageModel;
        }

        public static PackageAlertModel ToPackageAlertModel(Package? Package, bool? ShowAlert, string? DescAlert, bool? FromOwnCanteen)
        {
            PackageAlertModel PackageAlertModel = new PackageAlertModel();

            if (Package != null)
            {
                PackageAlertModel.Package = Package;
            }

            if (ShowAlert != null)
            {
                PackageAlertModel.ShowAlert = (bool)ShowAlert;
            }

            if (DescAlert != null)
            {
                PackageAlertModel.DescAlert = DescAlert;
            }

            if (FromOwnCanteen != null)
            {
                PackageAlertModel.FromOwnCanteen = FromOwnCanteen;
            }

            return PackageAlertModel;
        }
    }
}
