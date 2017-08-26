using System.Runtime.Serialization;

namespace SharedBusinessData.InstanceData
{
    [DataContract]
    public enum ProcessingQuality
    {
        High,
        Medium,
        Low
    }

    [DataContract]
    public enum ProcessingSpeed
    {
        Fast,
        Medium,
        Slow
    }
}
