using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsApp.Models;
using PagedList;
using System.Data.Objects;

namespace NewsApp.Controllers
{ 
    public class NewsController : Controller
    {
        private HilurEntities db = new HilurEntities();

        //
        // GET: /NewNews/

        public ViewResult Index(int? page)
        {
            int pageSize = 30;
            int pageIndex = (page ?? 1);

            ViewBag.Source = "Index";

            return View(db.Entries.OrderByDescending(x => x.PubDate).ToPagedList(pageIndex,pageSize));
        }

        public ActionResult Popular(int? page)
        {
            int pageSize = 30;
            int pageIndex = (page ?? 1);

            var scoredResult = db.Entries.Select(o => new { calcScore = (o.Score * 2) - (EntityFunctions.DiffDays(o.PubDate, DateTime.Now) * 2), o });
            var query = scoredResult.OrderByDescending(x => x.calcScore).Select(a => a.o).ToPagedList(pageIndex, pageSize);

            ViewBag.Source = "Popular";

            return View("Index",query);
        }

        //
        // GET: /NewNews/Details/5

        public ViewResult Details(int id)
        {
            Entry entry = db.Entries.Include("Comments").Single(a => a.Id == id);
            return View(entry);
        }

        [HttpPost]
        public ActionResult Details(int id, string comment)
        {
            Entry article = db.Entries.Include("Comments").Single(a => a.Id == id);

            if (Request.IsAuthenticated)
            {
                SiteUser user = db.SiteUsers.Single(n => n.Username == User.Identity.Name);

                Comment commentEntity = new Comment();
                commentEntity.ArticleId = article.Id;
                commentEntity.Description = comment;
                commentEntity.Title = "TBD";
                commentEntity.Date = DateTime.Now;
                commentEntity.UserId = user.Id;

                user.PostCount++;
                article.Comments.Add(commentEntity);
                db.SaveChanges();
            }

            return View(article);
        }

        [HttpPost]
        public bool VoteUp(int id)
        {
            bool result = false;
            if (Request.IsAuthenticated)
            {
                SiteUser user = db.SiteUsers.Single(n => n.Username == User.Identity.Name);

                var votequery = db.Votes.Where(x => x.ArticleId == id && x.UserId == user.Id);

                if (votequery.Count() == 0)
                {
                    Vote newScore = new Vote();
                    newScore.UserId = user.Id;
                    newScore.ArticleId = id;
                    newScore.Date = DateTime.Now;
                    newScore.Score = true;
                    db.Votes.Add(newScore);

                    var article = db.Entries.Single(a => a.Id == id);
                    article.Score++;

                    db.SaveChanges();
                    result = true;
                }
            }
            return result;
        }

        [HttpPost]
        public bool VoteDown(int id)
        {

            bool result = false;
            if (Request.IsAuthenticated)
            {
                SiteUser user = db.SiteUsers.Single(n => n.Username == User.Identity.Name);

                var votequery = db.Votes.Where(x => x.ArticleId == id && x.UserId == user.Id);

                if (votequery.Count() == 0)
                {
                    Vote newScore = new Vote();
                    newScore.UserId = user.Id;
                    newScore.ArticleId = id;
                    newScore.Date = DateTime.Now;
                    newScore.Score = false;
                    db.Votes.Add(newScore);

                    var article = db.Entries.Single(a => a.Id == id);
                    article.Score--;

                    db.SaveChanges();
                    result = true;
                }
            }
            return result;
        }
                     

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}