

paypal.Buttons({

    style: {
        color: 'blue',
        shape: 'pill',
        label: 'pay',
        height: 40
    },

    createOrder: function (data, actions) {
        return fetch('/api/paypal/createorder?cartId=' + cartId, {
            method: 'POST',
        }).then(function (res) {
            if (res.ok === false) {
                alert("Something went wrong");
                return;
            }
            return res.json();
        }).then(function (data) {
            return data.id;
        });
    },

    onApprove: function (data, actions) {
        return fetch('/api/paypal/approveorder?orderId=' + data.orderID, {
            method: 'POST'
        }).then(function (res) {

            if (res.ok === false) {
                alert("Something went wrong");
                return;
            }

            return res.json();
        }).then(function (orderData) {
            // Three cases to handle:
            //   (1) Recoverable INSTRUMENT_DECLINED -> call actions.restart()
            //   (2) Other non-recoverable errors -> Show a failure message
            //   (3) Successful transaction -> Show a success / thank you message

            // Your server defines the structure of 'orderData', which may differ
            var errorDetail = Array.isArray(orderData.details) && orderData.details[0];

            if (errorDetail && errorDetail.issue === 'INSTRUMENT_DECLINED') {
                // Recoverable state, see: "Handle Funding Failures"
                // https://developer.paypal.com/docs/checkout/integration-features/funding-failure/
                return actions.restart();
            }

            if (errorDetail) {
                var msg = 'Sorry, your transaction could not be processed.';
                if (errorDetail.description) msg += '\n\n' + errorDetail.description;
                if (orderData.debug_id) msg += ' (' + orderData.debug_id + ')';
                // Show a failure message
                return alert(msg);
            }

            // Show a success message to the buyer
            alert('Transaction completed by ' + orderData.payer.name.given_name);

            window.location = returnUrl + "/?orderId=" + orderData.id;
        });
    }
}).render('#paypal-button-container');
