using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Climate;
using StripeApi.Model;

namespace StripeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentIntentsController : Controller
    {
        private readonly string _stripeSecretKey;
        public PaymentIntentsController(IConfiguration configuration)
        {
            _stripeSecretKey = configuration["Stripe:SecretKey"];
            StripeConfiguration.ApiKey = _stripeSecretKey;
        }

        //One Time Payments Start
        //List All Payments

        [HttpGet("list-payment-intents")]
        public async Task<List<PaymentIntent>> ListPaymentIntents()
        {
            try
            {
                var options = new PaymentIntentListOptions
                {
                    Limit = 10 // Specify the number of payment intents to retrieve
                               // Add any other options as needed
                };

                var service = new PaymentIntentService();

                var paymentIntents = await service.ListAsync();

                return paymentIntents.ToList();
            }
            catch (StripeException ex)
            {
                // Handle Stripe API errors
                throw ex;
            }
        }

        //Create payments

        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentRequest request)
        {
            try
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = request.Amount,
                    Currency = request.Currency,
                    PaymentMethodTypes = new List<string> { "card" },
                    Description = request.Description,
                    Customer = request.Customer,
                    PaymentMethod = request.PaymentMethod
                };

                var service = new PaymentIntentService();

                var intent = await service.CreateAsync(options);

                return Ok(new { clientSecret = intent.ClientSecret });
            }
            catch (StripeException e)
            {
                return BadRequest(new { error = e.StripeError.Message });
            }
        }

        [HttpPost("update-payment-intent")]
        public IActionResult UpdatePaymentIntent([FromBody] UpdatePaymentIntentRequest request)
        {
            try
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = request.Amount,
                    Currency = request.Currency,
                    PaymentMethod = request.PaymentMethod,
                    PaymentMethodOptions = new PaymentIntentPaymentMethodOptionsOptions
                    {
                        Card = new PaymentIntentPaymentMethodOptionsCardOptions
                        {
                            RequestThreeDSecure = "automatic"
                        }
                    }
                };
                var service = new PaymentIntentService();
                var paymentIntent = service.Update(request.PaymentIntentId, options);

                return Ok(paymentIntent);
            }
            catch (StripeException e)
            {
                return BadRequest(new { error = e.StripeError.Message });
            }
        }

        //Confirm Payments

        [HttpPost("confirm-payment-intent")]
        public IActionResult ConfirmPaymentIntent([FromBody] ConfirmPaymentIntentRequest request)
        {
            var service = new PaymentIntentService();

            var intent = service.Confirm(request.PaymentIntentId);

            return Ok(intent);
        }

        //One Time Payments End

        //Recurring Payment Options Start

        //Create Payment Method
        [HttpPost("create-payment-method")]
        public IActionResult CreatePaymentMethod([FromBody] CreatePaymentMethodRequest request)
        {
            try
            {
                var options = new PaymentMethodCreateOptions
                {
                    Type = "card",

                    Card = new PaymentMethodCardOptions
                    {
                        Token = request.Token
                    }
                    // Add any other options as needed
                };
                var service = new PaymentMethodService();

                var paymentMethod = service.Create(options);

                return Ok(paymentMethod);
            }
            catch (StripeException e)
            {
                return BadRequest(new { error = e.StripeError.Message });
            }
        }

        [HttpPost("attach-payment-method")]
        public IActionResult AttachPaymentMethod([FromBody] AttachPaymentMethodRequest request)
        {
            try
            {
                var service = new PaymentMethodService();

                var paymentMethod = service.Attach(

                    request.PaymentMethodId,

                    new PaymentMethodAttachOptions
                    {
                        Customer = request.CustomerId
                    }
                );

                return Ok(paymentMethod);
            }
            catch (StripeException e)
            {
                return BadRequest(new { error = e.StripeError.Message });
            }
        }

        [HttpPost("create-product")]
        public IActionResult CreateProduct([FromBody] CreateProductRequest request)
        {
            try
            {
                var options = new ProductCreateOptions
                {
                    Name = request.Name,
                    Description = request.Description,
                    Metadata = new Dictionary<string, string>
                    {
                         { "order_id", request.OrderId } // Add metadata here
                    }
                };
                var service = new Stripe.ProductService();

                var product = service.Create(options);

                return Ok(product);
            }
            catch (StripeException e)
            {
                return BadRequest(new { error = e.StripeError.Message });
            }
        }

        [HttpPost("update-product")]
        public IActionResult UpdateProduct([FromBody] UpdateProductRequest request)
        {
            try
            {
                var options = new ProductUpdateOptions
                {
                    Name = request.Name,
                    Description = request.Description,
                    Metadata = new Dictionary<string, string>
                    {
                         { "order_id", request.OrderId } // Add metadata here
                    }
                };
                var service = new Stripe.ProductService();

                var product = service.Update(request.ProductId, options);

                return Ok(product);
            }
            catch (StripeException e)
            {
                return BadRequest(new { error = e.StripeError.Message });
            }
        }

        [HttpPost("create-price")]
        public IActionResult CreatePrice([FromBody] CreatePriceRequest request)
        {
            try
            {
                var options = new PriceCreateOptions
                {
                    Product = request.ProductId, // Product ID associated with the price
                    UnitAmount = request.UnitAmount, // Amount in cents (or lowest currency unit)
                    Currency = request.Currency,
                    Recurring = new PriceRecurringOptions
                    {
                        Interval = request.Interval, // e.g., "month" or "year"
                        IntervalCount = request.IntervalCount, 
                    },
                    // Add any other options as needed
                };
                var service = new Stripe.PriceService();
                var price = service.Create(options);

                return Ok(price);
            }
            catch (StripeException e)
            {
                return BadRequest(new { error = e.StripeError.Message });
            }
        }

        //[HttpPost("update-price")]
        //public IActionResult UpdatePrice([FromBody] UpdatePriceRequest request)
        //{
        //    try
        //    {
        //        var options = new PriceUpdateOptions
        //        {
        //            UnitAmount = request.UnitAmount, // Amount in cents (or lowest currency unit)
        //            Currency = request.Currency,
        //            Recurring = new PriceRecurringOptions
        //            {
        //                Interval = request.Interval, // e.g., "month" or "year"
        //            },
        //            // Add any other options as needed
        //        };
        //        var service = new Stripe.PriceService();
        //        var price = service.Update(request.PriceId, options);

        //        return Ok(price);
        //    }
        //    catch (StripeException e)
        //    {
        //        return BadRequest(new { error = e.StripeError.Message });
        //    }
        //}

        //Recurring Payment Options End

        //Subscription Method Start

        [HttpPost("create-subscription")]
        public IActionResult CreateSubscription([FromBody] CreateSubscriptionRequest request)
        {
            try
            {
                var options = new SubscriptionCreateOptions
                {
                    Customer = request.CustomerId, // Customer ID associated with the subscription

                    Items = new List<SubscriptionItemOptions>
                   {
                    new SubscriptionItemOptions
                    {
                        Price =request.PriceId // Price ID associated with the subscription
                    }
                   }
                    // Add any other options as needed
                };
                var service = new Stripe.SubscriptionService();

                var subscription = service.Create(options);

                return Ok(subscription);
            }
            catch (StripeException e)
            {
                return BadRequest(new { error = e.StripeError.Message });
            }
        }

        [HttpGet("list-subscription")]
        public IActionResult ListSubscriptions()
        {
            try
            {
                var options = new SubscriptionListOptions
                {
                    Limit = 10 // Limit the number of subscriptions returned (optional)
                               // Add any other options as needed
                };
                var service = new Stripe.SubscriptionService();

                var subscriptions = service.List(options);

                return Ok(subscriptions);
            }
            catch (StripeException e)
            {
                return BadRequest(new { error = e.StripeError.Message });
            }
        }

        [HttpPut("update-subscription")]
        public IActionResult UpdateSubscription([FromBody] UpdateSubscriptionRequest request)
        {
            try
            {
                var options = new SubscriptionUpdateOptions
                {
                    CancelAtPeriodEnd = request.CancelAtPeriodEnd,
                    // Add any other options as needed
                };
                var service = new Stripe.SubscriptionService();
                var subscription = service.Update(request.subscriptionId, options);

                return Ok(subscription);
            }
            catch (StripeException e)
            {
                return BadRequest(new { error = e.StripeError.Message });
            }
        }
        //Subscription Method End
    }
}
