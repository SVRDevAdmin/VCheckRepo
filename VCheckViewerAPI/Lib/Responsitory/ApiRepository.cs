﻿using log4net;
using Microsoft.Extensions.Hosting;
using Mysqlx.Session;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;
using VCheckViewerAPI.Lib.Util;

namespace VCheck.Lib.Data
{
    public class ApiRepository
    {
        private ClientAuthDBContext _AuthContext = new ClientAuthDBContext();
        //private ScheduleDBContext _scheduleTestContext = new ScheduleDBContext(Host.CreateApplicationBuilder().Configuration);
        //private TestResultDBContext _testResultContext = new TestResultDBContext(Host.CreateApplicationBuilder().Configuration);

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        public bool Authenticate(string clientKey)
        {
            try
            {
                var ClientAuthIndex = _AuthContext.Mst_Client_Auth.Where(x => x.ClientKey == clientKey).Select(x => x.ClientID).FirstOrDefault();
                return _AuthContext.Mst_Client.Where(x => x.ID == ClientAuthIndex && x.Status == 1).Any();
            }
            catch (Exception ex)
            {
                log.Error("Database Error >>> ", ex);
                return false;
            }
        }

        public bool ValidateTokenExpiry(String clientkey)
        {
            DateOnly sToday = DateOnly.FromDateTime(DateTime.Now);

            try
            {
                return _AuthContext.Mst_Client_Auth.Where(x => x.ClientKey == clientkey && (x.StartDate <= sToday && x.EndDate >= sToday)).Any();
            }
            catch (Exception ex)
            {
                log.Error("ApiRepository >>> ValidateTokenExpiry >>> " + ex.ToString());
                return false;
            }
        }

        public ClientModel GetClientProfileByClientKey(String sClientKey)
        {
            try
            {
                var sClientAuth = _AuthContext.Mst_Client_Auth.Where(x => x.ClientKey == sClientKey).FirstOrDefault();
                if (sClientAuth != null)
                {
                    return _AuthContext.Mst_Client.Where(x => x.ID == sClientAuth.ClientID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Error("ApiRepository >>> GetClientProfileByClientKey >>> " + ex.ToString());
            }

            return null;
        }

        public void GetTestResults(string patientID, out List<PatientDataObject> returnData, out string responseCode, out string responseMessage, out string responseStatus)
        {
            returnData = null;

            try
            {
                var data = TestResultsRepository.GetTestResultByPatientID(ConfigSettings.GetConfigurationSettings(), patientID);
                if (data.Count > 0) { 
                    responseCode = "VV.0001"; 
                    responseMessage = "Success"; 
                    returnData = data; 
                }
                else { 
                    responseCode = "VV.0002"; 
                    responseMessage = "No Data Found"; 
                }
                responseStatus = "Success";
            }
            catch (Exception ex)
            {
                responseCode = "VV.9999";
                responseStatus = "Fail";
                responseMessage = "Internal Error";

                log.Error("Database Error >>> ", ex);
            }
        }

        public void UpdateScheduledTest(string scheduledUniqueID ,string scheduledDatetime, string inchargePerson, out ScheduledTestModel returnData, out string responseCode, out string responseMessage, out string responseStatus)
        {
            returnData = null;

            try
            {
                var data = ScheduledTestRepository.GetScheduledTestByUniqueID(ConfigSettings.GetConfigurationSettings(), scheduledUniqueID);
                if (data == null)
                {
                    responseCode = "VV.0002";
                    responseMessage = "No Data Found";
                }
                else if ((!String.IsNullOrEmpty(scheduledDatetime) && data.ScheduledDateTime != DateTime.ParseExact(scheduledDatetime, "yyyyMMddHHmmss", null)) || (!String.IsNullOrEmpty(inchargePerson) != null && data.InchargePerson != inchargePerson))
                {
                    data.ScheduledDateTime = (!String.IsNullOrEmpty(scheduledDatetime)) ? DateTime.ParseExact(scheduledDatetime, "yyyyMMddHHmmss", null) : data.ScheduledDateTime;
                    data.InchargePerson = (!String.IsNullOrEmpty(inchargePerson)) ? inchargePerson : data.InchargePerson;

                    ScheduledTestRepository.UpdateScheduledTestByUniqueID(ConfigSettings.GetConfigurationSettings(), data);

                    responseCode = "VV.0001";
                    responseMessage = "Success";
                    returnData = data;
                }
                else
                {
                    responseCode = "VV.2001";
                    responseMessage = "No Changes Detected";
                    returnData = data;
                }

                responseStatus = "Success";
            }
            catch (Exception ex)
            {
                responseCode = "VV.9999";
                responseStatus = "Fail";
                responseMessage = "Internal Error";

                log.Error("Database Error >>> ", ex);
            }
        }
    }
}
