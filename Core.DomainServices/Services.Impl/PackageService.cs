namespace Core.DomainServices.Services.Impl
{
    public class PackageService : IPackageService
    {
        private IPackageRepository IPackageRepository;
        private ICanteenEmployeeService ICanteenEmployeeService;
        private IStudentService IStudentService;
        private ICanteenService ICanteenService;
        private IProductService IProductService;

        public PackageService(IPackageRepository IPackageRepository, ICanteenEmployeeService ICanteenEmployeeService, IStudentService IStudentService, ICanteenService ICanteenService, IProductService IProductService)
        {
            this.IPackageRepository = IPackageRepository;
            this.ICanteenEmployeeService = ICanteenEmployeeService;
            this.IStudentService = IStudentService;
            this.ICanteenService = ICanteenService;
            this.IProductService = IProductService;
        }

        public async Task<bool> Add(Package Package, ICollection<int> SelectedProducts, string UserId)
        {
            ValidatePickupTime((DateTime)Package.PickupTime!);
            ValidateLastPickupTime((DateTime)Package.LastPickupTime!, (DateTime)Package.PickupTime);

            CanteenEmployee CanteenEmployeeResult = ICanteenEmployeeService.GetById(UserId)!;
            Canteen Canteen = ICanteenService.GetByLocation(CanteenEmployeeResult.Location).Result;
            ICollection<Product> Products = SelectedProductsToProducts(SelectedProducts);

            ValidateWarmMealsAllowed((bool)Canteen.WarmMeals!, (MealTypeEnum)Package.MealType!);
            bool ForAdults = Products.Any(p => p.Alcoholic == true);

            Package ResultPackage = new Package
            {
                Name = Package!.Name,
                Products = Products,
                Canteen = Canteen,
                PickupTime = Package!.PickupTime,
                LastPickupTime = Package!.LastPickupTime,
                ForAdults = ForAdults,
                Price = Package!.Price,
                MealType = Package!.MealType
            };

            ProductLinkPackage(Products, Package);
            await IPackageRepository.AddAsync(ResultPackage);
            return true;
        }

        public bool AssignStudent(int PackageId, string UserId)
        {
            ICollection<Package> Packages = GetAllPackagesByStudent(UserId);
            Package Package = GetById(PackageId)!;
            Student Student = IStudentService.GetById(UserId)!;

            if (Package.Student != null)
            {
                throw new Exception("Het pakket is al gereserveerd door iemand anders!");
            }

            bool Contains = false;
            foreach (Package Element in Packages)
            {
                if (Package.PickupTime!.Value.Day == Element.PickupTime!.Value.Day && Package.PickupTime.Value.Month == Element.PickupTime!.Value.Month && Package.PickupTime.Value.Year == Element.PickupTime!.Value.Year)
                {
                    Contains = true;
                }
            }

            if (Contains)
            {
                throw new Exception("Je hebt al een pakket gereserveerd op de afhaaldag van dit pakket!");
            }

            bool Adult = Student.DateOfBirth!.Value.AddYears(18) <= Package.PickupTime!.Value;

            if (Package.ForAdults == true)
            {
                if (Adult)
                {
                    Package.Student = Student;
                    IPackageRepository.Update(Package);
                    return true;
                }
                else
                {
                    throw new Exception("Alleen volwassenen mogen dit pakket reserveren!");
                }
            } else
            {
                Package.Student = Student;
                IPackageRepository.Update(Package);
                return true;
            }
        }

        public bool Delete(int PackageId, string UserId)
        {
            Package Package = GetById(PackageId)!;
            CanteenEmployee CanteenEmployee = ICanteenEmployeeService.GetById(UserId)!;

            if (Package.Canteen!.Location != CanteenEmployee.Location)
            {
                throw new Exception("Dit pakket behoort niet tot jouw kantine dus je mag het niet verwijderen!");
            }

            if (Package.Student != null && Package.LastPickupTime > DateTime.Now)
            {
                throw new Exception("Er is al een reservering voor dit pakket dus je kan het niet verwijderen.");
            } else
            {
                IPackageRepository.Delete(Package.Id);
                return true;
            }
        }

        public async Task<ICollection<Package>> GetAll()
        {
            return await IPackageRepository.GetAllAsync();
        }

        public Package? GetById(int Id)
        {
            return IPackageRepository.GetAllAsync().Result.Where(p => p.Id == Id).FirstOrDefault();
        }

        public bool Update(Package NewData, ICollection<int> SelectedProducts, int PackageId, string UserId)
        {
            Package OldData = GetById(PackageId)!;
            CanteenEmployee CanteenEmployee = ICanteenEmployeeService.GetById(UserId)!;

            if (OldData.Canteen!.Location != CanteenEmployee.Location)
            {
                throw new Exception("Dit pakket behoort niet tot jouw kantine dus je mag het niet bewerken!");
            }

            ValidatePickupTime((DateTime)NewData.PickupTime!);
            ValidateLastPickupTime((DateTime)NewData.LastPickupTime!, (DateTime)NewData.PickupTime);

            Canteen Canteen = ICanteenService.GetByLocation(CanteenEmployee.Location).Result;
            ICollection<Product> Products = SelectedProductsToProducts(SelectedProducts);

            ValidateWarmMealsAllowed((bool)Canteen.WarmMeals!, (MealTypeEnum)NewData.MealType!);
            bool ForAdults = Products.Any(p => p.Alcoholic == true);

            OldData.Name = NewData.Name;
            OldData.Products = Products;
            OldData.Canteen = Canteen;
            OldData.PickupTime = NewData.PickupTime;
            OldData.LastPickupTime = NewData.LastPickupTime;
            OldData.ForAdults = ForAdults;
            OldData.Price = NewData.Price;
            OldData.MealType = NewData.MealType;

            ProductLinkPackage(Products, OldData);
            IPackageRepository.Update(OldData);
            return true;
        }

        public Package UpdateGetValidate(int PackageId, string UserId)
        {
            Package Package = GetById(PackageId)!;
            CanteenEmployee CanteenEmployee = ICanteenEmployeeService.GetById(UserId)!;

            if (Package.Student != null && Package.LastPickupTime > DateTime.Now)
            {
                throw new Exception("Er is al een reservering voor dit pakket dus je kan het niet bewerken!");
            }

            if (Package.Canteen!.Location != CanteenEmployee.Location)
            {
                throw new Exception("Dit pakket behoort niet tot jouw kantine dus je mag het niet bewerken!");
            }

            return Package;
        }

        public ICollection<Package> GetAllPackagesByStudent(string Id)
        {
            return IPackageRepository.GetAllAsync().Result.Where(p => p.Student?.StudentId == Id).ToList().OrderBy(p => p.PickupTime).ToList();
        }

        public ICollection<Package> GetAllPackagesByCanteen(LocationCanteenEnum LocationCanteenEnum)
        {
            return IPackageRepository.GetAllAsync().Result.Where(p => p.Canteen!.Location == LocationCanteenEnum).Where(p => p.LastPickupTime > DateTime.Now).OrderBy(p => p.PickupTime).ToList();
        }

        public ICollection<Package> GetAllAvailablePackages()
        {
            return IPackageRepository.GetAllAsync().Result.Where(p => p.LastPickupTime > DateTime.Now).Where(p => p.Student == null).OrderBy(p => p.PickupTime).ToList();
        }

        public ICollection<Package> GetAllNonExpiredPackages(LocationCanteenEnum LocationCanteenEnum)
        {
            return IPackageRepository.GetAllAsync().Result.Where(p => p.LastPickupTime > DateTime.Now).Where(p => p.Canteen!.Location != LocationCanteenEnum).OrderBy(p => p.PickupTime).ToList();
        }

        public void ValidatePickupTime(DateTime PickupTime)
        {
            if (PickupTime < DateTime.Now)
            {
                throw new Exception("De ophaaltijd moet in de toekomst liggen!");
            }

            if (PickupTime.Day > DateTime.Now.AddDays(2).Day || PickupTime.Month != DateTime.Now.AddDays(2).Month || PickupTime.Year != DateTime.Now.AddDays(2).Year)
            {
                throw new Exception("De ophaaltijd mag niet meer dan twee dagen in de toekomst liggen!");
            }
        }

        public void ValidateLastPickupTime(DateTime LastPickupTime, DateTime PickupTime)
        {
            if (LastPickupTime < DateTime.Now)
            {
                throw new Exception("De laatste ophaaltijd moet in de toekomst liggen!");
            }
            else if (LastPickupTime <= PickupTime)
            {
                throw new Exception("De laatste ophaaltijd moet na de afhaaltijd liggen!");
            }
            else if (LastPickupTime!.Day != PickupTime.Day || LastPickupTime.Month != PickupTime.Month || LastPickupTime.Year != PickupTime.Year)
            {
                throw new Exception("De laatste ophaaltijd moet op dezelfde dag als de afhaaltijd plaatsvinden.");
            }
        }

        public void ValidateWarmMealsAllowed(bool CanteenWarmMeals, MealTypeEnum PackageMealType)
        {
            if (CanteenWarmMeals == false && PackageMealType == MealTypeEnum.WarmDinner)
            {
                throw new Exception("Warme maaltijden zijn niet toegestaan op jouw locatie!");
            }
        }

        public ICollection<Product> SelectedProductsToProducts(ICollection<int> SelectedProducts)
        {
            ICollection<Product> Products = new List<Product>();
            
            foreach (int Id in SelectedProducts!)
            {
                Products.Add(IProductService.GetById(Id).Result!);
            }

            return Products;
        }

        public ICollection<Product> ProductLinkPackage(ICollection<Product> Products, Package Package)
        {
            foreach (Product Product in Products)
            {
                Product?.Package?.Add(Package);
            }

            return Products;
        }
    }
}
