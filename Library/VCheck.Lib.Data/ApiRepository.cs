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

namespace VCheck.Lib.Data
{
    public class ApiRepository
    {
        private ClientAuthDBContext _AuthContext = new ClientAuthDBContext();
        private ScheduleDBContext _scheduleTestContext = new ScheduleDBContext(Host.CreateApplicationBuilder().Configuration);
        private TestResultDBContext _testResultContext = new TestResultDBContext(Host.CreateApplicationBuilder().Configuration);

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

        public void GetTestResults(string patientID, out object? returnData, out string responseCode, out string responseMessage, out string responseStatus)
        {
            returnData = null;
            try
            {
                var data =  _testResultContext.txn_testResults.Where(x => x.PatientID == patientID).ToList();

                if (data.Count > 0) { responseCode = "VV.0001"; responseMessage = "Success"; returnData = data; }
                else { responseCode = "VV.0002"; responseMessage = "No Data Found"; }
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

        public void UpdateScheduledTest(string scheduledUniqueID ,string scheduledDatetime, string inchargePerson, out object? returnData, out string responseCode, out string responseMessage, out string responseStatus)
        {
            returnData = null;
            try
            {
                var data = _scheduleTestContext.Txn_ScheduledTests.Where(x => x.ScheduleUniqueID == scheduledUniqueID).FirstOrDefault();

                if(data == null)
                {
                    responseCode = "VV.0002";
                    responseMessage = "No Data Found";
                }
                else if ((scheduledDatetime != null && data.ScheduledDateTime != DateTime.Parse(scheduledDatetime)) || (inchargePerson != null && data.InchargePerson != inchargePerson))
                {
                    data.ScheduledDateTime = scheduledDatetime != null ? DateTime.Parse(scheduledDatetime) : data.ScheduledDateTime;
                    data.InchargePerson = inchargePerson != null ? inchargePerson : data.InchargePerson;
                    _scheduleTestContext.Txn_ScheduledTests.Update(data);
                    _scheduleTestContext.SaveChanges();
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
