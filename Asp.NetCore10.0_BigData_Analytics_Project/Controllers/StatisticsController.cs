using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.NetCore10._0_BigData_Analytics_Project.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly BigDataOrdersDBContext _context;

        public StatisticsController(BigDataOrdersDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.CategoryCount = _context.Categories.Count(); // Toplam kategori sayısı 
            ViewBag.CustomerCount = _context.Customers.Count(); // Toplam müşteri sayısı
            ViewBag.ProductCount = _context.Products.Count(); // Toplam ürün sayısı
            ViewBag.OrderCount = _context.Orders.Count(); // Toplam sipariş sayısı

            ViewBag.CustomerCount = _context.Customers.Select(x => x.CustomerCountry).Distinct().Count(); // Müşteriye ait Farklı ülke sayısı
            ViewBag.CustomerCity = _context.Customers.Select(x => x.CustomerCity).Distinct().Count(); // Müşteriye ait Farklı şehir sayısı
            ViewBag.OrderStatusByCompleted = _context.Orders.Where(x => x.OrderStatus == "Teslim Edildi").Count();
            ViewBag.OrderStatusByCancelled = _context.Orders.Where(x => x.OrderStatus == "Kargoda").Count();

            ViewBag.OctoberOrders = _context.Orders
   .FromSqlRaw("SELECT * FROM Orders WHERE OrderDate >= '2025-10-01' AND OrderDate < '2025-11-01'")
   .Count();

            ViewBag.Orders2025Count = _context.Orders.Where(x => x.OrderDate.Year == 2025).Count();

            ViewBag.AverageProductPrice = _context.Products.Average(x => x.UnitPrice);
            ViewBag.AverageProductQuantity = _context.Products.Average(x => x.StockQuantity);


            return View();
        }
        public IActionResult TextualStatistics()
        {
            ViewBag.MostExpensiveProduct = _context.Products.Where(x => x.UnitPrice == (_context.Products.Max(x => x.UnitPrice))).Select(y => y.ProductName).FirstOrDefault(); //En pahalı ürün
            ViewBag.CheapestProduct = _context.Products.Where(x => x.UnitPrice == (_context.Products.Min(y => y.UnitPrice))).Select(z => z.ProductName).FirstOrDefault(); //En ucuz ürün


            ViewBag.TopStockProduct = _context.Products.OrderByDescending(x => x.StockQuantity).Take(1).Select(y => y.ProductName).FirstOrDefault(); //En yüksek stoklu ürün

            ViewBag.LowestStockProduct = _context.Products.OrderBy(x => x.StockQuantity).Take(1).Select(y => y.ProductName).FirstOrDefault(); //En düşük stoklu ürün

            ViewBag.LastAddedProduct = _context.Products.OrderByDescending(x => x.ProductID).Take(1).Select(y => y.ProductName).FirstOrDefault(); //Son eklenen ürün

            ViewBag.LastAddedCustomer = _context.Customers.OrderByDescending(x => x.CustomerID).Take(1).Select(y => y.CustomerName + " " + y.CustomerSurname).FirstOrDefault(); //Son eklenen müşteri

            ViewBag.TopPaymentMethod = _context.Orders.GroupBy(o => o.PaymentMethod).Select(g => new 
            {
                PaymentMethod = g.Key,
                TotalOrders = g.Count(),
            }).OrderByDescending(x => x.TotalOrders).Select(y => y.PaymentMethod).FirstOrDefault(); //En çok kullanılan ödeme yöntemi
            ViewBag.TopOrderedProduct = _context.Orders.GroupBy(o => o.Product.ProductName).Select(g => new
            {
                ProductName = g.Key,
                TotalQuantity = g.Sum(o => o.Quantity)
            }).OrderByDescending(x => x.TotalQuantity).Select(y => y.ProductName).FirstOrDefault(); // En çok sipariş edilen ürün

            ViewBag.MinOrderedProduct = _context.Orders.GroupBy(o => o.Product.ProductName).Select(g => new
            {
                ProductName = g.Key,
                TotalQuantity = g.Sum(o => o.Quantity)
            }).OrderBy(x => x.TotalQuantity).Select(y => y.ProductName).FirstOrDefault(); // En az sipariş edilen ürün

            ViewBag.TopCountry = _context.Orders.GroupBy(o => o.Customer.CustomerCountry).Select(g => new
            {
                Country = g.Key,
                TotalOrders = g.Count()
            })
         .OrderByDescending(x => x.TotalOrders).Select(x => x.Country).FirstOrDefault(); // En çok sipariş veren ülke

            ViewBag.TopCity = _context.Orders.GroupBy(o => o.Customer.CustomerCity).Select(g => new
            {
                City = g.Key,
                TotalOrders = g.Count()
            }).OrderByDescending(x => x.TotalOrders).Select(x => x.City).FirstOrDefault(); // En çok sipariş veren şehir

            ViewBag.TopCategory = _context.Orders.GroupBy(o => o.Product.Category.CategoryName)
           .Select(g => new
           {
               CategoryName = g.Key,
               TotalSales = g.Sum(x => x.Quantity)
           }).OrderByDescending(x => x.TotalSales).Select(x => x.CategoryName).FirstOrDefault(); // En çok satan kategori

            ViewBag.LowestActiveCategory = _context.Orders.GroupBy(o => o.Product.Category.CategoryName)
          .Select(g => new
          {
              CategoryName = g.Key,
              TotalSales = g.Sum(x => x.Quantity)
          }).OrderBy(x => x.TotalSales).Select(x => x.CategoryName).FirstOrDefault(); // En az satan kategori

            ViewBag.TopCustomer = _context.Orders.GroupBy(o => new { o.Customer.CustomerName, o.Customer.CustomerSurname })
            .Select(g => new
            {
                FullName = g.Key.CustomerName + " " + g.Key.CustomerSurname,
                TotalOrders = g.Count()
            }).OrderByDescending(x => x.TotalOrders).Select(x => x.FullName).FirstOrDefault(); // En çok sipariş veren müşteri

            ViewBag.MostCompletedProduct = _context.Orders.Where(o => o.OrderStatus == "Teslim Edildi").GroupBy(o => o.Product.ProductName)
           .Select(g => new
           {
               ProductName = g.Key,
               CompletedOrders = g.Count()
           }).OrderByDescending(x => x.CompletedOrders).Select(x => x.ProductName).FirstOrDefault(); // En çok tamamlanan ürün

            ViewBag.TopReturnedProduct = _context.Orders.Where(o => o.OrderStatus == "Hazırlanıyor").GroupBy(o => o.Product.ProductName)
          .Select(g => new
          {
              ProductName = g.Key,
              CompletedOrders = g.Count()
          }).OrderByDescending(x => x.CompletedOrders).Select(x => x.ProductName).FirstOrDefault(); // En çok iade edilen ürün

            return View();
        }
    }
}
