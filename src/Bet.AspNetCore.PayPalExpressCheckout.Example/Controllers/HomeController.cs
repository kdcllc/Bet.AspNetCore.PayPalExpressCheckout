using System.Diagnostics;

using Bet.AspNetCore.PayPalExpressCheckout.Example.Models;
using Bet.AspNetCore.PayPalExpressCheckout.Options;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bet.AspNetCore.PayPalExpressCheckout.Example.Controllers
{
    public class HomeController : Controller
    {
        private readonly PayPalExpressCheckoutOptions _options;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IOptionsSnapshot<PayPalExpressCheckoutOptions> options,
            ILogger<HomeController> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("Success")]
        public IActionResult Success()
        {
            return View();
        }

        [Route("Cancel")]
        public IActionResult Cancel()
        {
            return View();
        }

        [Route("Checkout")]
        public IActionResult Checkout()
        {
            var model = new CheckoutFormViewModel
            {
                ClientId = _options.ClientId,
                ReturnUrl = _options.ReturnUrl,
                CancelUrl = _options.CancelUrl,
            };

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
