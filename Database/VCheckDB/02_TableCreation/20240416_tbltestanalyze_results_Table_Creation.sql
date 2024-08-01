CREATE TABLE `tbltestanalyze_results` (
	`ResultRowID` BIGINT(19) NOT NULL AUTO_INCREMENT,
	`MessageType` VARCHAR(30) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`MessageDateTime` DATETIME NULL DEFAULT NULL,
	`CreatedDate` DATETIME NULL DEFAULT NULL,
	`CreatedBy` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	PRIMARY KEY (`ResultRowID`) USING BTREE
)
COLLATE='utf8mb4_general_ci'
ENGINE=InnoDB
AUTO_INCREMENT=54;

CREATE TABLE `tbltestanalyze_results_messageheader` (
	`RowID` BIGINT(19) NOT NULL AUTO_INCREMENT,
	`ResultRowID` BIGINT(19) NULL DEFAULT NULL,
	`FieldSeparator` VARCHAR(1) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`EncodingCharacters` VARCHAR(4) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`SendingApplication` VARCHAR(250) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`SendingFacility` VARCHAR(250) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`ReceivingApplication` VARCHAR(250) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`ReceivingFacility` VARCHAR(250) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`DateTimeMessage` VARCHAR(30) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`MessageType` VARCHAR(20) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`MessageControlID` VARCHAR(200) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`ProcessingID` VARCHAR(3) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`VersionID` VARCHAR(60) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`AcceptAckmgtType` VARCHAR(2) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`AppAckmgtType` VARCHAR(2) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`CountryCode` VARCHAR(3) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`CharacterSet` VARCHAR(20) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`PrincipalLanguageMsg` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`MessageProfileIdentifier` VARCHAR(100) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	PRIMARY KEY (`RowID`) USING BTREE
)
COLLATE='utf8mb4_general_ci'
ENGINE=InnoDB
AUTO_INCREMENT=47;

CREATE TABLE `tbltestanalyze_results_patientidentification` (
	`RowID` BIGINT(19) NOT NULL AUTO_INCREMENT,
	`ResultRowID` BIGINT(19) NULL DEFAULT NULL,
	`SetID` VARCHAR(3) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`PatientID` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`PatientIdentifierList` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`AlternatePatientID` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`PatientName` VARCHAR(200) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`MotherMaidenName` VARCHAR(250) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`DateofBirth` VARCHAR(30) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	PRIMARY KEY (`RowID`) USING BTREE
)
COLLATE='utf8mb4_general_ci'
ENGINE=InnoDB
AUTO_INCREMENT=47;

CREATE TABLE `tbltestanalyze_results_observationrequest` (
	`RowID` BIGINT(19) NOT NULL AUTO_INCREMENT,
	`ResultRowID` BIGINT(19) NULL DEFAULT NULL,
	`SetID` VARCHAR(5) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`PlacerOrderNumber` VARCHAR(100) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`FillerOrderNumber` VARCHAR(100) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`UniversalServIdentifier` VARCHAR(60) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`Priority` VARCHAR(5) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`RequestedDateTime` VARCHAR(30) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`ObservationDateTime` VARCHAR(30) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`ObservationEndDateTime` VARCHAR(30) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`CollectVolume` VARCHAR(100) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`CollectorIdentifier` LONGTEXT NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`SpecimenActionCode` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	PRIMARY KEY (`RowID`) USING BTREE
)
COLLATE='utf8mb4_general_ci'
ENGINE=InnoDB
AUTO_INCREMENT=44;

