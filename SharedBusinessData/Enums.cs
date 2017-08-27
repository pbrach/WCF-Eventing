using System.Runtime.Serialization;

namespace SharedBusinessData
{
    [DataContract]
    public enum ProductionQuality
    {
        [EnumMember]
        High,
        [EnumMember]
        Medium,
        [EnumMember]
        Low,
        [EnumMember]
        Undefined
    }

    [DataContract]
    public enum ProductionSpeed
    {
        [EnumMember]
        Fast,
        [EnumMember]
        Medium,
        [EnumMember]
        Slow,
        [EnumMember]
        Undefined
    }

    [DataContract]
    public enum OrderStatus
    {
        [EnumMember]
        New,
        [EnumMember]
        Processing,
        [EnumMember]
        Finished,
        [EnumMember]
        Undefined
    }
}
