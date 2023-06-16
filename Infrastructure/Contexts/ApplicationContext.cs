namespace Infrastructure.Contexts
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Package> Packages { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Canteen> Canteens { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<CanteenEmployee> CanteenEmployees { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> ContextOptions) : base(ContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder ModelBuilder)
        {
            ModelBuilder.Entity<Canteen>().HasData(new Canteen[] {
                new Canteen()
                {
                    Id = 1,
                    City = CityEnum.Breda,
                    Location = LocationCanteenEnum.LA,
                    WarmMeals = false
                },
                new Canteen()
                {
                    Id = 2,
                    City = CityEnum.Breda,
                    Location = LocationCanteenEnum.LD,
                    WarmMeals = true
                },
                new Canteen()
                {
                    Id = 3,
                    City = CityEnum.Breda,
                    Location = LocationCanteenEnum.HA,
                    WarmMeals = true
                },
                new Canteen()
                {
                    Id = 4,
                    City = CityEnum.Breda,
                    Location = LocationCanteenEnum.HB,
                    WarmMeals = false
                },
                new Canteen()
                {
                    Id = 5,
                    City = CityEnum.DenBosch,
                    Location = LocationCanteenEnum.HC,
                    WarmMeals = false
                },
                new Canteen()
                {
                    Id = 6,
                    City = CityEnum.Tilburg,
                    Location = LocationCanteenEnum.HD,
                    WarmMeals = true
                }
            });

            ModelBuilder.Entity<Product>().HasData(new Product[]
            {
                new Product()
                {
                    Id = 1,
                    Name = "Appel",
                    Alcoholic = false,
                    Image = ReadStream(new HttpClient().GetStreamAsync("https://appleridgeorchards.com/wp-content/uploads/2021/05/Apple-Picking-Icon-1-400x400-1.png").Result)
                },
                new Product()
                {
                    Id = 2,
                    Name = "Bier",
                    Alcoholic = true,
                    Image = ReadStream(new HttpClient().GetStreamAsync("https://www.arjenverhuurt.nl/wp-content/uploads/rentman/jupiler-krat-24-x-25-cl-19625.jpg").Result)
                },
                new Product()
                {
                    Id = 3,
                    Name = "Pakje melk",
                    Alcoholic = false,
                    Image = ReadStream(new HttpClient().GetStreamAsync("https://cdn1.sph.harvard.edu/wp-content/uploads/sites/30/2012/09/milk.jpg").Result)
                },
                new Product()
                {
                    Id = 4,
                    Name = "Eiersalade",
                    Alcoholic = false,
                    Image = ReadStream(new HttpClient().GetStreamAsync("https://therecipecritic.com/wp-content/uploads/2019/02/besteggsalad-500x500.jpg").Result)
                },
                new Product()
                {
                    Id = 5,
                    Name = "Broodje gezond",
                    Alcoholic = false,
                    Image = ReadStream(new HttpClient().GetStreamAsync("https://www.bakkerbart.nl/media/catalog/product/cache/afb382f2589f4527c76899f7685dfe75/b/a/bartje_gezond_1.jpg").Result)
                },
                new Product()
                {
                    Id = 6,
                    Name = "Blikje Cola",
                    Alcoholic = false,
                    Image = ReadStream(new HttpClient().GetStreamAsync("https://www.shopbijalbatros.nl/media/catalog/product/cache/cd55d730358cf47f86f915a351923ecb/2/0/200.000343-voorkant.jpg_1.jpg").Result)
                },
                new Product()
                {
                    Id = 7,
                    Name = "Croissant",
                    Alcoholic = false,
                    Image = ReadStream(new HttpClient().GetStreamAsync("https://www.okokorecepten.nl/i/recepten/kookboeken/2009/kook-ook-brood/zelfgebakken-croissants-500.jpg").Result)
                },
                new Product()
                {
                    Id = 8,
                    Name = "Appelmoes",
                    Alcoholic = false,
                    Image = ReadStream(new HttpClient().GetStreamAsync("https://ilovefoodwine.nl/content/uploads/2021/03/shutterstock_177416972-500x375.jpg").Result)
                },
                new Product()
                {
                    Id = 9,
                    Name = "Wijn",
                    Alcoholic = true,
                    Image = ReadStream(new HttpClient().GetStreamAsync("https://cdn.webshopapp.com/shops/29951/files/378792565/image.jpg").Result)
                },
                new Product()
                {
                    Id = 10,
                    Name = "Koffie",
                    Alcoholic = false,
                    Image = ReadStream(new HttpClient().GetStreamAsync("https://image.gezondheid.be/XTRA/123m-koffie-espresso-27-8.jpg").Result)
                },
            });

            base.OnModelCreating(ModelBuilder);
        }

        public static byte[] ReadStream(Stream Stream)
        {
            byte[] Buffer = new byte[16 * 1024];
            using (MemoryStream MS = new MemoryStream())
            {
                int Read;
                while ((Read = Stream.Read(Buffer, 0, Buffer.Length)) > 0)
                {
                    MS.Write(Buffer, 0, Read);
                }
                return MS.ToArray();
            }
        }
    }
}