CREATE TABLE `tbltestanalyze_results_observationresult` (
	`RowID` BIGINT(19) NOT NULL AUTO_INCREMENT,
	`ResultRowID` BIGINT(19) NULL DEFAULT NULL,
	`SetID` VARCHAR(5) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`ValueType` VARCHAR(3) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`ObservationIdentifier` VARCHAR(500) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`ObservationSubID` VARCHAR(10) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`ObservationValue` VARCHAR(300) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`Units` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`ReferencesRange` VARCHAR(60) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`AbnormalFlag` VARCHAR(1) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`ObservationResultStatus` VARCHAR(1) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`ObservationDateTime` VARCHAR(30) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`ProducerID` VARCHAR(20) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`ResponsibleObserver` VARCHAR(20) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`ObservationMethod` VARCHAR(20) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`EquipmentInstanceIdentifier` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`AnalysisDateTime` VARCHAR(30) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	PRIMARY KEY (`RowID`) USING BTREE
)
COLLATE='utf8mb4_general_ci'
ENGINE=InnoDB
AUTO_INCREMENT=579;

CREATE TABLE `tbltestanalyze_results_notes` (
	`RowID` BIGINT(19) NOT NULL AUTO_INCREMENT,
	`ResultRowID` BIGINT(19) NULL DEFAULT NULL,
	`Segment` VARCHAR(10) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`SetID` VARCHAR(5) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`SourceComment` VARCHAR(10) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`Comment` LONGTEXT NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	PRIMARY KEY (`RowID`) USING BTREE
)
COLLATE='utf8mb4_general_ci'
ENGINE=InnoDB
AUTO_INCREMENT=36;

CREATE TABLE `tbltestanalyze_results_patientvisit` (
	`RowID` BIGINT NOT NULL AUTO_INCREMENT,
	`ResultRowID` BIGINT NULL DEFAULT NULL,
	`SetID` VARCHAR(5) NULL DEFAULT NULL,
	`PatientClass` VARCHAR(5) NULL DEFAULT NULL,
	`AssignedPatientLocation` VARCHAR(100) NULL DEFAULT NULL,
	`AdmissionType` VARCHAR(5) NULL DEFAULT NULL,
	`PreadmitNumber` VARCHAR(250) NULL DEFAULT NULL,
	`PriorPatientLocation` VARCHAR(100) NULL DEFAULT NULL,
	`AttendingDoctor` VARCHAR(250) NULL DEFAULT NULL,
	`ReferringDoctor` VARCHAR(250) NULL DEFAULT NULL,
	`ConsultingDoctor` VARCHAR(250) NULL DEFAULT NULL,
	`HospitalService` VARCHAR(5) NULL DEFAULT NULL,
	`TemporaryLocation` VARCHAR(100) NULL DEFAULT NULL,
	`PreadmitTestIndicator` VARCHAR(5) NULL DEFAULT NULL,
	`ReAdmissionIndicator` VARCHAR(5) NULL DEFAULT NULL,
	`AdmitSource` VARCHAR(10) NULL DEFAULT NULL,
	`AmbulatoryStatus` VARCHAR(5) NULL DEFAULT NULL,
	`VIPIndicator` VARCHAR(5) NULL DEFAULT NULL,
	`AdmittingDoctor` VARCHAR(250) NULL DEFAULT NULL,
	`PatientType` VARCHAR(5) NULL DEFAULT NULL,
	`VisitNumber` VARCHAR(250) NULL DEFAULT NULL,
	PRIMARY KEY (`RowID`)
)
COLLATE='utf8mb4_general_ci'
;

CREATE TABLE `tbltestanalyze_results_specimen` (
	`RowID` BIGINT NOT NULL AUTO_INCREMENT,
	`ResultRowID` BIGINT NULL DEFAULT NULL,
	`SetID` VARCHAR(5) NULL DEFAULT NULL,
	`SpecimenID` VARCHAR(100) NULL DEFAULT NULL,
	`SpecimentParentID` VARCHAR(100) NULL DEFAULT NULL,
	`SpecimenType` VARCHAR(250) NULL DEFAULT NULL,
	`SpecimenTypeModifier` VARCHAR(250) NULL DEFAULT NULL,
	`SpecimenAdditives` VARCHAR(250) NULL DEFAULT NULL,
	`SpecimenCollectionMethod` VARCHAR(250) NULL DEFAULT NULL,
	`SpecimenSourceSite` VARCHAR(250) NULL DEFAULT NULL,
	`SpecimenSourceSiteModifier` VARCHAR(250) NULL DEFAULT NULL,
	`SpecimenCollectionSite` VARCHAR(250) NULL DEFAULT NULL,
	`SpecimenRole` VARCHAR(250) NULL DEFAULT NULL,
	PRIMARY KEY (`RowID`)
)
COLLATE='utf8mb4_general_ci'
;

