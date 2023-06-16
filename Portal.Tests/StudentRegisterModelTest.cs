namespace Portal.Tests
{
    public class StudentRegisterModelTest
    {
        [Fact]
        public void Given_No_StudentId_Should_Return_Exception()
        {
            //Arrange
            StudentRegisterModel StudentRegisterModel = new StudentRegisterModel()
            {
                StudentId = null,
                Name = "John Doe",
                DateOfBirth = DateTime.Now.AddYears(-20),
                EmailAddress = "j.doe@gmail.com",
                City = CityEnum.Breda,
                PhoneNumber = "06 12345678"
            };

            //Act
            bool Result = ValidateModel(StudentRegisterModel).Any(p => p.ErrorMessage == "Vul een studentnummer in.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_Name_Should_Return_Exception()
        {
            //Arrange
            StudentRegisterModel StudentRegisterModel = new StudentRegisterModel()
            {
                StudentId = "1234",
                Name = null,
                DateOfBirth = DateTime.Now.AddYears(-20),
                EmailAddress = "j.doe@gmail.com",
                City = CityEnum.Breda,
                PhoneNumber = "06 12345678"
            };

            //Act
            bool Result = ValidateModel(StudentRegisterModel).Any(p => p.ErrorMessage == "Vul een naam in.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_Date_Of_Birth_Should_Return_Exception()
        {
            //Arrange
            StudentRegisterModel StudentRegisterModel = new StudentRegisterModel()
            {
                StudentId = "1234",
                Name = "John Doe",
                DateOfBirth = null,
                EmailAddress = "j.doe@gmail.com",
                City = CityEnum.Breda,
                PhoneNumber = "06 12345678"
            };

            //Act
            bool Result = ValidateModel(StudentRegisterModel).Any(p => p.ErrorMessage == "Vul een geboortedatum in.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_Email_Should_Return_Exception()
        {
            //Arrange
            StudentRegisterModel StudentRegisterModel = new StudentRegisterModel()
            {
                StudentId = "1234",
                Name = "John Doe",
                DateOfBirth = DateTime.Now.AddYears(-20),
                EmailAddress = null,
                City = CityEnum.Breda,
                PhoneNumber = "06 12345678"
            };

            //Act
            bool Result = ValidateModel(StudentRegisterModel).Any(p => p.ErrorMessage == "Vul een emailadres in.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_Invalid_Email_Should_Return_Exception()
        {
            //Arrange
            StudentRegisterModel StudentRegisterModel = new StudentRegisterModel()
            {
                StudentId = "1234",
                Name = "John Doe",
                DateOfBirth = DateTime.Now.AddYears(-20),
                EmailAddress = "hi",
                City = CityEnum.Breda,
                PhoneNumber = "06 12345678"
            };

            //Act
            bool Result = ValidateModel(StudentRegisterModel).Any(p => p.ErrorMessage == "The Emailadres field is not a valid e-mail address.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_City_Should_Return_Exception()
        {
            //Arrange
            StudentRegisterModel StudentRegisterModel = new StudentRegisterModel()
            {
                StudentId = "1234",
                Name = "John Doe",
                DateOfBirth = DateTime.Now,
                EmailAddress = "j.doe@gmail.com",
                City = null,
                PhoneNumber = "06 12345678"
            };

            //Act
            bool Result = ValidateModel(StudentRegisterModel).Any(p => p.ErrorMessage == "Selecteer een stad.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_PhoneNumber_Should_Return_Exception()
        {
            //Arrange
            StudentRegisterModel StudentRegisterModel = new StudentRegisterModel()
            {
                StudentId = "1234",
                Name = "John Doe",
                DateOfBirth = DateTime.Now,
                EmailAddress = "j.doe@gmail.com",
                City = CityEnum.Breda,
                PhoneNumber = null
            };

            //Act
            bool Result = ValidateModel(StudentRegisterModel).Any(p => p.ErrorMessage == "Vul een telefoonnummer in.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_Password_Should_Return_Exception()
        {
            //Arrange
            StudentRegisterModel StudentRegisterModel = new StudentRegisterModel()
            {
                StudentId = "1234",
                Name = "John Doe",
                DateOfBirth = DateTime.Now,
                EmailAddress = "j.doe@gmail.com",
                City = CityEnum.Breda,
                PhoneNumber = "06 12345678",
                Password = null
            };

            //Act
            bool Result = ValidateModel(StudentRegisterModel).Any(p => p.ErrorMessage == "Vul een wachtwoord in.");

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
