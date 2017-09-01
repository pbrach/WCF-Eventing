using System;
using System.Collections.Generic;
using SharedBusinessData;

namespace SharedServiceContracts
{
    public abstract class BaseEventingService : IBaseEventingService
    {
        private static readonly Dictionary<WcfEvents.EventName, List<ServiceConfigurations.ServiceName>> ListenerIds =
            new Dictionary<WcfEvents.EventName, List<ServiceConfigurations.ServiceName>>(); //EventType, ServiceType

        public void RegisterListener(WcfEvents.EventName eventName, ServiceConfigurations.ServiceName listenerName)
        {
            Console.WriteLine("{0}: listens to: [{2}] and [{1}Event]", listenerName, eventName, GetType().Name);
            var eventType = WcfEvents.GetEventType(eventName);
            if (!eventType.IsSubclassOf(typeof(BaseEvent)))
            {
                throw new Exception("Invalid event registration");
            }

            if (!ListenerIds.ContainsKey(eventName))
            {
                ListenerIds[eventName] = new List<ServiceConfigurations.ServiceName> { listenerName };
                return;
            }

            ListenerIds[eventName].Add(listenerName);
        }

        public abstract void InitEventListening();

        public abstract void HandleEvent(BaseEvent inEvent);

        protected void FireEvent(BaseEvent newEvent)
        {
            var eventName = WcfEvents.GetEventName(newEvent);
            Console.WriteLine("{1}: Firing event {0}", eventName, GetType().Name);

            var listeningServices = ListenerIds[eventName];

            foreach (var serviceName in listeningServices)
            {
                var listener = ServiceConfigurations.CreateEventingClient(serviceName);
                listener.HandleEvent(newEvent);
            }
        }
    }
}
