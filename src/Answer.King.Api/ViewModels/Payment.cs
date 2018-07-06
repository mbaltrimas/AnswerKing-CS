using System;

namespace Answer.King.Api.ViewModels
{
    public class Payment
    {
        public double Amount { get; set; }

        public Guid OrderId { get; set; }
    }
}