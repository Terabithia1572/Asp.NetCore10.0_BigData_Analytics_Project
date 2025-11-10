using Microsoft.AspNetCore.Mvc;

namespace Asp.NetCore10._0_BigData_Analytics_Project.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult CategoryList()
        {
            return View();
        }
    }
}
