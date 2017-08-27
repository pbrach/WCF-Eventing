using System;
using System.ServiceModel;
using SharedBusinessData;
using SharedServiceContracts;

namespace ServiceEventsWcf
{
    class Program
    {
        static void Main(string[] args)
        {
            var productionClient = (IProductionService)CreateClient(ServiceConfigurations.ServiceName.ProductionService);
            productionClient.SetProcessingQuality(ProductionQuality.Medium);
            productionClient.SetProcessingSpeed(ProductionSpeed.Fast);

            var orderClient = (IOrderService)CreateClient(ServiceConfigurations.ServiceName.OrderService);
            orderClient.RegisterNewOrder(new Order
            {
                Id = 1,
                Status = OrderStatus.New,
                Quality = ProductionQuality.Medium,
                Speed = ProductionSpeed.Fast,
                Quantity = 5
            });


            Console.ReadLine();
        }


        public static object CreateClient(ServiceConfigurations.ServiceName serviceName)
        {
            var serviceConf = ServiceConfigurations.ServiceConfigMapping[serviceName]; ;

            if (serviceConf == null)
            {
                throw new Exception("Tried to create a client to a not yet known Service");
            }


            var binding = new WSHttpBinding();
            var endpoint = new EndpointAddress(serviceConf.HttpBaseAddress);

            if (serviceName == ServiceConfigurations.ServiceName.OrderService)
                return new ChannelFactory<IOrderService>(binding, endpoint).CreateChannel();

            if (serviceName == ServiceConfigurations.ServiceName.ProductionService)
                return new ChannelFactory<IProductionService>(binding, endpoint).CreateChannel();

            return null;
        }
    }
}
