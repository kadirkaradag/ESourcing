using ESourcing.UI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ESourcing.UI.Controllers
{
    //[Authorize]
    public class AuctionController : Controller
    {
        public IActionResult Index()
        {
            List<AuctionViewModel> model = new List<AuctionViewModel>();
            return View(model);
        }
    }
}
