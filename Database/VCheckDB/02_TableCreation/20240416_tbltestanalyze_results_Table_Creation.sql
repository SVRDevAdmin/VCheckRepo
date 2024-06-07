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
	`EquipmentInstanceIdentifier` VARCHAR(20) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
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



