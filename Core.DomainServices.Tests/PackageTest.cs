namespace Core.DomainServices.Tests
{
    public class PackageTest
    {
        /*
         * EXCEPTION TESTS
        */

        [Fact]
        public void Given_Wrong_PickupTime_Should_Return_Future_Exception()
        {
            //Arrange
            var Package = new Package()
            {
                PickupTime = DateTime.Now.AddDays(-1)
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            Exception Exception = Record.ExceptionAsync(() => IPackageService.Add(Package, null!, null!)).Result;

            //Assert
            Assert.True(Exception.Message == "De ophaaltijd moet in de toekomst liggen!");
        }

        [Fact]
        public void Given_Wrong_PickupTime_Should_Return_More_Then_Two_Days_Future_Exception()
        {
            //Arrange
            var Package = new Package()
            {
                PickupTime = DateTime.Now.AddDays(3)
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            Exception Exception = Record.ExceptionAsync(() => IPackageService.Add(Package, null!, null!)).Result;

            //Assert
            Assert.True(Exception.Message == "De ophaaltijd mag niet meer dan twee dagen in de toekomst liggen!");
        }

        [Fact]
        public void Given_Wrong_LastPickupTime_Should_Return_Future_Exception()
        {
            //Arrange
            var Package = new Package()
            {
                PickupTime = DateTime.Now.AddDays(1),
                LastPickupTime = DateTime.Now.AddDays(-1)
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            Exception Exception = Record.ExceptionAsync(() => IPackageService.Add(Package, null!, null!)).Result;

            //Assert
            Assert.True(Exception.Message == "De laatste ophaaltijd moet in de toekomst liggen!");
        }

        [Fact]
        public void Given_Wrong_LastPickupTime_Should_Return_LastPickupTime_Must_Be_After_PickupTime_Exception()
        {
            //Arrange
            var Package = new Package()
            {
                PickupTime = DateTime.Now.AddMinutes(10),
                LastPickupTime = DateTime.Now.AddMinutes(5)
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            Exception Exception = Record.ExceptionAsync(() => IPackageService.Add(Package, null!, null!)).Result;

            //Assert
            Assert.True(Exception.Message == "De laatste ophaaltijd moet na de afhaaltijd liggen!");
        }

        [Fact]
        public void Given_Wrong_LastPickupTime_Should_Return_LastPickupTime_Must_Be_On_PickupTime_Day_Exception()
        {
            //Arrange
            var Package = new Package()
            {
                PickupTime = DateTime.Now.AddDays(1),
                LastPickupTime = DateTime.Now.AddDays(2)
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            Exception Exception = Record.ExceptionAsync(() => IPackageService.Add(Package, null!, null!)).Result;

            //Assert
            Assert.True(Exception.Message == "De laatste ophaaltijd moet op dezelfde dag als de afhaaltijd plaatsvinden.");
        }

        [Fact]
        public void Given_WarmMeal_MealType_Should_Return_WarmMeals_Not_Allowed_Exception()
        {
            //Arrange
            var Canteen = new Canteen()
            {
                Location = LocationCanteenEnum.LA,
                WarmMeals = false
            };

            var CanteenEmployee = new CanteenEmployee()
            {
                EmployeeId = "1",
                Location = LocationCanteenEnum.LA
            };

            var Package = new Package()
            {
                MealType = MealTypeEnum.WarmDinner,
                PickupTime = DateTime.Now.AddMinutes(5),
                LastPickupTime = DateTime.Now.AddMinutes(10)
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            ICanteenEmployeeService.Setup(Service => Service.GetById("1")).Returns(CanteenEmployee);
            ICanteenService.Setup(Service => Service.GetByLocation(LocationCanteenEnum.LA)).ReturnsAsync(Canteen);

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            Exception Exception = Record.ExceptionAsync(() => IPackageService.Add(Package, new List<int> { 1, 2 }, "1")).Result;

            //Assert
            Assert.True(Exception.Message == "Warme maaltijden zijn niet toegestaan op jouw locatie!");
        }

        [Fact]
        public void Given_Package_That_Is_Already_Reserved_Should_Return_Already_Reserved_Exception()
        {
            //Arrange
            var Student = new Student()
            {
                StudentId = "2",
                Name = "John Doe"
            };

            var Package = new Package()
            {
                Id = 1,
                Student = Student,
                PickupTime = DateTime.Now.AddDays(-1)
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);
            IStudentService.Setup(Service => Service.GetById("2")).Returns(Student);
            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package });

            //Act
            Exception Exception = Record.Exception(() => IPackageService.AssignStudent(1, "2"));

            //Assert
            Assert.True(Exception.Message == "Het pakket is al gereserveerd door iemand anders!");
        }

        [Fact]
        public void Given_Package_With_Same_PickupTime_Should_Return_Already_Reserved_On_That_Day_Exception()
        {
            //Arrange
            var Student = new Student()
            {
                StudentId = "2",
                Name = "John Doe"
            };

            var Package = new Package()
            {
                Id = 1,
                Student = Student,
                PickupTime = DateTime.Now.AddDays(-1)
            };

            var SecondPackage = new Package()
            {
                Id = 2,
                PickupTime = DateTime.Now.AddDays(-1)
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);
            IStudentService.Setup(Service => Service.GetById("2")).Returns(Student);
            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package, SecondPackage });

            //Act
            Exception Exception = Record.Exception(() => IPackageService.AssignStudent(2, "2"));

            //Assert
            Assert.True(Exception.Message == "Je hebt al een pakket gereserveerd op de afhaaldag van dit pakket!");
        }

        [Fact]
        public void Given_Package_With_Alcoholic_Product_Should_Return_Only_Adults_Exception()
        {
            //Arrange
            var Student = new Student()
            {
                StudentId = "2",
                Name = "John Doe",
                DateOfBirth = DateTime.Now
            };

            var Package = new Package()
            {
                Id = 1,
                PickupTime = DateTime.Now.AddMinutes(5),
                ForAdults = true,
                Products = new List<Product>() {
                    new Product()
                    {
                        Alcoholic = true
                    }
                }
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);
            IStudentService.Setup(Service => Service.GetById("2")).Returns(Student);
            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package });

            //Act
            Exception Exception = Record.Exception(() => IPackageService.AssignStudent(1, "2"));

            //Assert
            Assert.True(Exception.Message == "Alleen volwassenen mogen dit pakket reserveren!");
        }

        [Fact]
        public void Given_Package_Which_Does_Not_Belong_To_User_Canteen_Should_Return_Not_Your_Canteen_Exception()
        {
            //Arrange
            var Canteen = new Canteen()
            {
                Location = LocationCanteenEnum.LA
            };

            var Package = new Package()
            {
                Id = 1,
                Canteen = Canteen
            };

            var User = new CanteenEmployee()
            {
                EmployeeId = "2",
                Location = LocationCanteenEnum.LD
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package });
            ICanteenEmployeeService.Setup(Service => Service.GetById("2")).Returns(User);

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            Exception Exception = Record.Exception(() => IPackageService.Delete(1, "2"));

            //Assert
            Assert.True(Exception.Message == "Dit pakket behoort niet tot jouw kantine dus je mag het niet verwijderen!");
        }

        [Fact]
        public void Given_Package_With_Reservation_Should_Return_Can_Not_Delete_Exception()
        {
            //Arrange
            var Student = new Student()
            {
                StudentId = "3"
            };

            var Canteen = new Canteen()
            {
                Location = LocationCanteenEnum.LA
            };

            var Package = new Package()
            {
                Id = 1,
                Canteen = Canteen,
                Student = Student,
                LastPickupTime = DateTime.Now.AddMinutes(5)
            };

            var User = new CanteenEmployee()
            {
                EmployeeId = "2",
                Location = LocationCanteenEnum.LA
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package });
            ICanteenEmployeeService.Setup(Service => Service.GetById("2")).Returns(User);

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            Exception Exception = Record.Exception(() => IPackageService.Delete(1, "2"));

            //Assert
            Assert.True(Exception.Message == "Er is al een reservering voor dit pakket dus je kan het niet verwijderen.");
        }

        [Fact]
        public void Given_Update_Package_Which_Does_Not_Belong_To_User_Canteen_Should_Return_Not_Your_Canteen_Exception()
        {
            //Arrange
            var Package = new Package()
            {
                Id = 2,
                Canteen = new Canteen() {
                    Location = LocationCanteenEnum.LA
                }
            };

            var User = new CanteenEmployee()
            {
                EmployeeId = "1",
                Location = LocationCanteenEnum.LD
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            ICanteenEmployeeService.Setup(Service => Service.GetById("1")).Returns(User);
            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package });
            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            Exception Exception = Record.Exception(() => IPackageService.Update(Package, null!, 2, "1"));

            //Assert
            Assert.True(Exception.Message == "Dit pakket behoort niet tot jouw kantine dus je mag het niet bewerken!");
        }

        [Fact]
        public void Given_Wrong_Update_PickupTime_Should_Return_Future_Exception()
        {
            //Arrange
            var Package = new Package()
            {
                Id = 2,
                PickupTime = DateTime.Now.AddMinutes(-5),
                Canteen = new Canteen()
                {
                    Location = LocationCanteenEnum.LD
                }
            };

            var User = new CanteenEmployee()
            {
                EmployeeId = "1",
                Location = LocationCanteenEnum.LD
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            ICanteenEmployeeService.Setup(Service => Service.GetById("1")).Returns(User);
            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package });
            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            Exception Exception = Record.Exception(() => IPackageService.Update(Package, null!, 2, "1"));

            //Assert
            Assert.True(Exception.Message == "De ophaaltijd moet in de toekomst liggen!");
        }

        [Fact]
        public void Given_Wrong_Update_PickupTime_Should_Return_More_Then_Two_Days_Future_Exception()
        {
            //Arrange
            var Package = new Package()
            {
                Id = 2,
                PickupTime = DateTime.Now.AddDays(5),
                Canteen = new Canteen()
                {
                    Location = LocationCanteenEnum.LD
                }
            };

            var User = new CanteenEmployee()
            {
                EmployeeId = "1",
                Location = LocationCanteenEnum.LD
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            ICanteenEmployeeService.Setup(Service => Service.GetById("1")).Returns(User);
            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package });
            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            Exception Exception = Record.Exception(() => IPackageService.Update(Package, null!, 2, "1"));

            //Assert
            Assert.True(Exception.Message == "De ophaaltijd mag niet meer dan twee dagen in de toekomst liggen!");
        }

        [Fact]
        public void Given_Wrong_Update_LastPickupTime_Should_Return_Future_Exception()
        {
            //Arrange
            var Package = new Package()
            {
                Id = 2,
                PickupTime = DateTime.Now.AddMinutes(5),
                LastPickupTime = DateTime.Now.AddMinutes(-5),
                Canteen = new Canteen()
                {
                    Location = LocationCanteenEnum.LD
                }
            };

            var User = new CanteenEmployee()
            {
                EmployeeId = "1",
                Location = LocationCanteenEnum.LD
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            ICanteenEmployeeService.Setup(Service => Service.GetById("1")).Returns(User);
            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package });
            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            Exception Exception = Record.Exception(() => IPackageService.Update(Package, null!, 2, "1"));

            //Assert
            Assert.True(Exception.Message == "De laatste ophaaltijd moet in de toekomst liggen!");
        }

        [Fact]
        public void Given_Wrong_Update_LastPickupTime_Should_Return_LastPickupTime_Must_Be_After_PickupTime_Exception()
        {
            //Arrange
            var Package = new Package()
            {
                Id = 2,
                PickupTime = DateTime.Now.AddMinutes(10),
                LastPickupTime = DateTime.Now.AddMinutes(5),
                Canteen = new Canteen()
                {
                    Location = LocationCanteenEnum.LD
                }
            };

            var User = new CanteenEmployee()
            {
                EmployeeId = "1",
                Location = LocationCanteenEnum.LD
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            ICanteenEmployeeService.Setup(Service => Service.GetById("1")).Returns(User);
            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package });
            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            Exception Exception = Record.Exception(() => IPackageService.Update(Package, null!, 2, "1"));

            //Assert
            Assert.True(Exception.Message == "De laatste ophaaltijd moet na de afhaaltijd liggen!");
        }

        [Fact]
        public void Given_Wrong_Update_LastPickupTime_Should_Return_LastPickupTime_Must_Be_On_PickupTime_Day_Exception()
        {
            //Arrange
            var Package = new Package()
            {
                Id = 2,
                PickupTime = DateTime.Now.AddMinutes(5),
                LastPickupTime = DateTime.Now.AddDays(1),
                Canteen = new Canteen()
                {
                    Location = LocationCanteenEnum.LD
                }
            };

            var User = new CanteenEmployee()
            {
                EmployeeId = "1",
                Location = LocationCanteenEnum.LD
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            ICanteenEmployeeService.Setup(Service => Service.GetById("1")).Returns(User);
            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package });
            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            Exception Exception = Record.Exception(() => IPackageService.Update(Package, null!, 2, "1"));

            //Assert
            Assert.True(Exception.Message == "De laatste ophaaltijd moet op dezelfde dag als de afhaaltijd plaatsvinden.");
        }

        [Fact]
        public void Given_Update_WarmMeal_MealType_Should_Return_WarmMeals_Not_Allowed_Exception()
        {
            //Arrange
            var Canteen = new Canteen()
            {
                Location = LocationCanteenEnum.LD,
                WarmMeals = false
            };

            var Package = new Package()
            {
                Id = 2,
                PickupTime = DateTime.Now.AddMinutes(5),
                LastPickupTime = DateTime.Now.AddMinutes(10),
                MealType = MealTypeEnum.WarmDinner,
                Canteen = Canteen
            };

            var User = new CanteenEmployee()
            {
                EmployeeId = "1",
                Location = LocationCanteenEnum.LD
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            ICanteenService.Setup(Service => Service.GetByLocation(LocationCanteenEnum.LD)).ReturnsAsync(Canteen);
            ICanteenEmployeeService.Setup(Service => Service.GetById("1")).Returns(User);
            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package });
            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            Exception Exception = Record.Exception(() => IPackageService.Update(Package, new List<int>() { 1, 2 }, 2, "1"));

            //Assert
            Assert.True(Exception.Message == "Warme maaltijden zijn niet toegestaan op jouw locatie!");
        }

        [Fact]
        public void Given_Update_Package_With_Reservation_Should_Return_Can_Not_Update_Exception()
        {
            //Arrange
            var User = new CanteenEmployee()
            {
                EmployeeId = "2"
            };

            var Package = new Package()
            {
                Id = 1,
                Student = new Student()
                {
                    Name = "John Doe"
                },
                LastPickupTime = DateTime.Now.AddMinutes(5)
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            ICanteenEmployeeService.Setup(Service => Service.GetById("2")).Returns(User);
            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package });
            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            Exception Exception = Record.Exception(() => IPackageService.UpdateGetValidate(1, "2"));

            //Assert
            Assert.True(Exception.Message == "Er is al een reservering voor dit pakket dus je kan het niet bewerken!");
        }

        [Fact]
        public void Given_Update_Package_Which_Does_Not_Belong_To_User_Canteen_Should_Return_Not_Your_Canteen_Exception_2()
        {
            //Arrange
            var User = new CanteenEmployee()
            {
                EmployeeId = "2",
                Location = LocationCanteenEnum.LA
            };

            var Package = new Package()
            {
                Id = 1,
                LastPickupTime = DateTime.Now.AddMinutes(5),
                Canteen = new Canteen()
                {
                    Location = LocationCanteenEnum.LD
                }
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            ICanteenEmployeeService.Setup(Service => Service.GetById("2")).Returns(User);
            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package });
            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            Exception Exception = Record.Exception(() => IPackageService.UpdateGetValidate(1, "2"));

            //Assert
            Assert.True(Exception.Message == "Dit pakket behoort niet tot jouw kantine dus je mag het niet bewerken!");
        }

        /*
         * METHODS TESTS
        */

        [Fact]
        public void Given_Id_Should_Return_Correct_Package()
        {
            //Arrange
            var Package = new Package()
            {
                Id = 1
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package });

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            Package Result = IPackageService.GetById(1)!;

            //Assert
            Assert.True(Result.Id == Package.Id);
        }

        [Fact]
        public void Given_Non_Existing_Id_Should_Return_No_Package()
        {
            //Arrange
            var Package = new Package()
            {
                Id = 1
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package });

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            Package Result = IPackageService.GetById(2)!;

            //Assert
            Assert.True(Result == null);
        }

        [Fact]
        public void Given_Student_Id_Should_Return_All_Packages_Belonging_To_That_Student()
        {
            //Arrange
            var Student = new Student()
            {
                StudentId = "1"
            };

            var Package = new Package()
            {
                Id = 1,
                Student = Student
            };

            var SecondPackage = new Package()
            {
                Id = 2
            };
            
            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package, SecondPackage });

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            ICollection<Package> Result = IPackageService.GetAllPackagesByStudent("1");

            //Assert
            Assert.True(Result.Count == 1);
            Assert.True(Result.FirstOrDefault()!.Id == Package.Id);
        }

        [Fact]
        public void Given_Non_Existing_Student_Id_Should_Return_No_Packages()
        {
            //Arrange
            var Student = new Student()
            {
                StudentId = "1"
            };

            var Package = new Package()
            {
                Id = 1,
                Student = Student
            };

            var SecondPackage = new Package()
            {
                Id = 2
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package, SecondPackage });

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            ICollection<Package> Result = IPackageService.GetAllPackagesByStudent("3");

            //Assert
            Assert.True(Result.Count == 0);
        }

        [Fact]
        public void Given_Canteen_Location_Should_Return_All_Packages_With_That_Location()
        {
            //Arrange
            var Package = new Package()
            {
                Id = 1,
                Canteen = new Canteen()
                {
                    Location = LocationCanteenEnum.LA
                },
                LastPickupTime = DateTime.Now.AddMinutes(5)
            };

            var SecondPackage = new Package()
            {
                Id = 2,
                Canteen = new Canteen()
                {
                    Location = LocationCanteenEnum.HD
                },
                LastPickupTime = DateTime.Now.AddMinutes(5)
            };
            
            var ThirdPackage = new Package()
            {
                Id = 3,
                Canteen = new Canteen()
                {
                    Location = LocationCanteenEnum.HD
                },
                LastPickupTime = DateTime.Now.AddMinutes(-5)
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package, SecondPackage, ThirdPackage });

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            ICollection<Package> Result = IPackageService.GetAllPackagesByCanteen(LocationCanteenEnum.HD);

            //Assert
            Assert.True(Result.Count == 1);
            Assert.True(Result.FirstOrDefault()!.Id == SecondPackage.Id);
        }

        [Fact]
        public void Given_Not_Used_Canteen_Location_Should_Return_No_Packages()
        {
            //Arrange
            var Package = new Package()
            {
                Id = 1,
                Canteen = new Canteen()
                {
                    Location = LocationCanteenEnum.LA
                }
            };

            var SecondPackage = new Package()
            {
                Id = 2,
                Canteen = new Canteen()
                {
                    Location = LocationCanteenEnum.HD
                }
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package, SecondPackage });

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            ICollection<Package> Result = IPackageService.GetAllPackagesByCanteen(LocationCanteenEnum.HA);

            //Assert
            Assert.True(Result.Count == 0);
        }

        [Fact]
        public void Get_All_Available_Packages_Should_Return_All_Packages_With_No_Reservation_And_A_Future_LastPickupTime()
        {
            //Arrange
            var Package = new Package()
            {
                Id = 1,
                Student = new Student()
                {
                    Name = "John Doe"
                }
            };

            var SecondPackage = new Package()
            {
                Id = 2,
                LastPickupTime = DateTime.Now.AddMinutes(-5)
            };

            var ThirdPackage = new Package()
            {
                Id = 3,
                Student = null,
                LastPickupTime = DateTime.Now.AddMinutes(5)
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package, SecondPackage, ThirdPackage });

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            ICollection<Package> Result = IPackageService.GetAllAvailablePackages();

            //Assert
            Assert.True(Result.Count == 1);
            Assert.True(Result.FirstOrDefault()!.Id == ThirdPackage.Id);
        }

        [Fact]
        public void Get_All_Available_Packages_Should_Return_No_Packages_With_No_Reservation_And_A_Future_LastPickupTime()
        {
            //Arrange
            var Package = new Package()
            {
                Id = 1,
                Student = new Student()
                {
                    Name = "John Doe"
                }
            };

            var SecondPackage = new Package()
            {
                Id = 2,
                LastPickupTime = DateTime.Now.AddMinutes(-5)
            };

            var ThirdPackage = new Package()
            {
                Id = 3,
                Student = null,
                LastPickupTime = DateTime.Now.AddMinutes(-10)
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package, SecondPackage, ThirdPackage });

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            ICollection<Package> Result = IPackageService.GetAllAvailablePackages();

            //Assert
            Assert.Empty(Result);
        }

        [Fact]
        public void Given_A_List_Of_Selected_Products_Should_Return_List_Of_Products()
        {
            //Arrange
            var Product = new Product()
            {
                Id = 1
            };

            var SecondProduct = new Product()
            {
                Id = 5
            };
            
            var ThirdProduct = new Product()
            {
                Id = 10
            };

            var FourthProduct = new Product()
            {
                Id = 15
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IProductService.Setup(Service => Service.GetById(1)).ReturnsAsync(Product);
            IProductService.Setup(Service => Service.GetById(5)).ReturnsAsync(SecondProduct);
            IProductService.Setup(Service => Service.GetById(10)).ReturnsAsync(ThirdProduct);
            IProductService.Setup(Service => Service.GetById(15)).ReturnsAsync(FourthProduct);

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            ICollection<Product> Result = IPackageService.SelectedProductsToProducts(new List<int>() { 1, 5, 15 });

            //Assert
            Assert.True(Result.Count == 3);
            Assert.True(Result.ToArray()[0].Id == Product.Id);
            Assert.True(Result.ToArray()[1].Id == SecondProduct.Id);
            Assert.True(Result.ToArray()[2].Id == FourthProduct.Id);
        }

        [Fact]
        public void Given_Empty_List_Of_Selected_Products_Should_Return_Empty_List_Of_Products()
        {
            //Arrange
            var Product = new Product()
            {
                Id = 1
            };

            var SecondProduct = new Product()
            {
                Id = 5
            };

            var ThirdProduct = new Product()
            {
                Id = 10
            };

            var FourthProduct = new Product()
            {
                Id = 15
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IProductService.Setup(Service => Service.GetById(1)).ReturnsAsync(Product);
            IProductService.Setup(Service => Service.GetById(5)).ReturnsAsync(SecondProduct);
            IProductService.Setup(Service => Service.GetById(10)).ReturnsAsync(ThirdProduct);
            IProductService.Setup(Service => Service.GetById(15)).ReturnsAsync(FourthProduct);

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            ICollection<Product> Result = IPackageService.SelectedProductsToProducts(new List<int>());

            //Assert
            Assert.Empty(Result);
        }

        [Fact]
        public void Given_List_Of_Products_And_A_Package_Should_Return_A_List_Of_Products_With_Filled_Package_Attribute()
        {
            //Arrange
            ICollection<Product> Products = new List<Product>() {
                new Product() { Id = 1, Package = new List<Package>() },
                new Product() { Id = 2, Package = new List<Package>() }
            };

            Package Package = new Package()
            {
                Id = 3
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            ICollection<Product> Result = IPackageService.ProductLinkPackage(Products, Package);

            //Assert
            Assert.True(Result.Count == 2);
            Assert.True(Result.FirstOrDefault()!.Package!.FirstOrDefault()!.Id == 3);
        }

        [Fact]
        public void Given_List_Of_Products_But_No_Package_Should_Return_A_List_Of_Products_With_Unfilled_Package_Attribute()
        {
            //Arrange
            ICollection<Product> Products = new List<Product>() {
                new Product() { Id = 1, Package = new List<Package>() },
                new Product() { Id = 2, Package = new List<Package>() }
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            ICollection<Product> Result = IPackageService.ProductLinkPackage(Products, null!);

            //Assert
            Assert.True(Result.Count == 2);
            Assert.True(Result.FirstOrDefault()!.Package!.FirstOrDefault() == null);
        }

        [Fact]
        public void Given_Empty_List_Of_Products_And_No_Package_Should_Return_A_Empty_Product_List()
        {
            //Arrange
            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            ICollection<Product> Result = IPackageService.ProductLinkPackage(new List<Product>(), null!);

            //Assert
            Assert.Empty(Result);
        }

        [Fact]
        public void Given_Correct_Package_To_Add_Should_Return_True()
        {
            //Arrange
            var Canteen = new Canteen()
            {
                Location = LocationCanteenEnum.LA,
                WarmMeals = true
            };

            var CanteenEmployee = new CanteenEmployee()
            {
                EmployeeId = "1",
                Location = LocationCanteenEnum.LA
            };

            var Product = new Product()
            {
                Id = 1,
                Alcoholic = true
            };

            var SecondProduct = new Product()
            {
                Id = 2,
                Alcoholic = false
            };

            var Package = new Package()
            {
                MealType = MealTypeEnum.WarmDinner,
                PickupTime = DateTime.Now.AddMinutes(5),
                LastPickupTime = DateTime.Now.AddMinutes(10)
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            ICanteenEmployeeService.Setup(Service => Service.GetById("1")).Returns(CanteenEmployee);
            ICanteenService.Setup(Service => Service.GetByLocation(LocationCanteenEnum.LA)).ReturnsAsync(Canteen);
            IProductService.Setup(Service => Service.GetById(1)).ReturnsAsync(Product);
            IProductService.Setup(Service => Service.GetById(2)).ReturnsAsync(SecondProduct);

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            bool Result = IPackageService.Add(Package, new List<int> { 1, 2 }, "1").Result;

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_Correct_Package_And_Student_To_Reserve_Should_Return_True()
        {
            //Arrange
            var Student = new Student()
            {
                StudentId = "2",
                Name = "John Doe",
                DateOfBirth = DateTime.Now.AddYears(-20)
            };

            var Package = new Package()
            {
                Id = 1,
                PickupTime = DateTime.Now.AddMinutes(5),
                ForAdults = true,
                Products = new List<Product>() {
                    new Product()
                    {
                        Alcoholic = true
                    }
                }
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);
            IStudentService.Setup(Service => Service.GetById("2")).Returns(Student);
            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package });

            //Act
            bool Result = IPackageService.AssignStudent(1, "2");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_Correct_Package_And_User_To_Delete_Should_Return_True()
        {
            //Arrange
            var Canteen = new Canteen()
            {
                Location = LocationCanteenEnum.LA
            };

            var Package = new Package()
            {
                Id = 1,
                Canteen = Canteen,
                LastPickupTime = DateTime.Now.AddMinutes(5),
            };

            var User = new CanteenEmployee()
            {
                EmployeeId = "2",
                Location = LocationCanteenEnum.LA
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package });
            ICanteenEmployeeService.Setup(Service => Service.GetById("2")).Returns(User);

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            bool Result = IPackageService.Delete(1, "2");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Get_All_Should_Return_All_Packages()
        {
            //Arrange
            var Package = new Package()
            {
                Id = 1
            };

            var SecondPackage = new Package()
            {
                Id = 2
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package, SecondPackage });

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            ICollection<Package> Result = IPackageService.GetAll().Result;

            //Assert
            Assert.True(Result.Count == 2);
            Assert.True(Result.FirstOrDefault()!.Id == Package.Id);
        }

        [Fact]
        public void Get_All_Should_Return_No_Packages()
        {
            //Arrange
            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>());

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            ICollection<Package> Result = IPackageService.GetAll().Result;

            //Assert
            Assert.Empty(Result);
        }

        [Fact]
        public void Given_Correct_Package_To_Update_Should_Return_True()
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

            var Canteen = new Canteen()
            {
                Location = LocationCanteenEnum.LD,
                WarmMeals = true
            };

            var Package = new Package()
            {
                Id = 2,
                PickupTime = DateTime.Now.AddMinutes(5),
                LastPickupTime = DateTime.Now.AddMinutes(10),
                MealType = MealTypeEnum.WarmDinner,
                Canteen = Canteen
            };

            var User = new CanteenEmployee()
            {
                EmployeeId = "1",
                Location = LocationCanteenEnum.LD
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            ICanteenService.Setup(Service => Service.GetByLocation(LocationCanteenEnum.LD)).ReturnsAsync(Canteen);
            ICanteenEmployeeService.Setup(Service => Service.GetById("1")).Returns(User);
            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package });
            IProductService.Setup(Service => Service.GetById(1)).ReturnsAsync(Product);
            IProductService.Setup(Service => Service.GetById(2)).ReturnsAsync(SecondProduct);

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            bool Result = IPackageService.Update(Package, new List<int>() { 1, 2 }, 2, "1");

            //Assert
            Assert.True(Result);
        }

        [Fact]
        public void Given_Correct_Package_And_User_To_Update_Get_Validate_Should_Return_Package()
        {
            //Arrange
            var User = new CanteenEmployee()
            {
                EmployeeId = "2",
                Location = LocationCanteenEnum.LA
            };

            var Package = new Package()
            {
                Id = 1,
                LastPickupTime = DateTime.Now.AddMinutes(5),
                Canteen = new Canteen()
                {
                    Location = LocationCanteenEnum.LA
                }
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            ICanteenEmployeeService.Setup(Service => Service.GetById("2")).Returns(User);
            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package });
            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            Package Result = IPackageService.UpdateGetValidate(1, "2");

            //Assert
            Assert.True(Result.Id == Package.Id);
            Assert.True(Result.Canteen!.Location == Package.Canteen.Location);
            Assert.True(Result.LastPickupTime == Package.LastPickupTime);
        }

        [Fact]
        public void Given_Correct_PickupTime_Should_Return_No_Exception()
        {
            //Arrange
            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            Exception Exception = Record.Exception(() => IPackageService.ValidatePickupTime(DateTime.Now.AddMinutes(10)));

            //Assert
            Assert.True(Exception == null);
        }

        [Fact]
        public void Given_Correct_PickupTime_And_LastPickupTime_Should_Return_No_Exception()
        {
            //Arrange
            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            Exception Exception = Record.Exception(() => IPackageService.ValidateLastPickupTime(DateTime.Now.AddMinutes(15), DateTime.Now.AddMinutes(10)));

            //Assert
            Assert.True(Exception == null);
        }

        [Fact]
        public void Given_Correct_Bool_And_MealTypeEnum_Should_Return_No_Exception()
        {
            //Arrange
            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            Exception Exception = Record.Exception(() => IPackageService.ValidateWarmMealsAllowed(true, MealTypeEnum.WarmDinner));

            //Assert
            Assert.True(Exception == null);
        }

        [Fact]
        public void Get_All_Non_Expired_Packages_With_Different_Location_Should_Return_Correct_Packages()
        {
            //Arrange
            var Package = new Package()
            {
                Id = 1,
                LastPickupTime = DateTime.Now.AddMinutes(-5),
                Canteen = new Canteen()
                {
                    Location = LocationCanteenEnum.LD
                }
            };

            var SecondPackage = new Package()
            {
                Id = 2,
                LastPickupTime = DateTime.Now.AddMinutes(-5),
                Canteen = new Canteen()
                {
                    Location = LocationCanteenEnum.LA
                }
            };

            var ThirdPackage = new Package()
            {
                Id = 3,
                LastPickupTime = DateTime.Now.AddMinutes(5),
                Canteen = new Canteen()
                {
                    Location = LocationCanteenEnum.LD
                }
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package, SecondPackage, ThirdPackage });

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            ICollection<Package> Result = IPackageService.GetAllNonExpiredPackages(LocationCanteenEnum.LA);

            //Assert
            Assert.True(Result.Count == 1);
            Assert.True(Result.FirstOrDefault()!.Id == ThirdPackage.Id);
        }

        [Fact]
        public void Get_All_Non_Expired_Packages_With_Different_Location_Should_Return_No_Packages()
        {
            //Arrange
            var Package = new Package()
            {
                Id = 1,
                LastPickupTime = DateTime.Now.AddMinutes(-5),
                Canteen = new Canteen() { 
                    Location = LocationCanteenEnum.LD
                }
            };

            var SecondPackage = new Package()
            {
                Id = 2,
                LastPickupTime = DateTime.Now.AddMinutes(-5),
                Canteen = new Canteen()
                {
                    Location = LocationCanteenEnum.LD
                }
            };

            var ThirdPackage = new Package()
            {
                Id = 3,
                Student = null,
                LastPickupTime = DateTime.Now.AddMinutes(10),
                Canteen = new Canteen()
                {
                    Location = LocationCanteenEnum.LA
                }
            };

            var IPackageRepository = new Mock<IPackageRepository>();
            var ICanteenEmployeeService = new Mock<ICanteenEmployeeService>();
            var ICanteenService = new Mock<ICanteenService>();
            var IProductService = new Mock<IProductService>();
            var IStudentService = new Mock<IStudentService>();

            IPackageRepository.Setup(Service => Service.GetAllAsync()).ReturnsAsync(new List<Package>() { Package, SecondPackage, ThirdPackage });

            IPackageService IPackageService = new PackageService(IPackageRepository.Object, ICanteenEmployeeService.Object, IStudentService.Object, ICanteenService.Object, IProductService.Object);

            //Act
            ICollection<Package> Result = IPackageService.GetAllNonExpiredPackages(LocationCanteenEnum.LA);

            //Assert
            Assert.Empty(Result);
        }
    }
}