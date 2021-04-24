using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCBookstoreProject.Models;

namespace MVCBookstoreProject.ViewModels
{
    public class Sorting
    {
        public int sortingId { get; set; }
        public string sortingField { get; set; }
    }
    public class BookDisplayViewModel
    {
        public Book Book { get; set; }
        public Sorting Sorting { get; set; }
    }
}