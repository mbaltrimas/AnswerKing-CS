namespace Answer.King.Api.ViewModels
{
    public class CreateProduct
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public CategoryId Category { get; set; }
    }
}
