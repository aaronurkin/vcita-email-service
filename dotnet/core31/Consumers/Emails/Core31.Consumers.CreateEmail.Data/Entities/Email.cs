using Core31.Shared.Data;
using System;

namespace Core31.EventSubscribers.Emails.Data
{
    public class Email : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public string Address { get; set; }
    }
}
