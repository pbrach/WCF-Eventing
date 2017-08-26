using System;
using System.Runtime.Serialization;

namespace SharedBusinessData.InstanceData
{
    [DataContract]
    [KnownType(typeof(NewOrderPublished))]
    public abstract class BaseEvent
    {}

    [DataContract]
    public class NewOrderPublished : BaseEvent
    {
        [DataMember]
        public ProcessingQuality Quality;

        [DataMember]
        public ProcessingSpeed Speed;

        [DataMember]
        public int Quantity;
    }

    [DataContract]
    public class OrderFinished : BaseEvent
    {
        [DataMember]
        public TimeSpan TimeNeeded;

        [DataMember]
        public int BatchId;

        [DataMember]
        public string ProductionLineId;
    }

    [DataContract]
    public class SubOrderFinished : BaseEvent
    {
        [DataMember]
        public TimeSpan TimeNeeded;

        [DataMember]
        public ProcessingQuality Quality;

        [DataMember]
        public int ProductId;
    }
}
