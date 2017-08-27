using System.Collections.Generic;
using System.ServiceModel;
using SharedBusinessData;

namespace SharedServiceContracts
{
    [ServiceContract]
    public interface IOrderService : IBaseEventingService
    {
        [OperationContract]
        void RegisterNewOrder(Order order);

        [OperationContract]
        List<Order> GetAllOrdersWithStatus();
    }
}
