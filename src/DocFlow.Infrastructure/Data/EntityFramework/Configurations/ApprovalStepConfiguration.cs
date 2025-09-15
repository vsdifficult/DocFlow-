
using DocFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocFlow.Infrastructure.Data.EntityFramework.Configurations
{
    public class ApprovalStepConfiguration : IEntityTypeConfiguration<ApprovalStepEntity>
    {
        public void Configure(EntityTypeBuilder<ApprovalStepEntity> builder)
        {
            builder.ToTable("ApprovalSteps");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Step).IsRequired();

            builder.HasOne(a => a.Approver)
                .WithMany()
                .HasForeignKey(a => a.ApproverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
