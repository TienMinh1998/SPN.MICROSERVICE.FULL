using DatabaseCore.Domain.SeedWork;
using DatabaseCore.Infrastructure.ConfigurationEFContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCore.Infrastructure
{
    public static class MediatorExtension
    {
        public static async Task DispatDomain(this IMediator mediator, EFContext ctx)
        {
            var entyties = ctx.ChangeTracker.Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = entyties.SelectMany(x => x.Entity.DomainEvents)
                .ToList();
            foreach (var entity in domainEvents)
            {
                await mediator.Publish(entity);
            }
        }
    }
}
