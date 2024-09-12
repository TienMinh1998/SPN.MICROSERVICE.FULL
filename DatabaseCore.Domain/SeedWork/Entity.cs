using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCore.Domain.SeedWork
{
    public abstract class Entity
    {
        int? _requestedHashCode;
        int _Id;

        public virtual int Id
        {
            get
            {
                return _Id;
            }
            protected set
            {
                _Id = value;

            }
        }

        private List<INotification> _domainEvents;
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(INotification domainEvent)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(domainEvent);
        }















    }
}
