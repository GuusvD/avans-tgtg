namespace Core.Domain.Tests
{
    public class CanteenTest
    {
        [Fact]
        public void Given_No_City_Should_Return_Exception()
        {
            //Arrange
            Canteen Canteen = new Canteen()
            {
                City = null,
                Location = LocationCanteenEnum.LA,
                WarmMeals = true
            };

            //Act
            bool Result = ValidateModel(Canteen).Any(p => p.ErrorMessage == "Selecteer een stad.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_Location_Should_Return_Exception()
        {
            //Arrange
            Canteen Canteen = new Canteen()
            {
                City = CityEnum.Breda,
                Location = null,
                WarmMeals = true
            };

            //Act
            bool Result = ValidateModel(Canteen).Any(p => p.ErrorMessage == "Selecteer een locatie.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_WarmMeals_Should_Return_Exception()
        {
            //Arrange
            Canteen Canteen = new Canteen()
            {
                City = CityEnum.Breda,
                Location = LocationCanteenEnum.LA,
                WarmMeals = null
            };

            //Act
            bool Result = ValidateModel(Canteen).Any(p => p.ErrorMessage == "Geef aan of warme maaltijden zijn toegestaan.");

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
