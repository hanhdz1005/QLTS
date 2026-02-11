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
    public class CategoryConfiguration : IEntityTypeConfiguration<Categories>
    {
        public void Configure(EntityTypeBuilder<Categories> builder)
        {
            builder.Property(x => x.Id)
                   .IsRequired()
                   .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.Property(x => x.Name).HasMaxLength(100);

            
        }
    }
}
