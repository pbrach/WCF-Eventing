using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SharedBusinessData
{
    [DataContract]
    public class ProductionBatch
    {
        public ProductionBatch()
        {
            ContainedProducts = new List<Product>();
            TotalProductionTime = TimeSpan.Zero;
        }

        [DataMember]
        public List<Product> ContainedProducts;

        [DataMember]
        public TimeSpan TotalProductionTime;
    }
}
