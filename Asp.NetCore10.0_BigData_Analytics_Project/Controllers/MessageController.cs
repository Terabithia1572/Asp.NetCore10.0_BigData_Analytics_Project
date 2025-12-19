using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.NetCore10._0_BigData_Analytics_Project.Controllers
{
    public class MessageController : Controller
    {
        private readonly BigDataOrdersDBContext _context;
        public MessageController(BigDataOrdersDBContext context)
        {
            _context = context;
        }
        public IActionResult MessageList(int page=1)
        {
            int pageSize = 12; // her sayfada 12 kayıt
            var values = _context.Messages
                                 .OrderBy(p => p.MessageID)
                                 .Skip((page - 1) * pageSize)
                                 .Take(pageSize)
                                 .Include(y => y.Customer)
                                 .ToList();

            int totalCount = _context.Messages.Count();
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ViewBag.CurrentPage = page;

            return View(values);
        }
    }
}
