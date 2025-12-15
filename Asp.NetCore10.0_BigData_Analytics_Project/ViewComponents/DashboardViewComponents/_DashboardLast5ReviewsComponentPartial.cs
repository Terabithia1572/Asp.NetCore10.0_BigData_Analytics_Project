using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.NetCore10._0_BigData_Analytics_Project.ViewComponents.DashboardViewComponents
{
    public class _DashboardLast5ReviewsComponentPartial:ViewComponent
    {
        private readonly BigDataOrdersDBContext _context;
        public _DashboardLast5ReviewsComponentPartial(BigDataOrdersDBContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var values = _context.Reviews.OrderByDescending(x => x.ReviewID).Include(y => y.Customer).Include(z => z.Product).Take(5).ToList();
            return View(values);
        }
    }
}
