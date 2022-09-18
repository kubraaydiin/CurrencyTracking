using CurrencyTracking.Domain.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CurrencyTracking.API.Controllers
{
    [ApiController]
    [Route("currency")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyOperations _currencyOperations;

        public CurrencyController(ICurrencyOperations currencyOperations)
        {
            _currencyOperations = currencyOperations;
        }

        [HttpGet("")]
        public IActionResult GetCurrencies(string code)
        {
            try
            {
                var currencies = _currencyOperations.GetCurrencies(code);
                return Ok(currencies);
            }

            catch
            {
                return BadRequest();
            }         
        }


        [HttpPost("save")]
        public IActionResult SaveCurrencyRecords()
        {
            try
            {
                _currencyOperations.SaveCurrency();
                return Ok();
            }

            catch
            {
                return BadRequest();
            }
        }
    }
}
