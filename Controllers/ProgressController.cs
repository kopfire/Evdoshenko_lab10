using Evdoshenko_lab10.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Evdoshenko_lab10.Controllers
{
    public class ProgressController : Controller
    {
        private ApplicationContext db;

        public ProgressController(ApplicationContext context)
        {
            db = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await db.Progress.Where(p => p.User.Username == HttpContext.User.Identity.Name).ToListAsync());
        }
    }
}
