namespace Core.Domain.Tests
{
    public class ProductTest
    {
        [Fact]
        public void Given_No_Name_Should_Return_Exception()
        {
            //Arrange
            Product Product = new Product()
            {
                Name = null,
                Alcoholic = true,
                Image = new byte[] { }
            };

            //Act
            bool Result = ValidateModel(Product).Any(p => p.ErrorMessage == "Vul een naam in.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_Alcoholic_Should_Return_Exception()
        {
            //Arrange
            Product Product = new Product()
            {
                Name = "Product 1",
                Alcoholic = null,
                Image = new byte[] { }
            };

            //Act
            bool Result = ValidateModel(Product).Any(p => p.ErrorMessage == "Geef aan of het product alcohol bevat.");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_No_Image_Should_Return_Exception()
        {
            //Arrange
            Product Product = new Product()
            {
                Name = "Product 1",
                Alcoholic = true,
                Image = null
            };

            //Act
            bool Result = ValidateModel(Product).Any(p => p.ErrorMessage == "Geef een afbeelding.");

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
