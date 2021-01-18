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
            var model = _context.exConvertEntities.Select(x => new ExchangeRateViewModel
            {
                ExName = x.ExCorrencyName,
                Rate = x.Rate,
                UpdateDate = x.UpdateDate
            }).ToList();
       
            return model;
        }

        private async Task<List<ExConvertEntity>> getCurencyAsync()
        {
            List<ExConvertEntity> exchangeRateList = new List<ExConvertEntity>();
            var USD_ILS = await _currencyConverter.GetCurrencyExchange("USD","ILS");
            exchangeRateList.Add(new ExConvertEntity() { ExCorrencytId=1, ExCorrencyName = "USD/ILS", Rate = USD_ILS, UpdateDate = DateTime.Now });

            var GBP_EUR = await _currencyConverter.GetCurrencyExchange("GBP", "EUR");
            exchangeRateList.Add(new ExConvertEntity() { ExCorrencytId = 2, ExCorrencyName = "GBP/EUR", Rate = GBP_EUR, UpdateDate = DateTime.Now });

            var EUR_JPY = await _currencyConverter.GetCurrencyExchange("EUR", "JPY");
            exchangeRateList.Add(new ExConvertEntity() { ExCorrencytId = 3, ExCorrencyName = "EUR/JPY", Rate = EUR_JPY, UpdateDate = DateTime.Now });

            var EUR_USD = await _currencyConverter.GetCurrencyExchange("EUR", "USD");
            exchangeRateList.Add(new ExConvertEntity() { ExCorrencytId = 4, ExCorrencyName = "EUR/USD", Rate = EUR_USD, UpdateDate = DateTime.Now });

            //USD/ILS, GBP/EUR, EUR/JPY, EUR/USD


            return exchangeRateList;
        }

        public async Task SetData()
        {
            var data = await getCurencyAsync();           
            await _context.exConvertEntities.AddRangeAsync(data);
            await _context.SaveChangesAsync();
        }
    }
    public interface IExchangeService
    {
        Task SetData();
        Task<List<ExchangeRateViewModel>> GetData();
    }
}
