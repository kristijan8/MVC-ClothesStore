using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace cstore.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<cstoreContext>
    {
        public cstoreContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<cstoreContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=cstore.Data;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new cstoreContext(optionsBuilder.Options);
        }
    }
}
