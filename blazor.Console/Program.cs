using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using blazor.Shared;
using BlazorCausality;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddHttpClient<MessageBroker>(c => c.BaseAddress = new Uri("https://localhost:7199/"));
var serviceProvider = services.BuildServiceProvider();

var messageBroker = serviceProvider.GetService<MessageBroker>();

if (messageBroker is not null)
{
    var messages = await messageBroker
        .Where(m => m.Id > 0)
            .OrderBy(m => m.Id)
                .ToListAsync();

    foreach (var item in messages)
    {
        Console.WriteLine($"Id:{item.Id} Subject:{item.Subject} Body:{item.Body} UpdatedDate:{item.UpdatedDate}");
    }
}


public class MessageBroker : Linq<Message>
{
    public MessageBroker(HttpClient httpClient) : base(httpClient) { }
}
