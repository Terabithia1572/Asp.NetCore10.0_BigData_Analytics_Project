namespace Asp.NetCore10._0_BigData_Analytics_Project.Entities
{
    public class Order
    {
        public int OrderID { get; set; } // Sipariş ID'si
        public int ProductID { get; set; } // Ürün ID'si
        public int CustomerID { get; set; } // Müşteri ID'si
        public int Quantity { get; set; } // Miktar
        public string PaymentMethod { get; set; } // Ödeme Yöntemi
        public string OrderStatus { get; set; } // Sipariş Durumu
        public DateTime OrderDate { get; set; } // Sipariş Tarihi
        public string OrderNotes { get; set; } //Sipariş Notları
        public Product Product { get; set; }
        public Customer Customer { get; set; }
    } 
}
