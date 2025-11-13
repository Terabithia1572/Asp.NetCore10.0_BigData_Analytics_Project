using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Asp.NetCore10._0_BigData_Analytics_Project.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NetCore10._0_BigData_Analytics_Project.Controllers
{
    public class CustomerController : Controller
    {
        private readonly BigDataOrdersDBContext _context;
        public CustomerController(BigDataOrdersDBContext context)
        {
            _context = context;
        }
        public IActionResult CustomerList(int page = 1)
        {
            int pageSize = 12;
            var values = _context.Customers
                                 .OrderBy(p => p.CustomerID)
                                 .Skip((page - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToList();

            int totalCount = _context.Customers.Count();
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ViewBag.CurrentPage = page;
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCustomer(Customer Customer)
        {
            _context.Customers.Add(Customer);
            _context.SaveChanges();
            return RedirectToAction("CustomerList");
        }

        public IActionResult DeleteCustomer(int id)
        {
            var value = _context.Customers.Find(id);
            _context.Customers.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("CustomerList");
        }

        [HttpGet]
        public IActionResult UpdateCustomer(int id)
        {
            var value = _context.Customers.Find(id);
            return View(value);
        }

        [HttpPost]
        public IActionResult UpdateCustomer(Customer Customer)
        {
            _context.Customers.Update(Customer);
            _context.SaveChanges();
            return RedirectToAction("CustomerList");
        }
    }
}
