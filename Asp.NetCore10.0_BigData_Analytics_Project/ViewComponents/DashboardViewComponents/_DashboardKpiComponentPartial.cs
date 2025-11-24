using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq; // LINQ metotları için eklendi

namespace Asp.NetCore10._0_BigData_Analytics_Project.ViewComponents.DashboardViewComponents
{
    public class _DashboardKpiComponentPartial : ViewComponent
    {
        private readonly BigDataOrdersDBContext _context;

        public _DashboardKpiComponentPartial(BigDataOrdersDBContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);

            // --- VERİ TOPLAMA ---
            var todayOrderCount = _context.Orders.Count(x => x.OrderDate.Date == today);
            var yesterdayOrderCount = _context.Orders.Count(x => x.OrderDate.Date == yesterday);

            var sevenDaysAgo = today.AddDays(-7);
            var totalOrders7Days = _context.Orders.Count(x => x.OrderDate >= sevenDaysAgo && x.OrderDate < today.AddDays(1));
            var cancelledOrders7Days = _context.Orders.Count(x => x.OrderStatus == "İptal Edildi" && x.OrderDate >= sevenDaysAgo && x.OrderDate < today.AddDays(1));

            var totalOrders = _context.Orders.Count();
            var completedOrders = _context.Orders.Count(x => x.OrderStatus == "Tamamlandı");

            // Günlük ortalama: Boş küme durumunda null dönebilir, bu yüzden double? (nullable double) olarak tanımlanmalı.
            double? dailyAverageOrders = _context.Orders
                .GroupBy(x => x.OrderDate.Date)
                .Select(g => g.Count())
                .Average(); // ✅ CS1061 HATASI ÇÖZÜLDÜ (Implicit olarak double? tipini almalı)

            // --- KPI HESAPLAMALARI (SIFIR KONTROLLÜ) ---

            #region Kpi_1

            // 1. Dünkü Sipariş Değişim Oranı (Bölen: yesterdayOrderCount)
            decimal changeRate = 0;
            if (yesterdayOrderCount != 0) // ✅ DivideByZeroException ÇÖZÜLDÜ
            {
                changeRate = ((decimal)(todayOrderCount - yesterdayOrderCount) / yesterdayOrderCount) * 100;
            }
            else
            {
                // Dün hiç sipariş yokken bugün varsa, çok yüksek artış kabul edilebilir.
                changeRate = (todayOrderCount > 0) ? 10000m : 0m;
            }

            if (changeRate < 0)
            {
                ViewBag.ChangeRateColor = "red";
                ViewBag.TrendingIcon = "zmdi zmdi-trending-down float-right";
            }
            else
            {
                ViewBag.ChangeRateColor = "green";
                ViewBag.TrendingIcon = "zmdi zmdi-trending-up float-right";
            }

            // 2. Bugünün Ortalamaya Oranı (Bölen: dailyAverageOrders)
            double ratio = 0;
            // HasValue kontrolü doğru yapılıyor, çünkü dailyAverageOrders artık double? tipinde kabul ediliyor.
            if (dailyAverageOrders.HasValue && dailyAverageOrders.Value != 0) // ✅ DivideByZeroException ve CS1061 ÇÖZÜLDÜ
            {
                ratio = (todayOrderCount / dailyAverageOrders.Value) * 100.0;
            }


            ViewBag.TodayVsAverageRatio = Math.Round(ratio, 2);
            ViewBag.TodayOrderCount = todayOrderCount;
            ViewBag.DailyOrderChange = Math.Round(changeRate, 2);

            #endregion

            // ---

            #region Kpi_2

            // 3. İptal Oranı (Bölen: totalOrders7Days)
            decimal cancelRate = 0;
            if (totalOrders7Days != 0) // ✅ DivideByZeroException ÇÖZÜLDÜ
            {
                cancelRate = ((decimal)cancelledOrders7Days / totalOrders7Days) * 100;
            }

            ViewBag.CancelledOrders7Days = cancelledOrders7Days;
            ViewBag.CancelRate = Math.Round(cancelRate, 2);
            ViewBag.CancelColor = "red";
            ViewBag.CancelText = cancelRate > 5 ? "Yüksek İptal Oranı ⚠️" : "Normal Düzeyde";

            #endregion

            // ---

            #region Kpi_3

            // 4. Tamamlanma Oranı (Bölen: totalOrders)
            decimal completionRate = 0;
            if (totalOrders != 0) // ✅ DivideByZeroException ÇÖZÜLDÜ
            {
                completionRate = ((decimal)completedOrders / totalOrders) * 100;
            }

            ViewBag.CompletionRate = Math.Round(completionRate, 2);
            ViewBag.CompletedOrders = completedOrders;
            ViewBag.CompletionText = completionRate >= 80 ? "Mükemmel Performans 💪" : "İyileşme Devam Ediyor 📈";

            #endregion


            return View();
        }
    }
}