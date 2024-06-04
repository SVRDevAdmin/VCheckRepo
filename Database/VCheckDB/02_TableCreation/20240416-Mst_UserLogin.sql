CREATE TABLE `mst_userlogin` (
  `UserLoginID` INT NOT NULL AUTO_INCREMENT,
  `UserID` VARCHAR(50) NOT NULL,
  `LoginID` VARCHAR(50) NOT NULL,
  `LoginPassword` VARCHAR(100) NOT NULL,
  `IsLocked` BIT(1) NOT NULL,
  `LockedDateTime` DATETIME(2) NULL,
  `LastLoginDateTime` DATETIME(2) NULL,
  PRIMARY KEY (`UserLoginID`));