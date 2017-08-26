using System.ServiceModel;

namespace SharedServiceContracts
{
    [ServiceContract]
    public interface IOrderService
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
    }
}
