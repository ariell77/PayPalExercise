using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Configuration;

namespace PayPalExercise.Models
{
    public class Checkout
    {
        private string _amount;
        private string _curr;
        private string _OrderID;
        private string _AddLine1;
        private string _AddLine2;
        private string _addresscity;
        private string _countrycode;
        private string _state;
        private string _postalcode;
        private string _phone;
        private string _clienttoken;
        private string _orderdescription;

        public string Amount { get { return _amount; } set { _amount = value; } }
        public string Currency { get { return _curr; } set { _curr = value; } }
        public string OrderID { get { return _OrderID; } set { _OrderID = value; } }
        public string AddressLine1 { get { return _AddLine1; } set { _AddLine1 = value; } }
        [DataType(DataType.MultilineText)]
        public string AddressLine2 { get { return _AddLine2; } set { _AddLine2 = value; } }
        public string City { get { return _addresscity; } set { _addresscity = value; } }
        public string Country { get { return _countrycode; } set { _countrycode = value; } }
        public string State { get { return _state; } set { _state = value; } }
        public string PostalCode { get { return _postalcode; } set { _postalcode = value; } }
        public string Phone { get { return _phone; } set { _phone = value; } }
        public string ClientToken { get { return _clienttoken; } set { _clienttoken = value; } }
        public string OrderDescription { get { return _orderdescription; } set { _orderdescription = value; } }
        /// <summary>
        /// read the check out details from web config (as mock up data)
        /// </summary>
        /// <param name="clientoken"></param>
        public Checkout(string clientoken)
        {
            Amount = ConfigurationManager.AppSettings["Amount"];
            Currency = ConfigurationManager.AppSettings["Currency"];
            OrderID = GetRandomOrderID();
            AddressLine1 = ConfigurationManager.AppSettings["AddressLine1"];
            AddressLine2 = ConfigurationManager.AppSettings["AddressLine2"];
            City = ConfigurationManager.AppSettings["City"];
            Country = ConfigurationManager.AppSettings["Country"];
            State = ConfigurationManager.AppSettings["State"];
            PostalCode = ConfigurationManager.AppSettings["PostalCode"];
            Phone = ConfigurationManager.AppSettings["Phone"];
            OrderDescription = ConfigurationManager.AppSettings["OrderDescription"];
            ClientToken = clientoken;
        }
        //get a random order id for multiple testing
        private string GetRandomOrderID()
        {
            return new Random().Next().ToString();
        }
    }
}