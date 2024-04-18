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
    }
}
