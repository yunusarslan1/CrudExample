using CrudExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CrudExample.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetEmployees()
        {
            using (DatabaseEntities db = new DatabaseEntities())
            {
                var employees = db.Employees.OrderBy(x => x.FirstName).ToList();
                return Json(new { data = employees }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Save(int id)
        {
            using (DatabaseEntities db = new DatabaseEntities())
            {
                var v = db.Employees.Where(x => x.EmployeeID == id).FirstOrDefault();
                return View(v);
            }
        }
        [HttpPost]
        public ActionResult Save(Employee emp)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    if (emp.EmployeeID > 0)
                    {
                        var v = db.Employees.Where(x => x.EmployeeID == emp.EmployeeID).FirstOrDefault();
                        if (v != null)
                        {
                            v.FirstName = emp.FirstName;
                            v.LastName = emp.LastName;
                            v.EmailID = emp.EmailID;
                            v.City = emp.City;
                            v.Country = emp.Country;
                        }
                    }
                    else
                    {
                        db.Employees.Add(emp);
                    }
                    db.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (DatabaseEntities db = new DatabaseEntities())
            {
                var v = db.Employees.Where(x => x.EmployeeID == id).FirstOrDefault();
                if (v != null)
                {
                    return View(v);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteEmployye(int id)
        {
            bool status = false;
            using (DatabaseEntities db = new DatabaseEntities())
            {
                var v = db.Employees.Where(x => x.EmployeeID == id).FirstOrDefault();
                if (v != null)
                {
                    db.Employees.Remove(v);
                    db.SaveChanges();
                    status = true;
                }

            }

            return new JsonResult { Data = new { status = status } };
        }
    }
}