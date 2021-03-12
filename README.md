# Bet.AspNetCore.PayPalExpressCheckout
AspNetCore Example of using PayPal Express Checkout

The example utilizes the following libraries for communication with PayPal

- [`PayPalCheckoutSdk`](https://www.nuget.org/packages/PayPalCheckoutSdk/1.0.3)
- [`PayPalHttp`](https://www.nuget.org/packages/PayPalHttp/1.0.0)

### Issues
WSL2 has an issue with not able to serve to localhost.
WSL2 must be restarted to make it work:

```powershell
    wsl --showdown
```