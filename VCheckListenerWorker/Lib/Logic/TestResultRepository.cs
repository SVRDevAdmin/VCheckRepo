using log4net;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VCheckListenerWorker.Lib.DBContext;
using VCheckListenerWorker.Lib.Models;

namespace VCheckListenerWorker.Lib.Logic
{
    public class TestResultRepository
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Insert Test Result Raw Data
        /// </summary>
        /// <param name="sResult"></param>
        /// <param name="sResultMSH"></param>
        /// <param name="sResultPID"></param>
        /// <param name="sResultOBR"></param>
        /// <param name="sResultOBXs"></param>
        /// <param name="sResultNTEs"></param>
        /// <returns></returns>
        public static Boolean insertTestObservationMessage(tbltestanalyze_results sResult, 
                                                           tbltestanalyze_results_messageheader sResultMSH,
                                                           List<tbltestanalyze_results_patientidentification> sResultPID, 
                                                           tbltestanalyze_results_observationrequest sResultOBR,
                                                           List<tbltestanalyze_results_observationresult> sResultOBXs, 
                                                           List<tbltestanalyze_results_notes> sResultNTEs)
        {
            Boolean isSuccess = false;

            try
            {
                using (var ctx = new TestResultDBContext(GetConfigurationSettings()))
                {
                    ctx.tbltestanalyze_results.Add(sResult);
                    ctx.SaveChanges();

                    sResultMSH.ResultRowID = sResult.ResultRowID;
                    ctx.tbltestanalyze_results_messageheader.Add(sResultMSH);
                    ctx.SaveChanges();

                    if (sResultPID.Count > 0)
                    {
                        foreach (var PID in sResultPID)
                        {
                            PID.ResultRowID = sResult.ResultRowID;
                            ctx.tbltestanalyze_results_patientidentification.Add(PID);
                            ctx.SaveChanges();
                        }
                    }


                    sResultOBR.ResultRowID = sResult.ResultRowID;
                    ctx.tbltestanalyze_results_observationrequest.Add(sResultOBR);
                    ctx.SaveChanges();

                    if (sResultOBXs.Count > 0)
                    {
                        foreach(var OBX in sResultOBXs)
                        {
                            OBX.ResultRowID = sResult.ResultRowID;

                            ctx.tbltestanalyze_results_observationresult.Add(OBX);
                            ctx.SaveChanges();
                        }

                    }

                    if (sResultNTEs.Count > 0)
                    {
                        foreach(var NTE in sResultNTEs)
                        {
                            NTE.ResultRowID = sResult.ResultRowID;

                            ctx.tbltestanalyze_results_notes.Add(NTE);
                            ctx.SaveChanges();
                        }
                    }

                    isSuccess = true;
                }
            }
            catch(Exception ex)
            {
                log.Error("VCheckListenerWorker >>> TestResultRepository >>> InsertTestObservationMessag >>> " +ex.ToString());
                isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        /// Insert into Test Result Table
        /// </summary>
        /// <param name="sTestResult"></param>
        /// <returns></returns>
        public static Boolean createTestResult(txn_testresults sTestResult)
        {
            Boolean isSuccess = false;

            try
            {
                using (var ctx = new TestResultDBContext(GetConfigurationSettings()))
                {
                    ctx.txn_Testresults.Add(sTestResult);
                    ctx.SaveChanges();

                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                log.Error("VCheckListenerWorker >>> TestResultRepository >>> createTestResult >>> " + ex.ToString());
                isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        /// Get Notification Template By Code
        /// </summary>
        /// <param name="sTemplateCode"></param>
        /// <returns></returns>
        public static mst_template GetNotificationTemplate(String sTemplateCode)
        {
            try
            {
                using (var ctx = new TestResultDBContext(GetConfigurationSettings()))
                {
                    return ctx.mst_template.Where(x => x.TemplateCode == sTemplateCode).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Error("VCheckListenerWorker >>> TestResultRepository >>> GetNorificationTemplate >>> " + ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// Insert Notification 
        /// </summary>
        /// <param name="sNotification"></param>
        /// <returns></returns>
        public static Boolean insertNotification(txn_notification sNotification)
        {
            Boolean isSuccess = false;

            try
            {
                using (var ctx = new TestResultDBContext(GetConfigurationSettings()))
                {
                    ctx.txn_notification.Add(sNotification);
                    ctx.SaveChanges();

                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                log.Error("VCheckListenerWorker >>> TestResultRepository >>> insertNotification >>> " + ex.ToString());
                isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        /// Get Appsetting configuration
        /// </summary>
        /// <returns></returns>
        public static IConfiguration GetConfigurationSettings()
        {
            var iHost = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder();

            return iHost.Configuration;
        }
    }
}
