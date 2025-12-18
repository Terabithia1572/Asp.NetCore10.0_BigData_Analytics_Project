using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NetCore10._0_BigData_Analytics_Project.ViewComponents.CustomerDetailViewComponents
{
    public class _CustomerDetailAIAnalysisByLastOrdersComponentPartial:ViewComponent
    {
        private readonly BigDataOrdersDBContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        public _CustomerDetailAIAnalysisByLastOrdersComponentPartial(BigDataOrdersDBContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
