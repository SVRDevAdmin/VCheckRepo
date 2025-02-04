using NHapi.Base.Model;
using NHapi.Base.Parser;
using NHapi.Model.V25.Message;
using NHapi.Model.V25.Segment;
using NHapi.Model.V251.Message;
using NHapiTools.Base;
using NHapiTools.Model.V25.Group;
using NHapiTools.Model.V251.Group;
using NHapiTools.Model.V251.Message;
using NHapiTools.Model.V251.Segment;
using System.Buffers;
using System.Text;

namespace VCheckHL7QueryTestApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            String sBodyMsg = "";
            sBodyMsg = GenerateQueryAWOSMessage();
            //sBodyMsg = processQueryAWOS();
            //sBodyMsg = processAWOSBroadcast();
            //sBodyMsg = processAWOSStatusChanges();

            NHapiTools.Base.Net.SimpleMLLPClient sSimpleMLLPClient = new NHapiTools.Base.Net.SimpleMLLPClient("192.168.0.142", 5067);

            try
            {
                String sResp = sSimpleMLLPClient.SendHL7Message(sBodyMsg);

                OutputMessage("Ack", sResp);
            }
            catch (Exception ex)
            {
                String abc = ex.ToString();
            }

            sSimpleMLLPClient.Disconnect();
            sSimpleMLLPClient.Dispose();
        }

        private static String GenerateQueryAWOSMessage()
        {
            try
            {
                NHapi.Model.V251.Message.QBP_Q11 sQBP_Q11 = new NHapi.Model.V251.Message.QBP_Q11();

                Message resp = new Message();

                Segment sMSH = new Segment("MSH");
                sMSH.Field(1, "|");
                sMSH.Field(2, @"^~\&");
                sMSH.Field(3, "VCHECK C10"); //sending app
                sMSH.Field(4, ""); //Send facility
                sMSH.Field(5, ""); //Receiving app
                sMSH.Field(6, "LAB"); // receiving facility
                sMSH.Field(7, DateTime.Now.ToString("yyyyMMddHHmmss")); //Datetime of msg
                sMSH.Field(8, ""); //security
                sMSH.Field(9, "QBP^Q11^QBP_Q11"); //message type
                sMSH.Field(10, "{" + Guid.NewGuid().ToString() + "}"); //message control id
                sMSH.Field(11, "P"); // processing id
                sMSH.Field(12, "2.5.1"); // Version id
                sMSH.Field(13, ""); // sequence message
                sMSH.Field(14, ""); // 
                sMSH.Field(15, "NE");
                sMSH.Field(16, "AL");
                sMSH.Field(18, "UNICODE UTF-8");
                sMSH.Field(21, "LAB-27^IHE");
                resp.Add(sMSH);

                Segment sQPD = new Segment("QPD");
                sQPD.Field(1, "WOS_ALL^Work Order Step All^IHELAW");
                sQPD.Field(2, "20230404140913");
                resp.Add(sQPD);

                Segment sRCP = new Segment("RCP");
                sRCP.Field(1, "|");
                sRCP.Field(3, "R^Real Time^HL70394");
                resp.Add(sRCP);


                StringBuilder sb = new StringBuilder();
                sb.Append((char)0x0b);
                sb.Append(resp.SerializeMessage());
                sb.Append((char)0x1c);
                sb.Append((char)0x0d);

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }

        private static String GenerateAWOSBroadcast()
        {
            try
            {
                NHapi.Model.V251.Message.OML_O33 sOML_O33 = new NHapi.Model.V251.Message.OML_O33();

                Message resp = new Message();

                Segment sMSH = new Segment("MSH");
                sMSH.Field(1, "|");
                sMSH.Field(2, @"^~\&");
                sMSH.Field(3, "VCHECK C10"); //sending app
                sMSH.Field(4, ""); //Send facility
                sMSH.Field(5, ""); //Receiving app
                sMSH.Field(6, "LAB"); // receiving facility
                sMSH.Field(7, DateTime.Now.ToString("yyyyMMddHHmmss")); //Datetime of msg
                //sMSH.Field(8, ); //security
                sMSH.Field(9, "OML^O33^OML_O33"); //message type
                sMSH.Field(10, Guid.NewGuid().ToString()); //message control id
                sMSH.Field(11, "P"); // processing id
                sMSH.Field(12, "2.5.1"); // Version id
                //sMSH.Field(13, ) // sequence message
                //sMSH.Field(14, ) // 
                sMSH.Field(15, "NE");
                sMSH.Field(16, "AL");
                sMSH.Field(18, "UNICODE UTF-8");
                sMSH.Field(21, "LAB-28^IHE");
                resp.Add(sMSH);

                Segment sPID = new Segment("PID");
                sPID.Field(1, "1"); // set ID
                sPID.Field(2, ""); //Patient Id
                sPID.Field(3, "PatientID^^^^Hospital"); //patient identifier list
                sPID.Field(5, "Patient Name^^^^L");
                sPID.Field(7, "YYYYMMDD");
                sPID.Field(8, "Male");
                sPID.Field(21, "Pet Owner");
                sPID.Field(35, "Canine");
                sPID.Field(36, "Husky");
                resp.Add(sPID);

                Segment sPV1 = new Segment("PV1");
                sPV1.Field(1, "");
                sPV1.Field(2, "E"); 
                sPV1.Field(3, "3001");
                sPV1.Field(7, "Doctor");
                resp.Add(sPV1);

                Segment sSPM = new Segment("SPM");
                sSPM.Field(1, "1");
                sSPM.Field(4, "Serum");
                sSPM.Field(11, "P^Patient^HL70369");
                resp.Add(sSPM);

                Segment sSAC = new Segment("SAC");
                sSAC.Field(3, "g55643");

                Segment sORC = new Segment("ORC");
                sORC.Field(1, "NW");
                sORC.Field(2, "12345^SMC_AM^1.3.6.1.4.1.12559.11.1.2.2.4.2^ISO");
                sORC.Field(4, "12561^IHE_OM_OP^1.3.6.1.4.1.12559.11.1.2.2.4.2^ISO");
                sORC.Field(5, "IP");
                sORC.Field(9, DateTime.Now.ToString("yyyyMMddhhmmss"));
                resp.Add(sORC);

                Segment sOBR = new Segment("OBR");
                sOBR.Field(2, "fffffbbffffffef5f5f4cfffffffc10ffffffec");
                sOBR.Field(4, "DC001B^Comprehensive 17^VCHECK");
                resp.Add(sOBR);

                Segment sOBX = new Segment("OBX");
                sOBX.Field(1, "1");
                sOBX.Field(2, "NM");
                sOBX.Field(3, "Age");
                sOBX.Field(6, "Months");
                sOBX.Field(11, "F");
                sOBX.Field(5, "2");
                resp.Add(sOBX);

                Segment sOBX2 = new Segment("OBX");
                sOBX2.Field(1, "2");
                sOBX2.Field(2, "NM");
                sOBX2.Field(3, "Weight");
                sOBX2.Field(6, "kg");
                sOBX2.Field(11, "F");
                sOBX2.Field(5, "24");
                resp.Add(sOBX2);

                StringBuilder sb = new StringBuilder();
                sb.Append((char)0x0b);
                sb.Append(resp.SerializeMessage());
                sb.Append((char)0x1c);
                sb.Append((char)0x0d);

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }

        private static String processQueryAWOS()
        {
            NHapi.Model.V251.Message.QBP_Q11 qMessage = new NHapi.Model.V251.Message.QBP_Q11();

            // ---- MSH ----- //
            qMessage.MSH.FieldSeparator.Value = "|";
            qMessage.MSH.EncodingCharacters.Value = @"^~\&";
            qMessage.MSH.SendingApplication.NamespaceID.Value = "VCHECK C10";
            qMessage.MSH.ReceivingFacility.NamespaceID.Value = "LAB";
            qMessage.MSH.DateTimeOfMessage.Time.Value = DateTime.Now.ToString("yyyyMMddHHmmss");
            qMessage.MSH.MessageType.MessageCode.Value = "QBP";
            qMessage.MSH.MessageType.TriggerEvent.Value = "Q11";
            qMessage.MSH.MessageType.MessageStructure.Value = "QBP_Q11";
            qMessage.MSH.MessageControlID.Value = Guid.NewGuid().ToString();
            qMessage.MSH.ProcessingID.ProcessingID.Value = "P";
            qMessage.MSH.VersionID.VersionID.Value = "2.5.1";
            qMessage.MSH.AcceptAcknowledgmentType.Value = "NE";
            qMessage.MSH.ApplicationAcknowledgmentType.Value = "AL";
            qMessage.MSH.AddCharacterSet().Value = "UNICODE UTF-8";
            qMessage.MSH.AddMessageProfileIdentifier().EntityIdentifier.Value = "LAB-27";
            qMessage.MSH.AddMessageProfileIdentifier().NamespaceID.Value = "IHE";

            qMessage.QPD.MessageQueryName.Identifier.Value = "WOS_ALL^Work Order Step All^IHELAW";
            qMessage.QPD.QueryTag.Value = "20230404140913";

            qMessage.RCP.QueryPriority.Value = "|";
            qMessage.RCP.ResponseModality.Identifier.Value = "R";
            qMessage.RCP.ResponseModality.Text.Value = "Real Time";
            qMessage.RCP.ResponseModality.NameOfCodingSystem.Value = "R^Real Time^HL70394";


            PipeParser parser = new PipeParser();

            String sResult = parser.Encode(qMessage);

            OutputMessage("QueryAWOS", sResult);

            return sResult;
        }

        private static String processAWOSBroadcast()
        {
            NHapi.Model.V251.Message.OML_O33 oMessage = new NHapi.Model.V251.Message.OML_O33();

            oMessage.MSH.FieldSeparator.Value = "|";
            oMessage.MSH.EncodingCharacters.Value = @"^~\&";
            oMessage.MSH.SendingApplication.NamespaceID.Value = "VCheck C10";
            oMessage.MSH.SendingFacility.NamespaceID.Value = "";
            oMessage.MSH.ReceivingApplication.NamespaceID.Value = "";
            oMessage.MSH.ReceivingFacility.NamespaceID.Value = "LAB";
            oMessage.MSH.DateTimeOfMessage.Time.Value = DateTime.Now.ToString("yyyyMMddhhmmss");
            oMessage.MSH.MessageType.MessageCode.Value = "OML";
            oMessage.MSH.MessageType.TriggerEvent.Value = "O33";
            oMessage.MSH.MessageType.MessageStructure.Value = "OML_O33";
            oMessage.MSH.MessageControlID.Value = Guid.NewGuid().ToString();
            oMessage.MSH.ProcessingID.ProcessingID.Value = "P";
            oMessage.MSH.VersionID.VersionID.Value = "2.5.1";
            oMessage.MSH.AcceptAcknowledgmentType.Value = "NE";
            oMessage.MSH.ApplicationAcknowledgmentType.Value = "AL";
            oMessage.MSH.AddCharacterSet().Value = "UNICODE UTF-8";
            oMessage.MSH.AddMessageProfileIdentifier().NamespaceID.Value = "LAB-28";
            oMessage.MSH.AddMessageProfileIdentifier().EntityIdentifier.Value = "IHE";

            oMessage.PATIENT.PID.AddPatientIdentifierList().IDNumber.Value = "PatientID";
            oMessage.PATIENT.PID.AddPatientIdentifierList().AssigningFacility.NamespaceID.Value = "Hospital";

            oMessage.PATIENT.PID.AddPatientName().FamilyName.Surname.Value = "Patient Name";
            oMessage.PATIENT.PID.AddPatientName().NameTypeCode.Value = "L";
            oMessage.PATIENT.PID.DateTimeOfBirth.Time.Value = "YYYYMMDD";
            oMessage.PATIENT.PID.AdministrativeSex.Value = "Male";
            oMessage.PATIENT.PID.AddMotherSIdentifier().IDNumber.Value = "Pet Owner";
            oMessage.PATIENT.PID.SpeciesCode.Identifier.Value = "Canine";
            oMessage.PATIENT.PID.BreedCode.Identifier.Value = "Husky";

            oMessage.PATIENT.PATIENT_VISIT.PV1.PatientClass.Value = "E";
            oMessage.PATIENT.PATIENT_VISIT.PV1.AssignedPatientLocation.Room.Value = "3001";
            oMessage.PATIENT.PATIENT_VISIT.PV1.AddAttendingDoctor().IDNumber.Value = "Doctor";

            // ------ SPM ------ //
            oMessage.AddNonstandardSegment("SPM");
            var oSPM = oMessage.GetStructure("SPM") as NHapi.Model.V251.Segment.SPM;
            oSPM.SetIDSPM.Value = "1";
            oSPM.SpecimenType.Identifier.Value = "Serum";
            oSPM.AddSpecimenRole().Identifier.Value = "P";
            oSPM.AddSpecimenRole().Text.Value = "Patient";
            oSPM.AddSpecimenRole().NameOfCodingSystem.Value = "HL70369";

            // ----- SAC ------ //
            oMessage.AddNonstandardSegment("SAC");
            var oSAC = oMessage.GetStructure("SAC") as NHapi.Model.V251.Segment.SAC;
            oSAC.ContainerIdentifier.EntityIdentifier.Value = "g55643";

            // ------ ORC ------ //
            oMessage.AddNonstandardSegment("ORC");
            var oORC = oMessage.GetStructure("ORC") as NHapi.Model.V251.Segment.ORC;
            oORC.OrderControl.Value = "NW";
            oORC.PlacerOrderNumber.EntityIdentifier.Value = "12345";
            oORC.PlacerOrderNumber.NamespaceID.Value = "SMC_AM";
            oORC.PlacerOrderNumber.UniversalID.Value = "1.3.6.1.4.1.12559.11.1.2.2.4.2";
            oORC.PlacerOrderNumber.UniversalIDType.Value = "ISO";

            oORC.PlacerGroupNumber.EntityIdentifier.Value = "12561";
            oORC.PlacerGroupNumber.NamespaceID.Value = "IHE_OM_OP";
            oORC.PlacerGroupNumber.UniversalID.Value = "1.3.6.1.4.1.12559.11.1.2.2.4.2";
            oORC.PlacerGroupNumber.UniversalIDType.Value = "ISO";

            oORC.OrderStatus.Value = "IP";
            oORC.DateTimeOfTransaction.Time.Value = DateTime.Now.ToString("yyyyMMddhhmmss");

            // ----- OBR ----- //
            oMessage.AddNonstandardSegment("OBR");
            var oOBR = oMessage.GetStructure("OBR") as NHapi.Model.V251.Segment.OBR;
            oOBR.PlacerOrderNumber.EntityIdentifier.Value = "fffffbbffffffef5f5f4cfffffffc10ffffffec";
            oOBR.UniversalServiceIdentifier.Identifier.Value = "DC001B";
            oOBR.UniversalServiceIdentifier.Text.Value = "Comprehensive 17";
            oOBR.UniversalServiceIdentifier.NameOfCodingSystem.Value = "VCHECK";

            // ---- OBX SetID = 1 ---- //
            var oOBX = oMessage.AddSPECIMEN().GetOBX();
            oOBX.SetIDOBX.Value = "1";
            oOBX.ValueType.Value = "NM";
            oOBX.ObservationIdentifier.Text.Value = "Age";
            oOBX.Units.Text.Value = "Months";
            oOBX.ObservationResultStatus.Value = "F";

            NHapi.Model.V251.Datatype.CE c = new NHapi.Model.V251.Datatype.CE(oMessage);
            c.Identifier.Value = "2";
            Varies v = oOBX.GetObservationValue(0);
            v.Data = c;

            // ----- OBX SetID = 2 ------ //
            var oOBX2 = oMessage.AddSPECIMEN().GetOBX();
            oOBX2.SetIDOBX.Value = "2";
            oOBX2.ValueType.Value = "NM";
            oOBX2.ObservationIdentifier.Text.Value = "Weight";
            oOBX2.Units.Text.Value = "kg";
            oOBX2.ObservationResultStatus.Value = "F";

            NHapi.Model.V251.Datatype.CE c2 = new NHapi.Model.V251.Datatype.CE(oMessage);
            c2.Identifier.Value = "24";
            Varies v2 = oOBX2.GetObservationValue(0);
            v2.Data = c2;

            PipeParser parser = new PipeParser();

            String sResult = parser.Encode(oMessage);

            OutputMessage("AWOSBroadcast", sResult);

            return sResult;
        }

        private static String processAWOSStatusChanges()
        {
            NHapi.Model.V251.Message.OUL_R22 oMessage = new NHapi.Model.V251.Message.OUL_R22();

            oMessage.MSH.FieldSeparator.Value = "|";
            oMessage.MSH.EncodingCharacters.Value = @"^~\&";
            oMessage.MSH.SendingApplication.NamespaceID.Value = "VCheck C10";
            oMessage.MSH.SendingFacility.NamespaceID.Value = "";
            oMessage.MSH.ReceivingApplication.NamespaceID.Value = "";
            oMessage.MSH.ReceivingFacility.NamespaceID.Value = "LAB";
            oMessage.MSH.DateTimeOfMessage.Time.Value = DateTime.Now.ToString("yyyyMMddhhmmss");
            oMessage.MSH.MessageType.MessageCode.Value = "OUL";
            oMessage.MSH.MessageType.TriggerEvent.Value = "R22";
            oMessage.MSH.MessageType.MessageStructure.Value = "OUL_R22";
            oMessage.MSH.MessageControlID.Value = Guid.NewGuid().ToString();
            oMessage.MSH.ProcessingID.ProcessingID.Value = "P";
            oMessage.MSH.VersionID.VersionID.Value = "2.5.1";
            oMessage.MSH.AcceptAcknowledgmentType.Value = "NE";
            oMessage.MSH.ApplicationAcknowledgmentType.Value = "AL";
            oMessage.MSH.AddCharacterSet().Value = "UNICODE UTF-8";
            oMessage.MSH.AddMessageProfileIdentifier().NamespaceID.Value = "LAB-28";
            oMessage.MSH.AddMessageProfileIdentifier().EntityIdentifier.Value = "IHE";

            oMessage.PATIENT.PID.AddPatientIdentifierList().IDNumber.Value = "PatientID";
            oMessage.PATIENT.PID.AddPatientIdentifierList().AssigningFacility.NamespaceID.Value = "Hospital";

            oMessage.PATIENT.PID.AddPatientName().FamilyName.Surname.Value = "Patient Name";
            oMessage.PATIENT.PID.AddPatientName().NameTypeCode.Value = "L";
            oMessage.PATIENT.PID.DateTimeOfBirth.Time.Value = "YYYYMMDD";
            oMessage.PATIENT.PID.AdministrativeSex.Value = "Male";
            oMessage.PATIENT.PID.AddMotherSIdentifier().IDNumber.Value = "Pet Owner";
            oMessage.PATIENT.PID.SpeciesCode.Identifier.Value = "Canine";
            oMessage.PATIENT.PID.BreedCode.Identifier.Value = "Husky";

            oMessage.VISIT.PV1.PatientClass.Value = "E";
            oMessage.VISIT.PV1.AssignedPatientLocation.Room.Value = "3001";
            oMessage.VISIT.PV1.AddAttendingDoctor().IDNumber.Value = "Doctor";

            // ------ SPM ------ //
            oMessage.AddNonstandardSegment("SPM");
            var oSPM = oMessage.GetStructure("SPM") as NHapi.Model.V251.Segment.SPM;
            oSPM.SetIDSPM.Value = "1";
            oSPM.SpecimenType.Identifier.Value = "Serum";
            oSPM.AddSpecimenRole().Identifier.Value = "P";
            oSPM.AddSpecimenRole().Text.Value = "Patient";
            oSPM.AddSpecimenRole().NameOfCodingSystem.Value = "HL70369";

            // ----- SAC ------ //
            oMessage.AddNonstandardSegment("SAC");
            var oSAC = oMessage.GetStructure("SAC") as NHapi.Model.V251.Segment.SAC;
            oSAC.ContainerIdentifier.EntityIdentifier.Value = "g55643";

            // ------ ORC ------ //
            oMessage.AddNonstandardSegment("ORC");
            var oORC = oMessage.GetStructure("ORC") as NHapi.Model.V251.Segment.ORC;
            oORC.OrderControl.Value = "NW";
            oORC.PlacerOrderNumber.EntityIdentifier.Value = "12345";
            oORC.PlacerOrderNumber.NamespaceID.Value = "SMC_AM";
            oORC.PlacerOrderNumber.UniversalID.Value = "1.3.6.1.4.1.12559.11.1.2.2.4.2";
            oORC.PlacerOrderNumber.UniversalIDType.Value = "ISO";

            oORC.PlacerGroupNumber.EntityIdentifier.Value = "12561";
            oORC.PlacerGroupNumber.NamespaceID.Value = "IHE_OM_OP";
            oORC.PlacerGroupNumber.UniversalID.Value = "1.3.6.1.4.1.12559.11.1.2.2.4.2";
            oORC.PlacerGroupNumber.UniversalIDType.Value = "ISO";

            oORC.OrderStatus.Value = "IP";
            oORC.DateTimeOfTransaction.Time.Value = DateTime.Now.ToString("yyyyMMddhhmmss");

            // ----- OBR ----- //
            oMessage.AddNonstandardSegment("OBR");
            var oOBR = oMessage.GetStructure("OBR") as NHapi.Model.V251.Segment.OBR;
            oOBR.PlacerOrderNumber.EntityIdentifier.Value = "fffffbbffffffef5f5f4cfffffffc10ffffffec";
            oOBR.UniversalServiceIdentifier.Identifier.Value = "DC001B";
            oOBR.UniversalServiceIdentifier.Text.Value = "Comprehensive 17";
            oOBR.UniversalServiceIdentifier.NameOfCodingSystem.Value = "VCHECK";
            oOBR.ObservationDateTime.Time.Value = DateTime.Now.ToString("yyyyMMddHHmmss");

            // ---- OBX SetID = 1 ---- //
            var oOBX = oMessage.AddSPECIMEN().GetOBX();
            oOBX.SetIDOBX.Value = "1";
            oOBX.ValueType.Value = "NM";
            oOBX.ObservationIdentifier.Text.Value = "Age";
            oOBX.Units.Text.Value = "Months";
            oOBX.ObservationResultStatus.Value = "F";

            NHapi.Model.V251.Datatype.CE c = new NHapi.Model.V251.Datatype.CE(oMessage);
            c.Identifier.Value = "2";
            Varies v = oOBX.GetObservationValue(0);
            v.Data = c;

            // ----- OBX SetID = 2 ------ //
            var oOBX2 = oMessage.AddSPECIMEN().GetOBX();
            oOBX2.SetIDOBX.Value = "2";
            oOBX2.ValueType.Value = "NM";
            oOBX2.ObservationIdentifier.Text.Value = "Weight";
            oOBX2.Units.Text.Value = "kg";
            oOBX2.ObservationResultStatus.Value = "F";

            NHapi.Model.V251.Datatype.CE c2 = new NHapi.Model.V251.Datatype.CE(oMessage);
            c2.Identifier.Value = "24";
            Varies v2 = oOBX2.GetObservationValue(0);
            v2.Data = c2;

            // ---- OBX SetID = 3 -------//
            var oOBX3 = oMessage.AddSPECIMEN().GetOBX();
            oOBX3.SetIDOBX.Value = "3";
            oOBX3.ValueType.Value = "NM";
            oOBX3.ObservationIdentifier.Identifier.Value = "DIB013";
            oOBX3.ObservationIdentifier.Text.Value = "TBIL";
            oOBX3.ObservationIdentifier.NameOfCodingSystem.Value = "VCHECK";
            oOBX3.ObservationSubID.Value = "1.0.0.0";
            oOBX3.Units.Identifier.Value = "mg/dL";
            oOBX3.Units.Text.Value = "milligram per deciliter";
            oOBX3.Units.NameOfCodingSystem.Value = "UCUM";
            oOBX3.ReferencesRange.Value = "[0.00;1.00]";
            oOBX3.ObservationResultStatus.Value = "F";
            oOBX3.AddResponsibleObserver().IDNumber.Value = "ADMIN";
            oOBX3.AddEquipmentInstanceIdentifier().EntityIdentifier.Value = "C10";
            oOBX3.AddEquipmentInstanceIdentifier().NamespaceID.Value = "CB10X02AAA0015";
            oOBX3.AddEquipmentInstanceIdentifier().UniversalID.Value = "BioNote";
            oOBX3.DateTimeOfTheAnalysis.Time.Value = DateTime.Now.ToString("yyyyMMddHHmmss");


            NHapi.Model.V251.Datatype.CE c3 = new NHapi.Model.V251.Datatype.CE(oMessage);
            c3.Identifier.Value = "4.00";
            Varies v3 = oOBX3.GetObservationValue(0);
            v3.Data = c3;

            // ---- OBX SetID = 4 -------//
            var oOBX4 = oMessage.AddSPECIMEN().GetOBX();
            oOBX4.SetIDOBX.Value = "4";
            oOBX4.ValueType.Value = "NM";
            oOBX4.ObservationIdentifier.Identifier.Value = "DIB014";
            oOBX4.ObservationIdentifier.Text.Value = "TP";
            oOBX4.ObservationIdentifier.NameOfCodingSystem.Value = "VCHECK";
            oOBX4.ObservationSubID.Value = "1.0.0.1";
            oOBX4.Units.Identifier.Value = "g/dL";
            oOBX4.Units.Text.Value = "milligram per deciliter";
            oOBX4.Units.NameOfCodingSystem.Value = "UCUM";
            oOBX4.ReferencesRange.Value = "[5.3;8.4]";
            oOBX4.ObservationResultStatus.Value = "F";
            oOBX4.AddResponsibleObserver().IDNumber.Value = "ADMIN";
            oOBX4.AddEquipmentInstanceIdentifier().EntityIdentifier.Value = "C10";
            oOBX4.AddEquipmentInstanceIdentifier().NamespaceID.Value = "CB10X02AAA0015";
            oOBX4.AddEquipmentInstanceIdentifier().UniversalID.Value = "BioNote";
            oOBX4.DateTimeOfTheAnalysis.Time.Value = DateTime.Now.ToString("yyyyMMddHHmmss");

            NHapi.Model.V251.Datatype.CE c4 = new NHapi.Model.V251.Datatype.CE(oMessage);
            c4.Identifier.Value = "0.00";
            Varies v4 = oOBX4.GetObservationValue(0);
            v4.Data = c4;

            PipeParser parser = new PipeParser();

            String sResult = parser.Encode(oMessage);

            OutputMessage("AWOSStatus", sResult);

            return sResult;
        }

        private static void OutputMessage(String sTypeCall, String sHL7Message)
        {
            try
            {
                String sFileName = sTypeCall + "_requestMessage_" + DateTime.Now.ToString("yyyyMMddHHmmss");

                String sOutputPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Output/HL7");
                if (!Directory.Exists(sOutputPath))
                {
                    Directory.CreateDirectory(sOutputPath);
                }
                File.WriteAllText(sOutputPath + sFileName + ".hl7", sHL7Message, System.Text.Encoding.ASCII);

                //String sXMLOutPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Output/XML");
                //if (!Directory.Exists(sXMLOutPath))
                //{
                //    Directory.CreateDirectory(sXMLOutPath);
                //}
                //File.WriteAllText(sOutputPath + sFileName + ".xml", sHL7Message, System.Text.Encoding.ASCII);
            }
            catch (Exception ex)
            {
                String abc = ex.ToString();
            }
        }
    }
}
