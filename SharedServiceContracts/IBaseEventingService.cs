using System;
using System.ServiceModel;
using SharedBusinessData;

namespace SharedServiceContracts
{
    [ServiceContract]
    public interface IBaseEventingService
    {
        [OperationContract]
        void InitEventListening();

        [OperationContract]
        void HandleEvent(BaseEvent inEvent);

        [OperationContract]
        void RegisterListener(WcfEvents.EventName eventName, ServiceConfigurations.ServiceName listenerNam);
    }
}