using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using SharedServiceContracts;

namespace Host
{
    public class Host
    {
        public static void Main(string[] args)
        {
            ServiceConfigurations.registeredServices.Find(x => x.ContractType == typeof(IProductionService)).RealType = typeof(ProductionService.ProductionService);
            ServiceConfigurations.registeredServices.Find(x => x.ContractType == typeof(IOrderService)).RealType = typeof(OrderService.OrderService);

            try
            {
                foreach (var serviceConf in ServiceConfigurations.registeredServices)
                {
                    HostService(serviceConf);
                }

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("There is an issue with a service: " + ex.Message);
                Console.ReadKey();
            }
        }

        private static void HostService(IndividualConfiguration conf)
        {
            //Instantiate ServiceHost
            var serviceHost = new ServiceHost(conf.RealType, conf.HttpBaseAddress);

            //Add Endpoint to Host
            serviceHost.AddServiceEndpoint(conf.ContractType, new WSHttpBinding(), "");

            //Metadata Exchange
            ServiceMetadataBehavior serviceBehavior = new ServiceMetadataBehavior();
            serviceBehavior.HttpGetEnabled = true;
            serviceHost.Description.Behaviors.Add(serviceBehavior);

            //Open
            serviceHost.Open();
            Console.WriteLine("Service is running at: {0}", conf.HttpBaseAddress);
        }
    }
}
