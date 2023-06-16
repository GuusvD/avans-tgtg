namespace Core.Domain.Tests
{
    public class CanteenEmployeeTest
    {
        [Fact]
        public void Given_No_Employee_Id_Should_Return_Exception()
        {
            //Arrange
            CanteenEmployee CanteenEmployee = new CanteenEmployee()
            {
                EmployeeId = null,
                Name = "John Doe",
                Location = LocationCanteenEnum.LA
            };

            //Act
            bool Result = ValidateModel(CanteenEmployee).Any(p => p.ErrorMessage == "Vul een personeelsnummer in.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_Name_Should_Return_Exception()
        {
            //Arrange
            CanteenEmployee CanteenEmployee = new CanteenEmployee()
            {
                EmployeeId = "1234",
                Name = null,
                Location = LocationCanteenEnum.LA
            };

            //Act
            bool Result = ValidateModel(CanteenEmployee).Any(p => p.ErrorMessage == "Vul een naam in.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_Location_Should_Return_Exception()
        {
            //Arrange
            CanteenEmployee CanteenEmployee = new CanteenEmployee()
            {
                EmployeeId = "1234",
                Name = "John Doe",
                Location = null
            };

            //Act
            bool Result = ValidateModel(CanteenEmployee).Any(p => p.ErrorMessage == "Selecteer een locatie.");

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