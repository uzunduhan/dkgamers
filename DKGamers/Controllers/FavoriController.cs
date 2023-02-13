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
    public class FavoriController : Controller
    {
        private UserManager<Kullanici> kullaniciYoneticisi;
        private Favori favori = new Favori();
        private Context context;
        public FavoriController(UserManager<Kullanici> _kullaniciYoneticisi, Context context)
        {
            kullaniciYoneticisi = _kullaniciYoneticisi;
            this.context = context;
        }
        public IActionResult Index()
        {
            var user = kullaniciYoneticisi.FindByNameAsync(User.Identity.Name).Result;
            var favoriler = context.Favori.Include(i => i.Oyun).Where(i => i.KullaniciAdi == user.UserName).ToList();
            return View(new FavoriListViewModel()
            {
                Favoriler = favoriler
            });
        }

        public IActionResult FavoriyeEkle(int id)
        {
            var user = kullaniciYoneticisi.FindByNameAsync(User.Identity.Name).Result;
            if (!context.Favori.Any(i => i.OyunID == id && i.KullaniciAdi == user.UserName))
            {

                var Favori = new Favori()
                {
                    KullaniciAdi = user.UserName,
                    OyunID = id,
                };
                context.Add(Favori);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult FavoridenCikar(int id)
        {
            var user = kullaniciYoneticisi.FindByNameAsync(User.Identity.Name).Result;
            var cmd = "delete from Favori where KullaniciAdi=@p0 and OyunID=@p1";
            context.Database.ExecuteSqlRaw(cmd, user.UserName, id);

            return RedirectToAction("Index");
        }
    }
}