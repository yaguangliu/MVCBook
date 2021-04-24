using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCBookstoreProject.Models;

namespace MVCBookstoreProject.ViewModels
{
    public class OrderDetailViewModel
    {
        public Order Order { get; set; }
        public int OrderDetailId { get; set; }
        public int BookId { get; set; }
        
    }
}