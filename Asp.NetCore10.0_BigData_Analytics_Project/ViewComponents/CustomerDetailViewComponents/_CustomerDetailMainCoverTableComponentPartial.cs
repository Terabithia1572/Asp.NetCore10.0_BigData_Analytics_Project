using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NetCore10._0_BigData_Analytics_Project.ViewComponents.CustomerDetailViewComponents
{
    public class _CustomerDetailMainCoverTableComponentPartial:ViewComponent
    {
        private readonly BigDataOrdersDBContext _context;
        public _CustomerDetailMainCoverTableComponentPartial(BigDataOrdersDBContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var value = _context.Customers.Where(x => x.CustomerID == 8).FirstOrDefault();
            return View(value);
        }
    }
}
