using System;

using Bet.AspNetCore.PayPalExpressCheckout.Options;

using Microsoft.Extensions.Options;

using PayPalCheckoutSdk.Core;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class PayPalExpressCheckoutServiceExtensions
    {
        public static IServiceCollection AddPayPalExpressCheckout(
            this IServiceCollection services,
            string sectionName = nameof(PayPalExpressCheckoutOptions),
            Action<PayPalExpressCheckoutOptions>? configureOptions = default)
        {
            services.AddChangeTokenOptions<PayPalExpressCheckoutOptions>(sectionName, configureAction: o => configureOptions?.Invoke(o));

            services.AddTransient<PayPalHttp.HttpClient>(sp =>
            {
                PayPalEnvironment environment;

                var options = sp.GetRequiredService<IOptionsMonitor<PayPalExpressCheckoutOptions>>().CurrentValue;

                if (options.IsSandBox)
                {
                    environment = new SandboxEnvironment(options.ClientId, options.ClientSecret);
                }
                else
                {
                    environment = new LiveEnvironment(options.ClientId, options.ClientSecret);
                }

                return new PayPalHttpClient(environment);
            });

            return services;
        }
    }
}
