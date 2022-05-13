using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using instagram.Models;

namespace instagram.Controllers
{
    public class ProfileController : Controller
    {
        private readonly instagramEntities1 db = new instagramEntities1();

        public ActionResult Index(int id)
        {
            var userModel = db.users.Single(x => x.id == id);
            return View(userModel);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var user = db.users.Single(u => u.id == id);

            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(user userModel)
        {
            if (!ModelState.IsValid) return View("Edit", "Index", userModel);
            var userDb = db.users.Single(u => u.id == userModel.id);

            userDb.name = userModel.name;
            userDb.email = userModel.email;
            userDb.password = userModel.password;

            var fileName = Path.GetFileName(userModel.ImageFile.FileName);
            userModel.image = "~/Image/Profiles/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/Profiles/"), fileName);
            userModel.ImageFile.SaveAs(fileName);


            db.SaveChanges();
            return RedirectToAction("Index", "Profile", new { userModel.id });
        }

        [HttpGet]
        public ActionResult CreatePost(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatePost(post postModel, int id)
        {
            var fileName = Path.GetFileName(postModel.ImageFile.FileName);
            postModel.image = "~/Image/Posts/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/Posts/"), fileName);
            postModel.ImageFile.SaveAs(fileName);
            postModel.userId = id;

            using (var instagramEntities = new instagramEntities1())
            {
                instagramEntities.posts.Add(postModel);
                instagramEntities.SaveChanges();
            }

            ModelState.Clear();
            return View("CreatePost", new post());
        }


        public ActionResult ShowUserPosts(int id)
        {
            var posts = db.posts.Where(p => p.userId == id);

            return View(posts);
        }

        public ActionResult ShowUserFollowers(int id)
        {
            var followes = db.followers.Where(p => p.userId == id);

            return View(followes);
        }

        /*
        [HttpGet]
        */
        public ActionResult Search(string userName)
        {
            var users = db.users.Where(u => u.name == userName).ToList();
            return View(users);
        }

      

        /*[HttpPost]
        public ActionResult Search(string userName,int id)
        {
            var users = db.users.SqlQuery($"select * from users where users.name like '%'+{userName}+'%'");
            return View("Search",users);
        }*/



        public ActionResult ViewProfile(int id)
        {
            var user = db.users.FirstOrDefault(u => u.id == id);
            return View(user);

        }
        
    }
}