using System.ServiceModel;
using SharedBusinessData;

namespace SharedServiceContracts
{
    [ServiceContract]
    public interface IProductionService : IBaseEventingService
    {
        [OperationContract]
        void SetProcessingQuality(ProductionQuality quality);

        [OperationContract]
        void SetProcessingSpeed(ProductionSpeed speed);
    }
}
