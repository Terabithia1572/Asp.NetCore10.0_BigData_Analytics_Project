using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.NetCore10._0_BigData_Analytics_Project.ViewComponents.CustomerAnalyticsViewComponents
{
    public class _CustomerAnalyticsCategoryOnChartSegmentComponentPartial:ViewComponent
    {
        private readonly BigDataOrdersDBContext _context;

        public _CustomerAnalyticsCategoryOnChartSegmentComponentPartial(BigDataOrdersDBContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            #region Statistics

            var today = DateTime.Today;
            var topCategoriesToday = _context.Orders
                .Include(o => o.Product)
                .ThenInclude(p => p.Category)
                .Where(x => x.OrderDate.Date == today)
                .AsEnumerable()
                .GroupBy(o => o.Product.Category.CategoryName)
                .Select(g => new
                {
                    CategoryName = g.Key,
                    OrderCount = g.Count()
                }).OrderByDescending(x => x.OrderCount)
                .Take(3)
                .ToList();

            if (topCategoriesToday.Count > 0)
            {
                ViewBag.TopCategory1Name = topCategoriesToday[0].CategoryName;
                ViewBag.TopCategory1Count = topCategoriesToday[0].OrderCount;
            }

            if (topCategoriesToday.Count > 1)
            {
                ViewBag.TopCategory2Name = topCategoriesToday[1].CategoryName;
                ViewBag.TopCategory2Count = topCategoriesToday[1].OrderCount;
            }

            if (topCategoriesToday.Count > 2)
            {
                ViewBag.TopCategory3Name = topCategoriesToday[2].CategoryName;
                ViewBag.TopCategory3Count = topCategoriesToday[2].OrderCount;
            }

            #endregion
            return View();
        }
    }
}
