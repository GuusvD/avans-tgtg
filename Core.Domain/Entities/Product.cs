namespace Core.Domain.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Vul een naam in.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Geef aan of het product alcohol bevat.")]
        public bool? Alcoholic { get; set; }
        [Required(ErrorMessage = "Geef een afbeelding.")]
        public byte[]? Image { get; set; }
        public ICollection<Package>? Package { get; set; }
    }
}
