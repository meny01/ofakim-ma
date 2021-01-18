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

        private async Task getCurencyAsync()
        {
            List<ExConvertEntity> exisistData = _context.exConvertEntities.ToList(); //get all exist data
            List<ExConvertEntity> exchangeRateListNew = new List<ExConvertEntity>(); //for new data
            List<ExConvertEntity> exchangeRateListUpdate = new List<ExConvertEntity>(); //for update data

            //need to separate this function to update and add content 

            var USD_ILSdata = exisistData.FirstOrDefault(x => x.ExCorrencyName == "USD/ILS");
            var USD_ILS = await _currencyConverter.GetCurrencyExchange("USD", "ILS");

            if (USD_ILSdata != null)
            {
                USD_ILSdata.Rate = USD_ILS;
                USD_ILSdata.UpdateDate = DateTime.Now;
                exchangeRateListUpdate.Add(USD_ILSdata);
            }
            else
               exchangeRateListNew.Add(new ExConvertEntity() { ExCorrencyName = "USD/ILS", Rate = USD_ILS, UpdateDate = DateTime.Now });
            ///////////
            var GBP_EURdata = exisistData.FirstOrDefault(x => x.ExCorrencyName == "GBP/EUR");
            var GBP_EUR = await _currencyConverter.GetCurrencyExchange("GBP", "EUR");
            
            if (GBP_EURdata != null)
            {
                GBP_EURdata.Rate = GBP_EUR;
                GBP_EURdata.UpdateDate = DateTime.Now;
                exchangeRateListUpdate.Add(GBP_EURdata);
            }
            else
                exchangeRateListNew.Add(new ExConvertEntity() { ExCorrencyName = "GBP/EUR", Rate = GBP_EUR, UpdateDate = DateTime.Now });
            //////////
            var EUR_JPYdata = exisistData.FirstOrDefault(x => x.ExCorrencyName == "EUR/JPY");
            var EUR_JPY = await _currencyConverter.GetCurrencyExchange("EUR", "JPY");

            if (EUR_JPYdata != null)
            {
                EUR_JPYdata.Rate = EUR_JPY;
                EUR_JPYdata.UpdateDate = DateTime.Now;
                exchangeRateListUpdate.Add(EUR_JPYdata);
            }
            else
              exchangeRateListNew.Add(new ExConvertEntity() { ExCorrencyName = "EUR/JPY", Rate = EUR_JPY, UpdateDate = DateTime.Now });
            /////////
            var EUR_USDdata = exisistData.FirstOrDefault(x => x.ExCorrencyName == "EUR/USD");
            var EUR_USD = await _currencyConverter.GetCurrencyExchange("EUR", "USD");

            if (EUR_USDdata != null)
            {
                EUR_USDdata.Rate = EUR_USD;
                EUR_USDdata.UpdateDate = DateTime.Now;
                exchangeRateListUpdate.Add(EUR_USDdata);
            }
            else
                exchangeRateListNew.Add(new ExConvertEntity() { ExCorrencyName = "EUR/USD", Rate = EUR_USD, UpdateDate = DateTime.Now });

            //USD/ILS, GBP/EUR, EUR/JPY, EUR/USD

            await _context.exConvertEntities.AddRangeAsync(exchangeRateListUpdate);
            _context.exConvertEntities.UpdateRange();

            
        }

        public async Task SetData()
        {
            await getCurencyAsync();           
            await _context.SaveChangesAsync();
        }
    }
    public interface IExchangeService
    {
        Task SetData();
        Task<List<ExchangeRateViewModel>> GetData();
    }
}
