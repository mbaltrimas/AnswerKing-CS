using System;
using System.Collections.Generic;

namespace Answer.King.Api.ViewModels
{
    public class UpdateOrder
    {
        public Guid Id { get; set; }

        public IList<LineItem> LineItems { get; set; }
    }
}