using Bulky.DataAccess.Data;
using Bulky.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyWebApi.Test
{
    public class InMemoryDbContext : ApplicationDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        }
        
        public override void Dispose()
        {
            Database.EnsureDeleted();
            base.Dispose();
        }
    }
}