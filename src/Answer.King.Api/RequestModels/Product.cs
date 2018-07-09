namespace Answer.King.Api.RequestModels
{
    public class Product
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public CategoryId Category { get; set; }
    }
}
