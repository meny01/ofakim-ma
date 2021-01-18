using Nancy.Json;
using Newtonsoft.Json;
using ofakim_hw_ma.Data;
using ofakim_hw_ma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ofakim_hw_ma.Services
{
    public class ExchangeService : IExchangeService
    {
        private readonly exDBContext _context;
        private readonly ICurrencyConverterApi _currencyConverter;

        public ExchangeService(ICurrencyConverterApi currencyConverter, exDBContext context)
        {
            _currencyConverter = currencyConverter;
            _context = context;
        }
      
        public async Task<List<ExchangeRateViewModel>> GetData()
        {
            var model = await getCurencyAsync();
            //List<ExchangeRateViewModel> model = new List<ExchangeRateViewModel>
            //{
            //    new ExchangeRateViewModel(){ExName="ex1",Rate=22,UpdateDate = DateTime.Now },
            //    new ExchangeRateViewModel(){ExName="ex1",Rate=11,UpdateDate = DateTime.Now }

            //};
            return model;
        }

        private async Task<List<ExchangeRateViewModel>> getCurencyAsync()
        {
            List<ExchangeRateViewModel> exchangeRateList = new List<ExchangeRateViewModel>();
            var USD_ILS = await _currencyConverter.GetCurrencyExchange("USD","ILS");
            exchangeRateList.Add(new ExchangeRateViewModel() { ExName = "USD/ILS", Rate = USD_ILS, UpdateDate = DateTime.Now });

            var GBP_EUR = await _currencyConverter.GetCurrencyExchange("GBP", "EUR");
            exchangeRateList.Add(new ExchangeRateViewModel() { ExName = "GBP/EUR", Rate = GBP_EUR, UpdateDate = DateTime.Now });

            var EUR_JPY = await _currencyConverter.GetCurrencyExchange("EUR", "JPY");
            exchangeRateList.Add(new ExchangeRateViewModel() { ExName = "EUR/JPY", Rate = EUR_JPY, UpdateDate = DateTime.Now });

            var EUR_USD = await _currencyConverter.GetCurrencyExchange("EUR", "USD");
            exchangeRateList.Add(new ExchangeRateViewModel() { ExName = "EUR/USD", Rate = EUR_USD, UpdateDate = DateTime.Now });

            //USD/ILS, GBP/EUR, EUR/JPY, EUR/USD


            return exchangeRateList;
        }

        public async Task SetData()
        {
            var model = await getCurencyAsync();
            //var data = 
            _context.exConvertEntities.UpdateRange();
        }
    }
    public interface IExchangeService
    {
        Task SetData();
        Task<List<ExchangeRateViewModel>> GetData();
    }
}
