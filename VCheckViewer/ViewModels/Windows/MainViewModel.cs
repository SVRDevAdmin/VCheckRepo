using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;

namespace VCheckViewer.ViewModels.Windows
{
    public class MainViewModel
    {
        string _origin;

        public string Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        UserModel _currentUser;
        public UserModel CurrentUsers
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }

        UserModel _user;
        public UserModel Users
        {
            get { return _user; }
            set { _user = value; }
        }

        List<MasterCodeDataModel> _masterCodeDataModel;
        public List<MasterCodeDataModel> MasterCodeDataModel
        {
            get { return _masterCodeDataModel; }
            set { _masterCodeDataModel = value; }
        }

        List<RolesModel> _rolesModel;

        public List<RolesModel> RolesModels
        {
            get { return _rolesModel; }
            set { _rolesModel = value; }
        }


        ObservableCollection<ComboBoxItem> _cbTitle;
        public ObservableCollection<ComboBoxItem> cbTitle
        {
            get { return _cbTitle; }
            set { _cbTitle = value; }
        }

        ObservableCollection<ComboBoxItem> _cbGender;
        public ObservableCollection<ComboBoxItem> cbGender
        {
            get { return _cbGender; }
            set { _cbGender = value; }
        }

        ObservableCollection<ComboBoxItem> _cbRoles;
        public ObservableCollection<ComboBoxItem> cbRoles
        {
            get { return _cbRoles; }
            set { _cbRoles = value; }
        }

        ObservableCollection<ComboBoxItem> _cbStatus;
        public ObservableCollection<ComboBoxItem> cbStatus
        {
            get { return _cbStatus; }
            set { _cbStatus = value; }
        }

        ComboBoxItem _SelectedcbTitle;
        public ComboBoxItem SelectedcbTitle
        {
            get { return _SelectedcbTitle; }
            set { _SelectedcbTitle = value; }
        }

        ComboBoxItem _SelectedcbGender;
        public ComboBoxItem SelectedcbGender
        {
            get { return _SelectedcbGender; }
            set { _SelectedcbGender = value; }
        }

        ComboBoxItem _SelectedcbRoles;
        public ComboBoxItem SelectedcbRoles
        {
            get { return _SelectedcbRoles; }
            set { _SelectedcbRoles = value; }
        }

        ComboBoxItem _SelectedcbStatus;
        public ComboBoxItem SelectedcbStatus
        {
            get { return _SelectedcbStatus; }
            set { _SelectedcbStatus = value; }
        }

        DeviceModel _deviceModel;
        public DeviceModel DeviceModel
        {
            get { 
                return _deviceModel; 
            }
            set { 
                _deviceModel = value; 
            }
        }
    }

    public class ValidateUserModel
    {
        string _email;

        string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                if (!_email.Contains("@"))
                {
                    throw new Exception("Incorrect email format. Must have @.");
                }
            }
        }
    }
}
