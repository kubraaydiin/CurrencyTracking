using CurrencyTracking.Domain.Abstracts;
using CurrencyTracking.Domain.Models;
using CurrencyTracking.Repository.Abstracts;
using CurrencyTracking.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace CurrencyTracking.Domain
{
    public class CurrencyOperations : ICurrencyOperations
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyOperations(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public IEnumerable<Currency> GetCurrencies(string code)
        {
            var currencies = _currencyRepository.GetCurrencies(code);

            return currencies;
        }

        public void SaveCurrency()
        {
            _currencyRepository.RemoveOldRecords();

            var currencies = GetXmlContentFromTCMB();

            foreach (var currency in currencies)
            {
                _currencyRepository.SaveCurrency(new Currency
                {
                    code = currency.Code,
                    unit = int.Parse(currency.Unit),
                    type = currency.Isim,
                    forex_buying = string.IsNullOrEmpty(currency.ForexBuying) ? 0 : Convert.ToDecimal(currency.ForexBuying, new CultureInfo("en-US")),
                    forex_selling = string.IsNullOrEmpty(currency.ForexSelling) ? 0 : Convert.ToDecimal(currency.ForexSelling, new CultureInfo("en-US")),
                    banknote_buying = string.IsNullOrEmpty(currency.BanknoteBuying) ? 0 : Convert.ToDecimal(currency.BanknoteBuying, new CultureInfo("en-US")),
                    banknote_selling = string.IsNullOrEmpty(currency.BanknoteSelling) ? 0 : Convert.ToDecimal(currency.BanknoteSelling, new CultureInfo("en-US")),
                    created_on = Convert.ToDateTime(currency.Date)
                });
            }
        }

        private List<CurrencyModel> GetXmlContentFromTCMB()
        {
            var request = WebRequest.Create("https://www.tcmb.gov.tr/kurlar/today.xml");
            var response = string.Empty;

            using (var reader = new StreamReader((request.GetResponse()).GetResponseStream()))
            {
                response = reader.ReadToEnd();
                reader.Close();
            }

            var xmlDocument = XDocument.Parse(response);
            var query = (from data in xmlDocument.Descendants("Currency")
                         select new CurrencyModel
                         {
                             Date = (string)data.Parent.Attribute("Tarih"),
                             Code = (string)data.Attribute("CurrencyCode"),
                             Unit = data.Element("Unit").Value,
                             Isim = data.Element("Isim").Value,
                             CurrencyName = data.Element("CurrencyName").Value,
                             ForexBuying = data.Element("ForexBuying").Value,
                             ForexSelling = data.Element("ForexSelling").Value,
                             BanknoteBuying = data.Element("BanknoteBuying").Value,
                             BanknoteSelling = data.Element("BanknoteSelling").Value,
                         }).ToList();

            return query;
        }
    }
}
