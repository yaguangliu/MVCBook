using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCBookstoreProject.Models;

namespace MVCBookstoreProject.ViewModels
{
    public class PaymentViewModel
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public CreditCard CreditCard { get; set; }
        public bool isCash { get; set; }
    }
}