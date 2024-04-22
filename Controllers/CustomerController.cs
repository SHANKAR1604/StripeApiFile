using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using StripeApi.Model;
namespace StripeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly string _stripeSecretKey;

        public CustomerController(IConfiguration configuration)
        {
            _stripeSecretKey = configuration["Stripe:SecretKey"];
            StripeConfiguration.ApiKey = _stripeSecretKey;
        }

        //Stripe List All Customer
        [HttpGet]
        [Route("list-all-customers")]
        public async Task<List<Customer>> ListAllCustomers()
        {
            try
            {
                var service = new CustomerService();

                var customers = await service.ListAsync();

                return customers.ToList();
            }
            catch (StripeException ex)
            {
                // Handle Stripe API errors
                throw ex;
            }
        }


        //Stripe Create Customer
        [HttpPost]
        [Route("create-customer")]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateDto customerDto)
        {
            try
            {
                var options = new CustomerCreateOptions();

                if (!string.IsNullOrEmpty(customerDto.Email))
                {
                    options.Email = customerDto.Email;
                }

                if (!string.IsNullOrEmpty(customerDto.Name))
                {
                    options.Name = customerDto.Name;
                }

                if (customerDto.Address != null)
                {
                    options.Address = new AddressOptions
                    {
                        Line1 = customerDto.Address.AddressLine1,
                        City = customerDto.Address.City,
                        PostalCode = customerDto.Address.PostalCode,
                        State = customerDto.Address.State,
                        Country = customerDto.Address.Country
                    };
                }
                if (!string.IsNullOrEmpty(customerDto.Phone))
                {
                    options.Phone = customerDto.Phone;
                }
                if (!string.IsNullOrEmpty(customerDto.Description))
                {
                    options.Description = customerDto.Description;
                }

                var service = new CustomerService();

                var customer = await service.CreateAsync(options);

                return Ok(customer);
            }
            catch (StripeException e)
            {
                // Handle Stripe API errors
                return BadRequest(e.Message);
            }
        }

        [HttpPut("update-customer")]
        public IActionResult UpdateCustomer([FromBody] CustomerUpdateDto customerDto)
        {
            try
            {

                var options = new CustomerUpdateOptions();

                //if (!string.IsNullOrEmpty(customerDto.Email))
                //{
                //    options.Email = customerDto.Email;
                //}

                //if (!string.IsNullOrEmpty(customerDto.Name))
                //{
                //    options.Name = customerDto.Name;
                //}

                //if (customerDto.Address != null)
                //{
                //    options.Address = new AddressOptions
                //    {
                //        Line1 = customerDto.Address.AddressLine1,
                //        City = customerDto.Address.City,
                //        PostalCode = customerDto.Address.PostalCode,
                //        State = customerDto.Address.State,
                //        Country = customerDto.Address.Country
                //    };
                //}

                if (customerDto.InvoiceSettings != null)
                {
                    options.InvoiceSettings = new CustomerInvoiceSettingsOptions
                    {
                        DefaultPaymentMethod = customerDto.InvoiceSettings.DefaultPaymentMethodId,
                    };
                }

                //if (!string.IsNullOrEmpty(customerDto.Phone))
                //{
                //    options.Phone = customerDto.Phone;
                //}
                //if (!string.IsNullOrEmpty(customerDto.Description))
                //{
                //    options.Description = customerDto.Description;
                //}
                    // Add more options as needed
                var service = new CustomerService();
                var customer = service.Update(customerDto.CustomerId, options);

                return Ok(customer);
            }
            catch (StripeException ex)
            {
                // Handle Stripe-specific errors
                return StatusCode((int)ex.HttpStatusCode, $"Stripe Error: {ex.StripeError.Message}");
            }
            catch (Exception ex)
            {
                // Handle other errors
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
