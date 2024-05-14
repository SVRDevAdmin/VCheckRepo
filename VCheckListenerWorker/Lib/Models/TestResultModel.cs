using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheckListenerWorker.Lib.Models
{
    public class TestResultModel
    {
    }

    public class tbltestanalyze_results
    {
        [Key]
        public long ResultRowID { get; set; }
        public string? MessageType { get; set; }
        public DateTime MessageDateTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
    }

    public class tbltestanalyze_results_messageheader
    {
        [Key]
        public long RowID { get; set; }
        public long ResultRowID { get; set; }
        public String? FieldSeparator { get; set; }
        public String? EncodingCharacters {  get; set; }
        public String? SendingApplication {  get; set; }
        public String? SendingFacility {  get; set; }
        public String? ReceivingApplication { get; set; }
        public String? ReceivingFacility { get; set; }
        public String? DateTimeMessage {  get; set; }
        public String? MessageType { get; set; }
        public String? MessageControlID { get; set; }
        public String? ProcessingID {  get; set; }
        public String? VersionID {  get; set; }
        public String? AcceptAckmgtType { get; set; }
        public String? AppAckmgtType { get; set; }
        public String? CountryCode { get; set; }
        public String? CharacterSet { get; set; }
        public String? PrincipalLanguageMsg { get; set; }
        public String? MessageProfileIdentifier { get; set; }
    }

    public class tbltestanalyze_results_notes
    {
        [Key]
        public long RowID { get; set; } 
        public long ResultRowID { get; set; }
        public String? Segment { get; set; }
        public String? SetID { get; set; }
        public String SourceComment {  get; set; }
        public String? Comment {  get; set; }
    }

    public class tbltestanalyze_results_observationrequest
    {
        [Key]
        public long RowID { get; set; }
        public long ResultRowID { get; set; }
        public String? SetID { get; set; }
        public String? PlacerOrderNumber { get; set; }
        public String? FillerOrderNumber {  get; set; }
        public String? UniversalServIdentifier {  get; set; }
        public String? Priority { get; set; }
        public String? RequestedDateTime {  get; set; }
        public String? ObservationDateTime {  get; set; }
        public String? ObservationEndDateTime { get; set; }
        public String? CollectVolume {  get; set; }
        public String? CollectorIdentifier {  get; set; }
        public String? SpecimenActionCode {  get; set; }
    }

    public class tbltestanalyze_results_observationresult
    {
        [Key]
        public long RowID { get; set; }
        public long ResultRowID { get; set; }
        public String? SetID { get; set; }
        public String? ValueType { get; set; }
        public String? ObservationIdentifier { get; set; }
        public String? ObservationSubID { get; set; }
        public String? ObservationValue { get; set; }
        public String? Units { get; set; }
        public String? ReferencesRange { get; set; }
        public String? AbnormalFlag {  get; set; }
        public String? ObservationResultStatus {  get; set; }
        public String? ObservationDateTime { get; set; }
        public String? ProducerID { get; set; }
        public String? ResponsibleObserver {  get; set; }
        public String? ObservationMethod {  get; set; }
        public String? EquipmentInstanceIdentifier { get; set; }
        public String? AnalysisDateTime { get; set; }
    }

    public class tbltestanalyze_results_patientidentification
    {
        [Key]
        public long RowID { get; set; }
        public long ResultRowID { get; set; }
        public String? SetID { get; set; }
        public String? PatientID { get; set; }
        public String? PatientIdentifierList { get; set; }
        public String? AlternatePatientID { get; set; }
        public String? PatientName { get; set; }
        public String? MotherMaidenName { get; set; }
        public String? DateofBirth { get; set; }
    }

    public class txn_testresults
    {
        [Key]
        public long ID { get; set; }
        public DateTime? TestResultDateTime { get; set; }
        public String? TestResultType { get; set; }
        public String? OperatorID { get; set; }
        public String? PatientID { get; set; }
        public String? InchargePerson { get; set; }
        public String? ObservationStatus { get; set; }
        public String? TestResultStatus { get; set; }
        public Decimal? TestResultValue { get; set; }
        public String? TestResultRules { get; set; }
        public DateTime? CreatedDate { get; set; }
        public String? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public String? UpdatedBy { get; set; }
    }

}
