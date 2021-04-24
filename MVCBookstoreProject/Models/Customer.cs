using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Dynamic;

namespace MVCBookstoreProject.Models
{
    public enum CardIssuer
    {
        Visa,
        MasterCard
    }
    public class CreditCard
    {
        public int CreditCardId { get; set; }
        [Required]
        [StringLength(16)]
        [Display(Name = "Card No.")]
        public string CardNo { get; set; }
        [Required]
        [Display(Name = "Card Issuer")]
        public CardIssuer CardIssuer { get; set; }
        [Required]
        [Display(Name = "Holder Name")]
        public string HolderName { get; set; }
        [Required]
        [Display(Name = "Expire Date")]
        public DateTime ExpireDate { get; set; }
    }

    public class Country
    {
        public int CountryId { get; set; }
        [Required]
        [StringLength(255)]
        public string CountryName { get; set; }
    }

    public class Address
    {
        public int AddressId { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public string Street1 { get; set; }
        public string Street2 { get; set; }

        [Required]
        [StringLength(255)]
        public string City { get; set; }

        [Required]
        [StringLength(255)]
        public string State { get; set; }

        [Required]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        public string PostCode { get; set; }
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        [Required]
        [StringLength(255)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(255)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public int AddressId { get; set; }
        public virtual Address Address { get; set; }
        public int? CreditCardId { get; set; }
        public virtual CreditCard CreditCard { get; set; }

        public virtual ICollection<Order> Orders { get; set; }



    }
}