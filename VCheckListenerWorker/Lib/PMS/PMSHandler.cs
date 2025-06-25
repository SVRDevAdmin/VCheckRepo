using Mysqlx.Crud;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VCheck.Interface.API;
using VCheckListenerWorker.Lib.Logic;
using VCheckListenerWorker.Lib.Models;

namespace VCheckListenerWorker.Lib.PMS
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

        public async Task<bool> SendToPMS(txn_testresults sTestResults, List<txn_testresults_details> sTestResultDetails, ScheduledTestModel sScheduledTestObj)
        {
            VCheck.Interface.API.Greywind.RequestMessage.UpdateResultRequest sRequestAPI = new VCheck.Interface.API.Greywind.RequestMessage.UpdateResultRequest();

            List<VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPanelTestObject> sResultListing = new List<VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPanelTestObject>();
            List<VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPanelsObject> sPanelListing = new List<VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPanelsObject>();

            String sOrderID = "";

            if (sScheduledTestObj.ScheduleUniqueID.Contains("-"))
            {
                var UniqueIDSplit = sScheduledTestObj.ScheduleUniqueID.Split("-");
                if (UniqueIDSplit.Length > 0)
                {
                    sOrderID = UniqueIDSplit[1];
                    sRequestAPI.accessionnumber = UniqueIDSplit[2];
                }
            }

            //sRequestAPI.accessionnumber = iTestResultID.ToString();
            sRequestAPI.clinic_id = sScheduledTestObj.LocationID.ToString();
            sRequestAPI.reportdate = sTestResults.CreatedDate.Value.ToString("yyyy-MM-dd");
            sRequestAPI.providerid = "";

            VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPatientObject sPatientObj = new VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPatientObject();
            sPatientObj.patientid = sScheduledTestObj.PatientID;
            sPatientObj.firstname = (sScheduledTestObj != null) ? sScheduledTestObj.PatientName : "";
            sPatientObj.lastname = "";
            sPatientObj.gender = (sScheduledTestObj != null) ? sScheduledTestObj.Gender : "";
            sPatientObj.birthday = "2023-01-01";
            sPatientObj.species = (sScheduledTestObj != null) ? sScheduledTestObj.Species : "";
            sPatientObj.breed = "";

            sRequestAPI.patient = sPatientObj;

            VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPanelsObject sPanelObj = new VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPanelsObject();

            var panel = string.IsNullOrEmpty(sTestResults.TestResultType) ? "VCheck" : sTestResults.TestResultType;

            sPanelObj.code = panel;
            sPanelObj.name = panel;
            sPanelObj.status = "F";
            sPanelObj.source = "";
            sPanelObj.resultdate = sTestResults.CreatedDate.Value.ToString("yyyy-MM-dd");


            var sDetailsObj = sTestResultDetails;
            if (sDetailsObj != null && sDetailsObj.Count > 0)
            {
                foreach (var d in sDetailsObj)
                {
                    String[] sRange = Array.Empty<string>();
                    if (d.ReferenceRange != null)
                    {
                        String sReferenceRange = d.ReferenceRange.Replace("[", "").Replace("]", "");
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
                        }
                    }

                    sResultListing.Add(new VCheck.Interface.API.Greywind.RequestMessage.UpdateResultPanelTestObject
                    {
                        name = d.TestParameter,
                        code = d.TestParameter,
                        result = d.TestResultValue,
                        referencelow = (sRange.Length > 0) ? sRange[0] : "",
                        referencehigh = (sRange.Length > 0) ? sRange[1] : "",
                        unitofmeasure = d.TestResultUnit,
                        status = "F",
                        notes = ""
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
                sTestResults.PMSFunction = "Collapsed";

                TestResultRepository.createTestResult(sTestResults);
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
            var configuration = TestResultRepository.GetAllConfiguration();

            var specificConfiguration = configuration.FirstOrDefault(x => x.ConfigurationKey == "InterfaceSettingsPMS");

            PMS = specificConfiguration != null ? specificConfiguration.ConfigurationValue : "";

            specificConfiguration = configuration.FirstOrDefault(x => x.ConfigurationKey == "InterfaceSettingsIP");
            url = specificConfiguration != null ? specificConfiguration.ConfigurationValue : "";

            isIP = System.Net.IPAddress.TryParse(url, out iIP);

            specificConfiguration = configuration.FirstOrDefault(x => x.ConfigurationKey == "InterfaceSettingsPortNo");
            port = specificConfiguration != null ? specificConfiguration.ConfigurationValue : "80";

            specificConfiguration = configuration.FirstOrDefault(x => x.ConfigurationKey == "InterfaceSettingsUsername");
            username = specificConfiguration != null ? specificConfiguration.ConfigurationValue : "";

            specificConfiguration = configuration.FirstOrDefault(x => x.ConfigurationKey == "InterfaceSettingsPassword");
            password = specificConfiguration != null ? specificConfiguration.ConfigurationValue : "";
        }
    }
}
