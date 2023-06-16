namespace Portal.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Vul een student/personeelsnummer in.")]
        [Display(Name = "Student/personeelsnummer")]
        public string? UserId { get; set; }

        [Required(ErrorMessage = "Vul een wachtwoord in.")]
        [Display(Name = "Wachtwoord")]
        public string? Password { get; set; }

        public string ReturnUrl { get; set; } = "/";
    }
}