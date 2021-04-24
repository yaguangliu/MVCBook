using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MVCBookstoreProject.Models;

namespace MVCBookstoreProject.ViewModels
{
    public class BookViewModel
    {
        public int BookId { get; set; }
        [Required]
        [StringLength(500)]
        public string BookName { get; set; }
        [Required]
        [StringLength(500)]
        public string Author { get; set; }
        [Required]
        [Display(Name = "Publisher")]
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        [Required]
        public DateTime PublishedDate { get; set; }
        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [Required]
        [Display(Name = "Language")]
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        [Required]
        public decimal Price { get; set; }
        public HttpPostedFileBase Photo { get; set; }
        public byte[] PhotoDb { get; set; }

        public string Description { get; set; }
        [Display(Name = "Is Available Now?")]
        public bool isAvailable { get; set; } = true;
        [Required]
        [Range(0, Int32.MaxValue)]
        public int Stock { get; set; }
    }
}