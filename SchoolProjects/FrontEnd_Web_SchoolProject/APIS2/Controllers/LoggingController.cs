/// <summary>
/// This file is part of school project.
/// It is a FrontEnd part of cloud application that simulates
/// company that is making a microchips
/// 
/// Author:     Tomas Goffa
/// </summary>

namespace APIS2.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using APIS2.Models;

    public class LoggingController : Controller
    {
        [HttpGet]
        public ActionResult LoggingView()
        {
            Session["user"] = null;

            Logging logging = new Logging();
            return View(logging);
        }

        [HttpGet]
        public ActionResult ManagerView()
        {

            if (Session["user"] == null)
            {
                Logging newLogging = new Logging();
                return RedirectToAction("LoggingView", "Logging", newLogging);
            }
            else
            {
                var myUser = Session["user"] as LoggedUser;
                if (!myUser.UType.Equals(Models.User.TYPE_MANAGER))
                {
                    Logging newLogging = new Logging();
                    return RedirectToAction("LoggingView", "Logging", newLogging);
                }
            }

            ManagerPage managerPage = new ManagerPage();
            return View(managerPage);
        }

        [HttpGet]
        public ActionResult OperatorView()
        {
            if (Session["user"] == null)
            {
                Logging newLogging = new Logging();
                return RedirectToAction("LoggingView", "Logging", newLogging);
            }
            else
            {
                var myUser = Session["user"] as LoggedUser;
                if (!myUser.UType.Equals(Models.User.TYPE_OPERATOR))
                {
                    Logging newLogging = new Logging();
                    return RedirectToAction("LoggingView", "Logging", newLogging);
                }
            }

            OperatorPage operatorPage = new OperatorPage();
            return View(operatorPage);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Login")]
        public ActionResult Login(Logging logging)
        {
            logging.LoginToSystem (out string typeOfUser);

            if (typeOfUser.Equals(Models.User.TYPE_MANAGER))
            {
                Session["user"] = new LoggedUser()
                {
                    Name = logging.Login,
                    PWord = logging.Password,
                    UType = Models.User.TYPE_MANAGER
                };

                ManagerPage managerPage = new ManagerPage();
                return RedirectToAction("ManagerView", "Logging", managerPage);
            }
            else if (typeOfUser.Equals(Models.User.TYPE_OPERATOR))
            {
                Session["user"] = new LoggedUser()
                {
                    Name = logging.Login,
                    PWord = logging.Password,
                    UType = Models.User.TYPE_OPERATOR
                };

                OperatorPage operatorPage = new OperatorPage();
                return RedirectToAction("OperatorView", "Logging", operatorPage);
            }
            else
            {
                Session["user"] = null;

                Logging newLogging = new Logging();
                return View("LoggingView", newLogging);
            }
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "CreateAccount")]
        public ActionResult CreateAccount(ManagerPage managerPage)
        {
            if (string.IsNullOrEmpty(managerPage.FirstName) || string.IsNullOrEmpty(managerPage.LastName) ||
                string.IsNullOrEmpty(managerPage.NickName) || string.IsNullOrEmpty(managerPage.Password) ||
                string.IsNullOrEmpty(managerPage.PasswordVerify))
            {
                ManagerPage newManagerPage = new ManagerPage();
                ViewBag.Message = "Please set all attributes for the new account.";
                return View("ManagerView", newManagerPage);
            }

            if (managerPage.Password.Equals(managerPage.PasswordVerify))
            {
                MicroTechDatabaseContext database = new MicroTechDatabaseContext();
                var idS = database.Employees.ToList();
                int maxID = 0;

                for (int i = 0; i < idS.Count; i++)
                {
                    if (maxID <= idS.ElementAt(i).ID)
                    {
                        maxID = idS.ElementAt(i).ID;
                    }
                }

                database.Employees.Add(new Employee
                {
                    ID = maxID + 1,
                    FirstName = managerPage.FirstName,
                    LastName = managerPage.LastName,
                    NickName = managerPage.NickName,
                    Password = managerPage.Password,
                    Type = managerPage.UserType
                });

                database.SaveChanges();
                database.Dispose();
                ManagerPage newManagerPage = new ManagerPage();
                ViewBag.Message = "Account has been created";
                return View("ManagerView", newManagerPage);
            }
            else
            {
                managerPage.Password = string.Empty;
                managerPage.PasswordVerify = string.Empty;
                ManagerPage newManagerPage = new ManagerPage();
                ViewBag.Message = "Password and Password(Verify) are not the same.";
                return View("ManagerView", newManagerPage);
            }
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "DeleteAccount")]
        public ActionResult DeleteAccount(string IDofUserToDelete)
        {
            MicroTechDatabaseContext database = new MicroTechDatabaseContext();
            bool accountExists = false;
            var users = database.Employees.ToList();

            for (int i = 0; i < users.Count; i++)
            {
                if (IDofUserToDelete.Equals(users.ElementAt(i).ID.ToString()))
                {
                    Employee userToDelete = users.ElementAt(i);
                    database.Employees.Attach(userToDelete);
                    database.Employees.Remove(userToDelete);
                    database.SaveChanges();
                    accountExists = true;
                    break;
                }
            }

            database.Dispose();
            ManagerPage newManagerPage = new ManagerPage();

            if (accountExists)
            {
                ViewBag.Message = "Account has been deleted.";
            }
            else
            {
                ViewBag.Message = "Account has not been deleted.";
            }

            return View("ManagerView", newManagerPage);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "StartStop")]
        public ActionResult UpdateImage()
        {


            return View("OperatorView");
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ConfirmOrder")]
        public ActionResult ConfirmOrder(BillData selectedOrder)
        {
            // Confirming order as operator

            return View("OperatorView");
        }
    }
}