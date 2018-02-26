using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DotaAPI.Controllers
{
    public class AdminController : Controller
    {
        private DotaContext _context;
        public AdminController(DotaContext context)
        {
            _context = context;
        }

        [Route("admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("admin/login")]
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            Admin thisAdmin = _context.Admins.SingleOrDefault(a => a.username == username);
            if(thisAdmin == null) return RedirectToAction("Index");
            PasswordHasher<Admin> hasher = new PasswordHasher<Admin>();
            if(hasher.VerifyHashedPassword(thisAdmin, thisAdmin.password, password) != 0)
            {
                HttpContext.Session.SetString("admin", "true");
                return RedirectToAction("Main");
            }
            return RedirectToAction("Index");
        }

        [Route("admin/main")]
        public IActionResult Main()
        {
            if(HttpContext.Session.GetString("admin") != "true") return RedirectToAction("Index");
            return View();
        }

        [Route("admin/addhero")]
        public IActionResult AddHero()
        {
            if(HttpContext.Session.GetString("admin") != "true") return RedirectToAction("Index");
            return View();
        }

        [Route("admin/addhero/create")]
        [HttpPost]
        public IActionResult CreateHero(Hero newHero)
        {
            Hero heroToAdd = new Hero()
            {
                name = newHero.name,
                attribute = newHero.attribute,
                intelligence = newHero.intelligence,
                agility = newHero.agility,
                strength = newHero.strength,
                attack = newHero.attack,
                speed = newHero.speed,
                armor = newHero.armor,
                bio = newHero.bio,
                attack_range = newHero.attack_range,
                sight_range = newHero.sight_range,
                attack_type = newHero.attack_type,
                missile_speed = newHero.missile_speed,
                version = (decimal)7.09
            };
            _context.Add(heroToAdd);
            _context.SaveChanges();
            return RedirectToAction("AddHero");
        }
    }
}