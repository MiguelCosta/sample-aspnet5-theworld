using Microsoft.AspNet.Mvc;
using TheWorld.Services;

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _mailService;

        public AppController(IMailService service)
        {
            _mailService = service;
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ViewModels.ContactViewModel model)
        {
            if(ModelState.IsValid)
            {
                var email = Startup.Configuration["AppSettings:SiteEmailAddress"];

                if(string.IsNullOrWhiteSpace(email))
                {
                    ModelState.AddModelError("", "Not fount email configuration");
                }

                if(_mailService.SendMail(email, email, $"Email from {model.Email}", model.Message))
                {
                    ModelState.Clear();
                    ViewBag.Message = "Email sent! Thanks :)";
                }
            }
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
