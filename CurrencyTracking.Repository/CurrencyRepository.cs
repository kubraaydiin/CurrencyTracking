using Alachisoft.NCache.EntityFrameworkCore;
using CurrencyTracking.Repository.Abstracts;
using CurrencyTracking.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyTracking.Repository
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly PostgreSqlContext _context;

        public CurrencyRepository(PostgreSqlContext context)
        {
            _context = context;
        }

        public void SaveCurrency(Currency currency)
        {
            _context.currencies.Add(currency);
            _context.SaveChanges();
        }

        public void RemoveOldRecords()
        {
            _context.currencies.RemoveRange(_context.currencies.Where(x => x.created_on < DateTime.Today.AddMonths(-2)));
            _context.SaveChanges();
        }

        public IEnumerable<Currency> GetCurrencies(string code)
        {
            var options = new CachingOptions
            {
                StoreAs = StoreAs.SeperateEntities
            };

            var items = (from c in _context.currencies where c.code == code select c).FromCache(options);
            return items;
        }
    }
}
