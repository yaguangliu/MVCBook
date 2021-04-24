 using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCBookstoreProject.Models;

namespace MVCBookstoreProject.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Customer).Include(o=>o.OrderDetails).ToList();
            //var details = db.OrderDetails.ToList();
            //decimal orderTotal = 0;
            //var orderQuantity = 0;
            //foreach (var order in orders)
            //{
            //    foreach (var detail in details)
            //    {
            //        if (order.OrderId == detail.OrderId)
            //        {
            //            orderQuantity += detail.Quantity;
            //            orderTotal += (detail.Quantity * detail.Book.Price);
            //        }
            //    }
            //    ViewBag.Quantity = orderQuantity;
            //    ViewBag.Total = orderTotal;
            //}
            

            
            return View(orders.ToList());
        }

        public ActionResult ProcessingOrders()
        {
           var orderList = db.Orders.Where(o => o.OrderStatus == OrderStatus.Processing).ToList();
           return View(orderList);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            List<OrderDetail> details = db.OrderDetails.Include(d=>d.Book.Category).Include(d=>d.Book.Language).Include(d=>d.Book.Publisher).Where(d => d.OrderId == id).ToList();
            var customerId = order.CustomerId;
            ViewBag.id = customerId;
            return View(details);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,OrderDate,PaymentMethod,OrderStatus,CustomerId")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName", order.CustomerId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            var orderDetails = db.OrderDetails.Where(d=>d.OrderId == id).ToList();

            if (order == null)
            {
                return HttpNotFound();
            }

            decimal orderTotal = 0;
            var orderQuantity = 0;

            foreach (var detail in orderDetails)
            {
                orderQuantity += detail.Quantity;
                orderTotal += (detail.Quantity * detail.Book.Price);
            }

            ViewBag.Total = orderTotal;
            ViewBag.Quantity = orderQuantity;
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName", order.CustomerId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,OrderDate,PaymentMethod,OrderStatus,CustomerId")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName", order.CustomerId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
