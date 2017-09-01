using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using SharedBusinessData;
using SharedServiceContracts;

namespace ProductionService
{
    [ServiceBehavior(
        ConcurrencyMode = ConcurrencyMode.Single,
        InstanceContextMode = InstanceContextMode.Single
    )]
    public class ProductionService : BaseEventingService, IProductionService
    {
        private ProductionQuality _selectedQuality;
        private ProductionSpeed _selectedSpeed;

        public ProductionService()
        {
            _selectedQuality = ProductionQuality.Undefined;
            _selectedSpeed = ProductionSpeed.Undefined;
        }

        public void SetProcessingQuality(ProductionQuality quality)
        {
            _selectedQuality = quality;
            Console.WriteLine("Production Service, Changed Quality to: " + _selectedQuality);
        }

        public void SetProcessingSpeed(ProductionSpeed speed)
        {
            _selectedSpeed = speed;
            Console.WriteLine("Production Service, Changed Speed to: " + _selectedSpeed);
        }

        public override void InitEventListening()
        {
            ListenTo<NewOrderAccepted>(ServiceConfigurations.ServiceName.ProductionService, WcfEvents.EventName.NewOrderAccepted, HandleNewOrderAccepted);
        }


        private void HandleNewOrderAccepted(NewOrderAccepted newOrderAcceptedEvent)
        {
            Console.WriteLine("Production Service, Handling 'NewOrderAccepted' event");

            if (!CanProcessOrder(newOrderAcceptedEvent.InOrder))
            {
                return;
            }

            var batch = ProduceOrder(newOrderAcceptedEvent.InOrder);

            FireEvent(new OrderFinished
            {
                OriginalOrderId = newOrderAcceptedEvent.InOrder.Id,
                FinishedBatch = batch
            });
        }

        // The production line only processes an order if it is correctly configured for that job
        private bool CanProcessOrder(Order inEventInOrder)
        {
            return
                inEventInOrder.Quality == _selectedQuality &&
                inEventInOrder.Speed == _selectedSpeed;
        }


        private ProductionBatch ProduceOrder(Order InOrder)
        {
            var products = new List<Product>();
            for (var i = 1; i <= InOrder.Quantity; i++)
            {
                products.Add(ProduceNewProduct(InOrder.Id));
            }

            return new ProductionBatch
            {
                TotalProductionTime = products.Aggregate(TimeSpan.Zero, (span, product) => span + product.ProductionTime),
                ContainedProducts = products
            };
        }

        private Product ProduceNewProduct(int orderId)
        {
            var neededTime = GetCurrentProductionTime();
            Thread.Sleep(neededTime);

            var newProduct = new Product
            {
                Quality = _selectedQuality,
                ProductionTime = GetCurrentProductionTime(),
                ProductId = _selectedQuality.ToString() + _selectedSpeed
            };

            FireEvent(new ProductFinished
            {
                OriginalOrderId = orderId,
                FinishedProduct = newProduct
            });

            return newProduct;
        }

        private TimeSpan GetCurrentProductionTime()
        {
            switch (_selectedSpeed)
            {
                case ProductionSpeed.Fast:
                    return TimeSpan.FromMilliseconds(500);
                case ProductionSpeed.Medium:
                    return TimeSpan.FromMilliseconds(1500);
                case ProductionSpeed.Slow:
                    return TimeSpan.FromMilliseconds(3000);
                default:
                    throw new Exception("No valid production speed was selected");
            }
        }
    }
}
