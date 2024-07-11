CREATE TABLE `mst_template` (
  `TemplateID` int NOT NULL AUTO_INCREMENT,
  `TemplateType` varchar(50) NOT NULL,
  `TemplateCode` varchar(5) NOT NULL,
  `TemplateTitle` varchar(200) NOT NULL,
  `TemplateContent` Text NOT NULL,
  `CreatedDate` datetime(2) DEFAULT NULL,
  `CreatedBy` varchar(50) DEFAULT NULL,
  `UpdatedDate` datetime(2) DEFAULT NULL,
  `UpdatedBy` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`TemplateID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `mst_template_details` (
	`ID` INT(10) NOT NULL AUTO_INCREMENT,
	`TemplateID` INT(10) NOT NULL,
	`LangCode` VARCHAR(10) NULL DEFAULT NULL COLLATE 'utf8mb4_0900_ai_ci',
	`TemplateTitle` VARCHAR(200) NULL DEFAULT NULL COLLATE 'utf8mb4_0900_ai_ci',
	`TemplateContent` TEXT NULL DEFAULT NULL COLLATE 'utf8mb4_0900_ai_ci',
	`CreatedDate` DATETIME NULL DEFAULT NULL,
	`CreatedBy` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_0900_ai_ci',
	`UpdatedDate` DATETIME NULL DEFAULT NULL,
	`UpdatedBy` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8mb4_0900_ai_ci',
	PRIMARY KEY (`ID`) USING BTREE
)
COLLATE='utf8mb4_0900_ai_ci'
ENGINE=InnoDB
AUTO_INCREMENT=212
;