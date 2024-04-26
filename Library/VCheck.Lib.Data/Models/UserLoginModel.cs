using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Lib.Data.Models
{
    public class UserLoginModel
    {
        public int UserLoginID { get; set; }
        public string UserID { get; set; }
        public string LoginID { get; set; }
        public string Password { get; set; }
        public bool IsLocked { get; set; }
        public DateTime LockedDateTime { get; set; }
        public DateTime LastLoginDateTime { get; set; }
    }
}
