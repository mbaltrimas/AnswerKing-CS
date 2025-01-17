﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Answer.King.Domain.Orders;
using Answer.King.Domain.Repositories;
using Answer.King.Infrastructure.SeedData;
using LiteDB;

namespace Answer.King.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    public OrderRepository(ILiteDbConnectionFactory connections)
    {
        var db = connections.GetConnection();

        this.Collection = db.GetCollection<Order>();
        this.Collection.EnsureIndex("LineItems.Product._id");
    }

    private ILiteCollection<Order> Collection { get; }

    public Task<IEnumerable<Order>> Get()
    {
        return Task.FromResult(this.Collection.FindAll());
    }

    public Task<Order?> Get(long id)
    {
        return Task.FromResult(this.Collection.FindOne(c => c.Id == id))!;
    }

    public Task Save(Order item)
    {
        return Task.FromResult(this.Collection.Upsert(item));
    }
}
