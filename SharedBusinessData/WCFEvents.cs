using System;
using System.Runtime.Serialization;

namespace SharedBusinessData
{


    public static class WcfEvents
    {
        public enum EventName
        {
            NewOrderAccepted,
            OrderFinished,
            ProductFinished,
            Undefined
        }

        public static Type GetEventType(EventName eventName)
        {
            switch (eventName)
            {
                case EventName.NewOrderAccepted:
                    return typeof(NewOrderAccepted);

                case EventName.OrderFinished:
                    return typeof(OrderFinished);

                case EventName.ProductFinished:
                    return typeof(ProductFinished);

                default:
                    return null;
            }
        }

        public static EventName GetEventName(BaseEvent inevent)
        {
            EventName result;

            var success = Enum.TryParse(inevent.GetType().Name, true, out result);

            return success ? result : EventName.Undefined;
        }

        public static EventName GetEventName(Type intype)
        {
            EventName result;

            var success = Enum.TryParse(intype.Name, true, out result);

            return success ? result : EventName.Undefined;
        }

    }



    [DataContract]
    [KnownType(typeof(NewOrderAccepted))]
    [KnownType(typeof(OrderFinished))]
    [KnownType(typeof(ProductFinished))]
    public abstract class BaseEvent
    { }

    [DataContract]
    public class NewOrderAccepted : BaseEvent
    {
        [DataMember]
        public Order InOrder;
    }

    [DataContract]
    public class OrderFinished : BaseEvent
    {
        [DataMember]
        public ProductionBatch FinishedBatch;

        [DataMember]
        public int OriginalOrderId;
    }

    [DataContract]
    public class ProductFinished : BaseEvent
    {
        [DataMember]
        public Product FinishedProduct;

        [DataMember]
        public int OriginalOrderId;
    }
}
