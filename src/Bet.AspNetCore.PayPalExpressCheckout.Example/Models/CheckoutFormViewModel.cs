using System;

namespace Bet.AspNetCore.PayPalExpressCheckout.Example.Models
{
    public class CheckoutFormViewModel
    {
        public string ClientId { get; set; }

        public string CartId { get; set; } = Guid.NewGuid().ToString();

        public string ReturnUrl { get; set; }
    }
}
