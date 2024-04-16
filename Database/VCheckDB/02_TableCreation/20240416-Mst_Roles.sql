use vcheckdb;

CREATE TABLE `vcheckdb`.`mst_roles` (
  `RoleID` INT NOT NULL AUTO_INCREMENT,
  `RoleName` VARCHAR(100) NOT NULL,
  `IsActive` BIT(1) NOT NULL,
  `IsSuperadmin` BIT(1) NOT NULL,
  `IsAdmin` BIT(1) NOT NULL,
  `CreatedDate` DATETIME(2) NULL,
  `CreatedBy` VARCHAR(100) NULL,
  `UpdatedDate` DATETIME(2) NULL,
  `updatedBy` VARCHAR(100) NULL,
  PRIMARY KEY (`RoleID`));


INSERT INTO `vcheckdb`.`mst_roles`(`RoleName`,`IsActive`,`IsSuperadmin`,`IsAdmin`)
VALUES
('Superadmin',1,1,1),
('Lab SuperAdmin',1,0,1),
('Lab User',1,0,0);