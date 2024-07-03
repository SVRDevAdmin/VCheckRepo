using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Lib.Data.Models
{
    public class DeviceModel
    {
        [Key]
        public int id { get; set; }
        public string? DeviceName { get; set; }
        public string? Description { get; set; }
        public string? DeviceIPAddress { get; set;  }
        public string? DeviceImagePath { get; set; }
        public int? status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public String? DeviceSerialNo { get; set; }
    }
}
