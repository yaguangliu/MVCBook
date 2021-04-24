using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCBookstoreProject.Models
{
    public class Publisher
    {
        public int PublisherId { get; set; }
        [Required]
        [Display(Name = "Publisher")]
        [StringLength(255)]
        public string PublisherName { get; set; }
    }

    public class Category
    {
        public int CategoryId { get; set; }
        [Required]
        [Display(Name = "Category")]
        [StringLength(255)]
        public string CategoryName { get; set; }
    }

    public class Language
    {
        public int LanguageId { get; set; }
        [Required]
        [Display(Name = "Language")]
        [StringLength(255)]
        public string LanguageName { get; set; }
    }
    public class Book
    {
        public int BookId { get; set; }
        [Required]
        [StringLength(500)]
        public string BookName { get; set; }
        [Required]
        [StringLength(500)]
        public string Author { get; set; }
        [Required]
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        [Required]
        public DateTime PublishedDate { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [Required]
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        [Required]
        public decimal Price { get; set; }
        public byte[] Photo { get; set; }

        public string Description { get; set; }
        public bool isAvailable { get; set; } = true;
        [Required]
        [Range(0, Int32.MaxValue)]
        public int Stock { get; set; }
        
    }
}