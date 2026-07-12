using Microsoft.EntityFrameworkCore;
using PurchaseBillApi.Models;

namespace PurchaseBillApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<LocationDetail> Location_Details { get; set; }
    }
}
