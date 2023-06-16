namespace Portal.Models
{
    public class PackageAlertModel
    {
        public Package? Package { get; set; }
        public bool ShowAlert { get; set; }
        public string? DescAlert { get; set; }
        public bool? FromOwnCanteen { get; set; }
    }
}
