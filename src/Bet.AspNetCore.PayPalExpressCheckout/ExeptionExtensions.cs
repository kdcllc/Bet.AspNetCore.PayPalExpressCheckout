using System;
using System.Collections.Generic;

using Bet.AspNetCore.PayPalExpressCheckout.Models;

using Newtonsoft.Json;

namespace PayPalHttp
{
    public static class ExeptionExtensions
    {
        public static ErrorModel GetPayPalError(this Exception ex)
        {
            var error = new ErrorModel
            {
                Name = "INTERNAL_SERVER_ERROR",
                Message = "Error Occurred",
                Details = new List<Detail>
                    {
                        new Detail
                        {
                            Description = "Error Occured",
                        }
                    }
            };

            if (ex is HttpException
                && !string.IsNullOrEmpty(ex?.Message))
            {
                if (ex?.Message == null)
                {
                    return error;
                }

                error = JsonConvert.DeserializeObject<ErrorModel>(ex.Message);
            }

            return error;
        }
    }
}
