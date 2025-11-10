using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Asp.NetCore10._0_BigData_Analytics_Project.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NetCore10._0_BigData_Analytics_Project.Controllers
{
    public class ProductController : Controller
    {
        private readonly BigDataOrdersDBContext _context;
        //        Bu, Controller içinde kullanılacak bir alan(field).

        //“readonly” demek: Bu değişken sadece constructor içinde atanabilir, sonradan değiştirilemez.

        //Yani “bağlantıyı al, sakla, ama değiştirme” demek.

        public ProductController(BigDataOrdersDBContext context) // Constructor
        {
            _context = context; // Bağlantıyı al ve _context alanına ata.
        }
        //Bu, Controller'ın constructor'ı. Dependency Injection ile BigDataOrdersDBContext alınıyor ve _context alanına atanıyor.
        //        Dependency Injection(DI) Nedir?

        //🔧 Tanım(basitçe)

        //Bir sınıfın ihtiyaç duyduğu bağımlılıkları(örneğin veritabanı, servis, logger vs.) kendisinin oluşturmaması, dışarıdan (Framework veya başka bir yapı) verilmesidir.

        //Yani “Controller kendi başına new ile context oluşturmaz, dışarıdan alır.”

        public IActionResult ProductList()
        {
            var values = _context.Products.ToList(); //_context üzerinden Products tablosuna erişip, tüm kayıtları liste olarak alıyoruz.
            return View(values); // View'e bu listeyi gönderiyoruz.
        }
        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
         
            _context.Products.Add(product); // Yeni kategoriyi veritabanına ekliyoruz.
            _context.SaveChanges(); // Değişiklikleri kaydediyoruz.
            return RedirectToAction("ProductList"); // Kategori listesine yönlendiriyoruz.
        }
        public IActionResult DeleteProduct(int id)
        {
            var values = _context.Products.Find(id); // ID'ye göre kategoriyi buluyoruz.
            _context.Products.Remove(values); // Kategoriyi veritabanından siliyoruz.
            _context.SaveChanges(); // Değişiklikleri kaydediyoruz.
            return RedirectToAction("ProductList"); // Kategori listesine yönlendiriyoruz.
        }
        [HttpGet]
        public IActionResult UpdateProduct(int id)
        {
            var values = _context.Products.Find(id); // ID'ye göre kategoriyi buluyoruz.
            return View(values); // Kategoriyi View'e gönderiyoruz.
        }
        [HttpPost]
        public IActionResult UpdateProduct(Product product)
        {
          
            _context.Products.Update(product);
            _context.SaveChanges(); // Değişiklikleri kaydediyoruz.
            return RedirectToAction("ProductList"); // Kategori listesine yönlendiriyoruz.
        }
    }
}
