using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.PriceListAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class PriceConfiguration : IEntityTypeConfiguration<Price>
    {
        public void Configure(EntityTypeBuilder<Price> builder)
        {
            // builder.OwnsOne(pi => pi.ProductId, piid=>{piid.WithOwner();});
            builder.ToTable("ProductPrices");
            builder.Property(p=>p.Value).HasColumnType("decimal(18,2)");
            builder.HasOne(p=>p.PriceType).WithMany().HasForeignKey(pt=>pt.PriceTypeId);
        }
    }
}