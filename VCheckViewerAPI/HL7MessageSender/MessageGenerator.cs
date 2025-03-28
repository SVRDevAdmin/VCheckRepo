using System.Text;
using VCheck.Lib.Data;
using VCheck.Lib.Data.Models;
using VCheckViewerAPI.Lib.Object;
using VCheckViewerAPI.Lib.Util;
using VCheckViewerAPI.Message.CreateScheduledTest;

namespace VCheckViewerAPI.HL7MessageSender
{
    public class MessageGenerator
    {
        public static String GenerateOMLO33Message(CreateScheduledDataRequestBody info)
        {
            var message = new HL7MessageModel();
            message.MSH = new MSHModel()
            {
                SendingApplication = "LIS",
                SendingFacility = "",
                ReceivingApplication = "VCHECK C10",
                ReceivingFacility = "LAB",
                MessageType = "OML^O33^OML_O33",
                VersionID = "2.5.1",
                AcceptAckType = "NE",
                AppAckType = "AL",
                CharacterSet = "UNICODE UTF-8",
                MessageProfileIdentifier = "LAB-28^IHE"
            };

            var sLocation = LocationRepository.GetLocationByID(ConfigSettings.GetConfigurationSettings(), int.Parse(info.LocationID));

            message.PID = new PIDModel()
            {
                PatientID = info.PatientID,
                Hospital = sLocation.Name,
                PetName = info.PatientName,
                BirthDate = DateTime.Parse("2024-01-01"),
                Gender = info.Gender,
                OwnerName = info.OwnerName,
                Species = info.Species,
                Breed = "NA"
            };

            //message.PID = new PIDModel()
            //{
            //    PatientID = "A12345",
            //    Hospital = "Medical Center",
            //    PetName = "Patient T",
            //    BirthDate = DateTime.Parse("2024-01-01"),
            //    Gender = "Male",
            //    OwnerName = "Owner A",
            //    Species = "Canine",
            //    Breed = "Husky"
            //};

            message.PV1 = new PV1Model()
            {
                PatientClass = "E",
                Room = "^3001",
                DoctorName = info.PersonIncharges
            };

            message.SPM = new SPMModel()
            {
                SetID = "1",
                SpecimenType = "Serum",
                SpecimentRole = "P^Patient^HL70369"
            };

            message.SAC = new SACModel() { ContainerID = "g55643" };

            message.ORC = new ORCModel()
            {
                OrderControl = "NW",
                PlacerOrderNo = Guid.NewGuid().ToString(),
                PlacerGroupNo = Guid.NewGuid().ToString(),
                TransactionDatetime = DateTime.Now
            };

            string CartridgeID = "DC001B";
            string CartridgeName = "Comprehensive 17";

            message.OBR = new OBRModel()
            {
                PlacerOrderNo = Guid.NewGuid().ToString(),
                UniversalServiceID = CartridgeID + "^" + CartridgeName + "^VCHECK"
            };

            List<OBXModel> obx = new List<OBXModel>()
            {
                new OBXModel()
                {
                    SetID = "1", ValueType = "NM", ObservationIdentifier = "^Age", ObservationValue = "24", Units = "Months", ObservationResultStatus = "F"
                },
                new OBXModel()
                {
                    SetID = "2", ValueType = "NM", ObservationIdentifier = "^Weight", ObservationValue = "5.4", Units = "kg", ObservationResultStatus = "F"
                }
            };

            message.OBX = obx;

            return GenerateMessage(message);
        }

