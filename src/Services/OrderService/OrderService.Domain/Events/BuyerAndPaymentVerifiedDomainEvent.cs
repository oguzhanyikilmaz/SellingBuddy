using MediatR;
using OrderService.Domain.AggregateModels.BuyerAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Events
{
   public class BuyerAndPaymentVerifiedDomainEvent : INotification
    {
        public Buyer Buyer { get; private set; }
        public PaymentMethod Payment { get; private set; }
        public Guid OrderId { get; private set; }
        public BuyerAndPaymentVerifiedDomainEvent(Buyer buyer, PaymentMethod payment, Guid orderId)
        {
            Buyer = buyer;
            Payment = payment;
            OrderId = orderId;
        }
    }
}
