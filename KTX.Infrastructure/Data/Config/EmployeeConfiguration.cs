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
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employees>
    {
        public void Configure(EntityTypeBuilder<Employees> builder)
        {
            builder.Property(x => x.Id)
                   .IsRequired()
                   .HasDefaultValueSql("NEWSEQUENTIALID()");
            builder.Property(x => x.FullName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(100).IsRequired();
            builder.HasOne(x => x.Departments)
                   .WithMany(x => x.Employees)
                   .HasForeignKey(x => x.DeptId)
                   .OnDelete(DeleteBehavior.Cascade);

            
        }
    }
}
