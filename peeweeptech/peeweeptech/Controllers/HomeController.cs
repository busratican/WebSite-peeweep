using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using peeweeptech.Models;
using PagedList;
using PagedList.Mvc;

namespace peeweeptech.Controllers
{
    public class HomeController : Controller
    {

        cbgcode db = new cbgcode();
        //
        // GET: /Home/
        public ActionResult Index(int page=1)
        {
            var kod = db.Kod.OrderByDescending(m => m.kod_id).ToPagedList(page,5);
            return View(kod);
        }

        public ActionResult SearchCode(string search = null)
        {
            var searching = db.Kod.Where(m => m.baslik.Contains(search)).ToList();
            return View(searching.OrderByDescending(m=>m.tarih));
        }

     public ActionResult KategoriKod(int id)
        {
            var kod = db.Kod.Where(m => m.Kategori.kategori_id==id).ToList();
            return View(kod);
        }

        public ActionResult CodeDetails(int id)
        {
            var kod = db.Kod.Where(m => m.kod_id==id).SingleOrDefault();

            if(kod == null)
            {
                return HttpNotFound();
            }

            return View(kod);
        }

        public ActionResult About()
        {
            return View();
        }


        public ActionResult Codes()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult KategoriPartial()
        {
            return View(db.Kategori.ToList());
        }


        public ActionResult IncreaseRead(int id)
        {
            var kod = db.Kod.Where(m => m.kod_id == id).SingleOrDefault();
          //  kod.okunma += 1;
            db.SaveChanges();
            return View();
        }
	}
}