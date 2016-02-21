using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using HaojiLib.Localization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleWeb.Controllers
{
    public class LocaleController : Controller
    {
        private ILocalizationService locSvc;

        public LocaleController(ILocalizationService locSvc)
        {
            this.locSvc = locSvc;
        }
        // GET: /<controller>/
        public IActionResult Change(String culture)
        {
            locSvc.Culture = culture;
            Response.Redirect(Request.Headers["Referer"]);
            return Content("OK");
        }
    }
}
