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

ALTER TABLE `tbltestanalyze_results_observationresult`
CHANGE COLUMN `EquipmentInstanceIdentifier` `EquipmentInstanceIdentifier` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci' AFTER `ObservationMethod`;
	
	