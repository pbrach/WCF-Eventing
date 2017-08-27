using System;
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





        /// <summary>
        /// General EventHandler (more like an EventHub)
        /// </summary>
        public override void HandleEvent(BaseEvent inEvent)
        {
            if (inEvent is ProductFinished)
            {
                HandleSubOrderFinishedd((ProductFinished)inEvent);
            }
            else if (inEvent is OrderFinished)
            {
                HandleOrderFinished((OrderFinished)inEvent);
            }
        }

        // Handler for the OrderFinished Event
        private void HandleOrderFinished(OrderFinished orderFinishedEvent)
        {
            var orderToUpate = _registeredOrders.First(x => x.Id == orderFinishedEvent.OriginalOrderId);
            orderToUpate.Status = OrderStatus.Finished;
            Console.WriteLine("OrderSevice, Handling OrderFinished");
        }

        // Handler for the ProductFinished Event
        private void HandleSubOrderFinishedd(ProductFinished productFinishedEvent)
        {
            Console.WriteLine("OrderSevice, Handling ProductFinished");
            var orderToUpate = _registeredOrders.First(x => x.Id == productFinishedEvent.OriginalOrderId);

            if (orderToUpate.Status == OrderStatus.New)
            {
                orderToUpate.Status = OrderStatus.Processing;
            }
        }
    }
}
