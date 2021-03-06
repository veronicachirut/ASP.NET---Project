using Proj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proj.Controllers
{
    public class TrackController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Track
        public ActionResult Index()
        {
            List<Track> tracks = db.Tracks.ToList();
            ViewBag.Tracks = tracks;
            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Track track = db.Tracks.Find(id);
                if (track != null)
                {
                    return View(track);
                }
                return HttpNotFound("Couldn't find the track with id " + id.ToString());
            }
            return HttpNotFound("Missing track id parameter!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult New()
        {
            Track track = new Track();
            track.ArtistList = GetAllArtists();
            return View(track);
        }

        [HttpPost]
        public ActionResult New(Track trackRequest)
        {
            trackRequest.ArtistList = GetAllArtists();
            try
            {
                if (ModelState.IsValid) {
                    db.Tracks.Add(trackRequest);
                    db.SaveChanges();
                    return RedirectToAction("Index");
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
                Track track = db.Tracks.Find(id);
                track.ArtistList = GetAllArtists();
                track.AlbumsList = GetAllAlbums();

                foreach(Album checkedAlbum in track.Albums)
                {
                    track.AlbumsList.FirstOrDefault(g => g.Id == checkedAlbum.AlbumId).Checked = true;
                }

                if (track == null)
                {
                    return HttpNotFound("Coludn't find the track with id " + id.ToString());
                }
                return View(track);
            }
            return HttpNotFound("Missing track id parameter!");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult Edit(int id, Track trackRequest)
        {
            trackRequest.ArtistList = GetAllArtists();

            Track track = db.Tracks.Include("Artist").SingleOrDefault(b => b.TrackId.Equals(id));

            var selectedAlbums = trackRequest.AlbumsList.Where(b => b.Checked).ToList();
            try
            {
                if (ModelState.IsValid)
                {
                    if (TryUpdateModel(track))
                    {
                        track.Title = trackRequest.Title;
                        track.Time = trackRequest.Time;
                        track.ArtistId = trackRequest.ArtistId;

                            track.Albums.Clear();
                            track.Albums = new List<Album>();

                            for (int i = 0; i < selectedAlbums.Count(); i++)
                            {
                                Album album = db.Albums.Find(selectedAlbums[i].Id);
                                track.Albums.Add(album);
                            }

                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(trackRequest);
            }
            catch (Exception)
            {
                return View(trackRequest);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Track track = db.Tracks.Find(id);
            if (track != null)
            {
                db.Tracks.Remove(track);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Couldn't find the book with id " + id.ToString() + "!");
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllArtists()
        {
            var selectList = new List<SelectListItem>();
            foreach (var artist in db.Artists.ToList())
            {
                selectList.Add(new SelectListItem
                {
                    Value = artist.ArtistId.ToString(),
                    Text = artist.Name
                });
            }
            return selectList;
        }

        [NonAction]
        public List<CheckBoxViewModel> GetAllAlbums()
        {
            var checkboxList = new List<CheckBoxViewModel>();
            foreach (var album in db.Albums.ToList())
            {
                checkboxList.Add(new CheckBoxViewModel
                {
                    Id = album.AlbumId,
                    Title = album.Title,
                    Checked = false
                });
            }
            return checkboxList;
        }
    }
}