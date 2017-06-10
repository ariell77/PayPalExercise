using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Braintree;
using Braintree.Test;
using PayPalExercise.Models;
using System.Configuration;

namespace PayPalExercise.Controllers
{
    public class HomeController : Controller
    {
        BraintreeGateway gateway;
        Checkout CheckoutDetails;
        public HomeController()
        {
            //read from web config
            string AccessToken = ConfigurationManager.AppSettings["AccessToken"];
            gateway = new BraintreeGateway(AccessToken);
        }
        public ActionResult Index()
        {
            //generate client side token
            var clientToken = gateway.ClientToken.generate();
            CheckoutDetails = new Checkout(clientToken);
            Session["CheckoutDetails"] =CheckoutDetails;
            //return the checkout details
            return View(CheckoutDetails);
        }
        [HttpPost]
        [AllowCrossSiteJson]
        public JsonResult CreatePurchase(string nonce)
        {
            // Use payment method nonce here
            Result<Transaction>  result = CreateBraintreeTransaction(nonce);
            PaymentSummary PaymentSummary = new PaymentSummary(result);

            if (!result.IsSuccess())
            {
                string errormessage = string.Empty;
                //add some logging
                if (result.Transaction.Status == TransactionStatus.GATEWAY_REJECTED)
                {
                    errormessage = "Gateway rejected." + result.Transaction.GatewayRejectionReason; 
                }
                if (result.Errors.DeepCount > 0)
                {
                    foreach (ValidationError error in result.Errors.DeepAll())
                    {

                        if (errormessage.Equals(string.Empty))
                            errormessage = error.Message;
                        else
                            errormessage = ";" + errormessage;
                    }
                }
                PaymentSummary.Errors = errormessage;
            }
            
            //return payment summary details
            return Json(PaymentSummary);

        }

        /// <summary>
        /// create the transaction
        /// </summary>
        /// <param name="nonce"></param>
        /// <returns></returns>
        private Result<Transaction> CreateBraintreeTransaction(string nonce)
        {
            Result<Transaction> result = null;
            TransactionRequest request = new TransactionRequest
            {
                Amount =GetOrderAmount(),
                OrderId= GetOrderID(),
                MerchantAccountId = "AUD",
                PaymentMethodNonce = nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };
            try
            {
                 result = gateway.Transaction.Sale(request);
            }
            catch
            {
                //do some logging
            }
            
            //return transaction results
            return result;
            
        }
        /// <summary>
        /// get the order amount
        /// </summary>
        /// <returns></returns>
        private decimal GetOrderAmount()
        {
            decimal amount;
            Checkout checkoutdata =  Session["CheckoutDetails"] as  Checkout;
            if(decimal.TryParse( checkoutdata.Amount,out amount))
            {
                return amount;
            }
            else
            {
                //throw error and log in an error - just return a default value for now
                return 0.01M;
            }

        }
        /// <summary>
        /// get the order id
        /// </summary>
        /// <returns></returns>
        private string GetOrderID()
        {
            Checkout checkoutdata = Session["CheckoutDetails"] as Checkout;
            return checkoutdata.OrderID;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}