using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.PriceListAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class PriceItemConfiguration : IEntityTypeConfiguration<PriceItem>
    {
        public void Configure(EntityTypeBuilder<PriceItem> builder)
        {
            // builder.OwnsOne(pi => pi.ProductId, piid=>{piid.WithOwner();});
            builder.Property(p=>p.Price).HasColumnType("decimal(18,2)");
        }
    }
}