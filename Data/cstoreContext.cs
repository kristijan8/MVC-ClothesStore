using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using cstore.Models;
using cstore.Areas.Identity.Data;



namespace cstore.Data
{
    public class cstoreContext : IdentityDbContext<cstoreUser>
    {
        public cstoreContext (DbContextOptions<cstoreContext> options)
            : base(options)
        {
        }

        public DbSet<cstore.Models.Brand> Brand { get; set; } = default!;

        public DbSet<cstore.Models.Category>? Category { get; set; }

        public DbSet<cstore.Models.Product>? Product { get; set; }

        public DbSet<cstore.Models.Reviews>? Reviews { get; set; }

        public DbSet<cstore.Models.Users>? Users { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
