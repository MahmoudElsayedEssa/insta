using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using instagram.Models;
using instagram.ViewModels;

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


        public ActionResult ShowUserPosts(int id, int viewerId)
        {
            var posts = db.posts.Where(p => p.userId == id);

            var listOfPostReacts = new List<PostReacts>();
            foreach (var post in posts)
            {
                //comment
                var comments = db.comments.Where(p => p.postId == post.id);
                var userscommment = new List<UserComment>();

                foreach (var comment in comments)
                {
                    var user = db.users.Single(u => u.id == comment.userId);
                    var userComment = new UserComment
                    {
                        Comment = comment,
                        User = user
                    };
                    userscommment.Add(userComment);
                }

                var postComments = new PostComments
                {
                    Post = post,
                    UsersComments = userscommment
                };

                //react
                var reacts = db.postReacts.Where(r => r.postId == post.id);
                var usersReacts = new List<UserReact>();

                foreach (var React in reacts)
                {
                    var user = db.users.Single(u => u.id == React.userId);
                    var userReact = new UserReact
                    {
                        PostReact = React,
                        User = user
                    };
                    usersReacts.Add(userReact);
                }

                var postReact = new PostReacts
                {
                    Post = post,
                    UserReacts = usersReacts,
                    UsersComments = userscommment
                };

                listOfPostReacts.Add(postReact);
            }

            var model = new PostViewModel
            {
                PostReactsList = listOfPostReacts
            };
            Session["id"] = id;
            Session["viewerId"] = viewerId;


            return View(model);
        }

        public ActionResult ShowUserFollowers(int id)
        {
            var followes = db.followers.Where(p => p.userId == id && p.state == "following");
            /*var user = db.users.SingleOrDefault(p => p.id == id);
            var userFollow = new UserFollow
            {
                Followers = followes,
                User = user
            };*/
            return View(followes);
        }

        public ActionResult ShowUserRequest(int id)
        {
            var followes = db.followers.Where(p => p.userId == id && p.state == "requested");

            return View(followes);
        }


        public ActionResult Search(string userName, int id)
        {
            var users = db.users.Where(u => u.name == userName).ToList();
            ViewData["id"] = id;
            return View(users);
        }


        public ActionResult ViewProfile(int id, int viewerId)
        {
            var user = db.users.FirstOrDefault(u => u.id == id);
            var follower = db.followers.SingleOrDefault(f => f.userId == id && f.followerId == viewerId);


            ViewData["id"] = viewerId;
            ViewData["follower"] = follower;

            Session["id"] = id;
            Session["viewerId"] = viewerId;

            return View(user);
        }


        public ActionResult Like(int postID)
        {
            var viewerID = (int)Session["viewerId"];
            var post = db.posts.SingleOrDefault(p => p.id == postID);
            var userReactOnPost = db.postReacts.SingleOrDefault(r => r.postId == postID && r.userId == viewerID);

            //new like
            if (userReactOnPost == null)
            {
                var postReact = new postReact { postId = postID, userId = viewerID, state = 0 };

                post.LikeCount = post.LikeCount ?? 0 + 1;
                post.DislikeCount = post.DislikeCount ?? 0;

                db.postReacts.Add(postReact);
            }

            else
            {
                if (userReactOnPost.state == 0)
                {
                    userReactOnPost.state = (React?)-1;
                    post.LikeCount--;
                }
                else
                {
                    userReactOnPost.state = 0;
                    post.LikeCount++;
                }
            }


            db.SaveChanges();
            return RedirectToAction("ShowUserPosts", "Profile",
                new { id = Session["id"], viewerId = Session["viewerId"] });
        }

        public ActionResult Dislike(int postID)
        {
            var viewerID = (int)Session["viewerId"];
            var post = db.posts.SingleOrDefault(p => p.id == postID);

            var userReactOnPost = db.postReacts.SingleOrDefault(r => r.postId == postID && r.userId == viewerID);

            //new Dislike
            if (userReactOnPost == null)
            {
                var postReact = new postReact { postId = postID, userId = viewerID, state = (React?)1 };

                post.LikeCount = post.LikeCount ?? 0 + 1;
                post.DislikeCount = post.DislikeCount ?? 0;
                db.postReacts.Add(postReact);
            }

            else
            {
                if (userReactOnPost.state == (React?)1)
                {
                    userReactOnPost.state = (React?)-1;
                    post.DislikeCount--;
                }
                else
                {
                    userReactOnPost.state = (React?)1;
                    post.DislikeCount++;
                }
            }


            db.SaveChanges();
            return RedirectToAction("ShowUserPosts", "Profile",
                new { id = Session["id"], viewerId = Session["viewerId"] });
        }

        public ActionResult Comment(string value, int postID)
        {
            var viewerID = (int)Session["viewerId"];
            var comment = new comment
            {
                content = value,
                postId = postID,
                userId = viewerID
            };

            db.comments.Add(comment);
            db.SaveChanges();
            return RedirectToAction("ShowUserPosts", "Profile",
                new { id = Session["id"], viewerId = Session["viewerId"] });
        }

        public ActionResult ControlRequest(string value)
        {
            var viewerID = (int)Session["viewerId"];
            var id = (int)Session["id"];

            var ff = db.followers.SingleOrDefault(f => f.userId == id && f.followerId == viewerID);

            if (ff == null)
            {
                var req = new follower
                {
                    followerId = viewerID,
                    userId = id,
                    state = "Requested"
                };
                db.followers.Add(req);
            }
            else
            {
                if (ff.state == "Accept") ff.state = "Following";
            }

            db.SaveChanges();
            return RedirectToAction("ViewProfile", "Profile",
                new { id, viewerId = viewerID });
        }

        public ActionResult AcceptRequest(int ID, int ViewerID)
        {
            var ff = db.followers.SingleOrDefault(f => f.userId == ID && f.followerId == ViewerID);


            ff.state = "Following";

            db.SaveChanges();
            return RedirectToAction("ShowUserRequest", "Profile",
                new { ID });
        }
    }
}