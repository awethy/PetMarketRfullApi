using FluentValidation;

namespace PetMarketRfullApi.Domain.Models.OrderModels
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public int? PetId { get; set; }
        public Pet? Pet { get; set; }

        public int? ProductId { get; set; }
        public Product? Product { get; set; }

        public Guid CartId { get; set; }
        public Cart? Cart { get; set; }
    }

    public class CartItemValidator : AbstractValidator<CartItem>
    {
        public CartItemValidator()
        {
            {
                RuleFor(x => x)
                    .Must(x => (x.ProductId != default && x.PetId == default) ||
                               (x.ProductId == default && x.PetId != default))
                    .WithMessage("Items in cart не должны быть null.");
            }
        }
    }
}
