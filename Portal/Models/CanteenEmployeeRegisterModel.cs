namespace Portal.Models
{
    public class CanteenEmployeeRegisterModel
    {
        [Required(ErrorMessage = "Vul een personeelsnummer in.")]
        [Display(Name = "Personeelsnummer")]
        public string? EmployeeId { get; set; }
        [Required(ErrorMessage = "Vul een naam in.")]
        [Display(Name = "Naam")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Vul een locatie in.")]
        [Display(Name = "Locatie")]
        public LocationCanteenEnum? Location { get; set; }
        [Required(ErrorMessage = "Vul een wachtwoord in.")]
        [Display(Name = "Wachtwoord")]
        public string? Password { get; set; }
    }
}
