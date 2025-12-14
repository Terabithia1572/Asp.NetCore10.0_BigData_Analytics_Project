using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Asp.NetCore10._0_BigData_Analytics_Project.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.NetCore10._0_BigData_Analytics_Project.Controllers
{
    public class ReviewController : Controller
    {

        private readonly BigDataOrdersDBContext _context;
        public ReviewController(BigDataOrdersDBContext context)
        {
            _context = context;
        }
        public IActionResult ReviewList(int page = 1)
        {
            int pageSize = 12; // Sayfa başına gösterilecek kayıt sayısı
            var values = _context.Reviews // Reviews tablosuna erişiyoruz
                                 .OrderBy(p => p.ReviewID) // ReviewID'ye göre sıralıyoruz
                                 .Skip((page - 1) * pageSize) // Sayfalama için atlanacak kayıt sayısını hesaplıyoruz
                                 .Take(pageSize) // Sayfa başına kayıt sayısını alıyoruz
                                 .Include(y => y.Product) // İlişkili Product verisini yüklüyoruz
                                 .Include(z => z.Customer) // İlişkili Customer verisini yüklüyoruz
                                 .ToList(); // Sonuçları listeye dönüştürüyoruz

            int totalCount = _context.Reviews.Count(); // Toplam kayıt sayısını alıyoruz
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize); // Toplam sayfa sayısını hesaplıyoruz
            ViewBag.CurrentPage = page; // Mevcut sayfa numarasını ViewBag ile gönderiyoruz

            return View(values); // Verileri View'a gönderiyoruz

        }

        [HttpGet]
        public IActionResult CreateReview()
        {
            return View(); // Yeni yorum oluşturma sayfasını görüntülüyoruz
        }

        [HttpPost]
        public IActionResult CreateReview(Review review)
        {
            review.ReviewDate = DateTime.Now; // Yorum tarihini şu anki tarih ile ayarlıyoruz
            _context.Reviews.Add(review); // Yeni yorumu veritabanına ekliyoruz
            _context.SaveChanges(); // Değişiklikleri kaydediyoruz
            return RedirectToAction("ReviewList"); // Yorum listesine yönlendiriyoruz
        }

        public IActionResult DeleteReview(int id)
        {
            var value = _context.Reviews.Find(id); // Silinecek yorumu ID'sine göre buluyoruz
            _context.Reviews.Remove(value); // Yorumu veritabanından siliyoruz
            _context.SaveChanges(); // Değişiklikleri kaydediyoruz
            return RedirectToAction("ReviewList"); // Yorum listesine yönlendiriyoruz
        }

        [HttpGet]
        public IActionResult UpdateReview(int id)
        {
            var value = _context.Reviews.Find(id); // Güncellenecek yorumu ID'sine göre buluyoruz
            return View(value); // Yorum güncelleme sayfasını görüntülüyoruz
        }

        [HttpPost]
        public IActionResult UpdateReview(Review review) 
        {
            _context.Reviews.Update(review); // Yorumu veritabanında güncelliyoruz
            _context.SaveChanges(); // Değişiklikleri kaydediyoruz
            return RedirectToAction("ReviewList"); // Yorum listesine yönlendiriyoruz
        }
    }
}
