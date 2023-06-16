var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("TGTG_ApplicationDatabase"))
);

builder.Services.AddDbContext<SecurityContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("TGTG_SecurityDatabase"))
);

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.Password.RequiredLength = 0;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<SecurityContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.Cookie.Name = "TGTG.Cookie";
        options.LoginPath = "/Account/Login";
    });

builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Student", PolicyBuilder =>
        {
            PolicyBuilder.RequireClaim("Student");
        });

        options.AddPolicy("CanteenEmployee", PolicyBuilder =>
        {
            PolicyBuilder.RequireClaim("CanteenEmployee");
        });
    });

builder.Services.AddScoped<ICanteenEmployeeRepository, CanteenEmployeeRepository>();
builder.Services.AddScoped<ICanteenRepository, CanteenRepository>();
builder.Services.AddScoped<IPackageRepository, PackageRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

builder.Services.AddScoped<ICanteenEmployeeService, CanteenEmployeeService>();
builder.Services.AddScoped<ICanteenService, CanteenService>();
builder.Services.AddScoped<IPackageService, PackageService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
