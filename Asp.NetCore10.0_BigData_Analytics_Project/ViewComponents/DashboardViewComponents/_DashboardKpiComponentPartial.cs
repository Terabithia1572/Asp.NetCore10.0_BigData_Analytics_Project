using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NetCore10._0_BigData_Analytics_Project.ViewComponents.DashboardViewComponents
{
    public class _DashboardKpiComponentPartial : ViewComponent
    {
        private readonly BigDataOrdersDBContext _context;
        public _DashboardKpiComponentPartial(BigDataOrdersDBContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
