using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NetCore10._0_BigData_Analytics_Project.Controllers
{
    public class CategoryController : Controller
    {
        private readonly BigDataOrdersDBContext _context; 
//        Bu, Controller içinde kullanılacak bir alan(field).

//“readonly” demek: Bu değişken sadece constructor içinde atanabilir, sonradan değiştirilemez.

//Yani “bağlantıyı al, sakla, ama değiştirme” demek.

        public CategoryController(BigDataOrdersDBContext context) // Constructor
        {
            _context = context; // Bağlantıyı al ve _context alanına ata.
        }
        //Bu, Controller'ın constructor'ı. Dependency Injection ile BigDataOrdersDBContext alınıyor ve _context alanına atanıyor.
//        Dependency Injection(DI) Nedir?

//🔧 Tanım(basitçe)

//Bir sınıfın ihtiyaç duyduğu bağımlılıkları(örneğin veritabanı, servis, logger vs.) kendisinin oluşturmaması, dışarıdan (Framework veya başka bir yapı) verilmesidir.

//Yani “Controller kendi başına new ile context oluşturmaz, dışarıdan alır.”

        public IActionResult CategoryList()
        {
            var values=_context.Categories.ToList(); //_context üzerinden Categories tablosuna erişip, tüm kayıtları liste olarak alıyoruz.
            return View(values ); // View'e bu listeyi gönderiyoruz.
        }
    }
}
