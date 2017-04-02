using BotStock.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace BotStock.Services
{
    public class StockService
    {
        public static async Task<StockItem> GetStockPrice(string symbol)
        {
            string uri = $"http://dev.markitondemand.com/Api/v2/Quote/json?symbol={symbol}";
            var stockItem = new StockItem();
            using (var client = new WebClient())
            {
                var rawData = await client.DownloadStringTaskAsync(new Uri(uri));
                stockItem = JsonConvert.DeserializeObject<StockItem>(rawData);
                stockItem.Status = stockItem.Status ?? "FAIL";
                return stockItem;
            }
                 return null;

               /* if (stockItem.Status == "FAIL")
                {
                    return stockItem;
                }
                else
                {
                    return null;
                }*/

            

        }
    }
}