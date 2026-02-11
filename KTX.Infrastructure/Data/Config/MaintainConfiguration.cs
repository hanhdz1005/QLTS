using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QLTS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Infrastructure.Data.Config
{
    public class MaintainConfiguration : IEntityTypeConfiguration<Maintain>
    {
        public void Configure(EntityTypeBuilder<Maintain> builder)
        {
            builder.Property(x => x.Id)
                .IsRequired()
                .HasDefaultValueSql("NEWSEQUENTIALID()");
            builder.Property(x => x.RequestDate).IsRequired();
            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.EndDate).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.TotalCost).HasColumnType("decimal(18,3)").IsRequired();
           
            builder.HasOne(x => x.Asset)
                .WithMany(a => a.Maintain)
                .HasForeignKey(x => x.AssetId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Category)
                .WithMany(c => c.Maintain)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Employee)
                .WithMany(e => e.Maintain)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
