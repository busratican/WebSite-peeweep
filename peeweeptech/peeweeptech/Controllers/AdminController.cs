using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using peeweeptech.Models;

namespace peeweeptech.Controllers
{
    public class AdminController : Controller
    {

        cbgcode db = new cbgcode();

        //
        // GET: /Admin/
        public ActionResult Index()
        {

            ViewBag.CodeNumber = db.Kod.Count();
            ViewBag.Kategori = db.Kategori.Count();
            return View();
        }
	}
}