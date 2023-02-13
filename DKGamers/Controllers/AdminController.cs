﻿using DKGamers.Data;
using DKGamers.Identity;
using DKGamers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DKGamers.Controllers
{
    public class AdminController : Controller
    {
        private Context context;

        private UserManager<Kullanici> kullaniciYoneticisi;
        public AdminController(UserManager<Kullanici> _kullaniciYoneticisi, Context context)
        {
            kullaniciYoneticisi = _kullaniciYoneticisi;
            this.context = context;
        }

        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult Oyun()
        {
            var oyunlar = context.Oyun.OrderBy(o => o.OyunID).ToList();
            return View(new OyunListViewModel()
            {
                Oyunlar = oyunlar
            });
        }

        [Authorize(Roles = "admin")]
        public IActionResult OyunuGizle(int id)
        {
            Oyun oyun = context.Oyun.FirstOrDefault(i => i.OyunID == id);
            oyun.oyunGosterilsinmi = true;
            context.Update(oyun);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public IActionResult OyunuGoster(int id)
        {
            Oyun oyun = context.Oyun.FirstOrDefault(i => i.OyunID == id);
            oyun.oyunGosterilsinmi = false;
            context.Update(oyun);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin")]
        public IActionResult OyunuGuncelle(Oyun oyun)
        {

            context.Update(oyun);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult OyunEkle()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult OyunEkle(Oyun oyun)
        {
            if (ModelState.IsValid)
            {
                context.Add(oyun);
                context.SaveChanges();
            }
            return RedirectToAction("Oyun", "Admin");
        }

        [Authorize(Roles = "admin")]
        public IActionResult Haber()
        {
            var haberler = context.Haber.OrderBy(o => o.HaberID).ToList();
            return View(new HaberListViewModel()
            {
                Haberler = haberler
            });
        }

        [Authorize(Roles = "admin")]
        public IActionResult HaberiGizle(int id)
        {
            Haber haber = context.Haber.FirstOrDefault(i => i.HaberID == id);
            haber.haberGosterilsinmi = true;
            context.Update(haber);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public IActionResult HaberiGoster(int id)
        {
            Haber haber = context.Haber.FirstOrDefault(i => i.HaberID == id);
            haber.haberGosterilsinmi = false;
            context.Update(haber);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public IActionResult HaberGuncelle(Haber haber)
        {

            context.Update(haber);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult HaberEkle()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult HaberEkle(Haber haber)
        {
            if (ModelState.IsValid)
            {
                context.Add(haber);
                context.SaveChanges();
            }
            return RedirectToAction("Haber", "Admin");
        }

        [Authorize(Roles = "admin")]
        public IActionResult Kullanici()
        {
            return View(kullaniciYoneticisi.Users);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> KullaniciSil(string userId)
        {
            var user = await kullaniciYoneticisi.FindByIdAsync(userId);
            await kullaniciYoneticisi.DeleteAsync(user);
            var cmd = "delete from Favori where KullaniciAdi=@p0";
            context.Database.ExecuteSqlRaw(cmd, user.UserName);
            var cmd1 = "delete from Yorum where KullaniciAdi=@p0";
            context.Database.ExecuteSqlRaw(cmd1, user.UserName);
            return Redirect("/admin/kullanici");
        }

    }
}