﻿using log4net;
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
using System.Reflection;
using Org.BouncyCastle.Asn1.Mozilla;

namespace VCheck.Lib.Data
{
    public class DeviceRepository
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

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
                using (var ctx = new DeviceDBContext(config))
                {
                    ctx.mst_deviceslist.Add(sDevice);
                    ctx.SaveChanges();

                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error >>> " + ex.ToString());
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
                using (var ctx = new DeviceDBContext(config))
                {
                    return ctx.mst_deviceslist.Where(x => x.status == 1).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error >>> " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Get Device List
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static List<DeviceModel> GetAllDeviceList(IConfiguration config)
        {
            try
            {
                using (var ctx = new DeviceDBContext(config))
                {
                    return ctx.mst_deviceslist.ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error >>> " + ex.ToString());
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
                using (var ctx = new DeviceDBContext(config))
                {
                    return ctx.mst_deviceslist.Where(x => x.id == iID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error >>> " + ex.ToString());
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

                    if (sDevice.DeviceSerialNo != sDeviceObj.DeviceSerialNo)
                    {
                        sDevice.DeviceSerialNo = sDeviceObj.DeviceSerialNo;
                        isUpdate = true;
                    }

                    if (sDevice.DeviceImagePath != sDeviceObj.DeviceImagePath)
                    {
                        sDevice.DeviceImagePath = sDeviceObj.DeviceImagePath;
                        isUpdate = true;
                    }

                    if (sDevice.DeviceTypeID != sDeviceObj.DeviceTypeID)
                    {
                        sDevice.DeviceTypeID = sDeviceObj.DeviceTypeID;
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
                log.Error("Error >>> " + ex.ToString());
                isSuccess =  false;
            }

            return isSuccess;
        }

        /// <summary>
        /// Get Device Type List
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static List<DeviceTypeModel> GetDeviceTypeList(IConfiguration config)
        {
            try
            {
                using (var ctx = new DeviceDBContext(config))
                {
                    return ctx.mst_devicetype.Where(x => x.Status == 1).OrderBy(x => x.SeqNo).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error("DeviceRepository >>> GetDeviceTypeList >>> " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Get Device Type By Id
        /// </summary>
        /// <param name="config"></param>
        /// <param name="iID"></param>
        /// <returns></returns>
        public static DeviceTypeModel GetDeviceTypeByID(IConfiguration config, int iID)
        {
            try
            {
                using (var ctx = new DeviceDBContext(config))
                {
                    return ctx.mst_devicetype.Where(x => x.id == iID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Error("DeviceRepository >>> GetDeviceTypeByID >>> " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Get Two-Way Communication Device list
        /// </summary>
        /// <param name="config"></param>
        /// <param name="iID"></param>
        /// <returns></returns>
        public static List<DeviceModel> GetTwoWayCommDevice(IConfiguration config)
        {
            try
            {
                using (var ctx = new DeviceDBContext(config))
                {
                    var deviceTypeIDs = ctx.mst_devicetype.Where(x => x.TwoWayCommunication == 1).Select(y => y.id).ToList();

                    return ctx.mst_deviceslist.Where(x => deviceTypeIDs.Contains(x.DeviceTypeID.Value)).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error("DeviceRepository >>> GetTwoWayCommDevice >>> " + ex.ToString());
                return null;
            }
        }
    }
}
