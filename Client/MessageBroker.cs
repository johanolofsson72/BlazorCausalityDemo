using BlazorCausality;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using blazor.Shared;

namespace blazor.Client.Brokers
{
    public class MessageBroker : Linq<Message>
    {
        public MessageBroker(HttpClient httpClient) : base(httpClient)
        {
            
        }
    }
}
