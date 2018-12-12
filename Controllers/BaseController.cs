using Microsoft.AspNetCore.Mvc;

namespace COCAS.Controllers
{
    public class BaseController : Controller
    {
        protected static string UsernameSession { get; set; }
        protected static string UserTypeSession { get; set; }

        public bool IsLoggedIn()
        {
            if (!string.IsNullOrEmpty(UsernameSession))
                return true;
            return false;
        }

        protected bool IsStaff()
        {
            if (UserTypeSession == "Staff")
                return true;
            return false;
        }

        protected bool IsStudent()
        {
            if (UserTypeSession == "Student")
                return true;
            return false;
        }

        protected bool IsHoD()
        {
            if (UserTypeSession.Equals("HoD"))
                return true;
            return false;
        }
    }
}