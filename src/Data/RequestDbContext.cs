using Cuby.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Cuby.Data
{
    public class RequestDbContext : DbContext
    {
        public DbSet<Request> Requests { get; set; }

        public RequestDbContext(DbContextOptions<RequestDbContext> options) : base(options)
        {
        }

    }
}