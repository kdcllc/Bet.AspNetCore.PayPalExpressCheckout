namespace Bet.AspNetCore.PayPalExpressCheckout.Options
{
    public class PayPalExpressCheckoutOptions
    {
        /// <summary>
        /// Application Client Id created at:
        /// https://developer.paypal.com/developer/applications/.
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// Application Client Secret created at:
        /// https://developer.paypal.com/developer/applications/.
        /// </summary>
        public string ClientSecret { get; set; } = string.Empty;

        /// <summary>
        /// The flag to use either sandbox or live environment.
        /// </summary>
        public bool IsSandBox { get; set; } = true;

        /// <summary>
        /// If base URL is present that it will be used; otherwise the URL will be build based on
        ///  schema/host pattern.
        /// </summary>
        public string BaseHostUrl { get; set; } = string.Empty;

        /// <summary>
        /// Specify the cancel URL.
        /// </summary>
        public string CancelUrl { get; set; } = string.Empty;

        /// <summary>
        /// Specify Success URL.
        /// </summary>
        public string ReturnUrl { get; set; } = string.Empty;
    }
}
