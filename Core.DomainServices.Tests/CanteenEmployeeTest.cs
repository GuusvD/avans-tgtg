namespace Core.DomainServices.Tests
{
    public class CanteenEmployeeTest
    {
        [Fact]
        public void Given_Id_Should_Return_Correct_User()
        {
            //Arrange
            var CanteenEmployee = new CanteenEmployee()
            {
                EmployeeId = "1"
            };

            var ICanteenEmployeeRepository = new Mock<ICanteenEmployeeRepository>();
            ICanteenEmployeeRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<CanteenEmployee>() { CanteenEmployee });

            ICanteenEmployeeService ICanteenEmployeeService = new CanteenEmployeeService(ICanteenEmployeeRepository.Object);

            //Act
            CanteenEmployee Result = ICanteenEmployeeService.GetById("1")!;

            //Assert
            Assert.True(Result.EmployeeId == CanteenEmployee.EmployeeId);
        }

        [Fact]
        public void Given_Non_Existing_Id_Should_Return_No_User()
        {
            //Arrange
            var CanteenEmployee = new CanteenEmployee()
            {
                EmployeeId = "1"
            };

            var ICanteenEmployeeRepository = new Mock<ICanteenEmployeeRepository>();
            ICanteenEmployeeRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<CanteenEmployee>() { CanteenEmployee });

            ICanteenEmployeeService ICanteenEmployeeService = new CanteenEmployeeService(ICanteenEmployeeRepository.Object);

            //Act
            CanteenEmployee Result = ICanteenEmployeeService.GetById("2")!;

            //Assert
            Assert.True(Result == null);
        }

        [Fact]
        public void Given_Add_Null_Should_Return_Employee_Object_Is_Null_Exception()
        {
            //Arrange
            var ICanteenEmployeeRepository = new Mock<ICanteenEmployeeRepository>();
            ICanteenEmployeeService ICanteenEmployeeService = new CanteenEmployeeService(ICanteenEmployeeRepository.Object);

            //Act
            Exception Exception = Record.ExceptionAsync(() => ICanteenEmployeeService.Add(null!)).Result;

            //Assert
            Assert.True(Exception.Message == "Het CanteenEmployee object is null.");
        }

        [Fact]
        public void Given_Add_Employee_Should_Return_True()
        {
            //Arrange
            var ICanteenEmployeeRepository = new Mock<ICanteenEmployeeRepository>();
            ICanteenEmployeeService ICanteenEmployeeService = new CanteenEmployeeService(ICanteenEmployeeRepository.Object);

            //Act
            bool Result = ICanteenEmployeeService.Add(new CanteenEmployee()).Result;

            //Assert
            Assert.True(Result);
        }
    }
}