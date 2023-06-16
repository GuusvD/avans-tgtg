namespace Portal.Models
{
    public class PackageModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Vul een naam in.")]
        [Display(Name = "Naam")]
        public string? Name { get; set; }
        [Display(Name = "Producten")]
        public ICollection<Product>? Products { get; set; }
        [Required(ErrorMessage = "Vul een ophaaldatum in.")]
        [Display(Name = "Ophaaldatum")]
        public DateTime? PickupTime { get; set; }
        [Required(ErrorMessage = "Vul een laatste ophaaldatum in.")]
        [Display(Name = "Laatste ophaaldatum")]
        public DateTime? LastPickupTime { get; set; }
        public bool? ForAdults { get; set; }
        [Required(ErrorMessage = "Vul een prijs in.")]
        [Display(Name = "Prijs")]
        public decimal? Price { get; set; }
        [Required(ErrorMessage = "Selecteer een maaltijdtype.")]
        [Display(Name = "Maaltijdtype")]
        public MealTypeEnum? MealType { get; set; }
        public Student? Student { get; set; }
        [Required(ErrorMessage = "Selecteer producten.")]
        public List<int>? SelectedProducts { get; set; }
    }
}
