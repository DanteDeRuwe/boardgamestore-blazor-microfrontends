using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace BoardgameStore.Server.Controllers
{
    [ApiController]
    [Route("/api/assemblies")]
    public class AssembliesController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var files = Directory.GetFiles(@"CDN");
            var relativeUri = files.Select(f => $@"/{f.Replace('\\', '/')}");
            return Ok(relativeUri);
        }
    }
}
