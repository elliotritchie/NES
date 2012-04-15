using System.Web.Mvc;
using NES.Sample.Messages;
using NES.Sample.Services;
using NServiceBus;

namespace NES.Sample.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IFormsAuthenticationService _formsAuthenticationService;
        private readonly IBus _bus;

        public AccountController()
            : this (new FormsAuthenticationService(), MvcApplication.Bus)
        {
        }

        public AccountController(IFormsAuthenticationService formsAuthenticationService, IBus bus)
        {
            _formsAuthenticationService = formsAuthenticationService;
            _bus = bus;
        }

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(CreateUserCommand command)
        {
            if (ModelState.IsValid)
            {
                _bus.Send(command);
                _formsAuthenticationService.SignIn(command.UserId.Value, false);

                return RedirectToAction("Index", "Messages");
            }

            return View();
        }
    }
}
