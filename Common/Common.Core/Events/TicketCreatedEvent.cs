using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Core.Events
{
    public class TicketCreatedEvent : BaseEvent
    {
        public TicketCreatedEvent() : base(nameof(TicketCreatedEvent))
        { 
        
        }

        public required string UserName { get; set; }
        public string? TypeError { get; set; }
        public required string DetailError { get; set; }
    }
}
