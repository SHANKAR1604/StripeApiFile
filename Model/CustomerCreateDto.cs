using System.ComponentModel.DataAnnotations;

namespace StripeApi.Model
{
    public class CustomerCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Description { get; set; }
        public AddressDto Address { get; set; }
    }

    public class AddressDto
    {
        [Required]
        public string AddressLine1 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Country { get; set; }
    }
}
