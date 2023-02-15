using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class OrderDto
    {
        public string BasketId { get; set; }
        public string DeliveryMethodId { get; set; }
        public AddressDto ShipToAddress { get; set; }
    }
}