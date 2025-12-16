using System.Data.Entity;

namespace ProyectoProgramacionAvanzada.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("MSSConnectionString") { }

        public DbSet<Product> product { get; set; }
        public DbSet<Product_Images> product_images { get; set; }
        public DbSet<Product_Reviews> product_reviews { get; set; }
        public DbSet<User> user { get; set; }
        public DbSet<Role> role { get; set; }
        // public DbSet<Order> Orders { get; set; }
        // public DbSet<OrderDetail> OrderDetails { get; set; }
        // public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.name).IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(p => p.price).HasPrecision(10, 2);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.product_images)
                .WithRequired(pi => pi.product)
                .HasForeignKey(pi => pi.product_id);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.product_reviews)
                .WithRequired(pr => pr.product)
                .HasForeignKey(pr => pr.product_id);

            modelBuilder.Entity<User>()
                .Property(u => u.username).IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(u => u.hashed_password).IsUnicode(false);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.user)
                .WithMany(u => u.role)
                .Map(m => m.ToTable("user_role"));
        }
    }
}
