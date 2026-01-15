using Microsoft.EntityFrameworkCore;

namespace ZgjedhjetApi.Data
{
    // YOUR CODE HERE
    public class LifeDbContext : DbContext
    {
        public LifeDbContext(DbContextOptions<LifeDbContext> options) : base(options)
        {
        }
    }
}
