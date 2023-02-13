﻿using DKGamers.Data;
using DKGamers.Identity;
using DKGamers.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DKGamers.Controllers
{
    public class YorumController : Controller
    {
        private Context context;
        private UserManager<Kullanici> kullaniciYoneticisi;
        public YorumController(UserManager<Kullanici> _kullaniciYoneticisi, Context context)
        {
            kullaniciYoneticisi = _kullaniciYoneticisi;
            this.context = context;
        }
        public IActionResult Index()
        {
            var oyunlar = context.Oyun.ToList();
            var user = kullaniciYoneticisi.FindByNameAsync(User.Identity.Name).Result;
            var yorumlar = context.Yorum.Where(i => i.KullaniciAdi == user.UserName).ToList();
            return View(yorumlar);
        }

        public IActionResult YorumuSil(int id)
        {
            Yorum yorum = context.Yorum.FirstOrDefault(i => i.YorumID == id);
            context.Remove(yorum);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
