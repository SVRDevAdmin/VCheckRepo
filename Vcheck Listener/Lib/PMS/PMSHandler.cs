using LiteDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using VCheck.Interface.API;
using Vcheck_Listener.Lib.Models;
using Vcheck_Listener.Lib.Repositories;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static Vcheck_Listener.Views.ConfigurationWindow;

namespace Vcheck_Listener.Lib.PMS
{
    public class PMSHandler
    {
        public string PMS;
        public string url;
        public string port;
        public string username;
        public string password;
        public bool isIP;
        public System.Net.IPAddress iIP;

        TcpClient ourTcpClient = null;
        NetworkStream networkStream = null;
        String receivedMessage = "";

        private static readonly string subscriptDigits = "₀₁₂₃₄₅₆₇₈₉₋₊";
        private static readonly string superscriptDigits = "⁰¹²³⁴⁵⁶⁷⁸⁹⁻⁺";
        private static readonly string normalDigits = "0123456789-+";
        private static readonly string[] calItems = { "A/G", "B/C", "GLOB", "Na/K" };
        private static readonly string[] cCortisolParameter = { "Pre-ACTH", "Post-ACTH", "Pre-dose(L)", "Post-4Hours(L)", "Post-8Hours(L)", "Pre-dose(H)", "Post-4Hours(H)", "Post-8Hours(H)" };
        private static readonly string[] endParameter = { "Post-ACTH", "Post-8dose(L)", "Post-8dose(H)" };

        //public static LiteDatabase db = new LiteDatabase("Filename=Storage/TestResult.db;Password=Vch@ck123;");

