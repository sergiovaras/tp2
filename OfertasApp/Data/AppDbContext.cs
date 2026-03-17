using Microsoft.EntityFrameworkCore;
using OfertasApp.Models;

namespace OfertasApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Oferta> Ofertas { get; set; }
    }
}

