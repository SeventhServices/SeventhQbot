using Microsoft.AspNetCore.Mvc;

namespace Seventh.Bot.Controllers
{
    [Route("/")]
    public class IndexController : Controller
    {
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index()
        {
            return Redirect("/docs");
        }
    }
}