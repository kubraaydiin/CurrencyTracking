using System;

namespace CurrencyTracking.Repository.Entities
{
    public class Currency
    {
        public int id { get; set; }
        public DateTime created_on { get; set; }
        public string code { get; set; }
        public int unit { get; set; }
        public string type { get; set; }
        public decimal forex_buying { get; set; }
        public decimal forex_selling { get; set; }
        public decimal banknote_buying { get; set; }
        public decimal banknote_selling { get; set; }
    }
}
