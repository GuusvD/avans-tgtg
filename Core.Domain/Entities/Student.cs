namespace Core.Domain.Entities
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Vul een studentnummer in.")]
        public string? StudentId { get; set; }
        [Required(ErrorMessage = "Vul een naam in.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Vul een geboortedatum in.")]
        public DateTime? DateOfBirth { get; set; }
        [Required(ErrorMessage = "Vul een emailadres in.")]
        [EmailAddress]
        public string? EmailAddress { get; set; }
        [Required(ErrorMessage = "Selecteer een stad.")]
        public CityEnum? City { get; set; }
        [Required(ErrorMessage = "Vul een telefoonnummer in.")]
        [Phone]
        public string? PhoneNumber { get; set; }
    }
}
