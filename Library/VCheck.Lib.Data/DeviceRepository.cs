using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace VCheck.Lib.Data
{
    public class DeviceRepository
    {
        /// <summary>
        /// Add new Device record
        /// </summary>
        /// <param name="sDevice"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static Boolean InsertDevice(DeviceModel sDevice, IConfiguration config)
        {
            Boolean isSuccess = false;

            try
            {
                var ctx = new DeviceDBContext(config);
                ctx.mst_deviceslist.Add(sDevice);
                ctx.SaveChanges();

                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        /// Get Device List
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static List<DeviceModel> GetDeviceList(IConfiguration config)
        {
            try
            {
                var ctx = new DeviceDBContext(config);

                return ctx.mst_deviceslist.Where(x => x.status == 1).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Get Device Info by ID
        /// </summary>
        /// <param name="iID"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static DeviceModel? GetDeviceByID(int iID, IConfiguration config)
        {
            try
            {
                var ctx = new DeviceDBContext(config);
                return ctx.mst_deviceslist.Where(x => x.id == iID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Update Device Info
        /// </summary>
        /// <param name="sDeviceObj"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static Boolean UpdateDevice(DeviceModel sDeviceObj, IConfiguration config)
        {
            Boolean isSuccess = false;

            try
            {
                var ctx = new DeviceDBContext(config);

                var sDevice = ctx.mst_deviceslist.Where(x => x.id == sDeviceObj.id).FirstOrDefault();
                if (sDevice != null)
                {
                    Boolean isUpdate = false;

                    if (sDevice.DeviceName != sDeviceObj.DeviceName)
                    {
                        sDevice.DeviceName = sDeviceObj.DeviceName;
                        isUpdate = true;
                    }

                    if (sDevice.DeviceIPAddress != sDeviceObj.DeviceIPAddress)
                    {
                        sDevice.DeviceIPAddress = sDeviceObj.DeviceIPAddress;
                        isUpdate = true;
                    }

                    if (sDevice.status != sDeviceObj.status)
                    {
                        sDevice.status = sDeviceObj.status;
                        isUpdate = true;
                    }

                    if (isUpdate)
                    {
                        sDevice.UpdatedDate = sDeviceObj.UpdatedDate;
                        sDevice.UpdatedBy = sDeviceObj.UpdatedBy;
                    }

                    ctx.SaveChanges();

                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                isSuccess =  false;
            }

            return isSuccess;
        }
    }
}
