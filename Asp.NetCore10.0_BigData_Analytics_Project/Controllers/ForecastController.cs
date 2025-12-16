using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Asp.NetCore10._0_BigData_Analytics_Project.DTOs.ForecastDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult GermanyCitiesForecast()
        {
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2025, 12, 31);

            var germanyCityData = _context.Orders
                .Include(o => o.Customer)
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.Customer.CustomerCountry == "Almanya")
                .AsEnumerable()
                .GroupBy(o => new
                {
                    o.Customer.CustomerCity,
                    Year = o.OrderDate.Year,
                    Month = o.OrderDate.Month
                })
                .Select(g => new
                {
                    City = g.Key.CustomerCity,
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    DateKey = $"{g.Key.Year}-{g.Key.Month:D2}",
                    OrderCount = g.Count()
                })
                .OrderBy(xP => xP.City)
                .ThenBy(x => x.DateKey)
                .ToList();

            var forecasts = new List<object>();

            foreach (var city in germanyCityData.Select(x => x.City).Distinct())
            {
                var cityData = germanyCityData
                    .Where(x => x.City == city)
                    .Select((x, index) => new GermanyCitiesForecastData
                    {
                        City = city,
                        MonthIndex = index + 1,
                        OrderCount = x.OrderCount
                    }).ToList();

                if (cityData.Count < 4)
                    continue;

                var dataView = _mlContext.Data.LoadFromEnumerable(cityData);

                var pipeline = _mlContext.Forecasting.ForecastBySsa(
                    outputColumnName: "ForecastedValues",
                    inputColumnName: nameof(GermanyCitiesForecastData.OrderCount),
                    windowSize: 12,
                    seriesLength: cityData.Count,
                    trainSize: cityData.Count,
                    horizon: 12,
                    confidenceLevel: 0.95f
                    );

                var model = pipeline.Fit(dataView);
                var engine = model.CreateTimeSeriesEngine<GermanyCitiesForecastData, GermanyCitiesForecastPrediction>(_mlContext);

                var prediction = engine.Predict();

                var yearlyForecast = (int)prediction.ForecastedValues.Sum();

                var year2024Count = germanyCityData
                    .Where(x => x.City == city && x.Year == 2024)
                    .Sum(x => x.OrderCount);

                var year2025Count = germanyCityData
                    .Where(x => x.City == city && x.Year == 2025)
                    .Sum(x => x.OrderCount);

                var diff = yearlyForecast - year2025Count;
                double? growthRate = year2025Count > 0
                    ? (diff / (double)year2025Count) * 100.0
                    : (double?)null;


              
            }
            return View(forecasts);
        }
    }
}

