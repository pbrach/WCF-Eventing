using System.Runtime.Serialization;

namespace SharedBusinessData
{
    [DataContract]
    public class Order
    {
        [DataMember] public int Id;

        [DataMember]
        public ProductionQuality Quality;

        [DataMember]
        public ProductionSpeed Speed;

        [DataMember]
        public int Quantity;

        [DataMember]
        public OrderStatus Status;

        public Order()
        {
            Id = 0;
            Quality = ProductionQuality.Undefined;
            Speed = ProductionSpeed.Undefined;
            Quantity = 0;
            Status = OrderStatus.Undefined;
        }
    }
}
