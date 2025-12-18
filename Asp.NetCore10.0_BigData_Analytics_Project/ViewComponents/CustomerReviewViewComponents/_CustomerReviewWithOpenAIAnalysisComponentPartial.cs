using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NetCore10._0_BigData_Analytics_Project.ViewComponents.CustomerReviewViewComponents
{
    public class _CustomerReviewWithOpenAIAnalysisComponentPartial : ViewComponent
    {
        private readonly BigDataOrdersDBContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        public _CustomerReviewWithOpenAIAnalysisComponentPartial(BigDataOrdersDBContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
