using Braintree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayPalExercise.Models
{
    public class PaymentSummary
    {
        private string _TransactionId;
        private string _OrderId;
        private string _PaymentID;
        private string _SellerProtectionStatus;
        private string _errors;
        private string _debugid;
        private string _payerid;
        private string _status;

        public string TransactionID
        {
            get
            {
                return _TransactionId;
            }
            set
            {
                _TransactionId = value;
            }
        }
        public string OrderID
        {
            get
            {
                return _OrderId;
            }
            set
            {
                _OrderId = value;
            }
        }
        public string PaymentID
        {
            get
            {
                return _PaymentID;
            }
            set
            {
                _PaymentID = value;
            }
        }
        public string SellerProtectionStatus
        {
            get
            {
                return _SellerProtectionStatus;
            }
            set
            {
                _SellerProtectionStatus = value;
            }
        }
        public string Errors
        {
            get
            {
                return _errors;
            }
            set
            {
                _errors = value;
            }
        }
        public string DebugId
        {
            get
            {
                return _debugid;
            }
            set
            {
                _debugid = value;
            }
        }
        public string PayerId
        {
            get
            {
                return _payerid;
            }
            set
            {
                _payerid = value;
            }
        }
        
        public string PayerStatus
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }
        public PaymentSummary(Result<Transaction> result)
         {
            TransactionID = result.Target.PayPalDetails.AuthorizationId;
            OrderID = result.Target.OrderId;
            PaymentID = result.Target.PayPalDetails.PaymentId;
            SellerProtectionStatus = result.Target.PayPalDetails.SellerProtectionStatus;
            DebugId = result.Target.PayPalDetails.DebugId;
            PayerId = result.Target.PayPalDetails.PayerId;
            PayerStatus = result.Target.PayPalDetails.PayerStatus;
            Errors = string.Empty;

         }




    }
}