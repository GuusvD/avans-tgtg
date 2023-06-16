namespace Portal.Tests
{
    public class AccountTest
    {
        [Fact]
        public void Given_Student_With_Invalid_Date_Of_Birth_Should_Return_Exception()
        {
            //Arrange
            var StudentRegisterModel = new StudentRegisterModel()
            {
                DateOfBirth = DateTime.Now
            };

            var AccountController = new AccountController(null!, null!, null!, null!);

            //Act
            AccountController.RegisterStudent(StudentRegisterModel).Wait();

            //Assert
            Assert.True(AccountController.ViewData.ModelState.Count == 1);
            Assert.True(AccountController.ViewData.ModelState.ContainsKey("DateOfBirth"));
            Assert.False(AccountController.ModelState.IsValid);
        }
    }
}