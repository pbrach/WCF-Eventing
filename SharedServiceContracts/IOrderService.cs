using System.ServiceModel;

namespace SharedServiceContracts
{
    [ServiceContract]
    public interface IOrderService
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataContract(CompositeType composite);
    }
}
