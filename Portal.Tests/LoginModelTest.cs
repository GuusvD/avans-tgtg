namespace Portal.Tests
{
    public class LoginModelTest
    {
        [Fact]
        public void Given_No_Id_Should_Return_Exception()
        {
            //Arrange
            LoginModel LoginModel = new LoginModel()
            {
                UserId = null,
                Password = "1234"
            };

            //Act
            bool Result = ValidateModel(LoginModel).Any(p => p.ErrorMessage == "Vul een student/personeelsnummer in.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_Password_Should_Return_Exception()
        {
            //Arrange
            LoginModel LoginModel = new LoginModel()
            {
                UserId = "1234",
                Password = null
            };

            //Act
            bool Result = ValidateModel(LoginModel).Any(p => p.ErrorMessage == "Vul een wachtwoord in.");

            //Assert
            Assert.True(Result);
        }

        private IList<ValidationResult> ValidateModel(object Obj)
        {
            var ValidationResults = new List<ValidationResult>();
            var ValidationContext = new ValidationContext(Obj, null, null);
            Validator.TryValidateObject(Obj, ValidationContext, ValidationResults, true);
            return ValidationResults;
        }
    }
}
