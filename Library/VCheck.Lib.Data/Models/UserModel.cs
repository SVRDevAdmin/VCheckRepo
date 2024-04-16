using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Lib.Data.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string EmployeeID { get; set; }
        //public string Title { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        public string StaffName { get; set; }
        public string RegistrationNo { get; set; }
        public string Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string EmailAddress { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
    }
}
