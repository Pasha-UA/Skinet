using System.Runtime.Serialization;

namespace Core.Entities.OrderAggregate
{
    public enum OrderStatus
    {
        [EnumMember(Value ="Pending")]
        Pending,
        [EnumMember(Value ="Payment Received")]
        PaymentReceived,
        [EnumMember(Value ="Payment Failed")]
        PaymentFailed

    }

    // public class OrderStatuss
    // {
    //     public string Value {get; set;}
    //     public string Name {get; set;}
    // }

}