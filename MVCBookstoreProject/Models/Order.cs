using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCBookstoreProject.Models
{

    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        [Range(1,Int32.MaxValue)]
        public int Quantity { get; set; }

        public virtual Book Book { get; set; }
        public virtual Order Order { get; set; }
    }

    public enum PaymentMethod
    {
        [Display(Name = "Cash On Delivery")]
        CashOnDelivery,
        [Display(Name = "Credit Card")]
        CreditCard
    }

    public enum OrderStatus
    {
        Processing,
        Confirmed,
        Packed,
        Shipped,
        [Display(Name = "In-Transit")]
        Transit,
        [Display(Name = "Out for Delivery")]
        OutForDelivery,
        Delivered,
        [Display(Name = "Not Accepted")]
        NotAccepted,
        [Display(Name = "Returned Back")]
        ReturnedBack
    }
    public class Order
    {
        public int OrderId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public PaymentMethod PaymentMethod { get; set; }
        [Required]
        public OrderStatus OrderStatus { get; set; }
        [Required]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    }
}