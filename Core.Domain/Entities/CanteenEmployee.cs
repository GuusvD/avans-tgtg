namespace Core.Domain.Entities
{
    public class CanteenEmployee
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Vul een personeelsnummer in.")]
        public string? EmployeeId { get; set; }
        [Required(ErrorMessage = "Vul een naam in.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Selecteer een locatie.")]
        public LocationCanteenEnum? Location { get; set; }
    }
}
