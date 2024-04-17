using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCheck.Lib.Data.Models;

namespace VCheckViewer.ViewModels.Windows
{

    public class MainViewModel
    {
        UserModel _user;
        public UserModel Users {

            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }
    }
}