        public static String GenerateMessage(HL7MessageModel message)
        {
            StringBuilder frame = new StringBuilder();
            frame.Append((char)0x0b);

            // ------------- Message Header ------------//
            Lib.Object.Message response = new Lib.Object.Message();
            Segment msh = new Segment("MSH");
            msh.Field(1, "|");
            msh.Field(2, "^~\\&");
            msh.Field(3, message.MSH.SendingApplication);
            msh.Field(4, message.MSH.SendingFacility);
            msh.Field(5, message.MSH.ReceivingApplication);
            msh.Field(6, message.MSH.ReceivingFacility);
            msh.Field(7, DateTime.Now.ToString("yyyyMMddhhmmss"));
            msh.Field(9, message.MSH.MessageType);
            msh.Field(10, Guid.NewGuid().ToString());
            msh.Field(11, "P");
            msh.Field(12, message.MSH.VersionID);
            msh.Field(15, message.MSH.AcceptAckType); //optional
            msh.Field(16, message.MSH.AppAckType); //optional
            msh.Field(18, message.MSH.CharacterSet);
            msh.Field(21, message.MSH.MessageProfileIdentifier); //optional
            response.Add(msh);
            frame.Append(response.SerializeMessage());
            frame.Append((char)0x0d);

            // ------------- Patient Identification Segment ------------//
            response = new Lib.Object.Message();
            Segment pid = new Segment("PID");
            pid.Field(3, message.PID.PatientID + "^^^^^" + message.PID.Hospital);
            pid.Field(5, message.PID.PetName);
            pid.Field(7, message.PID.BirthDate.ToString("yyyyMMdd"));
            pid.Field(8, message.PID.Gender);
            pid.Field(21, message.PID.OwnerName);
            pid.Field(35, message.PID.Species);
            pid.Field(36, message.PID.Breed);
            response.Add(pid);
            frame.Append(response.SerializeMessage());
            frame.Append((char)0x0d);

            // ------------- Patient Visit Segment ------------//
            response = new Lib.Object.Message();
            Segment pv1 = new Segment("PV1");
            pv1.Field(2, message.PV1.PatientClass); //optional
            pv1.Field(3, message.PV1.Room); //optional
            pv1.Field(7, message.PV1.DoctorName);
            response.Add(pv1);
            frame.Append(response.SerializeMessage());
            frame.Append((char)0x0d);

            // ------------- Speciment Segment ------------//
            response = new Lib.Object.Message();
            Segment spm = new Segment("SPM");
            spm.Field(1, message.SPM.SetID);
            //spm.Field(4, "Serum^Respiratory^HL70487");
            spm.Field(4, message.SPM.SpecimenType);
            spm.Field(11, message.SPM.SpecimentRole); //optional
            response.Add(spm);
            frame.Append(response.SerializeMessage());
            frame.Append((char)0x0d);

            // ------------- Speciment Container Detail Segment ------------//
            response = new Lib.Object.Message();
            Segment sac = new Segment("SAC");
            sac.Field(3, message.SAC.ContainerID);
            response.Add(sac);
            frame.Append(response.SerializeMessage());
            frame.Append((char)0x0d);

            // ------------- Common Order Segment ------------//
            response = new Lib.Object.Message();
            Segment orc = new Segment("ORC");
            orc.Field(1, message.ORC.OrderControl);
            orc.Field(2, message.ORC.PlacerOrderNo);
            //orc.Field(4, "12345^IHE_OM_OP^1.3.6.1.4.1.12559.11.1.2.2.4.4^ISO");
            orc.Field(4, message.ORC.PlacerGroupNo);
            //orc.Field(5, "IP");
            orc.Field(9, message.ORC.TransactionDatetime.ToString("yyyyMMddhhmmss"));
            response.Add(orc);
            frame.Append(response.SerializeMessage());
            frame.Append((char)0x0d);

            // ------------- Observation Request Segment ------------//
            response = new Lib.Object.Message();
            Segment obr = new Segment("OBR");
            obr.Field(2, message.OBR.PlacerOrderNo); //optional
            obr.Field(4, message.OBR.UniversalServiceID);
            response.Add(obr);
            frame.Append(response.SerializeMessage());
            frame.Append((char)0x0d);

            foreach (var obxValue in message.OBX)
            {
                // ------------- Observation Result Segment ------------//
                response = new Lib.Object.Message();
                Segment obx = new Segment("OBX");
                obx.Field(1, obxValue.SetID);
                obx.Field(2, obxValue.ValueType);
                obx.Field(3, obxValue.ObservationIdentifier);
                obx.Field(5, obxValue.ObservationValue);
                obx.Field(6, obxValue.Units);
                obx.Field(11, obxValue.ObservationResultStatus);
                obx.Field(29, "SCI");
                response.Add(obx);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x0d);
            }

            frame.Append((char)0x1c);
            frame.Append((char)0x0d);

            return frame.ToString();
        }
    }
}
