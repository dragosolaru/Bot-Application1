using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using System;
using System.IO;
using BotStock.Services;

namespace BotStock

{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            if (activity.Type == ActivityTypes.Message)
            {
               

                var stockSymbol = activity.Text;
                var replyMessage = await PrimesteStock(stockSymbol);
                Activity reply = activity.CreateReply(replyMessage);
                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    
        private async Task<string> PrimesteStock(string symbolPrimit)
        {
            var stockItem = await StockService.GetStockPrice(symbolPrimit);
            string valoreReturnata;
            if ("FAIL" == stockItem.Status)
            {
                valoreReturnata = $"Nu am gasit nici un stock cu simbolul '{symbolPrimit}'";
            }
            else
            {
                valoreReturnata = $"Pentru stocul '{stockItem.Symbol}' al companiei '{stockItem.Name}' ultimul pret este: '{stockItem.LastPrice}'";
            }
            return valoreReturnata;

        }

        private Activity HandleSystemMessage(Activity message)
        {
            ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
            
            }
            else if (message.Type == ActivityTypes.Ping)
            {
             
            }
           
            return null;
        }
    }
}