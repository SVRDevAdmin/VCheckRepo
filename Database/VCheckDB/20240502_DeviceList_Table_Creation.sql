CREATE TABLE `mst_deviceslist` (
	`ID` INT(10) NOT NULL AUTO_INCREMENT,
	`DeviceName` VARCHAR(100) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`Description` VARCHAR(200) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`DeviceIPAddress` VARCHAR(100) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`DeviceImagePath` VARCHAR(500) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`Status` INT(10) NULL DEFAULT NULL,
	`CreatedDate` DATETIME NULL DEFAULT NULL,
	`CreatedBy` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	`UpdatedDate` DATETIME NULL DEFAULT NULL,
	`UpdatedBy` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	PRIMARY KEY (`ID`) USING BTREE
)
COLLATE='utf8mb4_general_ci'
ENGINE=InnoDB
;