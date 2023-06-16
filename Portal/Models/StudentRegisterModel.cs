namespace Portal.Models
{
    public class StudentRegisterModel
    {
        [Required(ErrorMessage = "Vul een studentnummer in.")]
        [Display(Name = "Studentnummer")]
        public string? StudentId { get; set; }
        [Required(ErrorMessage = "Vul een naam in.")]
        [Display(Name = "Naam")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Vul een geboortedatum in.")]
        [Display(Name = "Geboortedatum")]
        public DateTime? DateOfBirth { get; set; }
        [Required(ErrorMessage = "Vul een emailadres in.")]
        [EmailAddress]
        [Display(Name = "Emailadres")]
        public string? EmailAddress { get; set; }
        [Required(ErrorMessage = "Selecteer een stad.")]
        [Display(Name = "Stad")]
        public CityEnum? City { get; set; }
        [Required(ErrorMessage = "Vul een telefoonnummer in.")]
        [Phone]
        [Display(Name = "Telefoonnummer")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Vul een wachtwoord in.")]
        [Display(Name = "Wachtwoord")]
        public string? Password { get; set; }
    }
}
