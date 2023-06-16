namespace WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("package")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private IPackageService IPackageService;

        public PackageController(IPackageService IPackageService)
        {
            this.IPackageService = IPackageService;
        }

        [Authorize]
        [HttpPut("api/reserve/{PackageId}")]
        public IActionResult Reserve(int PackageId)
        {
            try
            {
                IPackageService.AssignStudent(PackageId, User.Identity!.Name!);
                Package Package = IPackageService.GetById(PackageId)!;
                
                foreach(Product Product in Package.Products!)
                {
                    Product.Package = null;
                    Product.Image = null;
                }
                
                return Ok(new { StatusCode = (int)HttpStatusCode.OK, Package });
            } catch (Exception E)
            {
                return BadRequest(new { StatusCode = (int)HttpStatusCode.BadRequest, E.Message });
            }
        }
    }
}
