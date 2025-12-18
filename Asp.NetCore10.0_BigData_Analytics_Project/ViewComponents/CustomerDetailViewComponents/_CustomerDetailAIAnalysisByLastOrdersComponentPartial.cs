using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Asp.NetCore10._0_BigData_Analytics_Project.ViewComponents.CustomerDetailViewComponents
{
    public class _CustomerDetailAIAnalysisByLastOrdersComponentPartial:ViewComponent
    {
        private readonly BigDataOrdersDBContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        public _CustomerDetailAIAnalysisByLastOrdersComponentPartial(BigDataOrdersDBContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            id = 8;

            //Müşteri Listesi
            var customer = _context.Customers
                .Include(c => c.Orders)
                .ThenInclude(o => o.Product)
                .ThenInclude(p => p.Category)
                .Where(c => c.CustomerID == id)
                .Select(c => new
                {
                    c.CustomerName,
                    c.CustomerSurname,
                    Orders = c.Orders
                    .OrderByDescending(o => o.OrderDate)
                    .Take(20)
                    .Select(o => new
                    {
                        o.OrderDate,
                        Product = o.Product.ProductName,
                        Category = o.Product.Category.CategoryName,
                        o.Quantity,
                        o.Product.UnitPrice,
                        TotalPrice = o.Quantity * o.Product.UnitPrice
                    })
                }).FirstOrDefault();

            var jsonData = JsonSerializer.Serialize(customer);


            //Prompt Yazımı
            string prompt = $@"
⚠️ Çok önemli:
Kesinlikle ``` (backtick) veya kod bloğu verme.
Sadece saf HTML üret. Markdown verme. Kod bloğu verme.

Sen bir veri analisti ve müşteri davranış uzmanısın.
Aşağıdaki veriyi analiz et ve sonucu HTML formatında ver.

Bu başlıkları kullan (sırasını ve isimleri değiştirme):

<h4>👤 Müşteri Profili</h4>
<p><b>Ad:</b> ...</p>
<p><b>Soyad:</b> ...</p>
<p><b>Toplam Sipariş:</b> ...</p>
<p><b>Toplam Harcama:</b> ...</p>

<h4>🛍️ Ürün Tercihleri</h4>
<ul>
  <li>🏠 Ev & Dekorasyon – X sipariş</li>
  <li>💄 Kozmetik – X sipariş</li>
</ul>
<p><b>Öne çıkan ürünler:</b></p>
<ul>
  <li>Ürün adı (adet — fiyat)</li>
</ul>

<h4>⏰ Zaman Bazlı Alışveriş Davranışı</h4>
<p>En yoğun ay: ...</p>
<p>En yoğun gün: ...</p>
<p>Favori saat aralığı: ...</p>

<h4>💰 Ortalama Harcama ve Sıklık</h4>
<p>Aylık ortalama sipariş: ...</p>
<p>Ortalama sepet tutarı: ...</p>
<p>En yüksek sipariş: ...</p>
<p>En düşük sipariş: ...</p>

<h4>🎯 Sadakat ve Tekrar Harcama Eğilimi</h4>
<p>Tekrar alışveriş eğilimi: ...</p>
<p>Marka sadakati: ...</p>
<p>Kategori sadakati: ...</p>

<h4>🚀 Pazarlama Önerileri</h4>
<ul>
  <li>🎁 Kampanya önerisi: ...</li>
  <li>✉️ Hedefli e-posta: ...</li>
  <li>🆕 Yeni ürün tanıtımı önerisi: ...</li>
</ul>

Veri:
{jsonData}
";
        }
    }
}
