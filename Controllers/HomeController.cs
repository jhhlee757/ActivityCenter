using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ActivityCenter.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;


namespace ActivityCenter.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            List<User> AllUsers = dbContext.Users.ToList();
            return View();
        }
        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email is already in use!");
                    return View ("Index");
                }

                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                dbContext.Add(user);
                dbContext.SaveChanges();
                
                HttpContext.Session.SetInt32("UserID", user.UserId);
                int? IntVariable = HttpContext.Session.GetInt32("UserID");
                return RedirectToAction("home");
            }
            return View("Index");
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost("process")]
        public IActionResult Process(LoginUser userSubmission)
        {
            if(ModelState.IsValid)
            {
                User userInDb = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.LEmail);
                if(userInDb == null)
                {
                    ModelState.AddModelError("LEmail", "Invalid Email/Password");
                    return View("Index");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LPassword);
                if(result == 0)
                {
                    ModelState.AddModelError("LPassword", "Invalid Email/Password");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("UserID", userInDb.UserId);
                return RedirectToAction("home");
            }
            return View("Index");
        }

        [HttpGet("home")]
        public IActionResult home(int ActivityId)
        {
            int? IntVariable = HttpContext.Session.GetInt32("UserID");
            if (IntVariable == null)
            {
                return RedirectToAction("Index");
            }
            User LoggedIn = dbContext.Users
            .SingleOrDefault(d => d.UserId == IntVariable);
            ViewBag.LoggedUser = LoggedIn.Name;
            ViewBag.LoggedInUser = dbContext.Users
            .SingleOrDefault(d => d.UserId == IntVariable);
            
            ViewBag.AllActivities = dbContext.Activities.ToList();
            
            var getsameactivity = dbContext.Activities
            .Include(w => w.ActivityParticipant )   
            .ThenInclude(u=>u.JoiningUser)
            .OrderBy(t => t.Date)
            .ToList();
            ViewBag.getsameactivity = getsameactivity;
            
           
            ViewBag.AllUsers = dbContext.Users.ToList();
            var participants = dbContext.Users
            .Include(w => w.UserParticipant)
            .ThenInclude(z => z.JoiningActivity)
            .ToList();
            ViewBag.Participants = participants;

            return View (LoggedIn);
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost("New")]
        public IActionResult New(Activity0 newActivity)
        {
            if (ModelState.IsValid)
            {
                if(newActivity.Date < DateTime.Today)
                {
                    ModelState.AddModelError("Date", "Plan must be a future date");
                    return View("Create");
                }
                int? IntVariable = HttpContext.Session.GetInt32("UserID");
                newActivity.CoordinatorId = (int)IntVariable;
                dbContext.Add(newActivity);
                dbContext.SaveChanges();
                return RedirectToAction("home");
            }
            return View("Create");
        }

        [HttpGet("activity/{ActivityId}")]
        public IActionResult OneActivity(int ActivityId)
        {
            int? IntVariable = HttpContext.Session.GetInt32("UserID");
            Activity0 r = dbContext.Activities.FirstOrDefault(c => c.ActivityId == ActivityId);
            ViewBag.thisActivity = r;
            ViewBag.AllUsers = dbContext.Users.ToList();
            Activity0 One = dbContext.Activities.Include(a => a.ActivityParticipant).SingleOrDefault(a => a.ActivityId == ActivityId);
            ViewBag.Participants = One;
            var getsameactivity1 = dbContext.Activities
            .Include(w => w.ActivityParticipant )   
            .ThenInclude(u=>u.JoiningUser)
            .ToList();
            ViewBag.getsameactivity1 = getsameactivity1;
             User LoggedIn = dbContext.Users
            .SingleOrDefault(d => d.UserId == IntVariable);
            // ViewBag.LoggedUser = LoggedIn.Name;
            ViewBag.LoggedInUser = dbContext.Users
            .SingleOrDefault(d => d.UserId == IntVariable);

            var getsameactivity = dbContext.Activities
            .Include(w => w.ActivityParticipant)   
            .ThenInclude(u=>u.JoiningUser)
            .FirstOrDefault(c => c.ActivityId == ActivityId);
            
            // List<Activity0> getsameactivity = dbContext.Activities.Where(p=>p.ActivityId==ActivityId)
            // .Include(w => w.ActivityParticipant )   
            // .ThenInclude(u=>u.JoiningUser)
            // .ToList();
            
            ViewBag.getsameactivity = getsameactivity;
            
            return View(One);
        }
        [HttpGet("Delete/{ActivityId}")]
        public IActionResult Delete(int ActivityId)
        {
            Activity0 activityToDelete = dbContext.Activities.SingleOrDefault(X => X.ActivityId ==ActivityId);
            dbContext.Activities.Remove(activityToDelete);
            dbContext.SaveChanges();
            return RedirectToAction("home");
        }
        [HttpGet("join/{Id}")]
        public IActionResult Join(int Id, Participant newParticipant)
        {
            newParticipant.ActivityId = Id;
            newParticipant.UserId = (int)HttpContext.Session.GetInt32("UserID");

            dbContext.Add(newParticipant);
            dbContext.SaveChanges();
            return RedirectToAction("Home");
        }
        [HttpGet("unjoin/{ActivityId}")]
        public IActionResult UnJoin (int ActivityId)
        {
            int? IntVariable = HttpContext.Session.GetInt32("UserID");
            Participant participantToDelete = dbContext.Participants
            .Where(x => x.ActivityId == ActivityId)
            .FirstOrDefault(b => b.UserId == IntVariable);
            
            
            dbContext.Remove(participantToDelete);
            dbContext.SaveChanges();
            return RedirectToAction("Home");
        }
    }
}
