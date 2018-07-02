using System;

namespace Answer.King.Domain.Aggregate
{
    public interface IAggregateRoot
    {
        Guid Id { get; }
    }
}