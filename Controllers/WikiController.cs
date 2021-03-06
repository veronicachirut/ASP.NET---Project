using Proj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proj.Controllers
{
    [Authorize(Roles = "Admin")]
    public class WikiController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Wiki
        public ActionResult Index()
        {
            List<Wiki> wikis = db.Wikis.ToList();
            ViewBag.Wikis = wikis;
            return View();
        }

        [HttpGet]
        public ActionResult New()
        {
            Wiki wiki = new Wiki();
            return View(wiki);
        }

        [HttpPost]
        public ActionResult New(Wiki wikiRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Wikis.Add(wikiRequest);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Artist");
                }
                return View(wikiRequest);
            }
            catch
            {
                return View(wikiRequest);
            }
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Wiki wiki = db.Wikis.Find(id);

                if (wiki == null)
                {
                    return HttpNotFound("Coludn't find the wiki with id " + id.ToString());
                }
                return View(wiki);
            }
            return HttpNotFound("Missing wiki id parameter!");
        }

        [HttpPut]
        public ActionResult Edit(int id, Wiki wikiRequest)
        {
            Wiki wiki = db.Wikis.Find(id);

            try
            {
                if (ModelState.IsValid)
                {
                    if (TryUpdateModel(wiki))
                    {
                        wiki.Description = wikiRequest.Description;
                        wiki.BirthYear = wikiRequest.BirthYear;
                        wiki.BirthMonth = wikiRequest.BirthMonth;
                        wiki.BirthDay = wikiRequest.BirthDay;
                        wiki.DebutYear = wikiRequest.DebutYear;

                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(wikiRequest);
            }
            catch (Exception)
            {
                return View(wikiRequest);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Wiki wiki = db.Wikis.Find(id);
            Artist artist = db.Artists.SingleOrDefault(b => b.Wiki.WikiId.Equals(wiki.WikiId));
            if (artist != null)
            {
                if (wiki != null)
                {
                    db.Wikis.Remove(wiki);
                    db.Artists.Remove(artist);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return HttpNotFound("Couldn't find the wiki with id " + id.ToString());
        }
    }
}