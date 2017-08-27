using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace SharedServiceContracts
{
    public class IndividualConfiguration
    {
        public Type RealType; //TODO: having a reference like this to the real type here in contracts needs to be avoided!!! (for clean separation)

        public Type ContractType;

        public int PortNumber;

        public Uri HttpBaseAddress => new Uri($"http://localhost:{PortNumber}/{ContractType.Name.Remove(0, 1)}");
    }



    public static class ServiceConfigurations
    {
        public enum ServiceName
        {
            ProductionService,
            OrderService
        }

        public static Dictionary<ServiceName, IndividualConfiguration> ServiceConfigMapping =
            new Dictionary<ServiceName, IndividualConfiguration>
            {
                {ServiceName.ProductionService, new IndividualConfiguration
                {
                    ContractType = typeof(IProductionService),
                    PortNumber = 8081
                }},
                {ServiceName.OrderService, new IndividualConfiguration
                {
                    ContractType = typeof(IOrderService),
                    PortNumber = 8081
                }}
            };


        public static IBaseEventingService CreateEventingClient(ServiceName serviceName)
        {
            var serviceConf = ServiceConfigMapping[serviceName]; ;

            if (serviceConf == null)
            {
                throw new Exception("Tried to create a client to a not yet known Service");
            }


            var binding = new WSHttpBinding();
            var endpoint = new EndpointAddress(serviceConf.HttpBaseAddress);

            return new ChannelFactory<IBaseEventingService>(binding, endpoint).CreateChannel();
        }
    }
}
