using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.PriceListAggregate
{
    public class OfferItem : BaseEntity
    {

        public string Name { get; set; }

        public string Description { get; set; }

        // public decimal? RetailPrice { get; set; }

        // public string RetailPriceCurrencyId { get; set; }

        public List<Price> Prices { get; set; }
        // public Price BulkPrice { get; set; } // оптовая цена

        public string CategoryId { get; set; }

        public string BarCode { get; set; } // код товара в 1с

        public string Available { get; set; }

        public string Presence { get; set; } // наличие

        public string[] SearchStrings { get; set; }

        public int? QuantityInStock { get; set; }

        public ParameterItem[] Parameters { get; set; }

        public bool Visible { get; set; } = true;
        public DateTime DeletedOn { get; set; } = DateTime.MinValue;


        // public string SellingType { get; set; }

        //... add necessary fields

    }

    public class ParameterItem : BaseEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }


}