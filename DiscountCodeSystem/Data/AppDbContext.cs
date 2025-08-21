using DiscountCodeSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DiscountCodeSystem.Data
{

    public class AppDbContext : DbContext
    {
        public DbSet<DiscountCode> DiscountCodes => Set<DiscountCode>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}

 
