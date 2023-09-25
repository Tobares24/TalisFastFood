using System.Web.Mvc;
using Talis_Fast_Food.Utils;

namespace Talis_Fast_Food.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (TempData.ContainsKey("mensaje"))
            {
                ViewBag.NotificationMessage = TempData["mensaje"];
            }
            return View();
        }

        public ActionResult SobreNosotros()
        {

            return View();
        }

        public ActionResult IndexUsers()
        {
            if (TempData.ContainsKey("mensaje"))
            {
                ViewBag.NotificationMessage = TempData["mensaje"];
            }
            Log.Info("Inicio");
            return View();
        }
    }
}