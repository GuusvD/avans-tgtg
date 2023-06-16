namespace Portal.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<IdentityUser> UserManager;
        private SignInManager<IdentityUser> SignInManager;
        private IStudentService IStudentService;
        private ICanteenEmployeeService ICanteenEmployeeService;

        public AccountController(UserManager<IdentityUser> UserManager,
            SignInManager<IdentityUser> SignInManager, IStudentService IStudentService, ICanteenEmployeeService ICanteenEmployeeService)
        {
            this.UserManager = UserManager;
            this.SignInManager = SignInManager;
            this.IStudentService = IStudentService;
            this.ICanteenEmployeeService = ICanteenEmployeeService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string ReturnUrl = "/")
        {
            return View(new LoginModel
            {
                ReturnUrl = ReturnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel LoginModel)
        {
            if (ModelState.IsValid)
            {
                var User = await UserManager.FindByNameAsync(LoginModel.UserId);

                if (User != null)
                {
                    await SignInManager.SignOutAsync();
                    if ((await SignInManager.PasswordSignInAsync(User, LoginModel.Password, false, false)).Succeeded)
                    {
                        return Redirect(LoginModel?.ReturnUrl ?? "/");
                    } else
                    {
                        ModelState.AddModelError(nameof(LoginModel.Password), "De combinatie van id en wachtwoord is incorrect!");
                    }
                } else
                {
                    ModelState.AddModelError(nameof(LoginModel.Password), "Geen gebruiker gevonden met deze gegevens!");
                }
            } else
            {
                if (ModelState["UserId"]?.Errors.Count > 0)
                {
                    ModelState["UserId"]?.Errors.Clear();
                    ModelState["UserId"]?.Errors.Add("Vul een nummer in.");
                }

                if (ModelState["Password"]?.Errors.Count > 0)
                {
                    ModelState["Password"]?.Errors.Clear();
                    ModelState["Password"]?.Errors.Add("Vul een wachtwoord in.");
                }

                return View(LoginModel);
            }

            return View(LoginModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<RedirectResult> Logout(string ReturnUrl = "/")
        {
            await SignInManager.SignOutAsync();
            return Redirect(ReturnUrl);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied(string ReturnUrl)
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterStudent()
        {
            return View(new StudentRegisterModel());
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterCanteenEmployee()
        {
            return View(new CanteenEmployeeRegisterModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterStudent(StudentRegisterModel StudentRegisterModel)
        {
            if (StudentRegisterModel.DateOfBirth != null && StudentRegisterModel.DateOfBirth!.Value.AddYears(16) > DateTime.Now)
            {
                ModelState.AddModelError("DateOfBirth", "Je moet minimaal 16 jaar zijn!");
            }

            if (!ModelState.IsValid)
            {
                if (ModelState["City"]?.Errors.Count > 0)
                {
                    ModelState["City"]?.Errors.Clear();
                    ModelState["City"]?.Errors.Add("Selecteer een stad.");
                }

                return View(StudentRegisterModel);
            }

            var DoubleStudentId = IStudentService.GetById(StudentRegisterModel.StudentId!);
            var DoubleCanteenEmployeeId = ICanteenEmployeeService.GetById(StudentRegisterModel.StudentId!);

            if (DoubleStudentId != null || DoubleCanteenEmployeeId != null)
            {
                ModelState["StudentId"]?.Errors.Add("Dit studentnummer is al in gebruik!");
                return View(StudentRegisterModel);
            }

            var HasNumber = new Regex(@"[0-9]+");
            var HasUpperCase = new Regex(@"[A-Z]+");
            var HasMinimunEightCharacters = new Regex(@".{8,}");
            var IsValid = HasNumber.IsMatch(StudentRegisterModel.Password!) && HasUpperCase.IsMatch(StudentRegisterModel.Password!) && HasMinimunEightCharacters.IsMatch(StudentRegisterModel.Password!);

            if (IsValid == false)
            {
                ModelState["Password"]?.Errors.Add("Wachtwoord moet minimaal 8 karakters bevatten inclusief 1 nummer en 1 hoofdletter!");
                return View(StudentRegisterModel);
            }

            var User = new IdentityUser
            {
                UserName = StudentRegisterModel.StudentId,
                EmailConfirmed = true
            };

            var Student = new Student
            {
                Name = StudentRegisterModel.Name,
                StudentId = StudentRegisterModel.StudentId,
                EmailAddress = StudentRegisterModel.EmailAddress,
                PhoneNumber = StudentRegisterModel.PhoneNumber,
                DateOfBirth = StudentRegisterModel.DateOfBirth,
                City = StudentRegisterModel.City
            };

            var Result = await UserManager.CreateAsync(User, StudentRegisterModel.Password);
            await UserManager.AddClaimAsync(User, new Claim("Student", "Student"));

            if (Result.Succeeded)
            {
                await IStudentService.Add(Student);
                await SignInManager.PasswordSignInAsync(User, StudentRegisterModel.Password, false, false);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterCanteenEmployee(CanteenEmployeeRegisterModel CanteenEmployeeRegisterModel)
        {
            if (!ModelState.IsValid)
            {
                if (ModelState["Location"]?.Errors.Count > 0)
                {
                    ModelState["Location"]?.Errors.Clear();
                    ModelState["Location"]?.Errors.Add("Selecteer een locatie.");
                }

                return View(CanteenEmployeeRegisterModel);
            }

            var DoubleStudentId = IStudentService.GetById(CanteenEmployeeRegisterModel.EmployeeId!);
            var DoubleCanteenEmployeeId = ICanteenEmployeeService.GetById(CanteenEmployeeRegisterModel.EmployeeId!);

            if (DoubleStudentId != null || DoubleCanteenEmployeeId != null)
            {
                ModelState["EmployeeId"]?.Errors.Add("Dit personeelsnummer is al in gebruik!");
                return View(CanteenEmployeeRegisterModel);
            }

            var HasNumber = new Regex(@"[0-9]+");
            var HasUpperCase = new Regex(@"[A-Z]+");
            var HasMinimunEightCharacters = new Regex(@".{8,}");
            var IsValid = HasNumber.IsMatch(CanteenEmployeeRegisterModel.Password!) && HasUpperCase.IsMatch(CanteenEmployeeRegisterModel.Password!) && HasMinimunEightCharacters.IsMatch(CanteenEmployeeRegisterModel.Password!);

            if (IsValid == false)
            {
                ModelState["Password"]?.Errors.Add("Wachtwoord moet minimaal 8 karakters bevatten inclusief 1 nummer en 1 hoofdletter!");
                return View(CanteenEmployeeRegisterModel);
            }

            var User = new IdentityUser
            {
                UserName = CanteenEmployeeRegisterModel.EmployeeId,
                EmailConfirmed = true
            };

            var CanteenEmployee = new CanteenEmployee
            {
                Name = CanteenEmployeeRegisterModel.Name,
                EmployeeId = CanteenEmployeeRegisterModel.EmployeeId,
                Location = CanteenEmployeeRegisterModel.Location
            };

            var Result = await UserManager.CreateAsync(User, CanteenEmployeeRegisterModel.Password);
            await UserManager.AddClaimAsync(User, new Claim("CanteenEmployee", "CanteenEmployee"));

            if (Result.Succeeded)
            {
                await ICanteenEmployeeService.Add(CanteenEmployee);
                await SignInManager.PasswordSignInAsync(User, CanteenEmployeeRegisterModel.Password, false, false);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
    }
}
