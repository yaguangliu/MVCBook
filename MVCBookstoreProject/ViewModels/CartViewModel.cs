using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCBookstoreProject.Models;

namespace MVCBookstoreProject.ViewModels
{
    public class CartViewModel
    {
        public Book Book { get; set; }
        public int Quantity { get; set; }
    }
}