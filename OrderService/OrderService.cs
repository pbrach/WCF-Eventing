using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using SharedBusinessData;
using SharedServiceContracts;
using ServiceName = SharedServiceContracts.ServiceConfigurations.ServiceName;

namespace OrderService
{
    [ServiceBehavior(
        ConcurrencyMode = ConcurrencyMode.Multiple,
        InstanceContextMode = InstanceContextMode.Single
    )]
    public class OrderService : BaseEventingService, IOrderService
    {
        private static readonly List<Order> RegisteredOrders = new List<Order>();

        public void RegisterNewOrder(Order order)
        {
            Console.WriteLine("OrderSevice, Call to RegisterNewOrder");

            RegisteredOrders.Add(order);
            FireEvent(new NewOrderAccepted
            {
                InOrder = order
            });
        }

        public List<Order> GetAllOrdersWithStatus()
        {
            Console.WriteLine("OrderSevice, Call to GetAllOrdersWithStatus");
            return RegisteredOrders;
        }


        protected override void InnerInitEventListening()
        {
            ListenTo<OrderFinished>(ServiceName.ProductionService, HandleOrderFinished);
            ListenTo<ProductFinished>(ServiceName.ProductionService, HandleSubOrderFinished);
        }

        // Handler for the OrderFinished Event
        private void HandleOrderFinished(BaseEvent inEvent)
        {
            var orderFinishedEvent = (OrderFinished)inEvent;
            Console.WriteLine("OrderSevice, Handling OrderFinished");

            var orderToUpate = RegisteredOrders.First(x => x.Id == orderFinishedEvent.OriginalOrderId);
            orderToUpate.Status = OrderStatus.Finished;

        }

        // Handler for the ProductFinished Event
        private void HandleSubOrderFinished(BaseEvent inEvent)
        {
            var productFinishedEvent = (ProductFinished)inEvent;
            Console.WriteLine("OrderSevice, Handling ProductFinished");

            var orderToUpate = RegisteredOrders.First(x => x.Id == productFinishedEvent.OriginalOrderId);

            if (orderToUpate.Status == OrderStatus.New)
            {
                orderToUpate.Status = OrderStatus.Processing;
            }
        }
    }
}
