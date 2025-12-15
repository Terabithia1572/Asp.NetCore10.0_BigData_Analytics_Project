using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Asp.NetCore10._0_BigData_Analytics_Project.DTOs.ForecastDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;

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
                                                                               //Forecast Modeli
                var pipeline = _mlContext.Forecasting.ForecastBySsa( // Zaman Serisi Tahmin Modeli
                    outputColumnName: "ForecastedValues", // Çıkış Kolonu
                    inputColumnName: nameof(PaymentForecastData.OrderCount), // Giriş Kolonu
                    windowSize: 4, // 
                    seriesLength: methodData.Count, // Seri Uzunluğu
                    trainSize: methodData.Count, // Eğitim Verisi Boyutu
                    horizon: 3, // Tahmin Edilecek Adım Sayısı
                    confidenceLevel: 0.95f // Güven Seviyesi
                    );
                var model = pipeline.Fit(dataView); // Modelin Eğitilmesi
                var engine = model.CreateTimeSeriesEngine<PaymentForecastData, PaymentForecastPrediction>(_mlContext); // Tahmin Motorunun Oluşturulması
                var prediction = engine.Predict(); // Tahminin Yapılması

                //2026 Yılı Ocak Şubat Mart Ayı Tahminleri

                for (int i = 0; i < prediction.ForecastedValues.Length; i++) // Tahmin Sonuçlarının İşlenmesi
                {
                    forecasts.Add(new // Tahmin Sonuçlarının Listeye Eklenmesi
                    {
                        PaymentMethod = method, // Ödeme Yöntemi
                        Month = new DateTime(2026, i + 1, 1).ToString("yyyy MMM"), // Ay
                        ForecastedCount = (int)prediction.ForecastedValues[i] // Tahmin Edilen Sipariş Sayısı
                    });
                }

            }
                return View(forecasts); // Tahmin Sonuçlarının Görüntülenmesi
        }
        }
    }
