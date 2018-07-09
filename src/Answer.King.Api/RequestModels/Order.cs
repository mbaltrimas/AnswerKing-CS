using System.Collections.Generic;

namespace Answer.King.Api.RequestModels
{
    public class Order
    {
        public IList<LineItem> LineItems { get; set; }
    }
}