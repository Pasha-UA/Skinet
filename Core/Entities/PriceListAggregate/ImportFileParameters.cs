using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.PriceListAggregate
{
    public enum NotFoundProduct { Ignore, Delete, Hide, NoStock }
    public enum NotFoundPriceType { Ignore, Delete }
    
    public class ImportFileParameters
    {
        public NotFoundProduct NotFoundProduct { get; } = NotFoundProduct.NoStock;
        public NotFoundPriceType NotFoundPriceType { get; } = NotFoundPriceType.Ignore;

    }

}
