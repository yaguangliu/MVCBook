using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCBookstoreProject.Models;

namespace MVCBookstoreProject.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        [Authorize(Roles = RoleName.CanManage)]
        public ActionResult Index()
        {
            var orderList = db.Orders;
            var todayOrders =orderList.Where(o => o.OrderDate == DateTime.Today).ToList();
            var toBeHandledOrders = orderList
                .Where(o => o.OrderStatus == OrderStatus.Processing).ToList();
            var customerNums = db.Customers.Count();
            var bookNums = db.Books.Count();

            ViewBag.orders = todayOrders.Count();
            ViewBag.toBeHandledOrderNum = toBeHandledOrders.Count();
            ViewBag.customerNums = customerNums;
            ViewBag.bookNums = bookNums;
            return View();
        }
    }
}