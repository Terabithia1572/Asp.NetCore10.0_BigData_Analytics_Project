using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Asp.NetCore10._0_BigData_Analytics_Project.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NetCore10._0_BigData_Analytics_Project.ViewComponents.DashboardViewComponents
{
    public class _DashboardFirstChartsComponentPartial : ViewComponent
    {
        private readonly BigDataOrdersDBContext _context;
        public _DashboardFirstChartsComponentPartial(BigDataOrdersDBContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var totalOrders = _context.Orders.Count();

            var preparingCount = _context.Orders.Where(x => x.OrderStatus == "Hazırlanıyor").Count();
            var canceledCount = _context.Orders.Where(x => x.OrderStatus == "İptal Edildi").Count();
            var shippedCount = _context.Orders.Where(x => x.OrderStatus == "Kargoda").Count();
            var completedCount = _context.Orders.Where(x => x.OrderStatus == "Teslim Edildi").Count();

            var result = new List<OrderStatusChartDTO>
            {
                new OrderStatusChartDTO
                {
                    Title = "Hazırlanıyor",
                    Percentage = totalOrders == 0 ? 0 : (int)Math.Round(preparingCount * 100.0 / totalOrders),
                    ChangeText = "+2% Artış ⬆",
                    IsPositive = true,
                    Color = "#00BCD4"
                },
                new OrderStatusChartDTO
                {
                    Title = "İptal Edildi",
                    Percentage = totalOrders == 0 ? 0 : (int)Math.Round(canceledCount * 100.0 / totalOrders),
                    ChangeText = "-1% Azalış ⬇",
                    IsPositive = false,
                    Color = "#FF7043"
                },
                new OrderStatusChartDTO
                {
                    Title = "Kargoda",
                    Percentage = totalOrders == 0 ? 0 : (int)Math.Round(shippedCount * 100.0 / totalOrders),
                    ChangeText = "+4% Artış ⬆",
                    IsPositive = true,
                    Color = "#66BB6A"
                },
                new OrderStatusChartDTO
                {
                    Title = "Teslim Edildi",
                    Percentage = totalOrders == 0 ? 0 : (int)Math.Round(completedCount * 100.0 / totalOrders),
                    ChangeText = "+6% Artış ⬆",
                    IsPositive = true,
                    Color = "#2196F3"
                }
            };

            return View(result);
        }
    }
}

