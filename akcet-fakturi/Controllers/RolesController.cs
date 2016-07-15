using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using akcet_fakturi.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;
using Tools;

namespace akcet_fakturi.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        // GET: /Roles/ViewAllRoles
        [HttpGet]
        public ActionResult ViewAllRoles()
        {
            var roles = context.Roles.ToList();
            var model = new ListOfRoles();

            int counter = 1;

            foreach (var role in roles)
            {
                var modelitem = new RolesInfo();
                modelitem.RoleName = role.Name;
                modelitem.RoleId = counter;
                IdentityRole myRole= context.Roles.First(r => r.Name == role.Name);
                Int32 count = context.Set<IdentityUserRole>().Count(r => r.RoleId == myRole.Id);
                modelitem.CountUsersInRole = count;
                model.ListRoles.Add(modelitem);
                counter++;

            }
            if (model.ListRoles.Count > 0)
            {
                return View(model);
            }
            else
            {
                TempData["ResultError"] = "There is no roles defined!";
                return View();
            }
  
        }


        // GET: /Roles/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Roles/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
                {
                    Name = collection["RoleName"]
                });
                context.SaveChanges();
                TempData["ResultSuccess"] = "Role created successfully !";
                return RedirectToAction("ViewAllRoles");
            }
            catch
            {
                TempData["ResultError"] = "Error in creating role!";
                return RedirectToAction("ViewAllRoles");
            }
        }




        // GET: /Roles/Edit/5
        [HttpGet]
        public ActionResult EditRole(string roleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            return View(thisRole);
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRole(IdentityRole role)
        {
            try
            {
                context.Entry(role).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                TempData["ResultSuccess"] = "Role edited successfully !";
                return RedirectToAction("ViewAllRoles");
            }
            catch
            {
                TempData["ResultError"] = "Error in editing role!";
                return RedirectToAction("ViewAllRoles");
            }
        }




        [HttpGet]
        public ActionResult Delete(string roleName)
        {
            try
            {
                var thisRole = context.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                context.Roles.Remove(thisRole);
                context.SaveChanges();

                TempData["ResultSuccess"] = "Role deleted successfully !";
                return RedirectToAction("ViewAllRoles");
            }
            catch (Exception)
            {
                TempData["ResultError"] = "Error in editing role!";
                return RedirectToAction("ViewAllRoles");
            }

        }



        [HttpGet]
        public ActionResult ManageUserRoles()
        {
            var model = new UserRolesModels();
            // prepopulat roles for the view dropdown


            ViewBag.Users = context.Users.OrderBy(u => u.UserName).ToList().Select(uu => new SelectListItem { Value = uu.Email, Text = uu.FirstName + " " + uu.LastName }).ToList();

            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

            ViewBag.Roles = list;



            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(UserRolesModels model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationUser user = context.Users.Where(u => u.UserName.Equals(model.SelectedUsers, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                    var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var idResult = um.AddToRole(user.Id, model.SelectedRoles);

                    ViewBag.Users = context.Users.OrderBy(u => u.UserName).ToList().Select(uu => new SelectListItem { Value = uu.Email, Text = uu.FirstName + " " + uu.LastName }).ToList();

                    ViewBag.RolesForThisUser = um.GetRoles(user.Id);

                    ViewBag.SelectedUser = String.Format("{0} {1}", user.FirstName, user.LastName);
                    // prepopulat roles for the view dropdown 
                    var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                    ViewBag.Roles = list;

                    TempData["ResultSuccess"] = String.Format("Role added successfully to user {0} {1}!", user.FirstName, user.LastName);

                    return View("ManageUserRoles");

                }
                catch (Exception ex)
                {
                    EmailFunctions.SendExceptionToAdmin(ex);
                    TempData["ResultError"] = "Error in adding role!";
                    return RedirectToAction("ManageUserRoles");
                }
            }

            return RedirectToAction("ManageUserRoles");


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(UserRolesModels model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationUser user = context.Users.Where(u => u.UserName.Equals(model.SelectedUsers, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                    var account = new AccountController();


                    //   ViewBag.RolesForThisUser = account.UserManager.GetRoles(user.Id);

                    var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    ViewBag.RolesForThisUser = um.GetRoles(user.Id);

                    ViewBag.Users = context.Users.OrderBy(u => u.UserName).ToList().Select(uu => new SelectListItem { Value = uu.Email, Text = uu.FirstName + " " + uu.LastName }).ToList();

                    // prepopulat roles for the view dropdown 
                    var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                    ViewBag.Roles = list;
                }
                catch (Exception ex)
                {
                    EmailFunctions.SendExceptionToAdmin(ex);
                }


            }

            return View("ManageUserRoles");
        }

        // GET: /Roles/Delete


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(UserRolesModels model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(model.SelectedUsers, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));


                if (um.IsInRole(user.Id, model.SelectedRoles))
                {
                    um.RemoveFromRole(user.Id, model.SelectedRoles);

                    TempData["ResultSuccess"] = String.Format("Role removed from user: {0} {1} successfully!", user.FirstName, user.LastName);
                }
                else
                {
                    TempData["ResultError"] = String.Format("User: {0} {1} doesn't belong to selected role.", user.FirstName, user.LastName);

                }


                ViewBag.Users = context.Users.OrderBy(u => u.UserName).ToList().Select(uu => new SelectListItem { Value = uu.Email, Text = uu.FirstName + " " + uu.LastName }).ToList();

                ViewBag.RolesForThisUser = um.GetRoles(user.Id);

                ViewBag.SelectedUser = String.Format("{0} {1}", user.FirstName, user.LastName);

                // prepopulat roles for the view dropdown 
                var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = list;

            }
            return View("ManageUserRoles");
        }



    }
}