namespace Core.DomainServices.Tests
{
    public class StudentTest
    {
        [Fact]
        public void Given_Id_Should_Return_Correct_Student()
        {
            //Arrange
            var Student = new Student()
            {
                StudentId = "1"
            };

            var IStudentRepository = new Mock<IStudentRepository>();
            IStudentRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Student>() { Student });

            IStudentService IStudentService = new StudentService(IStudentRepository.Object);

            //Act
            Student Result = IStudentService.GetById("1")!;

            //Assert
            Assert.True(Result.StudentId == Student.StudentId);
        }

        [Fact]
        public void Given_Non_Existing_Id_Should_Return_No_Student()
        {
            //Arrange
            var Student = new Student()
            {
                StudentId = "1"
            };

            var IStudentRepository = new Mock<IStudentRepository>();
            IStudentRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Student>() { Student });

            IStudentService IStudentService = new StudentService(IStudentRepository.Object);

            //Act
            Student Result = IStudentService.GetById("2")!;

            //Assert
            Assert.True(Result == null);
        }

        [Fact]
        public void Given_Add_Null_Should_Return_Student_Object_Is_Null_Exception()
        {
            //Arrange
            var IStudentRepository = new Mock<IStudentRepository>();
            IStudentService IStudentService = new StudentService(IStudentRepository.Object);

            //Act
            Exception Exception = Record.ExceptionAsync(() => IStudentService.Add(null!)).Result;
            
            //Assert
            Assert.True(Exception.Message == "Het Student object is null.");
        }

        [Fact]
        public void Given_Add_Student_Should_Return_True()
        {
            //Arrange
            var IStudentRepository = new Mock<IStudentRepository>();
            IStudentService IStudentService = new StudentService(IStudentRepository.Object);

            //Act
            bool Result = IStudentService.Add(new Student()).Result;

            //Assert
            Assert.True(Result);
        }
    }
}
