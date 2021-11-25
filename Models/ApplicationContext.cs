using Microsoft.EntityFrameworkCore;

namespace Rocket_Elevators_REST_API.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<Elevator> elevators { get; set; }
    }
}