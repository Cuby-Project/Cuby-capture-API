using Cuby.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Cuby.Data
{
    public class RequestDbContext(DbContextOptions<RequestDbContext> options) : DbContext(options)
    {
        public DbSet<Request> Requests { get; set; }
    }
}