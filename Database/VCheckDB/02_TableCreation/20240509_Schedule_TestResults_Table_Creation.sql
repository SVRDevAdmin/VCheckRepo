CREATE TABLE `txn_scheduledtests` (
	`ID` BIGINT NOT NULL AUTO_INCREMENT,
	`ScheduledTestType` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`ScheduledDateTime` DATETIME NULL DEFAULT NULL,
	`ScheduledBy` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`PatientID` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`PatientName` VARCHAR(200) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`Gender` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`Species` VARCHAR(100) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`OwnerName` VARCHAR(200) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`InchargePerson` VARCHAR(100) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`ScheduleTestStatus` TINYINT NULL DEFAULT NULL,
	`ScheduleUniqueID` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`TestCompleted` TINYINT NULL DEFAULT NULL,
	`CreatedDate` DATETIME NULL DEFAULT NULL,
	`CreatedBy` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`UpdatedDate` DATETIME NULL DEFAULT NULL,
	`UpdatedBy` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	PRIMARY KEY (`ID`) USING BTREE,
	INDEX `IX_UniqueID` (`ScheduleUniqueID`) USING BTREE,
	INDEX `IX_ScheduledDateTime` (`ScheduledDateTime`) USING BTREE,
	INDEX `IX_ScheduledDateTime_DESC` (`ScheduledDateTime` DESC) USING BTREE
)
COLLATE='utf8mb4_general_ci'
ENGINE=InnoDB
;


CREATE TABLE `txn_testresults` (
	`ID` BIGINT NOT NULL AUTO_INCREMENT,
	`TestResultDateTime` DATETIME NULL DEFAULT NULL,
	`TestResultType` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`OperatorID` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`PatientID` VARCHAR(100) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`InchargePerson` VARCHAR(100) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`OverallStatus` VARCHAR(300) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`CreatedDate` DATETIME NULL DEFAULT NULL,
	`CreatedBy` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`UpdatedDate` DATETIME NULL DEFAULT NULL,
	`UpdatedBy` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`DeviceSerialNo` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	PRIMARY KEY (`ID`) USING BTREE,
	INDEX `IX_TestResultID_PatientID` (`ID`, `PatientID`) USING BTREE,
	INDEX `IX_ID_TestResultDateTime_DESC` (`ID`, `TestResultDateTime` DESC) USING BTREE,
	INDEX `IX_TestResultDateTime` (`TestResultDateTime`) USING BTREE
)
COLLATE='utf8mb4_general_ci'
ENGINE=InnoDB
;

CREATE TABLE `txn_testresults_details` (
	`ID` BIGINT NOT NULL AUTO_INCREMENT,
	`TestResultRowID` BIGINT NULL DEFAULT NULL,
	`TestParameter` VARCHAR(100) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`SubID` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`ProceduralControl` VARCHAR(300) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`TestResultStatus` VARCHAR(100) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`TestResultValue` VARCHAR(300) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`TestResultUnit` VARCHAR(100) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`ReferenceRange` VARCHAR(100) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`Interpretation` VARCHAR(100) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	PRIMARY KEY (`ID`) USING BTREE,
	INDEX `IX_TestResultRowID` (`TestResultRowID`) USING BTREE
)
COLLATE='utf8mb4_general_ci'
ENGINE=InnoDB
;



