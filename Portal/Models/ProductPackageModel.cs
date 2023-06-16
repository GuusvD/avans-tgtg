namespace Portal.Models
{
    public class ProductPackageModel
    {
        public ICollection<Product>? Products { get; set; }
        public PackageModel? Package { get; set; }
    }
}
