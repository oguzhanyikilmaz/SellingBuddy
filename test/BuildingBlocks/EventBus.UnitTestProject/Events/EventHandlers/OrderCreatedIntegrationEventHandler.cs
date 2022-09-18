﻿using EventBus.Base.Abstraction;
using EventBus.UnitTestProject.Events.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.UnitTestProject.Events.EventHandlers
{
    public class OrderCreatedIntegrationEventHandler : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
    {
        public Task Handle(OrderCreatedIntegrationEvent @event)
        {
            Console.WriteLine("Handle method worked with id :"+@event.Id);
            return Task.CompletedTask;
        }
    }
}
