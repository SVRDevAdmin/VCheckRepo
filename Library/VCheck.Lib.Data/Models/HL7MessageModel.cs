using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Lib.Data.Models
{
    public class HL7MessageModel
    {
        public MSHModel MSH { get; set; }
        public PIDModel PID { get; set; }
        public PV1Model PV1 { get; set; }
        public SPMModel SPM { get; set; }
        public SACModel SAC { get; set; }
        public ORCModel ORC { get; set; }
        public OBRModel OBR { get; set; }
        public List<OBXModel> OBX { get; set; }
    }

    public class MSHModel
    {
        public string SendingApplication { get; set; }
        public string SendingFacility { get; set; }
        public string ReceivingApplication { get; set; }
        public string ReceivingFacility { get; set; }
        public string MessageType { get; set; }
        public string VersionID { get; set; }
        public string AcceptAckType { get; set; }
        public string AppAckType { get; set; }
        public string CharacterSet { get; set; }
        public string MessageProfileIdentifier { get; set; }
    }

    public class PIDModel
    {
        public string PatientID { get; set; }
        public string Hospital { get; set; }
        public string PetName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string OwnerName { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
    }

    public class PV1Model
    {
        public string PatientClass { get; set; }
        public string Room { get; set; }
        public string DoctorName { get; set; }
    }

    public class SPMModel
    {
        public string SetID { get; set; }
        public string SpecimenType { get; set; }
        public string SpecimentRole { get; set; }
    }

    public class SACModel
    {
        public string ContainerID { get; set; }
    }

    public class ORCModel
    {
        public string OrderControl { get; set; }
        public string PlacerOrderNo { get; set; }
        public string PlacerGroupNo { get; set; }
        public DateTime TransactionDatetime { get; set; }
    }

    public class OBRModel
    {
        public string PlacerOrderNo { get; set; }
        public string UniversalServiceID { get; set; }
    }

    public class OBXModel
    {
        public string SetID { get; set; }
        public string ValueType { get; set; }
        public string ObservationIdentifier { get; set; }
        public string ObservationValue { get; set; }
        public string Units { get; set; }
        public string ObservationResultStatus { get; set; }
    }
}
