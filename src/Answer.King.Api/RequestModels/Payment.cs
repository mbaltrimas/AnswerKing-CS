using System;

namespace Answer.King.Api.RequestModels
{
    public class MakePayment
    {
        public double Amount { get; set; }

        public Guid OrderId { get; set; }
    }
}