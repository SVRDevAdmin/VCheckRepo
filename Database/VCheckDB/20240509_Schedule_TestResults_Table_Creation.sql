CREATE TABLE `txn_scheduledtests` (
	`ID` BIGINT(19) NOT NULL AUTO_INCREMENT,
	`ScheduledTestType` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`ScheduledDateTime` DATETIME NULL DEFAULT NULL,
	`ScheduledBy` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`PatientID` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`InchargePerson` VARCHAR(100) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`TestCompleted` TINYINT(3) NULL DEFAULT NULL,
	`CreatedDate` DATETIME NULL DEFAULT NULL,
	`CreatedBy` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`UpdatedDate` DATETIME NULL DEFAULT NULL,
	`UpdatedBy` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	PRIMARY KEY (`ID`) USING BTREE
)
COLLATE='utf8mb4_general_ci'
ENGINE=InnoDB
;

CREATE TABLE `txn_testresults` (
	`ID` BIGINT(19) NOT NULL AUTO_INCREMENT,
	`TestResultDateTime` DATETIME NULL DEFAULT NULL,
	`TestResultType` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`OperatorID` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`PatientID` VARCHAR(100) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`InchargePerson` VARCHAR(100) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`ObservationStatus` VARCHAR(100) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`TestResultStatus` VARCHAR(100) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`TestResultValue` DECIMAL(18,4) NULL DEFAULT NULL,
	`TestResultRules` VARCHAR(100) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`CreatedDate` DATETIME NULL DEFAULT NULL,
	`CreatedBy` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`UpdatedDate` DATETIME NULL DEFAULT NULL,
	`UpdatedBy` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	PRIMARY KEY (`ID`) USING BTREE
)
COLLATE='utf8mb4_general_ci'
ENGINE=InnoDB
;

