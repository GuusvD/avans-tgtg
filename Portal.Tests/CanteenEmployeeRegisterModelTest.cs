namespace Portal.Tests
{
    public class CanteenEmployeeRegisterModelTest
    {
        [Fact]
        public void Given_No_Employee_Id_Should_Return_Exception()
        {
            //Arrange
            CanteenEmployeeRegisterModel CanteenEmployeeRegisterModel = new CanteenEmployeeRegisterModel()
            {
                EmployeeId = null,
                Name = "John Doe",
                Location = LocationCanteenEnum.LA
            };

            //Act
            bool Result = ValidateModel(CanteenEmployeeRegisterModel).Any(p => p.ErrorMessage == "Vul een personeelsnummer in.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_Name_Should_Return_Exception()
        {
            //Arrange
            CanteenEmployeeRegisterModel CanteenEmployeeRegisterModel = new CanteenEmployeeRegisterModel()
            {
                EmployeeId = "1234",
                Name = null,
                Location = LocationCanteenEnum.LA
            };

            //Act
            bool Result = ValidateModel(CanteenEmployeeRegisterModel).Any(p => p.ErrorMessage == "Vul een naam in.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_Location_Should_Return_Exception()
        {
            //Arrange
            CanteenEmployeeRegisterModel CanteenEmployeeRegisterModel = new CanteenEmployeeRegisterModel()
            {
                EmployeeId = "1234",
                Name = "John Doe",
                Location = null
            };

            //Act
            bool Result = ValidateModel(CanteenEmployeeRegisterModel).Any(p => p.ErrorMessage == "Vul een locatie in.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_Password_Should_Return_Exception()
        {
            //Arrange
            CanteenEmployeeRegisterModel CanteenEmployeeRegisterModel = new CanteenEmployeeRegisterModel()
            {
                EmployeeId = "1234",
                Name = "John Doe",
                Location = null,
                Password = null
            };

            //Act
            bool Result = ValidateModel(CanteenEmployeeRegisterModel).Any(p => p.ErrorMessage == "Vul een wachtwoord in.");

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
