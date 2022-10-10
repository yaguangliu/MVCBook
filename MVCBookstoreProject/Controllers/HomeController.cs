using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using Microsoft.AspNet.Identity;
using Microsoft.SqlServer.Server;
using MVCBookstoreProject.Models;
using MVCBookstoreProject.ViewModels;
using PagedList;


namespace MVCBookstoreProject.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // testing mirror git
        public ActionResult Index(string searchString, string currentFilter, int? page)
        {
            //string sorting = SortingList;
            /*this is a test for testing the mirror between github and cloud source repostories
            mirror action will involve pull or not? or egress traffic
            */


            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.currentFilter = searchString;
            var books = db.Books.Where(b => b.isAvailable == true).AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(b =>
                    ((b.Category.CategoryName.Contains(searchString)) || (b.BookName.Contains(searchString))));
            }

            if (Request.Form["SortingList"] != null)
            {
                var sorting = Request.Form["SortingList"];
                switch (sorting)
                {
                    case "1":
                        books = books.OrderBy(b => b.Price);
                        break;
                    case "2":
                        books = books.OrderByDescending(b => b.Price);
                        break;
                    case "3":
                        books = books.OrderBy(b => b.BookName);
                        break;
                    case "4":
                        books = books.OrderByDescending(b => b.BookName);
                        break;
                    default:
                        books = books.OrderBy(b => b.BookName);
                        break;
                }
            }
            else
            {
                books = books.OrderBy(b => b.PublishedDate);
            }

            int pageSize = 8;
            int pageNumber = page ?? 1;
            var data = books.Include(b=>b.Category).ToPagedList(pageNumber, pageSize);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_BookList", data);
            }
            return View(data);
        }

        public ActionResult BookDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Book book = db.Books.Include(b=>b.Category).Include(b=>b.Language).Include(b=>b.Publisher).Where(b=>b.BookId == id).FirstOrDefault();
            if (book == null)
            {
                return HttpNotFound();
            }

            return View(book);
        }

        public ActionResult AddMoreThanOne(int id)
        {
            var quantity = 0;
            if (Request.Form["OrderQty"] != null)
            {
                quantity = Convert.ToInt32(Request.Form["OrderQty"]);

            }
            Book book = db.Books.Find(id);
            List<CartViewModel> cartList = (List<CartViewModel>)Session["cart"] == null ? new List<CartViewModel>() : (List<CartViewModel>)Session["cart"];
            if (cartList.Count == 0)
            {
                cartList.Add(new CartViewModel()
                {
                    Book = book,
                    //Quantity = 1
                    Quantity = quantity
                });
            }
            else
            {
                bool hasID = false;
                foreach (var item in cartList)
                {
                    if (item.Book.BookId == id)
                    {
                        hasID = true;
                        int preQty = item.Quantity;
                        cartList.Remove(item);
                        cartList.Add(new CartViewModel()
                        {
                            Book = book,
                            //Quantity = preQty + 1
                            Quantity = preQty + quantity

                        });
                        break;
                    }
                }

                if (hasID == false)
                {
                    cartList.Add(new CartViewModel()
                    {
                        Book = book,
                        //Quantity = 1
                        Quantity = quantity

                    });
                }
            }

            Session["cart"] = cartList;
            return Redirect("~/Home/BookDetail/" + id);
        }

        public ActionResult AddToCart(int id)
        {
            //var quantity = 1;
            
            Book book = db.Books.Find(id);
            List<CartViewModel> cartList = (List<CartViewModel>)Session["cart"] == null ? new List<CartViewModel>() : (List<CartViewModel>)Session["cart"];
            if (cartList.Count == 0)
            {
                cartList.Add(new CartViewModel()
                {
                    Book = book,
                    Quantity = 1
                    //Quantity = quantity
                });
            }
            else
            {
                bool hasID = false;
                foreach (var item in cartList)
                {
                    if (item.Book.BookId == id)
                    {
                        hasID = true;
                        int preQty = item.Quantity;
                        cartList.Remove(item);
                        cartList.Add(new CartViewModel()
                        {
                            Book = book,
                            Quantity = preQty + 1
                            //Quantity = preQty + quantity
                            
                        });
                        break;
                    }
                }

                if (hasID == false)
                {
                    cartList.Add(new CartViewModel()
                    {
                        Book = book,
                        Quantity = 1
                        
                    });
                }
            }

            Session["cart"] = cartList;
            return Redirect("~/Home/Index");

        }

        public ActionResult ViewCart()
        {
            
            var customers = db.Customers.ToList();

            return View(customers);

        }

        public ActionResult RemoveFromCart(int id)
        {
            List<CartViewModel> cartList = (List<CartViewModel>)Session["cart"];
            foreach (var book in cartList)
            {
                if (book.Book.BookId == id)
                {
                    cartList.Remove(book);
                    break;
                }
            }

            Session["cart"] = cartList;
            //return Redirect("~/Home/Index");
            return Redirect("~/Home/ViewCart");
        }

        public ActionResult IncreaseQty(int id)
        {
            List<CartViewModel> cartList = (List<CartViewModel>)Session["cart"];
            foreach (var book in cartList)
            {
                if (book.Book.BookId == id)
                {
                    book.Quantity++;
                    break;
                }
            }

            Session["cart"] = cartList;
            return Redirect("~/Home/ViewCart");
        }
        public ActionResult DecreaseQty(int id)
        {
            List<CartViewModel> cartList = (List<CartViewModel>)Session["cart"];
            foreach (var book in cartList)
            {
                if (book.Book.BookId == id)
                {
                    book.Quantity--;
                    if (book.Quantity == 0)
                    {
                        cartList.Remove(book);
                    }
                    break;
                }
            }

            Session["cart"] = cartList;
            return Redirect("~/Home/ViewCart");
        }

        public ActionResult CheckOut()
        {

            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult EnterAddress()
        {
            decimal subTotal = 0;
            var totalItems = 0;
            decimal tax = 0;
            decimal total = 0;
            if (Session["cart"] != null)
            {
                foreach (var item in (List<CartViewModel>)Session["cart"])
                {
                    totalItems += item.Quantity;

                    subTotal += Convert.ToDecimal(item.Quantity * item.Book.Price);
                    subTotal = Convert.ToDecimal(subTotal.ToString("N2"));

                }
                tax = subTotal * Convert.ToDecimal(0.15);
                tax = Convert.ToDecimal(tax.ToString("N2"));
                total = subTotal + tax;
            }
            var countries = db.Countries.ToList();
            ViewBag.countryList = new SelectList(countries,"CountryId", "CountryName");
            ViewBag.subTotal = subTotal;
            ViewBag.total = total;
            ViewBag.tax = tax;
            ViewBag.totalItems = totalItems;


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EnterAddress(CheckOutAddressModel checkOutAddressModel)
        {
            var countries = db.Countries.ToList();
            ViewBag.countryList = new SelectList(countries, "CountryId", "CountryName");
            //if (ModelState.IsValid)
            //{
                var user = (System.Security.Claims.ClaimsIdentity)User.Identity;
                Address address = new Address();
                address.City = checkOutAddressModel.Address.City;
                address.CountryId = checkOutAddressModel.Country.CountryId;
                address.Number = checkOutAddressModel.Address.Number;
                address.PostCode = checkOutAddressModel.Address.PostCode;
                address.State = checkOutAddressModel.Address.State;
                address.Street1 = checkOutAddressModel.Address.Street1;
                address.Street2 = checkOutAddressModel.Address.Street2;
                db.Addresses.Add(address);
                db.SaveChanges();

                Customer customer = new Customer();
                customer.FirstName = user.FindFirstValue("FirstName");
                customer.LastName = user.FindFirstValue("LastName");
                customer.Email = user.GetUserName();
                customer.Phone = checkOutAddressModel.Customer.Phone;
                customer.AddressId = address.AddressId;
                db.Customers.Add(customer);
                db.SaveChanges();

                ViewBag.id = customer.CustomerId;
                return RedirectToAction("ChoosePaymentMethod", new{customerId = customer.CustomerId});
            //}
            

        }

        public ActionResult ChoosePaymentMethod(int? customerId)
        {
            if (customerId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customer = db.Customers.Find(customerId);
            if (customer == null)
            {
                return HttpNotFound();
            }

            ViewBag.CreditCard = db.CreditCards.Find(customer.CreditCardId);
            return View(customer);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ChoosePaymentMethod(int customerId)
        //{

        //    return View();
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCreditCard(PaymentViewModel paymentViewModel)
        {
            if (ModelState.IsValid)
            {
                CreditCard creditCard = new CreditCard();
                creditCard.CardIssuer = paymentViewModel.CreditCard.CardIssuer;
                creditCard.CardNo = paymentViewModel.CreditCard.CardNo;
                creditCard.ExpireDate = paymentViewModel.CreditCard.ExpireDate;
                creditCard.HolderName = paymentViewModel.CreditCard.HolderName;
                db.CreditCards.Add(creditCard);
                db.SaveChanges();

                //paymentViewModel.Customer.CreditCard.CreditCardId = creditCard.CreditCardId;
                Customer customer = db.Customers.Find(paymentViewModel.CustomerId);
                customer.CreditCardId = creditCard.CreditCardId;
                db.SaveChanges();
                //return RedirectToAction("ChoosePaymentMethod", customer.CustomerId);
                return Redirect("~/Home/ChoosePaymentMethod/?customerId=" + customer.CustomerId);
            }
            return RedirectToAction("ChoosePaymentMethod",paymentViewModel);
        }

        public ActionResult PlaceOrder(int customerId,PaymentViewModel paymentViewModel )
        {
            var payCash = paymentViewModel.isCash;
            Order order = new Order();
            if (payCash == true)
            {
                order.PaymentMethod = PaymentMethod.CashOnDelivery;
                order.OrderDate = DateTime.Today;
                order.CustomerId = customerId;
                order.OrderStatus = OrderStatus.Processing;
                db.Orders.Add(order);
                db.SaveChanges();
            }
            else
            {
                order.PaymentMethod = PaymentMethod.CreditCard;
                order.OrderDate = DateTime.Today;
                order.CustomerId = customerId;
                order.OrderStatus = OrderStatus.Processing;
                db.Orders.Add(order);
                db.SaveChanges();
            }

            List<CartViewModel> cartList = (List<CartViewModel>) Session["cart"];
            var num = 0;
            if (cartList != null)
            {
                foreach (var book in cartList)
                {
                    Book bookDb = db.Books.Find(book.Book.BookId);
                    bookDb.Stock = bookDb.Stock - book.Quantity;
                    if (bookDb.Stock == 0)
                    {
                        bookDb.isAvailable = false;
                    }
                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.OrderId = order.OrderId;
                    orderDetail.BookId = book.Book.BookId;
                    orderDetail.Quantity = book.Quantity;
                    db.OrderDetails.AddOrUpdate(orderDetail);
                    num += book.Quantity;

                }
            }
            

            db.SaveChanges();
            ViewBag.num = num;
            Session["cart"] = null;
            return View("OrderConfirmation");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
