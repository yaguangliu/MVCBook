using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCBookstoreProject.Models;

namespace MVCBookstoreProject.ViewModels
{
    public class CheckOutAddressModel
    {
        public Customer Customer { get; set; }
        public Address Address { get; set; }
        public Country Country { get; set; }
    }
}