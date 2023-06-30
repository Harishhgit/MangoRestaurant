using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
    public class CartHeaderDto
    {
        public int CartHeaderId { get; set; }

        public string UserId { get; set; }

        public string CouponCode { get; set; }

        public double OrderTotal { get; set; }

        //
        public double DiscountTotal { get; set; }

        [RegularExpression("^([a-zA-Z]{3,})", ErrorMessage = "Enter valid First Name")]
        public string FirstName { get; set; }

        [RegularExpression("^([a-zA-Z]{3,})", ErrorMessage = "Enter valid Last Name")]
        public string LastName { get; set; }
        
        public DateTime PickupDateTime { get; set; }

        [RegularExpression(@"^\d{10}", ErrorMessage = "Enter valid phone number")]
        public string Phone { get; set; }

        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Enter valid email address")]
        public string Email { get; set; }

        [RegularExpression(@"^[1-9][0-9]{12}(?:[0-9]{3})?$", ErrorMessage = "Enter valid Card Number")]
        public string CardNumber { get; set; }

        [RegularExpression(@"^[0-9]{3}$", ErrorMessage = "Enter valid CVV")]
        public string CVV { get; set; }

        [RegularExpression(@"^(0[1-9]|1[0-2])/([0-9]{2})$", ErrorMessage = "Enter valid Expiry")]
        public string ExpiryMonthYear { get; set; }

    }
}
