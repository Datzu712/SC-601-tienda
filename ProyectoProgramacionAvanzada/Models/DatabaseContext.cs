using System.Data.Entity;

namespace ProyectoProgramacionAvanzada.Models
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext() : base("MSSConnectionString")
        {
        }

        public virtual DbSet<Product> product { get; set; }
        public virtual DbSet<Product_Images> product_images { get; set; }
        public virtual DbSet<Product_Reviews> product_reviews { get; set; }
        public virtual DbSet<Role> role { get; set; }
        public virtual DbSet<User> user { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.product_images)
                .WithRequired(e => e.product)
                .HasForeignKey(e => e.product_id);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.product_reviews)
                .WithRequired(e => e.product)
                .HasForeignKey(e => e.product_id);

            modelBuilder.Entity<Product_Images>()
                .Property(e => e.image_url)
                .IsUnicode(false);

            modelBuilder.Entity<Product_Reviews>()
                .Property(e => e.content)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.user)
                .WithMany(e => e.role)
                .Map(m => m.ToTable("user_role"));

            modelBuilder.Entity<User>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.hashed_password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.product_reviews)
                .WithRequired(e => e.user)
                .HasForeignKey(e => e.author_id);
        }
    }
}