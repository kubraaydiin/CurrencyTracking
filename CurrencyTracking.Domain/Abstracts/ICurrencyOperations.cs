using CurrencyTracking.Repository.Entities;
using System.Collections.Generic;

namespace CurrencyTracking.Domain.Abstracts
{
    public interface ICurrencyOperations
    {
        IEnumerable<Currency> GetCurrencies(string code);
        void SaveCurrency();
    }
}
