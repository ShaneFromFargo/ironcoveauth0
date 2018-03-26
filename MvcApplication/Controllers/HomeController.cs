using PizzaAuth.Models;
using System.Web.Mvc;

namespace PizzaAuth.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            PizzaAuthUser user = new PizzaAuthUser();
            user.IsAuthenticated = false;
            return View(user);
        }
    }
}