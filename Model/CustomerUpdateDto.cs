using System.ComponentModel.DataAnnotations;

namespace StripeApi.Model
{
    public class CustomerUpdateDto
    {
        //[Required]
        //public string Name { get; set; }
        //[Required]
        //public string Email { get; set; }
        //public string Phone { get; set; }
        //public string Description { get; set; }
        //public AddressUptDto Address { get; set; }
        [Required]
        public InvoiceSettingsDto InvoiceSettings { get; set; }
        [Required]
        public string CustomerId { get; set; }
    }

    public class AddressUptDto
    {
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }

    public class InvoiceSettingsDto
    {
        public bool? CustomFields { get; set; }
        public bool? DefaultPaymentMethod { get; set; }
        public string DefaultPaymentMethodId { get; set; }
    }
}
