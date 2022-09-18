using CurrencyTracking.Repository.Entities;
using System.Collections.Generic;

namespace CurrencyTracking.Repository.Abstracts
{
    public interface ICurrencyRepository
    {
        IEnumerable<Currency> GetCurrencies(string code);
        void SaveCurrency(Currency currency);
        void RemoveOldRecords();
    }
}
