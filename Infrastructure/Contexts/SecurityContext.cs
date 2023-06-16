namespace Infrastructure.Contexts
{
    public class SecurityContext : IdentityDbContext
    {
        public SecurityContext(DbContextOptions<SecurityContext> ContextOptions) : base(ContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder ModelBuilder)
        {
            base.OnModelCreating(ModelBuilder);
        }
    }
}
