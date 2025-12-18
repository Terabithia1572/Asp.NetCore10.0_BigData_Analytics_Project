using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NetCore10._0_BigData_Analytics_Project.ViewComponents.CustomerDetailViewComponents
{
    public class _CustomerDetailStatisticsComponentPartial:ViewComponent
    {
        private readonly BigDataOrdersDBContext _context;
        public _CustomerDetailStatisticsComponentPartial(BigDataOrdersDBContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(int id)
        {
            id = 8;
            ViewBag.TotalOrderCount = _context.Orders.Where(x => x.CustomerID == id).Count();

            ViewBag.CompletedOrderCount = _context.Orders.Where(x => x.CustomerID == id && x.OrderStatus == "Tamamlandı").Count();

            ViewBag.CanceledOrderCount = _context.Orders.Where(x => x.CustomerID == id && x.OrderStatus == "İptal Edildi").Count();

            ViewBag.GetCustomerCountry = _context.Customers.Where(x => x.CustomerID == id).Select(y => y.CustomerCountry).FirstOrDefault();

            ViewBag.GetCustomerCity = _context.Customers.Where(x => x.CustomerID == id).Select(y => y.CustomerCity).FirstOrDefault();

           

            return View();
        }   
    }
}
