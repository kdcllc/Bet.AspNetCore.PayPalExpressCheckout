using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Bet.AspNetCore.PayPalExpressCheckout.Options;

using Bogus;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using PayPalCheckoutSdk.Orders;

using PayPalHttp;

namespace Bet.AspNetCore.PayPalExpressCheckout.Example.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayPalController : ControllerBase
    {
        private readonly PayPalExpressCheckoutOptions _options;
        private readonly HttpClient _httpClient;
        private readonly ILogger<PayPalController> _logger;

        public PayPalController(
            IOptionsSnapshot<PayPalExpressCheckoutOptions> options,
            HttpClient httpClient,
            ILogger<PayPalController> logger)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options.Value;
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("createorder")]
        public async Task<ActionResult> CreateOrder([FromQuery] string cartId)
        {
            if (string.IsNullOrEmpty(cartId))
            {
                return BadRequest();
            }

            try
            {
                // correlate cartid with result.id for paypal transactions

                var request = new OrdersCreateRequest();
                var orderRequest = BuildSimpleOrder(new Random((int)DateTime.Now.Ticks).Next(1, 100).ToString(), cartId);

                request.Headers.Add("prefer", "return=representation");
                request.RequestBody(orderRequest);

                var response = await _httpClient.Execute(request);
                var result = response.Result<Order>();

                return Ok(result);
            }
            catch (HttpException ex)
            {
                _logger.LogError(ex, "PayPal Create Order Exception");
                throw;
            }
        }

        [HttpPost("approveorder")]
        public async Task<ActionResult> ApproveOrder(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return BadRequest();
            }

            try
            {
                var request = new OrdersCaptureRequest(orderId);
                request.Prefer("return=representation");
                request.RequestBody(new OrderActionRequest());

                var response = await _httpClient.Execute(request);
                var result = response.Result<Order>();

                return Ok(result);
            }
            catch (HttpException ex)
            {
                _logger.LogError(ex, "PayPal Approve Order Exception");
                throw;
            }
        }

        private OrderRequest BuildSimpleOrder(string invoiceId, string cartId)
        {
            var host = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

            if (!string.IsNullOrEmpty(_options.BaseHostUrl))
            {
                host = _options.BaseHostUrl;
            }

            var cancelUrl = $"{host}{_options.CancelUrl}";
            var returnUrl = $"{host}{_options.ReturnUrl}";

            var fullName = new Faker<Name>()
                    .RuleFor(x => x.FullName, f => f.Person.FullName).Generate();

            var address = new Faker<AddressPortable>()
                       .RuleFor(u => u.AddressLine1, (f, u) => f.Address.StreetAddress())
                       .RuleFor(u => u.AdminArea2, f => f.Address.City())
                       .RuleFor(u => u.AdminArea1, f => f.Address.StreetAddress())
                       .RuleFor(u => u.PostalCode, (f, u) => f.Address.ZipCode())
                       .RuleFor(u => u.CountryCode, (f, u) => f.Address.CountryCode()).Generate();

            var rdn = new Random(100);

            return new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                ApplicationContext = new ApplicationContext
                {
                    CancelUrl = cancelUrl,
                    ReturnUrl = returnUrl
                },
                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest
                    {
                        CustomId = $"CustomId {rdn.NextDouble()}",
                        InvoiceId = invoiceId,
                        Description = $"Description {cartId}",
                        ShippingDetail = new ShippingDetail
                        {
                          Name = fullName,
                          AddressPortable = address
                        },
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = "USD",
                            Value = rdn.NextDouble().ToString("0.##")
                        }
                    }
                },
                //Payer = new Payer
                //{
                //    AddressPortable = address,
                //    Email = "payal_test@email.com",
                //    Name = new Name
                //    {
                //        GivenName = "John",
                //        Surname = "Smith"
                //    },
                //}
            };
        }
    }
}
