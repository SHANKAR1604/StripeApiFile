using System.ComponentModel.DataAnnotations;

namespace StripeApi.Model
{
    public class CreatePaymentIntentRequest
    {
        [Required]
        public int Amount { get; set; }
        [Required]
        public string Currency { get; set; }
        [Required]
        public string Customer { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
    }

    public class UpdatePaymentIntentRequest
    {
        [Required]
        public string PaymentIntentId { get; set; }
        [Required]
        public long Amount { get; set; }
        [Required]
        public string Currency { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
    }

    public class ConfirmPaymentIntentRequest
    {
        [Required]
        public string PaymentIntentId { get; set; }
    }

    public class CreatePaymentMethodRequest
    {
        [Required]
        public string Token { get; set; }
    }
    public class AttachPaymentMethodRequest
    {
        [Required]
        public string PaymentMethodId { get; set; }
        [Required]
        public string CustomerId { get; set; }
    }

    public class CreateProductRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string OrderId { get; set; }
    }

    public class UpdateProductRequest
    {
        [Required]
        public string ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string OrderId { get; set; }
    }

    public class CreatePriceRequest
    {
        [Required]
        public string ProductId { get; set; }
        [Required]
        public long UnitAmount { get; set; }
        [Required]
        public string Currency { get; set; }
        [Required]
        public string Interval { get; set; }
        [Required]
        public int IntervalCount { get; set; }
    }

    public class UpdatePriceRequest
    {
        [Required]
        public string PriceId { get; set; }
        [Required]
        public decimal UnitAmount { get; set; }
        [Required]
        public string Currency { get; set; }
        [Required]
        public string Interval { get; set; }
    }
    public class CreateSubscriptionRequest
    {
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string PriceId { get; set; }
    }

    public class UpdateSubscriptionRequest
    {
        [Required]
        public bool CancelAtPeriodEnd { get; set; }
        [Required]
        public string subscriptionId { get; set; }
    }
}
