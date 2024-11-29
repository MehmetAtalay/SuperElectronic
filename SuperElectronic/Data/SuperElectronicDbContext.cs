using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SuperElectronic.Models;

namespace SuperElectronic.Data
{
    public class SuperElectronicDbContext:DbContext
    {
        public SuperElectronicDbContext(DbContextOptions dbContextOptions) :base(dbContextOptions)
        {
           
        }
        public DbSet<Product> Products { get; set; }
    }
}
