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
        public string? DeviceSerialNo { get; set; }
        public string? Description { get; set; }
        public string? DeviceIPAddress { get; set;  }
        public string? DeviceImagePath { get; set; }
        public int? status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public int? DeviceTypeID { get; set; }
    }

    public class DeviceTypeModel
    {
        [Key]
        public int id { get; set; }
        public String? TypeName { get; set; }
        public int? Status { get; set; }
        public int? SeqNo { get; set; }
        public String? ImageSource { get; set; }
        public DateTime? CreatedDate { get; set; }
        public String? CreatedBy { get; set; }
     }
}
