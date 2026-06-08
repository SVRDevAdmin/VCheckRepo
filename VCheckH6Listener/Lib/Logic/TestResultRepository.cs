using log4net;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VCheckH6Listener.Lib.DBContext;
using VCheckH6Listener.Lib.Models;
using VCheckH6Listener.Lib.DBContext;
using Microsoft.Extensions.Hosting;

namespace VCheckH6Listener.Lib.Logic
{
    public class TestResultRepository
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);
        public static string graphFolder = Host.CreateApplicationBuilder().Configuration.GetSection("FileOutput:Graph").Value;

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
                                                           List<tbltestanalyze_results_notes> sResultNTEs,
                                                           tbltestanalyze_results_patientvisit sResultPV,
                                                           tbltestanalyze_results_specimen sResultSpecimen = null, 
                                                           tbltestanalyze_results_specimencontainer sResultContainer = null)
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

                    if (sResultPV != null)
                    {
                        sResultPV.ResultRowID = sResult.ResultRowID;

                        ctx.tbltestanalyze_results_patientvisit.Add(sResultPV);
                        ctx.SaveChanges();
                    }

                    if (sResultSpecimen != null)
                    {
                        sResultSpecimen.ResultRowID = sResult.ResultRowID;

                        ctx.tbltestanalyze_results_specimen.Add(sResultSpecimen);
                        ctx.SaveChanges();
                    }

                    if (sResultContainer != null)
                    {
                        sResultContainer.ResultRowID = sResult.ResultRowID;

                        ctx.tbltestanalyze_results_specimencontainer.Add(sResultContainer);
                        ctx.SaveChanges();
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
                    ctx.txn_Testresults.Update(sTestResult);
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
        /// Insert into Test Result table with multiple breakdown details
        /// </summary>
        /// <param name="sTestResult"></param>
        /// <param name="sTestResultDetail"></param>
        /// <returns></returns>
        public static Boolean createTestResultsMultipleParam(txn_testresults sTestResult, List<txn_testresults_details> sTestResultDetail, List<txn_testresults_graphsExtended> sTestResultGraph = null)
        {
            Boolean isSuccess = false;

            try
            {
                using (var ctx = new TestResultDBContext(GetConfigurationSettings()))
                {
                    ctx.txn_Testresults.Add(sTestResult);
                    ctx.SaveChanges();

                    foreach(var d in sTestResultDetail)
                    {
                        d.TestResultRowID = sTestResult.ID;

                        ctx.txn_testresults_details.Add(d);
                        ctx.SaveChanges();
                    }

                    if(sTestResultGraph != null && sTestResultGraph.Count() > 0)
                    {
                        foreach (var d in sTestResultGraph)
                        {
                            var graph = new txn_testresults_graphs() { TestResultRowID = sTestResult.ID, FileName = d.FileName, CreatedDate = DateTime.Now, CreatedBy = "VCheckViewer Listener" };
                            ctx.txn_testresults_graphs.Add(graph);

                            byte[] imageBytes = Convert.FromBase64String(d.Base64String);

                            if (!Directory.Exists(graphFolder + sTestResult.ID))
                            {
                                Directory.CreateDirectory(graphFolder + sTestResult.ID);
                            }

                            File.WriteAllBytes(graphFolder + sTestResult.ID + "\\" + d.FileName, imageBytes);
                        }
                        ctx.SaveChanges();
                    }

                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                log.Error("VCheckListenerWorker >>> TestResultRepository >>> createTestResultsMultipleParam >>> " + ex.ToString());
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
        /// Get Notification Template By Code & Locale
        /// </summary>
        /// <param name="sTemplateCode"></param>
        /// <param name="sLangCode"></param>
        /// <returns></returns>
        public static NotificationTemplateLang GetNotificationTemplateByLanguage(String sTemplateCode, String sLangCode)
        {
            var sTemplateObj = new NotificationTemplateLang();

            try
            {
                using (var ctx = new TestResultDBContext(GetConfigurationSettings()))
                {
                    var sResult = ctx.mst_template.Where(x => x.TemplateCode == sTemplateCode).FirstOrDefault();
                    if (sResult != null)
                    {
                        var sTemplateContentObj = ctx.mst_template_details.Where(x => x.TemplateID == sResult.TemplateID && x.LangCode == sLangCode).FirstOrDefault();
                        if (sTemplateContentObj != null)
                        {
                            sTemplateObj.TemplateID = sTemplateContentObj.TemplateID;
                            sTemplateObj.TemplateType = sResult.TemplateType;
                            sTemplateObj.TemplateCode = sResult.TemplateCode;
                            sTemplateObj.TemplateTitle = sTemplateContentObj.TemplateTitle;
                            sTemplateObj.TemplateContent = sTemplateContentObj.TemplateContent;
                            sTemplateObj.TemplateLangCode = sTemplateContentObj.LangCode;
                        }
                    }

                    return sTemplateObj;
                }
            }
            catch (Exception ex)
            {
                log.Error("VCheckListenerWorker >>> TestResultRepository >>> GetNotificationTemplateByLanguage >>> " + ex.ToString());
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
        /// Get Configuration Settings By Key
        /// </summary>
        /// <param name="sConfigurationkey"></param>
        /// <returns></returns>
        public static mst_configuration GetConfigurationByKey(String sConfigurationkey)
        {
            try
            {
                using (var ctx = new TestResultDBContext(GetConfigurationSettings()))
                {
                    return ctx.mst_configuration.Where(x => x.ConfigurationKey == sConfigurationkey).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Error("VCheckListenerWorker >>> TestResultRepository >>> GetConfigurationByKey >>> " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Get Configuration Settings By Key
        /// </summary>
        /// <param name="sConfigurationkey"></param>
        /// <returns></returns>
        public static List<mst_configuration> GetAllConfiguration()
        {
            try
            {
                using (var ctx = new TestResultDBContext(GetConfigurationSettings()))
                {
                    return ctx.mst_configuration.ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error("VCheckListenerWorker >>> TestResultRepository >>> GetConfigurationByKey >>> " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Get Device Settings By IP
        /// </summary>
        /// <param name="sConfigurationkey"></param>
        /// <returns></returns>
        public static DeviceModel GetDeviceByIPSerialNo(String sIP, String sSerialNo, out string sDeviceType)
        {
            try
            {
                using (var ctx = new TestResultDBContext(GetConfigurationSettings()))
                {
                    DeviceModel device = new DeviceModel();

                    if (!string.IsNullOrEmpty(sIP))
                    {
                        device =  ctx.mst_deviceslist.FirstOrDefault(x => x.DeviceIPAddress == sIP);
                    }
                    else if (!string.IsNullOrEmpty(sSerialNo))
                    {
                        device = ctx.mst_deviceslist.FirstOrDefault(x => x.DeviceSerialNo == sSerialNo);
                    }

                    sDeviceType = ctx.mst_devicetype.FirstOrDefault(x => x.id == device.DeviceTypeID).TypeName;

                    return device;
                }
            }
            catch (Exception ex)
            {
                log.Error("VCheckListenerWorker >>> TestResultRepository >>> GetScheduleByPatientID >>> " + ex.ToString());
                sDeviceType = "";
                return null;
            }
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
