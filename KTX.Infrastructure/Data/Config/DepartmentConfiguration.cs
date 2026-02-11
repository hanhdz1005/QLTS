using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using QLTS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Infrastructure.Data.Config
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Departments>
    {
        public void Configure(EntityTypeBuilder<Departments> builder)
        {
            builder.Property(x => x.Id)
                   .IsRequired()
                   .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.Property(x => x.Name).HasMaxLength(64).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(100);

           
        }
    }
}
