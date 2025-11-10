using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Asp.NetCore10._0_BigData_Analytics_Project.Entities;
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
            var values = _context.Categories.ToList(); //_context üzerinden Categories tablosuna erişip, tüm kayıtları liste olarak alıyoruz.
            return View(values); // View'e bu listeyi gönderiyoruz.
        }
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            category.IsActive = true; // Kategoriyi aktif yapıyoruz.
            category.CreatedDate = DateTime.Now; // Oluşturulma tarihini şu anki tarih olarak ayarlıyoruz.
            category.CategoryImageUrl = "default-category.png"; // Varsayılan resim URL'si atıyoruz.
            _context.Categories.Add(category); // Yeni kategoriyi veritabanına ekliyoruz.
            _context.SaveChanges(); // Değişiklikleri kaydediyoruz.
            return RedirectToAction("CategoryList"); // Kategori listesine yönlendiriyoruz.
        }
        public IActionResult DeleteCategory(int id)
        {
            var values = _context.Categories.Find(id); // ID'ye göre kategoriyi buluyoruz.
            _context.Categories.Remove(values); // Kategoriyi veritabanından siliyoruz.
            _context.SaveChanges(); // Değişiklikleri kaydediyoruz.
            return RedirectToAction("CategoryList"); // Kategori listesine yönlendiriyoruz.
        }
        [HttpGet]
        public IActionResult UpdateCategory(int id)
        {
            var values=_context.Categories.Find(id); // ID'ye göre kategoriyi buluyoruz.
            return View(values); // Kategoriyi View'e gönderiyoruz.
        }
        [HttpPost]
        public IActionResult UpdateCategory(Category category)
        {
            category.IsActive = true; // Kategoriyi aktif yapıyoruz.
            category.CreatedDate = DateTime.Now; // Oluşturulma tarihini şu anki tarih olarak ayarlıyoruz.
            category.CategoryImageUrl = "default-category.png"; // Varsayılan resim URL'si atıyoruz.
            _context.Categories.Update(category);
            _context.SaveChanges(); // Değişiklikleri kaydediyoruz.
            return RedirectToAction("CategoryList"); // Kategori listesine yönlendiriyoruz.
        }
    }
}
