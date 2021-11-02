using BlazorCausality;

namespace blazor.Shared
{
    public class Message : EntityBase
    {
        public string? Subject { get; set; }
        public string? Body { get; set; }
    }
}
