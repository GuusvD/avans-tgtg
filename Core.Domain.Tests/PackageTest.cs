namespace Core.Domain.Tests
{
    public class PackageTest
    {
        [Fact]
        public void Given_No_Name_Should_Return_Exception()
        {
            //Arrange
            Package Package = new Package()
            {
                Name = null,
                Products = new List<Product>() { new Product() { Name = "Product 1" } },
                Canteen = new Canteen() { Id = 1 },
                PickupTime = DateTime.Now,
                LastPickupTime = DateTime.Now,
                ForAdults = true,
                Price = 1,
                MealType = MealTypeEnum.Bread,
                Student = new Student() { Name = "John Doe" }
            };

            //Act
            bool Result = ValidateModel(Package).Any(p => p.ErrorMessage == "Vul een naam in.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_Products_Should_Return_Exception()
        {
            //Arrange
            Package Package = new Package()
            {
                Name = "Pakket 1",
                Products = null,
                Canteen = new Canteen() { Id = 1 },
                PickupTime = DateTime.Now,
                LastPickupTime = DateTime.Now,
                ForAdults = true,
                Price = 1,
                MealType = MealTypeEnum.Bread,
                Student = new Student() { Name = "John Doe" }
            };

            //Act
            bool Result = ValidateModel(Package).Any(p => p.ErrorMessage == "Selecteer producten.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_Canteen_Should_Return_Exception()
        {
            //Arrange
            Package Package = new Package()
            {
                Name = "Pakket 1",
                Products = new List<Product>() { new Product() { Name = "Product 1" } },
                Canteen = null,
                PickupTime = DateTime.Now,
                LastPickupTime = DateTime.Now,
                ForAdults = true,
                Price = 1,
                MealType = MealTypeEnum.Bread,
                Student = new Student() { Name = "John Doe" }
            };

            //Act
            bool Result = ValidateModel(Package).Any(p => p.ErrorMessage == "Selecteer een kantine.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_PickupTime_Should_Return_Exception()
        {
            //Arrange
            Package Package = new Package()
            {
                Name = "Pakket 1",
                Products = new List<Product>() { new Product() { Name = "Product 1" } },
                Canteen = new Canteen() { Id = 1 },
                PickupTime = null,
                LastPickupTime = DateTime.Now,
                ForAdults = true,
                Price = 1,
                MealType = MealTypeEnum.Bread,
                Student = new Student() { Name = "John Doe" }
            };

            //Act
            bool Result = ValidateModel(Package).Any(p => p.ErrorMessage == "Vul een ophaaldatum in.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_LastPickupTime_Should_Return_Exception()
        {
            //Arrange
            Package Package = new Package()
            {
                Name = "Pakket 1",
                Products = new List<Product>() { new Product() { Name = "Product 1" } },
                Canteen = new Canteen() { Id = 1 },
                PickupTime = DateTime.Now,
                LastPickupTime = null,
                ForAdults = true,
                Price = 1,
                MealType = MealTypeEnum.Bread,
                Student = new Student() { Name = "John Doe" }
            };

            //Act
            bool Result = ValidateModel(Package).Any(p => p.ErrorMessage == "Vul een laatste ophaaldatum in.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_ForAdults_Should_Return_Exception()
        {
            //Arrange
            Package Package = new Package()
            {
                Name = "Pakket 1",
                Products = new List<Product>() { new Product() { Name = "Product 1" } },
                Canteen = new Canteen() { Id = 1 },
                PickupTime = DateTime.Now,
                LastPickupTime = DateTime.Now,
                ForAdults = null,
                Price = 1,
                MealType = MealTypeEnum.Bread,
                Student = new Student() { Name = "John Doe" }
            };

            //Act
            bool Result = ValidateModel(Package).Any(p => p.ErrorMessage == "Geef aan of het pakket 18+ is.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_Price_Should_Return_Exception()
        {
            //Arrange
            Package Package = new Package()
            {
                Name = "Pakket 1",
                Products = new List<Product>() { new Product() { Name = "Product 1" } },
                Canteen = new Canteen() { Id = 1 },
                PickupTime = DateTime.Now,
                LastPickupTime = DateTime.Now,
                ForAdults = true,
                Price = null,
                MealType = MealTypeEnum.Bread,
                Student = new Student() { Name = "John Doe" }
            };

            //Act
            bool Result = ValidateModel(Package).Any(p => p.ErrorMessage == "Vul een prijs in.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_MealType_Should_Return_Exception()
        {
            //Arrange
            Package Package = new Package()
            {
                Name = "Pakket 1",
                Products = new List<Product>() { new Product() { Name = "Product 1" } },
                Canteen = new Canteen() { Id = 1 },
                PickupTime = DateTime.Now,
                LastPickupTime = DateTime.Now,
                ForAdults = true,
                Price = 1,
                MealType = null,
                Student = new Student() { Name = "John Doe" }
            };

            //Act
            bool Result = ValidateModel(Package).Any(p => p.ErrorMessage == "Selecteer een maaltijdtype.");

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
