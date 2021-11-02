
using blazor.Shared;

namespace blazor.Client.Services
{
    public interface IMessageService
    {
        Task TryGetAsync(Action<List<Message>> onSuccess, Action<string> onFailure);
    }
}
