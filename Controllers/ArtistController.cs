using Proj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proj.Controllers
{
    public class ArtistController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Artist
        [HttpGet]
        public ActionResult Index()
        {
            List<Artist> artists = db.Artists.ToList();
            ViewBag.Artists = artists;
            return View();
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Artist artist = db.Artists.Find(id);
                if (artist != null)
                {
                    return View(artist);
                }
                return HttpNotFound("Couldn't find the artist with id " + id.ToString());
            }
            return HttpNotFound("Missing artist id parameter");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult New()
        {
            ArtistWikiViewModel artist = new ArtistWikiViewModel();
            return View(artist);
        }

        [HttpPost]
        public ActionResult New(ArtistWikiViewModel artistWikiRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Wiki artistWiki = new Wiki
                    {
                        Description = artistWikiRequest.Description,
                        BirthYear = artistWikiRequest.BirthYear,
                        BirthMonth = artistWikiRequest.BirthMonth,
                        BirthDay = artistWikiRequest.BirthDay,
                        DebutYear = artistWikiRequest.DebutYear
                    };

                    db.Wikis.Add(artistWiki);
                    Artist artist = new Artist
                    {
                        Name = artistWikiRequest.Name,
                        Country = artistWikiRequest.Country,
                        Wiki = artistWiki
                    };
                    db.Artists.Add(artist);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(artistWikiRequest);
            }
            catch
            {
                return View(artistWikiRequest);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult NewTrack(int? id)
        {
            if (id.HasValue)
            {
                Track track = new Track();
                Artist artist = db.Artists.Find(id);
                ViewBag.id = id;
                
                if (artist != null)
                {
                    track.Artist = db.Artists.Find(id);
                    return View(track);
                }
                return HttpNotFound("Couldn't find the artist with id " + id.ToString());
            }
            return HttpNotFound("Missing id parameter");
        }

        [HttpPost]
        public ActionResult NewTrack(int id, Track trackRequest)
        {
            trackRequest.Artist = db.Artists.Find(id);
            try
            {
                if (ModelState.IsValid)
                {
                    db.Tracks.Add(trackRequest);
                    db.SaveChanges();
                    var path = "Details/" + id + "";
                    return RedirectToAction(path);
                }
                return View(trackRequest);
            }
            catch
            {
                return View(trackRequest);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Artist artist = db.Artists.Find(id);

                if (artist == null)
                {
                    return HttpNotFound("Couldn't find the artist with id " + id);
                }
                Wiki wiki = db.Wikis.Find(artist.Wiki.WikiId);

                ArtistWikiViewModel artistWiki = new ArtistWikiViewModel
                {
                    Name = artist.Name,
                    Country = artist.Country,
                    Description = wiki.Description,
                    BirthYear = wiki.BirthYear,
                    BirthMonth = wiki.BirthMonth,
                    BirthDay = wiki.BirthDay,
                    DebutYear = wiki.DebutYear
                };
                return View(artistWiki);
            }
            return HttpNotFound("Missing artist id parameter!");
        }

        [HttpPut]
        public ActionResult Edit(int id, ArtistWikiViewModel artistViewRequest)
        {
            Artist artist = db.Artists.Find(id);
            Wiki wiki = db.Wikis.Find(artist.Wiki.WikiId);

            try
            {
                if (ModelState.IsValid)
                {
                    if (TryUpdateModel(artist) && TryUpdateModel(wiki))
                    {
                        artist.Name = artistViewRequest.Name;
                        artist.Country = artistViewRequest.Country;

                        wiki.Description = artistViewRequest.Description;
                        wiki.BirthYear = artistViewRequest.BirthYear;
                        wiki.BirthMonth = artistViewRequest.BirthMonth;
                        wiki.BirthDay = artistViewRequest.BirthDay;
                        wiki.DebutYear = artistViewRequest.DebutYear;

                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(artistViewRequest);
            }
            catch (Exception)
            {
                return View(artistViewRequest);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Artist artist = db.Artists.Find(id);
            Wiki artistWiki = db.Wikis.Find(artist.Wiki.WikiId);
            if (artist != null)
            {
                db.Artists.Remove(artist);
                db.Wikis.Remove(artistWiki);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Couldn't find the artist with id " + id.ToString());
        }
    }
}