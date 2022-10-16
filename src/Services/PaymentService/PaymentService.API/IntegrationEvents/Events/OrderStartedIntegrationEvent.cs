using EventBus.Base.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.API.IntegrationEvents.Events
{
    public class OrderStartedIntegrationEvent:IntegrationEvent
    {
        public int OrderId { get; set; }
        public OrderStartedIntegrationEvent()
        {

        }
        public OrderStartedIntegrationEvent(int orderId)
        {
            OrderId = orderId;
        }
    }
}
