using System.Collections.Generic;

namespace Answer.King.Api.ViewModels
{
    public class CreateOrder
    {
        public IList<LineItem> LineItems { get; set; }
    }
}