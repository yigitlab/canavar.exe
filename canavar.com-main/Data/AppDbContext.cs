using Microsoft.EntityFrameworkCore;
using ProductWizardApp.Models;
using System.IO;

namespace ProductWizardApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Veritabanı dosyasının yolunu belirliyoruz
            // AppData içindeki canavar.com dizininde saklanacak
            string dbPath = Path.Combine(DataService.GetAppDataDir(), "products.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Table-Per-Hierarchy (TPH) Yapılandırması
            // Tüm ürünler tek bir tabloda tutulacak ve "ProductType" sütunu ile ayrıştırılacak.
            modelBuilder.Entity<Product>()
                .HasDiscriminator<string>("ProductType")
                .HasValue<CPU>("CPU")
                .HasValue<GPU>("GPU")
                .HasValue<RAM>("RAM")
                .HasValue<Motherboard>("Motherboard")
                .HasValue<Storage>("Storage")
                .HasValue<PC_Case>("PC_Case")
                .HasValue<PSU>("PSU")
                .HasValue<ProductWizardApp.Models.Monitor>("Monitor")
                .HasValue<Phone>("Phone")
                .HasValue<Tablet>("Tablet")
                .HasValue<Laptop>("Laptop")
                .HasValue<Television>("Television");

            base.OnModelCreating(modelBuilder);
        }
    }
}
