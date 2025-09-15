
using DocFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocFlow.Infrastructure.Data.EntityFramework.Configurations
{
    public class DocumentVersionConfiguration : IEntityTypeConfiguration<DocumentVersionEntity>
    {
        public void Configure(EntityTypeBuilder<DocumentVersionEntity> builder)
        {
            builder.ToTable("DocumentVersions");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.VersionNumber).IsRequired();

            builder.Property(v => v.FilePath).IsRequired();
        }
    }
}
