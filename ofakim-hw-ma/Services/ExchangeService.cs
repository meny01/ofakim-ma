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
        /// <summary>
        /// get data from db 
        /// use ExchangeRateViewModel because the Id not relevent
        /// </summary>
        /// <returns>List of data</returns>
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

        private async Task GetAndUpdateCurencyAsync()
        {
            List<ExConvertEntity> exisistData = _context.exConvertEntities.ToList(); //get all exist data

            await AddOrUpdate(exisistData, "USD/ILS");
            await AddOrUpdate(exisistData, "GBP/EUR");
            await AddOrUpdate(exisistData, "EUR/JPY");
            await AddOrUpdate(exisistData, "EUR/USD");
    
            
        }
        private async Task AddOrUpdate(List<ExConvertEntity> exisistData, string Currency)
        {
            string[] strSplit = Currency.Split("/");
            var Currencydata = exisistData.FirstOrDefault(x => x.ExCorrencyName == Currency);
            var Rate = await _currencyConverter.GetCurrencyExchange(strSplit[0], strSplit[1]);

            if (Currencydata != null)
            {
                Currencydata.Rate = Rate;
                Currencydata.UpdateDate = DateTime.Now;
                _context.Update(Currencydata);
            }
            else
                _context.Add(new ExConvertEntity() { ExCorrencyName = Currency, Rate = Rate, UpdateDate = DateTime.Now });
        }
        /// <summary>
        /// update or add data to db
        /// </summary>
        /// <returns></returns>
        public async Task SetData()
        {
            await GetAndUpdateCurencyAsync();           
            await _context.SaveChangesAsync();
        }
    }
    public interface IExchangeService
    {
        Task SetData();
        Task<List<ExchangeRateViewModel>> GetData();
    }
}
