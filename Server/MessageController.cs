using BlazorCausality;
using Microsoft.AspNetCore.Mvc;
using club.Server.Data;
using blazor.Shared;

namespace blazor.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessageController : GenericController<Message, ApplicationDbContext>
    {
        public MessageController(ServerRepository<Message, ApplicationDbContext> messageManager) : base(messageManager)
        {
            
        }

    }
}

