using System;
using Answer.King.Domain;

namespace Answer.King.Api.ViewModels
{
    public class MakePayment
    {
        public double Amount { get; set; }

        public Guid OrderId { get; set; }
    }
}