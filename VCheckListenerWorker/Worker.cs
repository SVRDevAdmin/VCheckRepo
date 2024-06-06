using Mysqlx.Crud;
using NHapiTools.Model.V26.Segment;
using System.Reflection;
using System.IO;
using VCheckListenerWorker.Lib.Logic;
using VCheckListenerWorker.Lib.Models;
using log4net.Config;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using NHapi.Model.V23.Segment;
using Org.BouncyCastle.Asn1;

namespace VCheckListenerWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        System.Net.Sockets.Socket sListener;
        VCheckListenerWorker.Lib.Util.Logger sLogger;

        public String sSystemName = "VCheckViewer Listener";

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Main Logic process the data
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            XmlConfigurator.Configure(log4net.LogManager.GetRepository(Assembly.GetEntryAssembly()),
                                      new FileInfo("log4Net.config"));
            sLogger = new VCheckListenerWorker.Lib.Util.Logger();

            var configBuilder = Host.CreateApplicationBuilder();

            while (!stoppingToken.IsCancellationRequested)
            {
                while (true)
                {
                    System.Net.Sockets.Socket sClient = sListener.Accept();
                    Console.WriteLine("Connection Accepted.");

                    byte[] bBuffer = new byte[4096];

                    var childSocket = new Thread(() =>
                    {
                        int s = sClient.Receive(bBuffer);
                        Console.WriteLine("Received Data.");

                        String sData = System.Text.Encoding.ASCII.GetString(bBuffer, 0, s);
                        sData = sData.Replace("\u001c", "")
                                     .Replace("\n", "\r");

                        if (!String.IsNullOrEmpty(sData))
                        {
                            Console.WriteLine(sData);
   
                            NHapi.Base.Parser.PipeParser sParser = new NHapi.Base.Parser.PipeParser();
                            NHapi.Base.Model.IMessage sIMessage = sParser.Parse(sData.Trim());

                            NHapi.Model.V26.Message.ORU_R01 sRU_R01 = (NHapi.Model.V26.Message.ORU_R01)sIMessage;
                            NHapi.Base.Parser.XMLParser sXMLParser = new NHapi.Base.Parser.DefaultXMLParser();
                            String sXMLMessage = sXMLParser.Encode(sIMessage);
                            Console.WriteLine(sXMLMessage);

                            // Populate Acknowledge message & Send back
                            String sAckMessage = Lib.Util.ResponseRepo.CreateResponseMessage(sRU_R01);
                            var sMessageByte = System.Text.Encoding.UTF8.GetBytes(sAckMessage);
                            sClient.SendAsync(sMessageByte, System.Net.Sockets.SocketFlags.None);

                            String sFileName = "TestResult_" + DateTime.Now.ToString("yyyyMMddHHmmss");

                            // Ouput Message to file
                            OutputMessage(configBuilder, sFileName, sData, sXMLMessage, sAckMessage);

                            // Save raw data
                            ProcessHL7Message(sIMessage);

                            Console.WriteLine("---------------------------------------------------------------------------------");
                        }

                        sClient.Close();
                    });

                    childSocket.Start();
                }
            }
        }

        /// <summary>
        /// When program start execution
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine("Start Listener connection");

                var builder = Host.CreateApplicationBuilder();
                String sHostIP = builder.Configuration.GetSection("Listener:HostIP").Value;
                int iPortNo = Convert.ToInt32(builder.Configuration.GetSection("Listener:Port").Value);

                System.Net.IPEndPoint sIPEndPoint = System.Net.IPEndPoint.Parse(String.Concat(sHostIP, ":", iPortNo));

                sListener = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
                                                                                    System.Net.Sockets.SocketType.Stream,
                                                                                    System.Net.Sockets.ProtocolType.Tcp);
                sListener.Bind(sIPEndPoint);
                sListener.Listen(3);

                Console.WriteLine("Listener Start Successful.");
                Console.WriteLine("IP Address : " + sHostIP);
                Console.WriteLine("Port No : " + iPortNo);
                Console.WriteLine("-------------------------------------------------------------------");

                return base.StartAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("StartAsync >>> " + ex.ToString());
                return base.StopAsync(cancellationToken);
            }           
        }

        /// <summary>
        /// When program stopped
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine("Initiated Stop Listener connection.");

                sListener.Disconnect(true);
                sListener.Dispose();

                //_logger.LogInformation("Listener connection closed.");
                Console.WriteLine("Listener connection closed.");

            }
            catch (Exception ex)
            {
                _logger.LogError("StopAsync >>>> " + ex.ToString());
                sLogger.Error("StopAsync >>>> " + ex.ToString());
            }

            return base.StopAsync(cancellationToken);
        }

        /// <summary>
        /// Process data and save to DB
        /// </summary>
        /// <param name="sIMessage"></param>
        public void ProcessHL7Message(NHapi.Base.Model.IMessage sIMessage)
        {
            try
            {
                NHapi.Model.V26.Message.ORU_R01 sRU_R01 = (NHapi.Model.V26.Message.ORU_R01)sIMessage;
                String sResultRule = "";
                String sResultStatus = "";
                String sResultTestType = "";
                String sOperatorID = "";
                String sPatientID = "";
                String strObserveValue = "";
                Decimal iResultValue = 0;
                DateTime dAnalysisDateTime = DateTime.MinValue;

                // --------------- Message Header --------------//
                tbltestanalyze_results_messageheader sMSHObj = new tbltestanalyze_results_messageheader
                {
                    FieldSeparator = sRU_R01.MSH.FieldSeparator.Value,
                    EncodingCharacters = sRU_R01.MSH.EncodingCharacters.Value,
                    SendingApplication = sRU_R01.MSH.SendingApplication.NamespaceID.Value.Trim() + "^" +
                                         sRU_R01.MSH.SendingApplication.UniversalID.Value.Trim() + "^" +
                                         sRU_R01.MSH.SendingApplication.UniversalIDType.Value.Trim(),
                    SendingFacility = sRU_R01.MSH.SendingFacility.NamespaceID.Value,
                    ReceivingApplication = sRU_R01.MSH.ReceivingApplication.NamespaceID.Value,
                    ReceivingFacility = sRU_R01.MSH.ReceivingFacility.NamespaceID.Value,
                    DateTimeMessage = sRU_R01.MSH.DateTimeOfMessage.Value,
                    MessageType = sRU_R01.MSH.MessageType.MessageCode.Value + "^" +
                                  sRU_R01.MSH.MessageType.TriggerEvent.Value + "^" +
                                  sRU_R01.MSH.MessageType.MessageStructure.Value,
                    MessageControlID = sRU_R01.MSH.MessageControlID.Value.ToString(),
                    ProcessingID = sRU_R01.MSH.ProcessingID.ProcessingID.Value,
                    VersionID = sRU_R01.MSH.VersionID.VersionID.Value,
                    AcceptAckmgtType = sRU_R01.MSH.AcceptAcknowledgmentType.Value,
                    AppAckmgtType = sRU_R01.MSH.ApplicationAcknowledgmentType.Value,
                    CountryCode = sRU_R01.MSH.CountryCode.Value,
                    CharacterSet = (sRU_R01.MSH.GetCharacterSet().Length > 0) ? sRU_R01.MSH.GetCharacterSet().FirstOrDefault().Value : null,
                    PrincipalLanguageMsg = sRU_R01.MSH.PrincipalLanguageOfMessage.Identifier.Value + "^" +
                                           sRU_R01.MSH.PrincipalLanguageOfMessage.Text.Value + "^" +
                                           sRU_R01.MSH.PrincipalLanguageOfMessage.NameOfCodingSystem.Value,
                    MessageProfileIdentifier = (sRU_R01.MSH.GetMessageProfileIdentifier().Length > 0) ?
                                               sRU_R01.MSH.GetMessageProfileIdentifier().FirstOrDefault().EntityIdentifier.Value + "^" +
                                               sRU_R01.MSH.GetMessageProfileIdentifier().FirstOrDefault().NamespaceID.Value + "^" +
                                               sRU_R01.MSH.GetMessageProfileIdentifier().FirstOrDefault().UniversalID.Value + "^" +
                                               sRU_R01.MSH.GetMessageProfileIdentifier().FirstOrDefault().UniversalIDType.Value : null
                };

                // ------------ Patient Identification --------------------//
                List<tbltestanalyze_results_patientidentification> sPIDObj = new List<tbltestanalyze_results_patientidentification>();

                tbltestanalyze_results_patientidentification sPID = new tbltestanalyze_results_patientidentification();
                if (sRU_R01.GetPATIENT_RESULT().PATIENT.PID.PatientIdentifierListRepetitionsUsed > 0)
                {
                    sPID.SetID = sRU_R01.GetPATIENT_RESULT().PATIENT.PID.SetIDPID.Value;
                    sPID.PatientID = sRU_R01.GetPATIENT_RESULT().PATIENT.PID.PatientID.IDNumber.Value;
                    sPID.AlternatePatientID = (sRU_R01.GetPATIENT_RESULT().PATIENT.PID.GetAlternatePatientIDPID().Length > 0) ?
                                               sRU_R01.GetPATIENT_RESULT().PATIENT.PID.GetAlternatePatientIDPID().FirstOrDefault().IDNumber.ToString() : null;

                    sOperatorID = sPID.PatientID;
                }

                if (sRU_R01.GetPATIENT_RESULT().PATIENT.PID.PatientNameRepetitionsUsed > 0)
                {
                    var sNameObj = sRU_R01.GetPATIENT_RESULT().PATIENT.PID.GetPatientName().FirstOrDefault();

                    if (sNameObj != null)
                    {
                        sPID.PatientName = "" + "^" +
                                           sNameObj.GivenName + "^" +
                                           sNameObj.SecondAndFurtherGivenNamesOrInitialsThereof + "^" +
                                           sNameObj.SuffixEgJRorIII + "^" +
                                           sNameObj.PrefixEgDR + "^" +
                                           sNameObj.DegreeEgMD + "^" +
                                           sNameObj.NameTypeCode;

                        if (sPID.PatientName.Replace("^", "").Length == 0)
                        {
                            sPID.PatientName = "";
                        }
                    }
                    else
                    {
                        sPID.PatientName = "";
                    }
                }
                sPIDObj.Add(sPID);

                //----------------- Observation Request ----------------------//
                var sNTEObj = new List<tbltestanalyze_results_notes>();
                var sOBXObjList = new List<tbltestanalyze_results_observationresult>();
                var sOBRObj = new tbltestanalyze_results_observationrequest();
                foreach (var observation in sRU_R01.PATIENT_RESULTs.FirstOrDefault().ORDER_OBSERVATIONs)
                {
                    sResultTestType = observation.OBR.UniversalServiceIdentifier.Text.Value;

                    sOBRObj.SetID = observation.OBR.SetIDOBR.Value;
                    sOBRObj.PlacerOrderNumber = observation.OBR.PlacerOrderNumber.EntityIdentifier.Value + "^" +
                                                observation.OBR.PlacerOrderNumber.NamespaceID.Value + "^" +
                                                observation.OBR.PlacerOrderNumber.UniversalID.Value + "^" +
                                                observation.OBR.PlacerOrderNumber.UniversalIDType.Value;
                    sOBRObj.FillerOrderNumber = observation.OBR.FillerOrderNumber.EntityIdentifier.Value + "^" +
                                                observation.OBR.FillerOrderNumber.NamespaceID.Value + "^" +
                                                observation.OBR.FillerOrderNumber.UniversalID.Value + "^" +
                                                observation.OBR.FillerOrderNumber.UniversalIDType.Value;
                    sOBRObj.UniversalServIdentifier = observation.OBR.UniversalServiceIdentifier.Identifier.Value + "^" +
                                                      observation.OBR.UniversalServiceIdentifier.Text.Value + "^" +
                                                      observation.OBR.UniversalServiceIdentifier.NameOfCodingSystem.Value;
                    sOBRObj.Priority = observation.OBR.Priority.Value;
                    sOBRObj.RequestedDateTime = observation.OBR.RequestedDateTime.Value;
                    sOBRObj.ObservationDateTime = observation.OBR.ObservationDateTime.Value.Trim();
                    sOBRObj.ObservationEndDateTime = observation.OBR.ObservationEndDateTime.Value.Trim();
                    sOBRObj.CollectVolume = observation.OBR.CollectionVolume.Quantity.Value;
                    sOBRObj.CollectorIdentifier = (observation.OBR.GetCollectorIdentifier().Count() > 0) ?
                                                   observation.OBR.GetCollectorIdentifier().FirstOrDefault().IDNumber.Value : null;
                    sOBRObj.SpecimenActionCode = observation.OBR.SpecimenActionCode.Value;

                    if (observation.NTEs.Count() > 0)
                    {
                        sNTEObj.Add(new tbltestanalyze_results_notes
                        {
                            SetID = (observation.NTEs.Count() > 0) ?
                                    observation.NTEs.FirstOrDefault().SetIDNTE.Value : null,
                            Segment = "OBR",
                            SourceComment = (observation.NTEs.Count() > 0) ?
                                             observation.NTEs.FirstOrDefault().SourceOfComment.Value : null,
                            Comment = GenerateNTEComments(observation.NTEs.ToList())
                        });
                    }

                    // --------------- Observation Results ----------------//
                    foreach (var observationDetail in observation.OBSERVATIONs)
                    {
                        String sObservValue = "";
                        strObserveValue = "";
                        if (observationDetail.OBX.GetObservationValue().Count() > 0)
                        {
                            if (observationDetail.OBX.GetObservationValue().FirstOrDefault().Data.GetType() == typeof(NHapi.Model.V26.Datatype.NA))
                            {
                                var sNAObject = observationDetail.OBX.GetObservationValue().FirstOrDefault().Data;

                                int iTotalComponent = sNAObject.ExtraComponents.NumComponents();
                                List<String> sVal = new List<String>();

                                // --- Get from Component ------ //
                                PropertyInfo[] props = sNAObject.GetType().GetProperties();
                                foreach (PropertyInfo p in props)
                                {
                                    if (p.PropertyType == typeof(NHapi.Model.V26.Datatype.NM))
                                    {
                                        sVal.Add(p.GetValue(sNAObject, null).ToString());
                                    };
                                }

                                // -- Get From Extra Component -----//
                                for (int i = 0; i < iTotalComponent; i++)
                                {
                                    if (sNAObject.ExtraComponents.GetComponent(i).Data.ToString() != null)
                                    {
                                        sVal.Add(sNAObject.ExtraComponents.GetComponent(i).Data.ToString());
                                    }
                                }

                                if (sVal.Count() > 0)
                                {
                                    sObservValue = String.Join("^", sVal);
                                }
                            }
                            else
                            {
                                if (observationDetail.OBX.GetObservationValue().FirstOrDefault().Data.GetType() == typeof(NHapi.Model.V26.Datatype.CWE))
                                {
                                    var sCWEObject = observationDetail.OBX.GetObservationValue().FirstOrDefault().Data;

                                    List<String> sCWEVal = new List<String>();
                                    PropertyInfo[] propCWE = sCWEObject.GetType().GetProperties();
                                    foreach (PropertyInfo c in propCWE)
                                    {
                                        if (c.PropertyType == typeof(NHapi.Base.Model.IType[]))
                                        {
                                            NHapi.Base.Model.IType[] iTypeObj = (NHapi.Base.Model.IType[])c.GetValue(sCWEObject, null);
                                            for (int i = 0; i < 4; i++)
                                            {
                                                sCWEVal.Add(iTypeObj[i].ToString());
                                            }
                                        }
                                    }

                                    if (sCWEVal.Count() > 0)
                                    {
                                        sObservValue = String.Join("^", sCWEVal);
                                    }
                                }
                                else
                                {
                                    sObservValue = observationDetail.OBX.GetObservationValue().FirstOrDefault().Data.ToString();
                                }

                            }

                            strObserveValue = sObservValue;
                        }
                        else
                        {
                            if (observationDetail.OBX.ObservationResultStatus != null)
                            {
                                if (observationDetail.OBX.ObservationResultStatus.Value == "F")
                                {
                                    strObserveValue = "Valid";
                                }
                                if (observationDetail.OBX.ObservationResultStatus.Value == "X")
                                {
                                    strObserveValue = "Invalid";
                                }
                            }
                        }

                        String sUnitValue = "";
                        sUnitValue = observationDetail.OBX.Units.Identifier.Value + "^" +
                                observationDetail.OBX.Units.Text.Value + "^" +
                                observationDetail.OBX.Units.NameOfCodingSystem;

                        if (sUnitValue.Replace("^", "").Length == 0)
                        {
                            sUnitValue = "";
                        }

                        String sObservIdentifier = "";
                        sObservIdentifier = observationDetail.OBX.ObservationIdentifier.Identifier.Value + "^" +
                                            observationDetail.OBX.ObservationIdentifier.Text.Value + "^" +
                                            observationDetail.OBX.ObservationIdentifier.NameOfCodingSystem.Value + "^" +
                                            observationDetail.OBX.ObservationIdentifier.AlternateIdentifier.Value;
                        if (sObservIdentifier.Replace("^", "").Length == 0)
                        {
                            sObservIdentifier = "";
                        }

                        if (observationDetail.OBX.GetResponsibleObserver().Length > 0)
                        {
                            sOperatorID = observationDetail.OBX.GetResponsibleObserver().FirstOrDefault().IDNumber.Value;
                        }
                        else
                        {
                            sOperatorID = null;
                        }

                        if (!String.IsNullOrEmpty(observationDetail.OBX.DateTimeOfTheAnalysis.Value))
                        {
                            //dAnalysisDateTime = DateTime.ParseExact(observationDetail.OBX.DateTimeOfTheAnalysis.Value, "yyyyMMddHHmmss-ZZZZ", System.Globalization.CultureInfo.InvariantCulture);
                            if (observationDetail.OBX.DateTimeOfTheAnalysis.Value.Length == 14)
                            {
                                dAnalysisDateTime = DateTime.ParseExact(observationDetail.OBX.DateTimeOfTheAnalysis.Value, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                dAnalysisDateTime = DateTime.ParseExact(observationDetail.OBX.DateTimeOfTheAnalysis.Value, "yyyyMMddHHmmss-ffff", System.Globalization.CultureInfo.InvariantCulture);
                            }
                        }

                        sOBXObjList.Add(new tbltestanalyze_results_observationresult
                        {
                            SetID = observationDetail.OBX.SetIDOBX.Value,
                            ValueType = observationDetail.OBX.ValueType.Value,
                            ObservationIdentifier = sObservIdentifier,
                            ObservationSubID = observationDetail.OBX.ObservationSubID.Value,
                            ObservationValue = sObservValue,
                            Units = sUnitValue,
                            ReferencesRange = observationDetail.OBX.ReferencesRange.Value,
                            AbnormalFlag = (observationDetail.OBX.GetAbnormalFlags().Length > 0) ?
                                           observationDetail.OBX.GetAbnormalFlags().FirstOrDefault().Value : null,
                            ObservationResultStatus = observationDetail.OBX.ObservationResultStatus.Value,
                            ObservationDateTime = observationDetail.OBX.DateTimeOfTheObservation.Value,
                            ProducerID = observationDetail.OBX.ProducerSID.Text.Value,
                            ResponsibleObserver = sOperatorID,
                            ObservationMethod = (observationDetail.OBX.GetObservationMethod().Length > 0) ?
                                                 observationDetail.OBX.GetObservationMethod().FirstOrDefault().Text.ToString() : null,
                            EquipmentInstanceIdentifier = (observationDetail.OBX.GetEquipmentInstanceIdentifier().Length > 0) ?
                                                          observationDetail.OBX.GetEquipmentInstanceIdentifier().FirstOrDefault().EntityIdentifier.Value : null,
                            AnalysisDateTime = observationDetail.OBX.DateTimeOfTheAnalysis.Value

                        });

                        // ------------ Notes -------------------//
                        if (observationDetail.NTEs.Count() > 0)
                        {
                            String sComment = GenerateNTEComments(observationDetail.NTEs.ToList());

                            sNTEObj.Add(new tbltestanalyze_results_notes
                            {
                                SetID = (observationDetail.NTEs.Count() > 0) ?
                                        observationDetail.NTEs.FirstOrDefault().SetIDNTE.Value.Trim() : null,
                                Segment = "OBX",
                                SourceComment = (observationDetail.NTEs.Count() > 0) ?
                                                 observationDetail.NTEs.FirstOrDefault().SourceOfComment.Value : null,
                                Comment = sComment
                                //Comment = GenerateNTEComments(observationDetail.NTEs.ToList())
                            });

                            if (sComment.ToLower().Contains("cut off index"))
                            {
                                sResultRule = "COI";

                                String sValue = "";
                                String[] strArryValue = sComment.Split(",");
                                if (strArryValue.Length > 0)
                                {
                                    sValue = strArryValue[1].Replace("Value=", "").Trim();
                                }

                                Decimal.TryParse(sValue, out iResultValue);
                            }
                        }

                    }
                }

                txn_testresults sTestResultObj = new txn_testresults();
                sTestResultObj.TestResultDateTime = dAnalysisDateTime;
                sTestResultObj.TestResultType = sResultTestType;
                sTestResultObj.OperatorID = sOperatorID;
                sTestResultObj.PatientID = sPatientID;
                sTestResultObj.InchargePerson = "";
                sTestResultObj.ObservationStatus = strObserveValue;
                sTestResultObj.TestResultStatus = (iResultValue >= 1 ? "Positive" : "Negative");
                sTestResultObj.TestResultValue = iResultValue;
                sTestResultObj.TestResultRules = sResultRule;
                sTestResultObj.CreatedDate = DateTime.Now;
                sTestResultObj.CreatedBy = sSystemName;

                tbltestanalyze_results sResultObj = new tbltestanalyze_results
                {
                    MessageType = sRU_R01.MSH.MessageType.MessageStructure.Value,
                    MessageDateTime = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CreatedBy = sSystemName
                };

                Boolean bResult = TestResultRepository.insertTestObservationMessage(sResultObj, sMSHObj, sPIDObj, sOBRObj, sOBXObjList, sNTEObj);
                if (bResult)
                {
                    // Insert into Test Result table & create notification 
                    TestResultRepository.createTestResult(sTestResultObj);
                    SendNotification(sTestResultObj);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Function ProcessHL7Message >>> " + ex.ToString());
            }

        }

        /// <summary>
        /// Insert Notification
        /// </summary>
        /// <param name="sResult"></param>
        public void SendNotification(txn_testresults sResult)
        {
            try
            {
                String sNotificationContent = "";

                var sTemplateObj = TestResultRepository.GetNotificationTemplate("TR01");
                if (sTemplateObj != null)
                {
                    sNotificationContent = sTemplateObj.TemplateContent;
                }

                sNotificationContent = sNotificationContent.Replace("###<patient_id>###", sResult.PatientID);

                txn_notification sNotificationSend = new txn_notification()
                {
                    NotificationType = "Completed Test Results",
                    NotificationTitle = (sTemplateObj != null) ? sTemplateObj.TemplateTitle : "",
                    NotificationContent = sNotificationContent,
                    CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    CreatedBy = sSystemName
                };

                TestResultRepository.insertNotification(sNotificationSend);
            }
            catch (Exception ex)
            {
                _logger.LogError("Function SendNotification >>> " + ex.ToString());
            }

        }

        /// <summary>
        /// Output data to file
        /// </summary>
        /// <param name="sBuilder"></param>
        /// <param name="sFileName"></param>
        /// <param name="sData"></param>
        /// <param name="sXMLMessage"></param>
        /// <param name="sAckMessage"></param>
        public void OutputMessage(HostApplicationBuilder sBuilder, String sFileName, String sData, String sXMLMessage, String sAckMessage)
        {
            try
            {
                String sOutputPathHL7 = sBuilder.Configuration.GetSection("FileOutput:HL7").Value;
                String sOutputPathXML = sBuilder.Configuration.GetSection("FileOutput:XML").Value;
                String sOutputPathACK = sBuilder.Configuration.GetSection("FileOutput:ACK").Value;

                if (!String.IsNullOrEmpty(sOutputPathHL7))
                {
                    String outputPathHL7 = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), sOutputPathHL7);
                    if (!Directory.Exists(outputPathHL7))
                    {
                        Directory.CreateDirectory(outputPathHL7);
                    }
                    File.WriteAllText(outputPathHL7 + sFileName + ".hl7", sData, System.Text.Encoding.ASCII);
                }

                // Output to XML file
                if (!String.IsNullOrEmpty(sOutputPathXML))
                {
                    String outputPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), sOutputPathXML);
                    if (!Directory.Exists(outputPath))
                    {
                        Directory.CreateDirectory(outputPath);
                    }
                    File.WriteAllText(outputPath + sFileName + ".xml", sXMLMessage, System.Text.Encoding.ASCII);
                }

                if (!String.IsNullOrEmpty(sOutputPathACK))
                {
                    String outputPathACK = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), sOutputPathACK);
                    if (!Directory.Exists(outputPathACK))
                    {
                        Directory.CreateDirectory(outputPathACK);
                    }
                    File.WriteAllText(outputPathACK + sFileName + ".hl7", sAckMessage, System.Text.Encoding.ASCII);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Function OutputMessage >>> " + ex.ToString());
            }

        }

        /// <summary>
        /// Get NTE Comment data 
        /// </summary>
        /// <param name="nte"></param>
        /// <returns></returns>
        public String GenerateNTEComments(List<NHapi.Model.V26.Segment.NTE> nte)
        {
            string nteComment = "";
            try
            {
                for (int i = 0; i <= nte.Count() - 1; i++)
                {
                    if (nte[i].GetComment() != null && nte[i].GetComment().FirstOrDefault() != null && nte[i].GetComment().FirstOrDefault().Value != null)
                    {
                        if (nteComment != "")
                        {
                            nteComment += ", ";

                        }
                        nteComment += nte[i].GetComment().FirstOrDefault().Value;
                    }
                }
            }
            catch (Exception ex)
            {
                sLogger.Error("GenerateNTEComments >>> " + ex.ToString());
            }

            return nteComment;
        }
    }
}
