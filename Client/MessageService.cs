using blazor.Client.Brokers;
using blazor.Shared;

namespace blazor.Client.Services
{
    public class MessageService : IMessageService
    {
        private MessageBroker MessageBroker { get; set; }

        public MessageService(MessageBroker messageBroker)
        {
            MessageBroker = messageBroker;
        }

        public async Task TryGetAsync(Action<List<Message>> onSuccess, Action<string> onFailure)
        {
            try
            {
                List<Message> messages = await MessageBroker
                    .Where(m => m.Id > 0)
                        .OrderBy(m => m.UpdatedDate)
                            .ToListAsync();

                if (messages is null)
                {
                    throw new Exception("No rows found");
                }

                onSuccess.Invoke(messages);
            }
            catch (Exception ex)
            {
                onFailure.Invoke(ex.ToString());
            }
        }
    }
}
