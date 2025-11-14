namespace Asp.NetCore10._0_BigData_Analytics_Project.Entities
{
    public class Customer
    {
        public int CustomerID { get; set; } //Müşteri ID'sini Aldık
        public string CustomerName { get; set; }  //Müşteri Adını Aldık
        public string CustomerSurname { get; set; } //
        public string CustomerEmail { get; set; } //Müşteri Emailini Aldık
        public string CustomerPhone { get; set; }  //Müşteri Şehrini Aldık
        public string CustomerCity { get; set; } //Müşteri Telefonunu Aldık
        public string CustomerDistrict { get; set; } //
        public string CustomerCountry { get; set; } // Müşterinin Ülke Bilgilerini Aldık
        public string CustomerImageURL { get; set; } //Müşteri Resim URL'sini Aldık
        public string CustomerAddress { get; set; } // Müşterinin Adres Bilgilerini Aldık
        public List<Order> Orders { get; set; } // Siparişi Bir liste halinde aldık ilişkiden dolayı

    }
}
