using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace ProductionService
{
    public class Host
    {
        public static void Main(string[] args)
        {
            ServiceHost productionServiceHost = null;
            try
            {
                //Base Address for StudentService
                Uri httpBaseAddress = new Uri("http://localhost:4321/ProductionService");

                //Instantiate ServiceHost
                productionServiceHost = new ServiceHost(typeof(ProductionService),httpBaseAddress);

                //Add Endpoint to Host
                productionServiceHost.AddServiceEndpoint(typeof(SharedServiceContracts.IProductionService),new WSHttpBinding(), "");

                //Metadata Exchange
                ServiceMetadataBehavior serviceBehavior = new ServiceMetadataBehavior();
                serviceBehavior.HttpGetEnabled = true;
                productionServiceHost.Description.Behaviors.Add(serviceBehavior);

                //Open
                productionServiceHost.Open();
                Console.WriteLine("Service is live now at: {0}", httpBaseAddress.ToString());
                Console.ReadKey();
        }
        catch (Exception ex)
        {
            productionServiceHost = null;
            Console.WriteLine("There is an issue with StudentService" + ex.Message);
        }
}
    }
}
