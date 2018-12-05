using System.Web.Mvc;

namespace Manager.Web.Controllers
{
    public class AboutController : ManagerControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}