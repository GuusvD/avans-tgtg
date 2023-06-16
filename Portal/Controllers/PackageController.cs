namespace Portal.Controllers
{
    public class PackageController : Controller
    {
        private IPackageService IPackageService;
        private ICanteenEmployeeService ICanteenEmployeeService;
        private IProductService IProductService;

        public PackageController(IPackageService IPackageService, ICanteenEmployeeService ICanteenEmployeeService, IProductService IProductService)
        {
            this.IPackageService = IPackageService;
            this.ICanteenEmployeeService = ICanteenEmployeeService;
            this.IProductService = IProductService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Packages()
        {
            ICollection<Package> Packages = IPackageService.GetAllAvailablePackages();

            foreach(Package Package in Packages)
            {
                if (Package.Name!.Length > 14)
                {
                    Package.Name = Package.Name!.Remove(14) + "...";
                }
            }

            return View(Packages);
        }

        [HttpGet]
        [Authorize(Policy = "CanteenEmployee")]
        public IActionResult OurPackages()
        {
            CanteenEmployee? CanteenEmployee = ICanteenEmployeeService.GetById(User.Identity!.Name!);
            return View(IPackageService.GetAllPackagesByCanteen((LocationCanteenEnum)CanteenEmployee!.Location!));
        }

        [HttpGet]
        [Authorize(Policy = "CanteenEmployee")]
        public IActionResult CreatePackage()
        {
            ICollection<Product> Products = IProductService.GetAll().Result;
            ProductPackageModel Model = ViewModelHelpers.ToProductPackageModel(Products, null);
            return View(Model);
        }

        [HttpPost]
        [Authorize(Policy = "CanteenEmployee")]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePackage(ProductPackageModel ProductPackageModel)
        {
            try
            {
                ICollection<Product> AllProducts = IProductService.GetAll().Result;
                ProductPackageModel.Products = AllProducts;

                if (ModelState.IsValid)
                {
                    Package Package = new Package
                    {
                        Name = ProductPackageModel.Package!.Name,
                        Products = null,
                        Canteen = null,
                        PickupTime = ProductPackageModel.Package!.PickupTime,
                        LastPickupTime = ProductPackageModel.Package!.LastPickupTime,
                        ForAdults = null,
                        Price = ProductPackageModel.Package!.Price,
                        MealType = ProductPackageModel.Package!.MealType
                    };

                    IPackageService.Add(Package, ProductPackageModel.Package.SelectedProducts!, User.Identity!.Name!).Wait();
                    return Redirect("Packages");
                } else
                {
                    return View(ProductPackageModel);
                }
            } catch (Exception E)
            {
                if (E.InnerException!.Message == "De ophaaltijd moet in de toekomst liggen!")
                {
                    ModelState.AddModelError("Package.PickupTime", "De ophaaltijd moet in de toekomst liggen!");
                } else if (E.InnerException.Message == "De ophaaltijd mag niet meer dan twee dagen in de toekomst liggen!")
                {
                    ModelState.AddModelError("Package.PickupTime", "De ophaaltijd mag niet meer dan twee dagen in de toekomst liggen!");
                } else if (E.InnerException.Message == "De laatste ophaaltijd moet in de toekomst liggen!")
                {
                    ModelState.AddModelError("Package.LastPickupTime", "De laatste ophaaltijd moet in de toekomst liggen!");
                } else if (E.InnerException.Message == "De laatste ophaaltijd moet na de afhaaltijd liggen!")
                {
                    ModelState.AddModelError("Package.LastPickupTime", "De laatste ophaaltijd moet na de afhaaltijd liggen!");
                } else if (E.InnerException.Message == "De laatste ophaaltijd moet op dezelfde dag als de afhaaltijd plaatsvinden.")
                {
                    ModelState.AddModelError("Package.LastPickupTime", "De laatste ophaaltijd moet op dezelfde dag als de afhaaltijd plaatsvinden.");
                } else if (E.InnerException.Message == "Warme maaltijden zijn niet toegestaan op jouw locatie!")
                {
                    ModelState.AddModelError("Package.MealType", "Warme maaltijden zijn niet toegestaan op jouw locatie!");
                }

                return View(ProductPackageModel);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Detail(int PackageId, bool ShowAlert = false, string DescAlert = null!)
        {
            Package? Package = IPackageService.GetById(PackageId);

            if (Package == null)
            {
                return Redirect("/Home/Index");
            }

            PackageAlertModel PackageAlertModel = ViewModelHelpers.ToPackageAlertModel(Package, ShowAlert, DescAlert, null);

            if (User.GetRole() == "CanteenEmployee")
            {
                CanteenEmployee CanteenEmployee = ICanteenEmployeeService.GetById(User.Identity!.Name!)!;
                
                if (Package.Canteen!.Location == CanteenEmployee.Location)
                {
                    PackageAlertModel.FromOwnCanteen = true;
                } else
                {
                    PackageAlertModel.FromOwnCanteen = false;
                }
            }

            return View("Detail", PackageAlertModel);
        }

        [HttpPost]
        [Authorize(Policy = "CanteenEmployee")]
        public IActionResult Delete(int PackageId)
        {   
            try
            {
                IPackageService.Delete(PackageId, User.Identity!.Name!);
                return Redirect("Packages");
            } catch (Exception E)
            {
                if (E.Message == "Dit pakket behoort niet tot jouw kantine dus je mag het niet verwijderen!")
                {
                    return RedirectToAction("Detail", new { PackageId = PackageId, ShowAlert = true, DescAlert = "Dit pakket behoort niet tot jouw kantine dus je mag het niet verwijderen!" });
                } else if (E.Message == "Er is al een reservering voor dit pakket dus je kan het niet verwijderen.")
                {
                    return RedirectToAction("Detail", new { PackageId = PackageId, ShowAlert = true, DescAlert = "Er is al een reservering voor dit pakket dus je kan het niet verwijderen." });
                } else
                {
                    return RedirectToAction("Detail", new { PackageId = PackageId});
                }
            }
        }

        [HttpPost]
        [Authorize(Policy = "Student")]
        public IActionResult Reserve(int PackageId)
        {
            try
            {
                IPackageService.AssignStudent(PackageId, User.Identity!.Name!);
                return Redirect("MyPackages");
            } catch (Exception E)
            {
                if (E.Message == "Het pakket is al gereserveerd door iemand anders!")
                {
                    return RedirectToAction("Detail", new { PackageId = PackageId, ShowAlert = true, DescAlert = "Het pakket is al gereserveerd door iemand anders!" });
                } else if (E.Message == "Je hebt al een pakket gereserveerd op de afhaaldag van dit pakket!")
                {
                    return RedirectToAction("Detail", new { PackageId = PackageId, ShowAlert = true, DescAlert = "Je hebt al een pakket gereserveerd op de afhaaldag van dit pakket!" });
                } else if (E.Message == "Alleen volwassenen mogen dit pakket reserveren!") {
                    return RedirectToAction("Detail", new { PackageId = PackageId, ShowAlert = true, DescAlert = "Alleen volwassenen mogen dit pakket reserveren!" });
                } else
                {
                    return Redirect("MyPackages");
                }
            }
        }

        [HttpGet]
        [Authorize(Policy = "Student")]
        public IActionResult MyPackages()
        {
            ICollection<Package> Packages = IPackageService.GetAllPackagesByStudent(User.Identity!.Name!);

            foreach (Package Package in Packages)
            {
                if (Package.Name!.Length > 14)
                {
                    Package.Name = Package.Name!.Remove(14) + "...";
                }
            }

            return View("Packages", Packages);
        }

        [HttpGet]
        [Authorize(Policy = "CanteenEmployee")]
        public IActionResult UpdatePackageGet(int PackageId)
        {
            try
            {
                Package Package = IPackageService.GetById(PackageId)!;
                ICollection<Product> Products = IProductService.GetAll().Result;
                ICollection<int> SelectedProducts = new List<int>();

                foreach (Product Product in Package.Products!)
                {
                    SelectedProducts.Add(Products.ToList().FindIndex(p => p.Id == Product.Id) + 1);
                }

                Package ResultPackage = IPackageService.UpdateGetValidate(PackageId, User.Identity!.Name!);
                ProductPackageModel ProductPackageModel = ViewModelHelpers.ToProductPackageModel(Products, ViewModelHelpers.ToPackageModel(ResultPackage, SelectedProducts.ToList()));

                return View("UpdatePackage", ProductPackageModel);
            }
            catch (Exception E)
            {
                if (E.Message == "Er is al een reservering voor dit pakket dus je kan het niet bewerken!")
                {
                    return RedirectToAction("Detail", new { PackageId = PackageId, ShowAlert = true, DescAlert = "Er is al een reservering voor dit pakket dus je kan het niet bewerken!" });
                } else if (E.Message == "Dit pakket behoort niet tot jouw kantine dus je mag het niet bewerken!")
                {
                    return RedirectToAction("Detail", new { PackageId = PackageId, ShowAlert = true, DescAlert = "Dit pakket behoort niet tot jouw kantine dus je mag het niet bewerken!" });
                }

                return RedirectToAction("Detail", new { PackageId = PackageId });
            }
        }

        [HttpPost]
        [Authorize(Policy = "CanteenEmployee")]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePackagePost(int PackageId, ProductPackageModel ProductPackageModel)
        {
            try
            {
                ICollection<Product> AllProducts = IProductService.GetAll().Result;
                ProductPackageModel.Products = AllProducts;

                if (ModelState.IsValid)
                {
                    ICollection<Product> Products = new List<Product>();
                    foreach (int Id in ProductPackageModel.Package!.SelectedProducts!)
                    {
                        Products.Add(IProductService.GetById(Id).Result!);
                    }

                    Package NewData = new Package();
                    bool ForAdults = Products.Any(p => p.Alcoholic == true);

                    NewData.Name = ProductPackageModel.Package!.Name;
                    NewData.Products = Products;
                    NewData.Canteen = null;
                    NewData.PickupTime = ProductPackageModel.Package!.PickupTime;
                    NewData.LastPickupTime = ProductPackageModel.Package!.LastPickupTime;
                    NewData.ForAdults = ForAdults;
                    NewData.Price = ProductPackageModel.Package!.Price;
                    NewData.MealType = ProductPackageModel.Package!.MealType;

                    IPackageService.Update(NewData, ProductPackageModel.Package.SelectedProducts, PackageId, User.Identity!.Name!);
                    return RedirectToAction("Detail", new { PackageId });
                }
                else
                {
                    return View("UpdatePackage", ProductPackageModel);
                }
            } catch (Exception E)
            {
                if (E.Message == "Dit pakket behoort niet tot jouw kantine dus je mag het niet bewerken!")
                {
                    return RedirectToAction("Detail", new { PackageId = PackageId, ShowAlert = true, DescAlert = "Dit pakket behoort niet tot jouw kantine dus je mag het niet bewerken!" });
                } else if (E.Message == "De ophaaltijd moet in de toekomst liggen!")
                {
                    ModelState.AddModelError("Package.PickupTime", "De ophaaltijd moet in de toekomst liggen!");
                } else if (E.Message == "De ophaaltijd mag niet meer dan twee dagen in de toekomst liggen!") 
                {
                    ModelState.AddModelError("Package.PickupTime", "De ophaaltijd mag niet meer dan twee dagen in de toekomst liggen!");
                } else if (E.Message == "De laatste ophaaltijd moet in de toekomst liggen!") 
                {
                    ModelState.AddModelError("Package.LastPickupTime", "De laatste ophaaltijd moet in de toekomst liggen!");
                } else if (E.Message == "De laatste ophaaltijd moet na de afhaaltijd liggen!")
                {
                    ModelState.AddModelError("Package.LastPickupTime", "De laatste ophaaltijd moet na de afhaaltijd liggen!");
                } else if (E.Message == "De laatste ophaaltijd moet op dezelfde dag als de afhaaltijd plaatsvinden.")
                {
                    ModelState.AddModelError("Package.LastPickupTime", "De laatste ophaaltijd moet op dezelfde dag als de afhaaltijd plaatsvinden.");
                } else if (E.Message == "Warme maaltijden zijn niet toegestaan op jouw locatie!")
                {
                    ModelState.AddModelError("Package.MealType", "Warme maaltijden zijn niet toegestaan op jouw locatie!");
                }

                return View("UpdatePackage", ProductPackageModel);
            }
        }

        [HttpGet]
        [Authorize(Policy = "CanteenEmployee")]
        public IActionResult OtherPackages()
        {
            CanteenEmployee? CanteenEmployee = ICanteenEmployeeService.GetById(User.Identity!.Name!);
            return View("Packages", IPackageService.GetAllNonExpiredPackages((LocationCanteenEnum) CanteenEmployee!.Location!));
        }
    }
}