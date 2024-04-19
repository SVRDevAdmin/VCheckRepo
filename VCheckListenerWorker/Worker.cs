using Mysqlx.Crud;
using NHapiTools.Model.V26.Segment;
using System.Reflection;
using System.IO;
using VCheckListenerWorker.Lib.Logic;
using VCheckListenerWorker.Lib.Models;

namespace VCheckListenerWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        System.Net.Sockets.Socket sListener;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var configBuilder = Host.CreateApplicationBuilder();
            String sOutputPathHL7 = configBuilder.Configuration.GetSection("FileOutput:HL7").Value;
            String sOutputPathXML = configBuilder.Configuration.GetSection("FileOutput:XML").Value;

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


                            //String sAckMessage = ListenerSampleProgra_.Utils.CreateResponseMessage(sRU_R01, sRU_R01.MSH.MessageControlID.Value);
                            String sAckMessage = Lib.Util.ResponseRepo.CreateResponseMessage(sRU_R01);
                            var sMessageByte = System.Text.Encoding.UTF8.GetBytes(sAckMessage);
                            sClient.SendAsync(sMessageByte, System.Net.Sockets.SocketFlags.None);

                            String sFileName = "TestResult_" + DateTime.Now.ToString("yyyyMMddHHmmss");

                            // Output to HL7 file
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

                            // Save raw data
                            ProcessHL7Message(sIMessage);


                            Console.WriteLine();
                        }

                        sClient.Close();
                    });

                    childSocket.Start();
                }
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Test 1");

            var builder = Host.CreateApplicationBuilder();
            String sHostIP = builder.Configuration.GetSection("Listener:HostIP").Value;
            int iPortNo = Convert.ToInt32(builder.Configuration.GetSection("Listener:Port").Value); 

            //IPAddress sIPAddress = IPAddress.Parse(strIP);
            System.Net.IPEndPoint sIPEndPoint = System.Net.IPEndPoint.Parse(String.Concat(sHostIP, ":", iPortNo));

            sListener = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
                                                                                System.Net.Sockets.SocketType.Stream,
                                                                                System.Net.Sockets.ProtocolType.Tcp);
            sListener.Bind(sIPEndPoint);
            sListener.Listen(3);

            Console.WriteLine("Listener Start.");
            Console.WriteLine("IP Address :" + sHostIP);
            Console.WriteLine("Port No :" + iPortNo);


            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Test 2");

            sListener.Disconnect(true);
            sListener.Dispose();

            return base.StopAsync(cancellationToken);  
        }

        public void ProcessHL7Message(NHapi.Base.Model.IMessage sIMessage)
        {
            NHapi.Model.V26.Message.ORU_R01 sRU_R01 = (NHapi.Model.V26.Message.ORU_R01)sIMessage;

            tbltestanalyze_results_messageheader sMSHObj = new tbltestanalyze_results_messageheader
            {
                FieldSeparator = sRU_R01.MSH.FieldSeparator.Value,
                EncodingCharacters = sRU_R01.MSH.EncodingCharacters.Value,
                SendingApplication = sRU_R01.MSH.SendingApplication.NamespaceID.Value + "^" +
                                     sRU_R01.MSH.SendingApplication.UniversalID.Value + "^" +
                                     sRU_R01.MSH.SendingApplication.UniversalIDType.Value,
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

            List<tbltestanalyze_results_patientidentification> sPIDObj = new List<tbltestanalyze_results_patientidentification>();
            var sPatientResult = sRU_R01.GetPATIENT_RESULT().PATIENT.PID.GetPatientIdentifierList();
            if (sPatientResult.Length > 0)
            {
                foreach(var p in sPatientResult)
                {
                    var sNameObj = sRU_R01.GetPATIENT_RESULT().PATIENT.PID.GetPatientName().FirstOrDefault();
                    String sPatientName = "";
                    if (sNameObj != null)
                    {
                        sPatientName = "" + "^" +
                                       sNameObj.GivenName + "^" +
                                       sNameObj.SecondAndFurtherGivenNamesOrInitialsThereof + "^" +
                                       sNameObj.SuffixEgJRorIII + "^" +
                                       sNameObj.PrefixEgDR + "^" +
                                       sNameObj.DegreeEgMD + "^" +
                                       sNameObj.NameTypeCode;
                    }
                    if (sPatientName.Replace("^", "").Length == 0)
                    {
                        sPatientName = "";
                    }

                    sPIDObj.Add(new tbltestanalyze_results_patientidentification
                    {
                        SetID = sRU_R01.GetPATIENT_RESULT().PATIENT.PID.SetIDPID.Value,
                        PatientID = sRU_R01.GetPATIENT_RESULT().PATIENT.PID.PatientID.IDNumber.Value,
                        PatientIdentifierList = p.IDNumber.ToString(),
                        AlternatePatientID = (sRU_R01.GetPATIENT_RESULT().PATIENT.PID.GetAlternatePatientIDPID().Length > 0) ?
                                             sRU_R01.GetPATIENT_RESULT().PATIENT.PID.GetAlternatePatientIDPID().FirstOrDefault().IDNumber.ToString() : null,
                        PatientName = sPatientName
                    }); 
                }
            }

            var sNTEObj = new List<tbltestanalyze_results_notes>();
            var sOBXObjList = new List<tbltestanalyze_results_observationresult>();
            var sOBRObj = new tbltestanalyze_results_observationrequest();
            foreach (var observation in sRU_R01.PATIENT_RESULTs.FirstOrDefault().ORDER_OBSERVATIONs)
            {
                sOBRObj.SetID = observation.OBR.SetIDOBR.Value;
                sOBRObj.PlacerOrderNumber = observation.OBR.PlacerOrderNumber.EntityIdentifier.Value;
                sOBRObj.FillerOrderNumber = observation.OBR.FillerOrderNumber.EntityIdentifier.Value;
                sOBRObj.UniversalServIdentifier = observation.OBR.UniversalServiceIdentifier.Identifier.Value + "^" + 
                                                  observation.OBR.UniversalServiceIdentifier.Text.Value + "^" +  
                                                  observation.OBR.UniversalServiceIdentifier.NameOfCodingSystem.Value;
                sOBRObj.Priority = observation.OBR.Priority.Value;
                sOBRObj.RequestedDateTime = observation.OBR.RequestedDateTime.Value;
                sOBRObj.ObservationDateTime = observation.OBR.ObservationDateTime.Value;
                sOBRObj.ObservationEndDateTime = observation.OBR.ObservationEndDateTime.Value;
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


                foreach (var observationDetail in observation.OBSERVATIONs)
                {
                    String sObservValue = "";
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
                            sObservValue = observationDetail.OBX.GetObservationValue().FirstOrDefault().Data.ToString();
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
                        ResponsibleObserver = (observationDetail.OBX.GetResponsibleObserver().Length > 0) ?
                                              observationDetail.OBX.GetResponsibleObserver().FirstOrDefault().IDNumber.Value : null,
                        ObservationMethod = (observationDetail.OBX.GetObservationMethod().Length > 0) ?
                                             observationDetail.OBX.GetObservationMethod().FirstOrDefault().Text.ToString() : null,
                        EquipmentInstanceIdentifier = (observationDetail.OBX.GetEquipmentInstanceIdentifier().Length > 0) ?
                                                      observationDetail.OBX.GetEquipmentInstanceIdentifier().FirstOrDefault().EntityIdentifier.Value : null,
                        AnalysisDateTime = observationDetail.OBX.DateTimeOfTheAnalysis.Value

                    });

                    if (observationDetail.NTEs.Count() > 0)
                    {
                        sNTEObj.Add(new tbltestanalyze_results_notes
                        {
                            SetID = (observationDetail.NTEs.Count() > 0) ? 
                                    observationDetail.NTEs.FirstOrDefault().SetIDNTE.Value : null,
                            Segment = "OBX",
                            SourceComment = (observationDetail.NTEs.Count() > 0) ?
                                             observationDetail.NTEs.FirstOrDefault().SourceOfComment.Value : null,
                            Comment = GenerateNTEComments(observationDetail.NTEs.ToList())
                        });
                    }

                }
            }

            tbltestanalyze_results sResultObj = new tbltestanalyze_results
            {
                MessageType = sRU_R01.MSH.MessageType.MessageStructure.Value,
                MessageDateTime = DateTime.Now,
                CreatedDate = DateTime.Now,
                CreatedBy = "Testing"
            };

            TestResultRepository.insertTestObservationMessage(sResultObj, sMSHObj, sPIDObj, sOBRObj, sOBXObjList, sNTEObj);
        }

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

            }
            return nteComment;
        }
    }
}
