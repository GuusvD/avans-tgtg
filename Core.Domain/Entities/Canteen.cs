namespace Core.Domain.Entities
{
    public class Canteen
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Selecteer een stad.")]
        public CityEnum? City { get; set; }
        [Required(ErrorMessage = "Selecteer een locatie.")]
        public LocationCanteenEnum? Location { get; set; }
        [Required(ErrorMessage = "Geef aan of warme maaltijden zijn toegestaan.")]
        public bool? WarmMeals { get; set; }
    }
}
