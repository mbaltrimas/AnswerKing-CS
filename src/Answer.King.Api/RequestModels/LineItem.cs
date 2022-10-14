namespace Answer.King.Api.RequestModels;

public record LineItem
{
    public ProductId Product { get; init; } = null!;

    public int Quantity { get; init; }
}