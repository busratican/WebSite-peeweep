using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using peeweeptech.Models;
using System.Web.Helpers;
using System.IO;

namespace peeweeptech.Controllers
{
    public class UyeController : Controller
    {

        cbgcode db = new cbgcode();
        //
        // GET: /Uye/
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Uye uye)
        {
            var login = db.Uye.Where(u =>u.email == uye.email).SingleOrDefault();

            if(login.email == uye.email && login.şifre == uye.şifre)
            {
                Session["uye_id"] = uye.üye_id;
                Session["uye_mail"] = uye.email;
                Session["yetki_id"] = login.yetki_id;
                

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Uyari = "Mail or Password Wrong! Please try again..";
                return View();
            }
           
        }

        public ActionResult Logout()
        {
            Session["uye_id"] = null;
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Uye uye, HttpPostedFileBase foto)
        {

            if(ModelState.IsValid)
            {
                if(foto != null)
                {
                    WebImage img = new WebImage(foto.InputStream);
                    FileInfo fotoInfo = new FileInfo(foto.FileName);

                    string newFoto = Guid.NewGuid().ToString() + fotoInfo.Extension;
                    img.Resize(150, 150);
                    img.Save("~/Uploads/UyeFoto/" + newFoto);
                    uye.foto = "/Uploads/UyeFoto/" + newFoto;
                    uye.yetki_id = 2;
                    db.Uye.Add(uye);
                    db.SaveChanges();
                    Session["uye_id"] = uye.üye_id;
                    Session["kullanici_adi"] = uye.kullanici_adi;
                    return RedirectToAction("Index", "Home");
                  
                }
                else
                {
                    ModelState.AddModelError("Fotoğraf", "Fotoğraf Seçiniz..");
                }
            }
            return View(uye);
        }
	}
}