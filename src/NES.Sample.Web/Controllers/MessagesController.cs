using System.Web.Mvc;
using NES.Sample.Messages;
using NES.Sample.Services;
using NServiceBus;

namespace NES.Sample.Web.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly IMessagesService _messagesService;
        private readonly IBus _bus;

        public MessagesController()
            : this(new MessagesService(), MvcApplication.Bus)
        {
        }

        public MessagesController(IMessagesService messagesService, IBus bus)
        {
            _messagesService = messagesService;
            _bus = bus;
        }

        public ActionResult Index()
        {
            return View(_messagesService.Get());
        }

        public ActionResult Send()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Send(SendMessageCommand command)
        {
            if (ModelState.IsValid)
            {
                _bus.Send(command);

                return View("Sent");
            }

            return View("Send");
        }
    }
}
