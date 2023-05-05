using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Store_Core7.Model
{
    public class AppDBContext : IdentityDbContext<UserModel>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<LogModel> logs { get; set; }
        public DbSet<SystemSettingModel> systemSettings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //* One-to-Many
            modelBuilder.Entity<ProductModel>()
                .HasOne(p => p.Category)
                .WithMany(c => c.products)
                .HasForeignKey(o => o.CategoryId);

            //* Many-to-Many
            modelBuilder.Entity<OrderModel>()
                .HasKey(o => new { o.UserId, o.ProductId });

            modelBuilder.Entity<OrderModel>()
                .HasOne(o => o.User)
                .WithMany(u => u.orders)
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<OrderModel>()
                .HasOne(o => o.Product)
                .WithMany(p => p.orders)
                .HasForeignKey(o => o.ProductId);
           
            modelBuilder.Entity<UserModel>().ToTable("Users");
            //* This table stores role information, such as role name and normalized role name
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            //* This table stores the association between users and roles. Each row represents a user that is a member of a specific role
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRole");

            //*  Ignore unnecessary tables
            modelBuilder.Ignore<IdentityUserClaim<string>>();
            modelBuilder.Ignore<IdentityRoleClaim<string>>();
            modelBuilder.Ignore<IdentityUserLogin<string>>();
            modelBuilder.Ignore<IdentityUserToken<string>>();
        }
    }
}
