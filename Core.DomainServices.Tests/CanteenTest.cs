namespace Core.DomainServices.Tests
{
    public class CanteenTest
    {
        [Fact]
        public void Get_By_Location_Should_Return_Canteen_With_That_Location()
        {
            //Arrange
            var Canteen = new Canteen()
            {
                Id = 1,
                Location = LocationCanteenEnum.LA
            };

            var ICanteenRepository = new Mock<ICanteenRepository>();
            ICanteenRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Canteen>() { Canteen });

            ICanteenService ICanteenService = new CanteenService(ICanteenRepository.Object);

            //Act
            Canteen Result = ICanteenService.GetByLocation(LocationCanteenEnum.LA).Result;

            //Assert
            Assert.True(Canteen.Id == Result.Id);
        }

        [Fact]
        public void Get_By_Location_Should_Return_Zero_Canteens_With_That_Location()
        {
            //Arrange
            var Canteen = new Canteen()
            {
                Id = 1,
                Location = LocationCanteenEnum.LA
            };

            var ICanteenRepository = new Mock<ICanteenRepository>();
            ICanteenRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Canteen>() { Canteen });

            ICanteenService ICanteenService = new CanteenService(ICanteenRepository.Object);

            //Act
            Canteen Result = ICanteenService.GetByLocation(LocationCanteenEnum.HD).Result;

            //Assert
            Assert.True(Result == null);
        }
    }
}
