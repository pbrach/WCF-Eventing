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
            ServiceConfigurations.ServiceConfigMapping[ServiceConfigurations.ServiceName.ProductionService].RealType = typeof(ProductionService.ProductionService);
            ServiceConfigurations.ServiceConfigMapping[ServiceConfigurations.ServiceName.OrderService].RealType = typeof(OrderService.OrderService);

            try
            {
                foreach (var dictItem in ServiceConfigurations.ServiceConfigMapping)
                {
                    StartHosting(dictItem.Value);
                }

                foreach (var dictItem in ServiceConfigurations.ServiceConfigMapping)
                {
                    var service = ServiceConfigurations.CreateEventingClient(dictItem.Key);
                    service.InitEventListening();
                }

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("There is an issue with a service: " + ex.Message);
                Console.ReadKey();
            }
        }

        private static void StartHosting(IndividualConfiguration conf)
        {
            //Instantiate ServiceHost
            var serviceHost = new ServiceHost(conf.RealType, conf.HttpBaseAddress);

            //Add Endpoint to Host
            serviceHost.AddServiceEndpoint(conf.ContractType, new WSHttpBinding(), "");

            //Metadata Exchange
            serviceHost.Description.Behaviors.Add(new ServiceMetadataBehavior
            {
                HttpGetEnabled = true
            });

            //Open
            serviceHost.Open();
            Console.WriteLine("Service is running at: {0}", conf.HttpBaseAddress);
        }
    }
}
