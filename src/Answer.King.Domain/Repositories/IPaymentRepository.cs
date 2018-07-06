﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Answer.King.Domain.Repositories.Models;

namespace Answer.King.Domain.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> Get(Guid id);

        Task<IEnumerable<Payment>> Get();

        Task Add(Payment payment);
    }
}