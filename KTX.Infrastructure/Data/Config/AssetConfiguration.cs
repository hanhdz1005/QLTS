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
    public class AssetConfiguration : IEntityTypeConfiguration<Assets>
    {
        public void Configure(EntityTypeBuilder<Assets> builder)
        {
            builder.Property(x => x.Id)
                   .IsRequired()
                   .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Status).IsRequired();

           
        }
    }

   
}
