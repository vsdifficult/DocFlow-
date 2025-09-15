using System.Reflection;
using DocFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocFlow.Infrastructure.Data.EntityFramework
{
    public class DocFlowDbContext : DbContext
    {
        public DocFlowDbContext(DbContextOptions<DocFlowDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}