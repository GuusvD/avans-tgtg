namespace Core.DomainServices.Tests
{
    public class ProductTest
    {
        [Fact]
        public void Given_Non_Existing_Id_Should_Return_Exception()
        {
            //Arrange
            var IProductRepository = new Mock<IProductRepository>();

            IProductService IProductService = new ProductService(IProductRepository.Object);

            //Act
            Exception Exception = Record.ExceptionAsync(() => IProductService.GetById(1)).Result;

            //Assert
            Assert.True(Exception.Message == "Het gegeven Product Id word niet gebruikt.");
        }

        [Fact]
        public void Given_Existing_Id_Should_Return_Product()
        {
            //Arrange
            var IProductRepository = new Mock<IProductRepository>();

            IProductRepository.Setup(Service => Service.GetByIdAsync(1)).ReturnsAsync(new Product() { Id = 1 });

            IProductService IProductService = new ProductService(IProductRepository.Object);

            //Act
            Product? Product = IProductService.GetById(1).Result;

            //Assert
            Assert.True(Product != null);
            Assert.True(Product!.Id == 1);
        }

        [Fact]
        public void Get_All_Should_Return_All_Products()
        {
            //Arrange
            var Product = new Product()
            {
                Id = 1
            };

            var SecondProduct = new Product()
            {
                Id = 2
            };

            var ThirdProduct = new Product()
            {
                Id = 3
            };

            var IProductRepository = new Mock<IProductRepository>();

            IProductRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Product>() { Product, SecondProduct, ThirdProduct });

            IProductService IProductService = new ProductService(IProductRepository.Object);

            //Act
            ICollection<Product> Result = IProductService.GetAll().Result;

            //Assert
            Assert.True(Result.Count == 3);
            Assert.True(Result.FirstOrDefault()!.Id == Product.Id);
        }

        [Fact]
        public void Get_All_Should_Return_No_Products()
        {
            //Arrange
            var IProductRepository = new Mock<IProductRepository>();

            IProductRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Product>());

            IProductService IProductService = new ProductService(IProductRepository.Object);

            //Act
            ICollection<Product> Result = IProductService.GetAll().Result;

            //Assert
            Assert.Empty(Result);
        }
    }
}
