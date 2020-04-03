using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        // GET: Movies
        public ActionResult Random()
        {
            var movie = new Movie()
            {
                Name = "Shrek!"
            };

            // ViewData["RandomMovie"] = movie;
            // ViewBag.RandomMovie = movie;
            // var viewResult = new ViewResult();
            // viewResult.ViewData.Model = movie;

            return View(movie);
        }

        [Route("movies/release/{year}/{month:regex(\\d{4}):range(1,12)}")]
        public ActionResult ByReleasedDate(int year, int month)
        {
            return Content(year + "/" + month);
        }
    }
}