using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {

            builder.OwnsOne(o => o.ShipToAddress, a =>
            {
                a.WithOwner();
            });

            builder.HasOne(s => s.Status).WithMany().HasForeignKey(o => o.StatusId).OnDelete(DeleteBehavior.Cascade);
            // builder.HasOne(s => s.Status).WithMany(s=>s.Orders).HasForeignKey(o => o.StatusId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}