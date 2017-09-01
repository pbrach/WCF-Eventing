﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using SharedBusinessData;
using SharedServiceContracts;

namespace OrderService
{
    [ServiceBehavior(
        ConcurrencyMode = ConcurrencyMode.Multiple,
        InstanceContextMode = InstanceContextMode.Single
    )]
    public class OrderService : BaseEventingService, IOrderService
    {
        private static readonly List<Order> _registeredOrders = new List<Order>();

        public void RegisterNewOrder(Order order)
        {
            Console.WriteLine("OrderSevice, Call to RegisterNewOrder");

            _registeredOrders.Add(order);
            this.FireEvent(new NewOrderAccepted
            {
                InOrder = order
            });
        }

        public List<Order> GetAllOrdersWithStatus()
        {
            Console.WriteLine("OrderSevice, Call to GetAllOrdersWithStatus");
            return _registeredOrders;
        }


        public override void InitEventListening()
        {
            ListenTo(ServiceConfigurations.ServiceName.OrderService, WcfEvents.EventName.OrderFinished, HandleOrderFinished);
            ListenTo(ServiceConfigurations.ServiceName.OrderService, WcfEvents.EventName.ProductFinished, HandleSubOrderFinished);
        }

        // Handler for the OrderFinished Event
        private void HandleOrderFinished(BaseEvent inEvent)
        {
            var orderFinishedEvent = (OrderFinished)inEvent;
            var orderToUpate = _registeredOrders.First(x => x.Id == orderFinishedEvent.OriginalOrderId);
            orderToUpate.Status = OrderStatus.Finished;
            Console.WriteLine("OrderSevice, Handling OrderFinished");
        }

        // Handler for the ProductFinished Event
        private void HandleSubOrderFinished(BaseEvent inEvent)
        {
            var productFinishedEvent = (ProductFinished)inEvent;
            Console.WriteLine("OrderSevice, Handling ProductFinished");
            var orderToUpate = _registeredOrders.First(x => x.Id == productFinishedEvent.OriginalOrderId);

            if (orderToUpate.Status == OrderStatus.New)
            {
                orderToUpate.Status = OrderStatus.Processing;
            }
        }
    }
}
