namespace Portal.Controllers
{
    public class HomeController : Controller
    {
        private IPackageService IPackageService;
        private ICanteenEmployeeService ICanteenEmployeeService;

        public HomeController(IPackageService IPackageService, ICanteenEmployeeService ICanteenEmployeeService)
        {
            this.IPackageService = IPackageService;
            this.ICanteenEmployeeService = ICanteenEmployeeService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.GetRole() == "CanteenEmployee")
            {
                CanteenEmployee? CanteenEmployee = ICanteenEmployeeService.GetById(User.Identity!.Name!);
                ICollection<Package> Packages = IPackageService.GetAllPackagesByCanteen((LocationCanteenEnum)CanteenEmployee!.Location!).Take(8).ToList();
                return View(Packages);
            } else
            {
                return View(IPackageService.GetAllAvailablePackages().Take(8).ToList());
            }
        }
    }
}