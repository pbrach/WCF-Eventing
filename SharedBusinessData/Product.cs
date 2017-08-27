using System;
using System.Runtime.Serialization;

namespace SharedBusinessData
{
    [DataContract]
    public class Product
    {
        [DataMember]
        public TimeSpan ProductionTime;

        [DataMember]
        public ProductionQuality Quality;

        [DataMember]
        public string ProductId;

        public Product()
        {
            ProductionTime = TimeSpan.Zero;
            Quality = ProductionQuality.Undefined;
            ProductId = "Undefined";
        }
    }
}
