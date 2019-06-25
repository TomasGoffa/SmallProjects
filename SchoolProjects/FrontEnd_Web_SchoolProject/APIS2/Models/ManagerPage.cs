/// <summary>
/// This file is part of school project.
/// It is a FrontEnd part of cloud application that simulates
/// company that is making a microchips
/// 
/// Author:     Tomas Goffa
/// </summary>

namespace APIS2.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    public class ManagerPage
    {
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Nick name")]
        public string NickName { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Password (Verify)")]
        public string PasswordVerify { get; set; }

        [Display(Name = "Type of employee")]
        public string UserType { get; set; }

        public List<SelectListItem> UsersToDelete { get; set; }

        public List<SelectListItem> PossibleUserTypes { get; set; }

        public ManagerPage()
        {
            this.PossibleUserTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = "Manager", Value = User.TYPE_MANAGER, Selected = true },
                new SelectListItem { Text = "Operator", Value = User.TYPE_MANAGER }
            };

            this.UsersToDelete = new List<SelectListItem>();

            MicroTechDatabaseContext database = new MicroTechDatabaseContext();
            var listOfUsers = database.Employees.ToList();

            string currentUser = string.Empty;

            for (int i = 0; i < listOfUsers.Count; i++)
            {
                currentUser = listOfUsers.ElementAt(i).FirstName + " ";
                currentUser += listOfUsers.ElementAt(i).LastName + ", ";
                currentUser += listOfUsers.ElementAt(i).Type;

                this.UsersToDelete.Add(new SelectListItem { Text = currentUser, Value = listOfUsers.ElementAt(i).ID.ToString() });
            }

            database.Dispose();
        }
    }
}