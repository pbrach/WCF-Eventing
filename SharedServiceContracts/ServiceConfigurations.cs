using System;
using System.Collections.Generic;

namespace SharedServiceContracts
{
    public class IndividualConfiguration
    {
        public Type RealType;

        public Type ContractType;

        public int PortNumber;

        public Uri HttpBaseAddress => new Uri($"http://localhost:{PortNumber}/{ContractType.Name.Remove(0, 1)}");
    }
    public static class ServiceConfigurations
    {
        public static List<IndividualConfiguration> registeredServices = new List<IndividualConfiguration>
        {
            new IndividualConfiguration
            {
                ContractType = typeof(IProductionService),
                PortNumber = 8081
            },
            new IndividualConfiguration
            {
                ContractType = typeof(IOrderService),
                PortNumber = 8082
            }
        };
    }
}
