using System.IO;
using System.Linq;
using System.Web.Mvc;
using instagram.Models;
namespace instagram.Controllers
{

    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            user userModel = new user();
            return View(userModel);
        }

        [HttpPost]
        public ActionResult AddOrEdit(user userModel)

        {
            string fileName = Path.GetFileName(userModel.ImageFile.FileName);
            userModel.image = "~/Image/Profiles/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/Profiles/"), fileName);
            userModel.ImageFile.SaveAs(fileName);

            using (instagramEntities1 instagramEntities = new instagramEntities1())
            {
                if (instagramEntities.users.Any(x => x.name == userModel.name))
                {
                    return View("AddOrEdit", userModel);

                } 
                instagramEntities.users.Add(userModel);
                instagramEntities.SaveChanges();
            }
            ModelState.Clear();

            
            return View("AddOrEdit",new user());
        }

        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Authorize(user userModel)
        {
            using (instagramEntities1 db = new instagramEntities1())
            {
                var userDetails = db.users.Where(x => x.name == userModel.name && x.password == userModel.password).FirstOrDefault();
                if (userDetails == null)
                {
                    userModel.LoginErrorMessage = "Wrong username or password.";
                    return View("Login", userModel);
                }
                else
                {
                    Session["userID"] = userDetails.id;
                    Session["userName"] = userDetails.name;
                    return RedirectToAction("Index", "Home");
                }
            }
        }


    }
}