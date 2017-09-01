using System;
using System.Collections.Generic;
using SharedBusinessData;

namespace SharedServiceContracts
{
    public abstract class BaseEventingService : IBaseEventingService
    {
        private static readonly Dictionary<WcfEvents.EventName, List<ServiceConfigurations.ServiceName>> ListenerIds =
            new Dictionary<WcfEvents.EventName, List<ServiceConfigurations.ServiceName>>(); //EventType, ServiceType

        private readonly Dictionary<Type, List<Action<BaseEvent>>> _handlerActions = new Dictionary<Type, List<Action<BaseEvent>>>();

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


        /// <summary>
        /// General EventHandler (more like an EventHub)
        /// </summary>
        public void HandleEvent(BaseEvent inEvent)
        {
            var eventType = inEvent.GetType();

            var handlerList = _handlerActions[eventType];

            foreach (var action in handlerList)
            {
                action.Invoke(inEvent);
            }
        }


        protected void ListenTo<TEvent>(
            ServiceConfigurations.ServiceName serviceName,
            WcfEvents.EventName eventName,
            Action<TEvent> handlingMethod) where TEvent : BaseEvent
        {
            RegisterMeAsListener(serviceName, eventName);

            var eventType = typeof(TEvent);
            List<Action<BaseEvent>> handlerList;

            if (!_handlerActions.ContainsKey(eventType))
            {
                handlerList = new List<Action<BaseEvent>>();
                _handlerActions[eventType] = handlerList;
            }
            else
            {
                handlerList = _handlerActions[eventType];
            }

            handlerList.Add(handlingMethod);
        }


        private void RegisterMeAsListener(ServiceConfigurations.ServiceName serviceName, WcfEvents.EventName eventName)
        {
            var otherService = ServiceConfigurations.CreateEventingClient(serviceName);

            var thisName = GetType().Name;

            ServiceConfigurations.ServiceName thisServiceEnum;
            if (Enum.TryParse(thisName, true, out thisServiceEnum))
            {
                thisServiceEnum = ServiceConfigurations.ServiceName.Undefined;
            }

            otherService.RegisterListener(eventName, thisServiceEnum);
        }
    }
}
