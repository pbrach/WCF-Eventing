using System;
using SharedBusinessData.InstanceData;
using System.Collections.Generic;
using System.ServiceModel;

namespace SharedServiceContracts
{
    [ServiceContract]
    public abstract class BaseEventingService
    {
        private static readonly Dictionary<Type, List<Type>> ListenerIds = new Dictionary<Type, List<Type>>(); //EventType, ServiceType

        [OperationContract]
        public void RegisterListener(Type eventType, Type listenerType)
        {
            if (!eventType.IsSubclassOf(typeof(BaseEvent)))
            {
                throw new Exception("Invalid event registration");
            }

            if (!ListenerIds.ContainsKey(eventType))
            {
                ListenerIds[eventType] = new List<Type> {listenerType};
            }

            ListenerIds[eventType].Add(listenerType);
        }


        [OperationContract]
        public abstract void HandleEvent(BaseEvent inEvent);

        protected void FireEvent(BaseEvent newEvent)
        {
            var listenerForEvent = ListenerIds[newEvent.GetType()];

            foreach (var listenerType in listenerForEvent)
            {
                var listener = getListener(listenerType);
                listener.HandleEvent(newEvent);
            }
        }

        // TODO: implement getting the correct service from the ServiceType
        // Preconditions: Binding and Endpoint is known for each service
        private BaseEventingService getListener(Type serviceType)
        {
            return (BaseEventingService)new object();
        }
    }
}
