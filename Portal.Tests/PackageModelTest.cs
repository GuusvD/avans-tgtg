namespace Portal.Tests
{
    public class PackageModelTest
    {
        [Fact]
        public void Given_No_Name_Should_Return_Exception()
        {
            //Arrange
            PackageModel PackageModel = new PackageModel()
            {
                Name = null,
                PickupTime = DateTime.Now.AddMinutes(5),
                LastPickupTime = DateTime.Now.AddMinutes(10),
                Price = 3,
                MealType = MealTypeEnum.Bread,
                SelectedProducts = new List<int>() { 1, 2 }
            };

            //Act
            bool Result = ValidateModel(PackageModel).Any(p => p.ErrorMessage == "Vul een naam in.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_PickupTime_Should_Return_Exception()
        {
            //Arrange
            PackageModel PackageModel = new PackageModel()
            {
                Name = "Pakket 1",
                PickupTime = null,
                LastPickupTime = DateTime.Now.AddMinutes(10),
                Price = 3,
                MealType = MealTypeEnum.Bread,
                SelectedProducts = new List<int>() { 1, 2 }
            };

            //Act
            bool Result = ValidateModel(PackageModel).Any(p => p.ErrorMessage == "Vul een ophaaldatum in.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_LastPickupTime_Should_Return_Exception()
        {
            //Arrange
            PackageModel PackageModel = new PackageModel()
            {
                Name = "Pakket 1",
                PickupTime = DateTime.Now.AddMinutes(5),
                LastPickupTime = null,
                Price = 3,
                MealType = MealTypeEnum.Bread,
                SelectedProducts = new List<int>() { 1, 2 }
            };

            //Act
            bool Result = ValidateModel(PackageModel).Any(p => p.ErrorMessage == "Vul een laatste ophaaldatum in.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_Price_Should_Return_Exception()
        {
            //Arrange
            PackageModel PackageModel = new PackageModel()
            {
                Name = "Pakket 1",
                PickupTime = DateTime.Now.AddMinutes(5),
                LastPickupTime = DateTime.Now.AddMinutes(10),
                Price = null,
                MealType = MealTypeEnum.Bread,
                SelectedProducts = new List<int>() { 1, 2 }
            };

            //Act
            bool Result = ValidateModel(PackageModel).Any(p => p.ErrorMessage == "Vul een prijs in.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_MealType_Should_Return_Exception()
        {
            //Arrange
            PackageModel PackageModel = new PackageModel()
            {
                Name = "Pakket 1",
                PickupTime = DateTime.Now.AddMinutes(5),
                LastPickupTime = DateTime.Now.AddMinutes(10),
                Price = 3,
                MealType = null,
                SelectedProducts = new List<int>() { 1, 2 }
            };

            //Act
            bool Result = ValidateModel(PackageModel).Any(p => p.ErrorMessage == "Selecteer een maaltijdtype.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_SelectedProducts_Should_Return_Exception()
        {
            //Arrange
            PackageModel PackageModel = new PackageModel()
            {
                Name = "Pakket 1",
                PickupTime = DateTime.Now.AddMinutes(5),
                LastPickupTime = DateTime.Now.AddMinutes(10),
                Price = 3,
                MealType = MealTypeEnum.Bread,
                SelectedProducts = null
            };

            //Act
            bool Result = ValidateModel(PackageModel).Any(p => p.ErrorMessage == "Selecteer producten.");

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
