using System.Collections.Generic;

namespace CurrencyTracking.Domain.Models
{
	public class CurrencyModel
    {
        public string Date { get; set; }
        public string Code { get; set; }
        public string Unit { get; set; }
        public string Isim { get; set; }
        public string CurrencyName { get; set; }
        public string ForexBuying { get; set; }
        public string ForexSelling { get; set; }
        public string BanknoteBuying { get; set; }
        public string BanknoteSelling { get; set; }
    }
}
