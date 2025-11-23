using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Microsoft.AspNetCore.Mvc;

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
            #region Kpi_1
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);

            var todayOrderCount = _context.Orders.Where(x => x.OrderDate == today).Count();
            var yesterdayOrderCount = _context.Orders.Where(x => x.OrderDate == yesterday).Count();

            if (todayOrderCount > yesterdayOrderCount)
            {
                ViewBag.TrendingIcon = "zmdi zmdi-trending-up float-right";
            }
            else
            {
                ViewBag.TrendingIcon = "zmdi zmdi-trending-down float-right";
            }

            // ✅ Önerilen Çözüm 1
            decimal changeRate = 0;

            if (yesterdayOrderCount != 0)
            {
                changeRate = ((decimal)(todayOrderCount - yesterdayOrderCount) / yesterdayOrderCount) * 100;
            }
            else
            {
                // Dün hiç sipariş yoksa ve bugün varsa, değişim %100'den fazladır veya özel bir değer atayın.
                // Ancak sadece 0 atamak, kodun çökmesini engeller.
                changeRate = (todayOrderCount > 0) ? 10000m : 0m; // Örn: Bugün varsa çok yüksek bir artış sayılabilir
            }

            if (changeRate < 0)
            {
                ViewBag.ChangeRateColor = "red";
            }
            else
            {
                ViewBag.ChangeRateColor = "green";
            }

            var dailyAverageOrders = _context.Orders.GroupBy(x => x.OrderDate.Date).Select(g => g.Count()).Average();

            double ratio = 0;
            ratio = (todayOrderCount / dailyAverageOrders) * 100.0;


            ViewBag.TodayVsAverageRatio = Math.Round(ratio, 2);
            ViewBag.TodayOrderCount = todayOrderCount;
            ViewBag.DailyOrderChange = Math.Round(changeRate, 2);

            #endregion

            #region Kpi_2

            var sevenDaysAgo = today.AddDays(-7);

            var totalOrders7Days = _context.Orders.Count(x => x.OrderDate >= sevenDaysAgo && x.OrderDate < today.AddDays(1));

            var cancelledOrders7Days = _context.Orders.Count(x => x.OrderStatus == "İptal Edildi" && x.OrderDate >= sevenDaysAgo && x.OrderDate < today.AddDays(1));


            decimal cancelRate = 0;
            cancelRate = ((decimal)cancelledOrders7Days / totalOrders7Days) * 100;

            ViewBag.CancelledOrders7Days = cancelledOrders7Days;
            ViewBag.CancelRate = Math.Round(cancelRate, 2);
            ViewBag.CancelColor = "red";
            ViewBag.CancelText = cancelRate > 5 ? "Yüksek İptal Oranı ⚠️" : "Normal Düzeyde";

            #endregion

            #region Kpi_3

            var totalOrders = _context.Orders.Count();

            var completedOrders = _context.Orders.Count(x => x.OrderStatus == "Tamamlandı");
            decimal completionRate = 0;

            completionRate = ((decimal)completedOrders / totalOrders) * 100;

            ViewBag.CompletionRate = Math.Round(completionRate, 2);
            ViewBag.CompletedOrders = completedOrders;
            ViewBag.CompletionText = completionRate >= 80 ? "Mükemmel Performans 💪" : "İyileşme Devam Ediyor 📈";


            #endregion


            return View();
        }
    }
}

