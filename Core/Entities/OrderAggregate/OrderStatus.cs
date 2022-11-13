using DocumentFormat.OpenXml;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.Entities.OrderAggregate
{
    public enum OrderStatus
    {
        //[EnumMember(Value = "Pending")]
        [EnumStringAttribute("Новый")]
        Pending,
        //[EnumMember(Value = "Payment Received")]
        [EnumStringAttribute("Оплачен")]
        PaymentReceived,
        //[EnumMember(Value = "Payment Failed")]
        [EnumStringAttribute("Не удалось оплатить")]
        PaymentFailed,
        //[EnumMember(Value = "Принят в обработку")]
        [EnumStringAttribute("Принят в обработку")]
        Accepted,

    }

    public class OrderStatusDto
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }

    }


    public class OrderStatuses
    {
        public List<OrderStatusDto> Statuses { get; set; }
        public OrderStatuses()
        {
            this.Statuses = new List<OrderStatusDto>();
            //this.Statuses = Enum.GetValues(typeof(OrderStatus))
            //    .Cast<OrderStatus>()
            //    .ToDictionary(t => (int)t, t => t.ToString());

            foreach (var status in Enum.GetValues(typeof(OrderStatus)))
            {
                EnumStringAttribute[] attribute = (EnumStringAttribute[])status
                    .GetType()
                    .GetField(status.ToString())
                    .GetCustomAttributes(typeof(EnumStringAttribute), false);

                Statuses.Add(new OrderStatusDto { Id = (int)status, Value = status.ToString(), Name = attribute[0].StringValue });
            }

        }

    }


    public class EnumStringAttribute : Attribute
    {
        public EnumStringAttribute(string stringValue)
        {
            this.stringValue = stringValue;
        }
        private string stringValue;
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }


}