using Evdoshenko_lab10.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static System.Reflection.Metadata.BlobBuilder;

namespace Evdoshenko_lab10.Controllers
{
    public class TeacherController : Controller
    {
        private ApplicationContext db;

        public TeacherController(ApplicationContext context)
        {
            db = context;
        }

        public string GetData()
        {
            return JsonConvert.SerializeObject(db.Teacher.ToList());
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await db.Teacher.ToListAsync());
        }

        [Authorize]
        public IActionResult Create()
        {
            if (!db.Users.FirstOrDefaultAsync(u => u.Username == HttpContext.User.Identity.Name).Result.Role.Equals("OPERATOR") &&
                !db.Users.FirstOrDefaultAsync(u => u.Username == HttpContext.User.Identity.Name).Result.Role.Equals("ADMIN"))
            {
                return RedirectToAction("Index", "Teacher");
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Teacher teacher)
        {
            db.Teacher.Add(teacher);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Teacher");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (!db.Users.FirstOrDefaultAsync(u => u.Username == HttpContext.User.Identity.Name).Result.Role.Equals("OPERATOR") &&
                !db.Users.FirstOrDefaultAsync(u => u.Username == HttpContext.User.Identity.Name).Result.Role.Equals("ADMIN"))
            {
                return RedirectToAction("Index", "Teacher");
            }
            if (id == null)
            {
                return RedirectToAction("Index", "Teacher");
            }

            Teacher teacher = db.Teacher.Find(id);
            if (teacher == null)
            {
                return RedirectToAction("Index");
            }
            return View(teacher);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(Teacher teacher)
        {
            db.Entry(teacher).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Delete(int id)
        {
            if (!db.Users.FirstOrDefaultAsync(u => u.Username == HttpContext.User.Identity.Name).Result.Role.Equals("ADMIN"))
            {
                return RedirectToAction("Index", "Teacher");
            }
            Teacher district = db.Teacher.Find(id);
            if (district == null)
            {
                return RedirectToAction("Index", "Teacher");
            }
            return View(district);
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Teacher teacher = db.Teacher.Find(id);
            if (teacher == null)
            {
                return RedirectToAction("Index", "Teacher");
            }
            db.Teacher.Remove(teacher);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