        public async Task<bool> SendToPMS(txn_testresults sTestResults, List<txn_testresults_details> sTestResultDetails, ScheduledTestModel sScheduledTestObj, string sTestCode, string sNotes = "")
        {
            VCheck.Interface.API.Greywind.RequestMessage.UpdateResultRequest sRequestAPI = new VCheck.Interface.API.Greywind.RequestMessage.UpdateResultRequest();

            List<VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPanelTestObject> sResultListing = new List<VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPanelTestObject>();
            List<VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPanelsObject> sPanelListing = new List<VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPanelsObject>();

            String sOrderID = "";
            string[] ownerName = sScheduledTestObj.OwnerName.Split(" ");
            var lastName = "";
            int start = 0;

            foreach (var name in ownerName)
            {
                if (ownerName.Length == 1)
                {
                    lastName = name;
                }
                else if (start != 0)
                {
                    lastName = string.IsNullOrEmpty(lastName) ? lastName + ownerName[start] : lastName + " " + ownerName[start];
                }

                start++;
            }

            if (sScheduledTestObj.ScheduleUniqueID.Contains("-"))
            {
                var UniqueIDSplit = sScheduledTestObj.ScheduleUniqueID.Split("-");
                if (UniqueIDSplit.Length > 0)
                {
                    sOrderID = UniqueIDSplit[1];
                    sRequestAPI.accessionnumber = UniqueIDSplit[2];
                }
            }

            sRequestAPI.clinic_id = sScheduledTestObj.LocationID.ToString();
            sRequestAPI.reportdate = sTestResults.CreatedDate.Value.ToString("yyyy-MM-dd");
            sRequestAPI.providerid = "";

            VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPatientObject sPatientObj = new VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPatientObject();
            sPatientObj.patientid = sScheduledTestObj.PatientID;
            sPatientObj.firstname = (sScheduledTestObj != null) ? sScheduledTestObj.PatientName : "";
            sPatientObj.lastname = lastName;
            sPatientObj.gender = (sScheduledTestObj != null) ? sScheduledTestObj.Gender : "";
            sPatientObj.birthday = sScheduledTestObj.PatientDOB != null ? sScheduledTestObj.PatientDOB.Value.ToString("yyyy-MM-dd") : "";
            sPatientObj.species = (sScheduledTestObj != null) ? sScheduledTestObj.Species : "";
            sPatientObj.breed = "";

            sRequestAPI.patient = sPatientObj;

            VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPanelsObject sPanelObj = new VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPanelsObject();

            var panel = string.IsNullOrEmpty(sTestResults.TestResultType) ? "VCheck" : sTestResults.TestResultType;

            sPanelObj.code = sTestCode;
            sPanelObj.name = panel;
            sPanelObj.status = "F";
            sPanelObj.source = "";
            sPanelObj.resultdate = sTestResults.CreatedDate.Value.ToString("yyyy-MM-dd");

            var sDetailsObj = new List<txn_testresults_details>();
            ILiteCollection<txn_testresults_details_extended> sTestResultDetailsTemp = null;

            if (panel == "cCortisol" && sTestResultDetails.FirstOrDefault().TestParameter != "cCortisol")
            {
                using (var db = new LiteDatabase("Filename=Storage/TestResult.db;Password=Vch@ck123;"))
                {
                    sTestResultDetailsTemp = db.GetCollection<txn_testresults_details_extended>("txn_testresults_details_extended");
                    var sTestResultDetailsTempExtended = sTestResultDetailsTemp != null ? sTestResultDetailsTemp.Find(x => x.PatientID == sScheduledTestObj.PatientID && cCortisolParameter.Contains(x.TestParameter)).ToList() : null;

                    if (sTestResultDetailsTempExtended != null && sTestResultDetailsTempExtended.Any())
                    {
                        foreach (var detail in sTestResultDetailsTempExtended)
                        {
                            if(sTestResultDetails.FirstOrDefault(x => x.TestParameter == detail.TestParameter) == null)
                            {
                                sDetailsObj.Add(new txn_testresults_details
                                {
                                    TestParameter = detail.TestParameter,
                                    SubID = detail.SubID,
                                    ProceduralControl = detail.ProceduralControl,
                                    TestResultStatus = detail.TestResultStatus,
                                    TestResultValue = detail.TestResultValue,
                                    TestResultUnit = detail.TestResultUnit,
                                    ReferenceRange = detail.ReferenceRange,
                                    MeasuringRange = detail.MeasuringRange,
                                    Interpretation = detail.Interpretation
                                });
                            }
                        }
                    }
                }
            }                       

            sDetailsObj.AddRange(sTestResultDetails);
            if (sDetailsObj != null && sDetailsObj.Count > 0)
            {
                int count = 0;

                foreach (var d in sDetailsObj)
                {
                    String[] sRange = Array.Empty<string>();
                    string referenceLow = "";
                    string referenceHigh = "";



                    if (!string.IsNullOrEmpty(d.ReferenceRange))
                    {
                        String sReferenceRange = d.ReferenceRange.Replace("[", "").Replace("]", "");
                        if (sReferenceRange != "")
                        {
                            if (sReferenceRange.Contains(","))
                            {
                                var temp = sReferenceRange.Split(",");
                                string[] result = new string[2];
                                Array.Copy(temp, 0, result, 0, 1);
                                Array.Copy(temp, 2, result, 1, 1);
                                sRange = result;
                            }
                            else if (sReferenceRange.Contains(";"))
                            {
                                sRange = sReferenceRange.Split(";");
                            }
                            else if (sReferenceRange.Contains("-"))
                            {
                                sRange = sReferenceRange.Split("-");
                            }
                            else if (sReferenceRange.Contains("<"))
                            {
                                sRange = new string[] { sReferenceRange, "" };
                            }
                            else if (sReferenceRange.Contains(">"))
                            {
                                sRange = new string[] { "", sReferenceRange };
                            }

                            if (sRange.Length > 1)
                            {
                                referenceLow = (sRange.Length > 0) ? sRange[0] : "";
                                referenceHigh = (sRange.Length > 0) ? sRange[1] : "";
                            }
                        }
                    }
                    else if(!string.IsNullOrEmpty(d.MeasuringRange))
                    {
                        String sReferenceRange = d.MeasuringRange.Replace("[", "").Replace("]", "");
                        if (sReferenceRange != "")
                        {
                            if (sReferenceRange.Contains(";"))
                            {
                                sRange = sReferenceRange.Split(";");
                            }
                            else if (sReferenceRange.Contains("-"))
                            {
                                sRange = sReferenceRange.Split("-");
                            }

                            referenceLow = (sRange.Length > 0) ? sRange[0] : "";
                            referenceHigh = (sRange.Length > 0) ? sRange[1] : "";
                        }
                    }

                    var flags = "";

                    if (d.TestResultStatus == "Low") { flags = "L"; }
                    else if (d.TestResultStatus == "High") { flags = "H"; }

                    var Parameter = CheckParameter(d.TestParameter);

                    if (cCortisolParameter.Contains(Parameter))
                    {
                        foreach (var parameter in cCortisolParameter)
                        {
                            if(parameter == Parameter) { break; }
                            
                            count++;
                        }

                        using (var db = new LiteDatabase("Filename=Storage/TestResult.db;Password=Vch@ck123;"))
                        {
                            sTestResultDetailsTemp = db.GetCollection<txn_testresults_details_extended>("txn_testresults_details_extended");

                            if (endParameter.Contains(Parameter) && sTestResultDetailsTemp != null)
                            {
                                sTestResultDetailsTemp.DeleteMany(x => x.PatientID == sScheduledTestObj.PatientID && cCortisolParameter.Contains(x.TestParameter));
                            }
                            else
                            {
                                var sTestResultDetailsTempExtended = sTestResultDetailsTemp != null ? sTestResultDetailsTemp.FindOne(x => x.PatientID == sScheduledTestObj.PatientID && x.TestParameter == Parameter) : null;

                                if (sTestResultDetailsTempExtended == null)
                                {
                                    sTestResultDetailsTemp.Insert(new txn_testresults_details_extended
                                    {
                                        TestParameter = Parameter,
                                        SubID = d.SubID,
                                        ProceduralControl = d.ProceduralControl,
                                        TestResultStatus = d.TestResultStatus,
                                        TestResultValue = d.TestResultValue,
                                        TestResultUnit = d.TestResultUnit,
                                        ReferenceRange = d.ReferenceRange,
                                        MeasuringRange = d.MeasuringRange,
                                        Interpretation = d.Interpretation,
                                        PatientID = sScheduledTestObj.PatientID
                                    });
                                }
                            }
                        }
                    }

                    sResultListing.Add(new VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPanelTestObject
                    {
                        name = Parameter,
                        code = sTestCode + "-" + count++,
                        result = d.TestResultValue,
                        referencelow = referenceLow,
                        referencehigh = referenceHigh,
                        unitofmeasure = d.TestResultUnit,
                        nature = d.TestResultStatus == "Normal" | d.TestResultStatus == "Positive" ? "N" : "A",
                        flags = flags,
                        status = "F",
                        notes = d.Interpretation
                    });
                }

                sPanelObj.tests = sResultListing;
            }
            sPanelListing.Add(sPanelObj);

            sRequestAPI.panels = sPanelListing;

            GetInterfaceInfo();

            var sRespAPI = false;
            VCheckAPI VcheckAPI = new VCheckAPI();

            if (PMS == "Greywind")
            {
                GreywindAPI sAPI = new GreywindAPI();
                url = await VcheckAPI.GetPMSUrl(2);
                sRespAPI = sAPI.UpdateResult(sRequestAPI, sOrderID, url);
            }
            else
            {
                if (isIP)
                {
                    sRespAPI = await SendData(sRequestAPI, iIP, int.Parse(port));
                }
                else
                {
                    url = port == "80" ? url : url + ":" + port;

                    GeneralAPI sAPI = new GeneralAPI();
                    sRespAPI = await sAPI.SendData(url, sRequestAPI, username, password);
                }
            }

            if (sRespAPI)
            {
                return true;
            }

            return false;

        }

