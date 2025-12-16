using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NetCore10._0_BigData_Analytics_Project.ViewComponents.CustomerAnalyticsViewComponents
{
    public class _CustomerAnalyticsMainStatisticsComponentPartial:ViewComponent
    {
        private readonly BigDataOrdersDBContext _context;

        public _CustomerAnalyticsMainStatisticsComponentPartial(BigDataOrdersDBContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
