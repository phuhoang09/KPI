using KPI.Model.DAO;
using KPI.Model.EF;
using KPI.Model.helpers;
using KPI.Web.helpers;
using KPI.Model.ViewModel;
using KPI.Web.Common;
using KPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace KPI.Web.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
          //var model =  new LevelDAO().GetListTree2(4);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UserProfile objUser)
        {
            if (ModelState.IsValid)
            {

                var obj = new UserLoginDAO().GetUserProfile(objUser.Username, objUser.Password);
                if (obj != null)
                {
                    //Session["UserID"] = obj.ID.ToInt();
                    //Session["UserName"] = obj.Username.ToSafetyString();
                    //Session["FullName"] = obj.FullName.ToSafetyString();
                    //Session["Role"] = obj.Role.ToInt();
                    Session["UserProfile"] = obj as UserProfileVM;
                    Session.Timeout = 525600;
                    return RedirectToAction("Index","Home");
                }
            }
            return View(objUser);
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return Redirect("/");
        }

    }
}