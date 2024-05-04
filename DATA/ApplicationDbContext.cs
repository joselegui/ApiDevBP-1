using ApiDevBP.Entities;
using ApiDevBP.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ApiDevBP.DATA
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
                
        }

        public DbSet<UserEntity> userEntities { get; set; }
    }
}
