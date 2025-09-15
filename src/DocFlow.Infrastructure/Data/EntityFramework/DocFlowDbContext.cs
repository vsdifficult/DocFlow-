
using System.Reflection;
using DocFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocFlow.Infrastructure.Data.EntityFramework
{
    public class DocFlowDbContext : DbContext
    {
        public DocFlowDbContext(DbContextOptions<DocFlowDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; } = null!;
        public DbSet<DocumentEntity> Documents { get; set; } = null!;
        public DbSet<DocumentVersionEntity> DocumentVersions { get; set; } = null!;
        public DbSet<ApprovalStepEntity> ApprovalSteps { get; set; } = null!;
        public DbSet<AuditLogEntity> AuditLogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
