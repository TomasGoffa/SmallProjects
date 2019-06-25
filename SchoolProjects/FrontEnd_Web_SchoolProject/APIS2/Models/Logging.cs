/// <summary>
/// This file is part of school project.
/// It is a FrontEnd part of cloud application that simulates
/// company that is making a microchips
/// 
/// Author:     Tomas Goffa
/// </summary>

namespace APIS2.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Logging
    {

        [Display(Name = "Login")]
        public string Login { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        public bool LoginToSystem(out string typeOfUser)
        {
            bool loggedIn = false;
            typeOfUser = string.Empty;

            try
            {
                loggedIn = User.ValidateUser(Login, Password, out typeOfUser);
            }
            catch
            {
                typeOfUser = string.Empty;
                loggedIn = false;
            }
            return loggedIn;
        }
    }

    public class LoggedUser
    {
        public string Name { get; set; }

        public string PWord { get; set; }

        public string UType { get; set; }
    }

    public static class CreatedOrder
    {
        public static string ProcessorType { get; set; }

        public static string FlashMemory { get; set; }

        public static string RAMMemmory { get; set; }

        public static string EEPROM { get; set; }

        public static string Frequency { get; set; }

        public static int NumberOfChips { get; set; }
    }
}