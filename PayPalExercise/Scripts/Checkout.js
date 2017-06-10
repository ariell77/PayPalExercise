$(document).ready(function () {
   
    var BRAINTREE_SANDBOX_AUTH = $('#ClientToken').val();

    $('#message').fadeOut('');
    // Render the PayPal button
    if (braintree.paypalCheckout.isSupported()) {
        paypal.Button.render({

            // Pass in the Braintree SDK

            braintree: braintree,

            // Pass in your Braintree authorization key

            client: {
                sandbox: BRAINTREE_SANDBOX_AUTH
            },

            // Set your environment

            env: 'sandbox', // sandbox | production

            // Wait for the PayPal button to be clicked

            payment: function (data, actions) {

                // Call Braintree to create the payment
                return actions.braintree.create({
                    flow: 'checkout',
                    amount: document.getElementById('txnamount').value,
                    currency: document.getElementById('txncur').value,
                    intent: 'sale',
                    locale: 'en_US',
                    enableShippingAddress: true,
                    shippingAddressEditable: false,
                    shippingAddressOverride: {
                        recipientName: 'Buyer1',
                        line1: document.getElementById('addline1').value,
                        line2: document.getElementById('addline2').value,
                        city: document.getElementById('addresscity').value,
                        countryCode: document.getElementById('countrycode').value,
                        postalCode: document.getElementById('postalcode').value,
                        state: document.getElementById('state').value,
                        phone: document.getElementById('phone').value
                    }
                });
            },

            // Wait for the payment to be authorized by the customer

            onAuthorize: function (data, actions) {

                // Call your server with data.nonce to finalize the payment
                $.ajax({
                    type: 'POST',
                    crossOrigin: true,
                    url: "/Home/CreatePurchase",
                    dataType: "json",
                    processData: true,
                    data: JSON.stringify(data),
                    contentType: "application/json; charset=utf-8",
                    beforeSend: function () {
                        $('#message').fadeIn('');
                    },
                    success: function (paymentsummary) {
                        if (paymentsummary.Errors == "") {
                            PaymentSuccess(paymentsummary);
                        }
                        else
                            PaymentFailure(paymentsummary.Errors);
                        
                    },
                    //Http error
                    error: function (xhr) {
                        bootbox.alert('Payment failed!  ' + xhr);
                    }
                });

            },
            onCancel: function (data) {
                bootbox.alert("checkout payment cancelled" + JSON.stringify(data, 0, 2));
            },
            onError: function (err) {
                console.error('checkout.js error', err);
            },
        }, '#paypal-button-container');
    }
    else
    {
        //  PayPal payment option not available
        bootbox.alert("Browser uanble to display PayPal payment option");
    }
    //gather all the payment summary details
    function PaymentSuccess(paymentsummary)
    {
        var TransactionID = paymentsummary.TransactionID;
        var OrderID = paymentsummary.OrderID; 
        var PaymentID = paymentsummary.PaymentID;
        var SellerProtectionStatus = paymentsummary.SellerProtectionStatus;
        var DebugId = paymentsummary.DebugId;
        var PayerId = paymentsummary.PayerId;
        var PayerStatus = paymentsummary.PayerStatus;
        bootbox.alert("Your Payment has been successfull !!<BR>Here are your payment details :<BR><ul><li>TransactionID :" + TransactionID + "</li><li>OrderID :" + OrderID + "</li><li>PaymentID :" + PaymentID + "</li><li>SellerProtectionStatus :" + SellerProtectionStatus + "</li><li>DebugId :" + DebugId + "</li><li>PayerId :" + PayerId + "</li><li>PayerStatus :" + PayerStatus + "</li></ul>", function () { $('#message').fadeOut(''); });
    }
    //alert the user for any failure
    function PaymentFailure(errors)
    {
        var Errors = new Array();
        Errors = errors.split(';');
        var ListOfErrors = "Your payment was not successful , the following errors were found";
        for (var i = 0; i < Errors.length; ++i) {
            ListOfErrors = ListOfErrors.concat("<BR>",Errors[i]);
        }
        bootbox.alert(ListOfErrors);
    }

});

