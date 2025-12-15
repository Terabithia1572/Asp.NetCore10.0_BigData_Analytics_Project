using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Asp.NetCore10._0_BigData_Analytics_Project.DTOs.ForecastDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;

namespace Asp.NetCore10._0_BigData_Analytics_Project.Controllers
{
    public class ForecastController : Controller
    {
        private readonly BigDataOrdersDBContext _context;
        private readonly MLContext _mlContext;

        public ForecastController(BigDataOrdersDBContext context, MLContext mlContext)
        {
            _context = context;
            _mlContext = mlContext;
        }

        public IActionResult PaymentMethodForecast()
        {
            //2025 Yılı Verilerinin Çekilmesi
            var startDate = new DateTime(2025, 1, 1); //2025 yılının başı
            var endDate = new DateTime(2025, 12, 31); //2025 yılının sonu

            var monthlyPaymentData = _context.Orders // Sipariş Verilerinin Çekilmesi
               .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate) // 2025 Yılı Filtrelemesi
               .AsEnumerable() // Bellekte İşleme
               .GroupBy(o => new // Ay ve Ödeme Yöntemine Göre Gruplama
               {
                   Month = new DateTime(o.OrderDate.Year, o.OrderDate.Month, 1), // Ay
                   o.PaymentMethod // Ödeme Yöntemi
               })
               .Select(g => new // Model Oluşturma
               {
                   Month = g.Key.Month, // Ay
                   PaymentMethod = g.Key.PaymentMethod, // Ödeme Yöntemi
                   OrderCount = g.Count() // Sipariş Sayısı
               })
               .OrderBy(x => x.Month) // Ay'a Göre Sıralama
               .ToList(); // Listeye Dönüştürme

            //Tahmin Sonuçlarını Tutacak Liste
            var forecasts = new List<Object>();

            //Her Ödeme Yöntemi İçin Ayrı Ayrı Model Oluşturulması
            foreach (var method in monthlyPaymentData.Select(x => x.PaymentMethod).Distinct()) // Her ödeme yöntemi için döngü
            {
                var methodData = monthlyPaymentData // Belirli ödeme yöntemine ait veriler
                    .Where(x => x.PaymentMethod == method) // Filtreleme
                   .Select((x, index) => new PaymentForecastData // Ay indeksini ekleme
                   {
                       PaymentMethod = method, // Ödeme Yöntemi
                       MonthIndex = index + 1, // Ay İndeksi (1'den başlar)
                       OrderCount = x.OrderCount // Sipariş Sayısı
                   }).ToList(); // Listeye dönüştürme
                var dataView = _mlContext.Data.LoadFromEnumerable(methodData); // Veriyi ML.NET DataView formatına dönüştürme

                return View();
            }
        }
    }
}