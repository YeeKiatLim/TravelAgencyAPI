using TravelAgencyAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace TravelAgencyAPI
{
    public class ToursDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public ToursDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder
        optionsBuilder)
        {
            string? connectionString = _configuration.GetConnectionString(
            "MyConnection");
            if (connectionString != null)
            {
                optionsBuilder.UseMySQL(connectionString);
            }
        }
        public DbSet<Tour> Tours { get; set; }
    }
}
