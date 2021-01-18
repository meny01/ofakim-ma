using Nancy.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ofakim_hw_ma.Services
{
    public class CurrencyConverterApi : ICurrencyConverterApi
    {
        private readonly String BASE_URI = "https://free.currconv.com";
        private readonly String API_VERSION = "v7";
        private readonly String API_KEY = "a7525e59152a8ec1d1e2"; //free api key(need to put inside app-setting)

        public CurrencyConverterApi() { }

        public async Task<decimal> GetCurrencyExchange(String localCurrency, String foreignCurrency)
        {
            var code = $"{localCurrency}_{foreignCurrency}";
            var newRate = await FetchSerializedData(code);
            return newRate;
        }

        private async Task<decimal> FetchSerializedData(String code)
        {
            var url = $"{BASE_URI}/api/{API_VERSION}/convert?apiKey={API_KEY}&q={code}&compact=y";
            HttpClient client = new HttpClient();

            var conversionRate = 1.0m;
            try
            {
                var response = await client.GetStringAsync(url);
                var data = JObject.Parse(response);
                var val = Convert.ToString(data[code]["val"]);
                conversionRate = Convert.ToDecimal(val);


            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return conversionRate;
        }
    }
    public interface ICurrencyConverterApi
    {
        Task<decimal> GetCurrencyExchange(String localCurrency, String foreignCurrency);
    }
}
