namespace Core.Domain.Entities
{
    public class Package
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Vul een naam in.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Selecteer producten.")]
        public ICollection<Product>? Products { get; set; }
        [Required(ErrorMessage = "Selecteer een kantine.")]
        public Canteen? Canteen { get; set; }
        [Required(ErrorMessage = "Vul een ophaaldatum in.")]
        public DateTime? PickupTime { get; set; }
        [Required(ErrorMessage = "Vul een laatste ophaaldatum in.")]
        public DateTime? LastPickupTime { get; set; }
        [Required(ErrorMessage = "Geef aan of het pakket 18+ is.")]
        public bool? ForAdults { get; set; }
        [Required(ErrorMessage = "Vul een prijs in.")]
        public decimal? Price { get; set; }
        [Required(ErrorMessage = "Selecteer een maaltijdtype.")]
        public MealTypeEnum? MealType { get; set; }
        public Student? Student { get; set; }
    }
}