CREATE TABLE `tbltestanalyze_results_specimencontainer` (
	`RowID` BIGINT NOT NULL AUTO_INCREMENT,
	`ResultRowID` BIGINT NULL DEFAULT NULL,
	`ExternalAccessionIdentifier` VARCHAR(80) NULL DEFAULT NULL,
	`AccessionIdentifier` VARCHAR(80) NULL DEFAULT NULL,
	`ContainerIdentifier` VARCHAR(80) NULL DEFAULT NULL,
	`PrimaryContainerIdentifier` VARCHAR(80) NULL DEFAULT NULL,
	`EquipmentContainerIdentifier` VARCHAR(80) NULL DEFAULT NULL,
	`SpecimenSource` VARCHAR(300) NULL DEFAULT NULL,
	`RegistrationDateTime` VARCHAR(30) NULL DEFAULT NULL,
	`ContainerStatus` VARCHAR(250) NULL DEFAULT NULL,
	`CarrierType` VARCHAR(250) NULL DEFAULT NULL,
	`CarrierIdentifier` VARCHAR(80) NULL DEFAULT NULL,
	`PositionInCarrier` VARCHAR(80) NULL DEFAULT NULL,
	`TrayTypeSAC` VARCHAR(250) NULL DEFAULT NULL,
	`TrayIdentifier` VARCHAR(80) NULL DEFAULT NULL,
	`PositionInTray` VARCHAR(80) NULL DEFAULT NULL,
	`Location` VARCHAR(250) NULL DEFAULT NULL,
	`ContainerHeight` VARCHAR(20) NULL DEFAULT NULL,
	`ContainerDiameter` VARCHAR(20) NULL DEFAULT NULL,
	`BarrierDelta` VARCHAR(20) NULL DEFAULT NULL,
	`BottomDelta` VARCHAR(20) NULL DEFAULT NULL,
	`ContainerHeightDiamtrUnits` VARCHAR(250) NULL DEFAULT NULL,
	`ContainerVolume` VARCHAR(20) NULL DEFAULT NULL,
	`AvailableSpecimenVolume` VARCHAR(20) NULL DEFAULT NULL,
	`volumeUnits` VARCHAR(250) NULL DEFAULT NULL,
	`SeparatorType` VARCHAR(250) NULL DEFAULT NULL,
	`CapType` VARCHAR(250) NULL DEFAULT NULL,
	`Additive` VARCHAR(250) NULL DEFAULT NULL,
	`SpecimenComponent` VARCHAR(250) NULL DEFAULT NULL,
	`DilutionFactor` VARCHAR(20) NULL DEFAULT NULL,
	`Treatment` VARCHAR(250) NULL DEFAULT NULL,
	`Temperature` VARCHAR(20) NULL DEFAULT NULL,
	`HemolysisIndex` VARCHAR(20) NULL DEFAULT NULL,
	`HemolysisIndexUnits` VARCHAR(250) NULL DEFAULT NULL,
	`LipemiaIndex` VARCHAR(20) NULL DEFAULT NULL,
	`LipemiaIndexUnits` VARCHAR(250) NULL DEFAULT NULL,
	`IcterusIndex` VARCHAR(20) NULL DEFAULT NULL,
	`IcterusIndexUnits` VARCHAR(250) NULL DEFAULT NULL,
	PRIMARY KEY (`RowID`)
)
COLLATE='utf8mb4_general_ci'
;



