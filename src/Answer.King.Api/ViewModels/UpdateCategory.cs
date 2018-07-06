using System;

namespace Answer.King.Api.ViewModels
{
    public class UpdateCategory
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}