        public async Task<Boolean> SendData(VCheck.Interface.API.Greywind.RequestMessage.UpdateResultRequest sResultRequest, System.Net.IPAddress ip, int port)
        {
            String sContent = JsonConvert.SerializeObject(sResultRequest);
            int timeout = 5000;

            try
            {
                using (var cts = new CancellationTokenSource())
                {
                    cts.CancelAfter(timeout);

                    try
                    {

                        ourTcpClient = new TcpClient();
                        await ourTcpClient.ConnectAsync(ip, port).WaitAsync(cts.Token);

                        //get the IO stream on this connection to write to
                        networkStream = ourTcpClient.GetStream();
                        var sendMessageByteBuffer = Encoding.UTF8.GetBytes(sContent);

                        if (networkStream.CanWrite)
                        {
                            networkStream.Write(sendMessageByteBuffer, 0, sendMessageByteBuffer.Length);
                            byte[] receiveMessageByteBuffer = new byte[ourTcpClient.ReceiveBufferSize];
                            var bytesReceivedFromServer = await networkStream.ReadAsync(receiveMessageByteBuffer, 0, receiveMessageByteBuffer.Length).WaitAsync(cts.Token);

                            if (bytesReceivedFromServer > 0 && networkStream.CanRead)
                            {
                                receivedMessage = Encoding.UTF8.GetString(receiveMessageByteBuffer);

                                var message = receivedMessage.Replace("\0", string.Empty);

                                return true;
                            }
                        }

                        return false;

                    }
                    catch (TaskCanceledException cancelled)
                    {
                        return false;
                    }
                    catch (Exception ex)
                    {

                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                //close the IO strem and the TCP connection
                networkStream?.Close();
                ourTcpClient?.Close();
            }
        }

        private void GetInterfaceInfo()
        {
            var listenerConfig = App.db.GetCollection<ListenerConfig>("ListenerConfig");

            var configuration = listenerConfig.FindOne(x => x.Id == 1);

            if(configuration != null)
            {
                PMS = configuration.ConversionChart;
                url = configuration.PIMSIP;
                isIP = System.Net.IPAddress.TryParse(configuration.PIMSIP, out iIP);
                port = !string.IsNullOrEmpty(configuration.PIMSPort) ? configuration.PIMSPort : "80";

                username = "";
                password = "";
            }
        }


        public static string CheckParameter(string input)
        {
            foreach (var item in calItems)
            {
                if (input.ToLower().Contains(item.ToLower())) { return item + "*"; }
            }

            return ConvertSubscriptsToNormal(input);
        }

        public static string ConvertSubscriptsToNormal(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            StringBuilder result = new StringBuilder(input.Length);

            foreach (char c in input)
            {
                int subindex = subscriptDigits.IndexOf(c);
                int superindex = superscriptDigits.IndexOf(c);
                if (subindex >= 0 || superindex >= 0)
                {
                    result.Append(subindex >= 0 ? normalDigits[subindex] : normalDigits[superindex]); // Replace with normal digit
                    break;
                }
                else
                    result.Append(c); // Keep unchanged
            }

            return result.ToString();
        }
    }
}
