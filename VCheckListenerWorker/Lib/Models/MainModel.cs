using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheckListenerWorker.Lib.Models
{
    public class MainModel
    {
        string _deviceSerialNum;

        public string DeviceSerialNum
        {
            get { return _deviceSerialNum; }
            set { _deviceSerialNum = value; }
        }
    }
}
