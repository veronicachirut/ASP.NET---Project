using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Proj.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proj.Controllers
{
    public class AlbumController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Album
        [HttpGet]
        public ActionResult Index()
        {
            List<Album> albums = db.Albums.ToList();
            ViewBag.Albums = albums;
            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Album album = db.Albums.Find(id);
                if (album != null)
                {
                    return View(album);
                }
                return HttpNotFound("Couldn't find the album with id " + id.ToString() + "!");
            }
            return HttpNotFound("Missing book id parameter!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult New()
        {
            Album album = new Album();
            album.TracksList = GetAllTracks();
            album.Tracks = new List<Track>();
            return View(album);
        }

        [HttpPost]
        public ActionResult New (Album albumRequest)
        { 
            var selectedTracks = albumRequest.TracksList.Where(b => b.Checked).ToList();

            try
            {
                if (ModelState.IsValid)
                {
                    albumRequest.Tracks = new List<Track>();
                    for (int i = 0; i < selectedTracks.Count(); i++)
                    {
                        Track track = db.Tracks.Find(selectedTracks[i].Id);
                        albumRequest.Tracks.Add(track);
                    }
                    db.Albums.Add(albumRequest);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(albumRequest);
            }
            catch
            {
                return View(albumRequest);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Album album = db.Albums.Find(id);
                album.TracksList = GetAllTracks();

                foreach (Track checkedTrack in album.Tracks)
                {
                    album.TracksList.FirstOrDefault(g => g.Id == checkedTrack.TrackId).Checked = true;
                }

                if (album == null)
                {
                    return HttpNotFound("Coludn't find the album with id " + id.ToString());
                }
                return View(album);
            }
            return HttpNotFound("Missing album id parameter!");
        }

        [HttpPut]
        public ActionResult Edit(int id, Album albumRequest)
        {
            Album album = db.Albums.Find(id);

            var selectedTracks = albumRequest.TracksList.Where(b => b.Checked).ToList();
            try
            {
                if (ModelState.IsValid)
                {
                    if (TryUpdateModel(album))
                    {
                        album.Title = albumRequest.Title;
                        album.Year = albumRequest.Year;

                        album.Tracks.Clear();
                        album.Tracks = new List<Track>();

                        for (int i = 0; i < selectedTracks.Count(); i++)
                        {
                            Track track = db.Tracks.Find(selectedTracks[i].Id);
                            album.Tracks.Add(track);
                        }
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(albumRequest);
            }
            catch (Exception)
            {
                return View(albumRequest);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Album album = db.Albums.Find(id);
            if (album != null)
            {
                db.Albums.Remove(album);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Couldn't find the ALBUM with id " + id.ToString());
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public ActionResult Order(int? id)
        {
            if (id.HasValue)
            {
                Album album = db.Albums.Find(id);

                if (album == null)
                {
                    return HttpNotFound("Coludn't find the album with id " + id.ToString());
                }
                return View(album);
            }
            return HttpNotFound("Missing album id parameter!");
        }

        [HttpPut]
        public ActionResult Order(int id)
        {
            string userId = User.Identity.GetUserId();
            Album album = db.Albums.Find(id);

            if (TryUpdateModel(album))
            {
                album.Users.Add(db.Users.Find(userId));
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [NonAction]
        public List<CheckBoxViewModel> GetAllTracks()
        {
            var checkboxList = new List<CheckBoxViewModel>();
            foreach (var track in db.Tracks.ToList())
            {
                checkboxList.Add(new CheckBoxViewModel
                {
                    Id = track.TrackId,
                    Title = track.Title,
                    ArtistName = track.Artist.Name,
                    Checked = false
                });
            }
            return checkboxList;
        }
    }
}