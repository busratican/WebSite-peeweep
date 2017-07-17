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
    public class AdminKodController : Controller
    {

        cbgcode db = new cbgcode();
        //
        // GET: /AdminKod/
        public ActionResult Index()
        {

            var codes = db.Kod.ToList();
            return View(codes);
        }

        //
        // GET: /AdminKod/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /AdminKod/Create
        public ActionResult Create()
        {

            ViewBag.kategori_id = new SelectList(db.Kategori, "kategori_id", "kategori_adi");
            return View();
        }

        //
        // POST: /AdminKod/Create
        [HttpPost]
        public ActionResult Create(Kod kod,string etiketler, HttpPostedFileBase foto)
        {

            if (ModelState.IsValid)
            {
                if (foto != null)
                {
                    WebImage img = new WebImage(foto.InputStream);
                    FileInfo fotoInfo = new FileInfo(foto.FileName);

                    string newFoto = Guid.NewGuid().ToString() + fotoInfo.Extension;
                    img.Resize(800, 350);
                    img.Save("~/Uploads/KodFoto/" + newFoto);
                    kod.foto = "/Uploads/KodFoto/" + newFoto;
                  
                }

                if (etiketler != null)
                {
                    string[] etiketdizi = etiketler.Split(',');
                    foreach (var i in etiketdizi)
                    {
                        var yeniEtiket = new Etiket { etiket_adi = i };
                        db.Etiket.Add(yeniEtiket);
                        kod.Etiket.Add(yeniEtiket);
                    }

                }

                    kod.okunma = 1;
                    db.Kod.Add(kod);
                    db.SaveChanges();
                    return RedirectToAction("Index");

            }
            
                return View(kod);
            
        }

        //
        // GET: /AdminKod/Edit/5
        public ActionResult Edit(int id)
        {
            var kod = db.Kod.Where(m => m.kod_id == id).SingleOrDefault();

            if(kod == null)
            {
                return HttpNotFound();
            }

            ViewBag.kategori_id = new SelectList(db.Kategori, "kategori_id", "kategori_adi",kod.kategori_id);
            return View(kod);
        }

        //
        // POST: /AdminKod/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, HttpPostedFileBase foto,Kod kod)
        {
            if(ModelState.IsValid){

                var kods = db.Kod.Where(m => m.kod_id == id).SingleOrDefault();

                if(foto != null)
                {
                    if(System.IO.File.Exists(kods.foto))
                    {
                        System.IO.File.Delete(kods.foto);
                    }

                    WebImage img = new WebImage(foto.InputStream);
                    FileInfo fotoInfo = new FileInfo(foto.FileName);

                    string newFoto = Guid.NewGuid().ToString() + fotoInfo.Extension;
                    img.Resize(800, 350);
                    img.Save("~/Uploads/KodFoto/" + newFoto);
                    kods.foto = "/Uploads/KodFoto/" + newFoto;
                    kods.baslik = kod.baslik;
                    kods.icerik = kod.icerik;
                    kods.kategori_id = kod.kategori_id;
                    db.SaveChanges();
             
                }
                return RedirectToAction("Index");
            }
        
                return View();
            
        }

        //
        // GET: /AdminKod/Delete/5
        public ActionResult Delete(int id)
        {
            var kod = db.Kod.Where(m => m.kod_id == id).SingleOrDefault();

            if (kod == null)
            {
                return HttpNotFound();
            }

            
            return View(kod);
        }

        //
        // POST: /AdminKod/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            if(ModelState.IsValid){
                var kods = db.Kod.Where(m => m.kod_id == id).SingleOrDefault();
                if (kods == null)
                {
                    return HttpNotFound();
                }

                if (System.IO.File.Exists(kods.foto))
                {
                    System.IO.File.Delete(kods.foto);
                }

                foreach (var i in kods.Yorum.ToList())
                {
                    db.Yorum.Remove(i);
                }

                foreach (var i in kods.Etiket.ToList())
                {
                    db.Etiket.Remove(i);
                }

                db.Kod.Remove(kods);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            
                return View();
            
        }
    }
}
