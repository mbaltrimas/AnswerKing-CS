using System.Collections.Generic;

namespace Answer.King.Api.ViewModels
{
    public class Order
    {
        public IList<LineItem> LineItems { get; set; }
    }
}