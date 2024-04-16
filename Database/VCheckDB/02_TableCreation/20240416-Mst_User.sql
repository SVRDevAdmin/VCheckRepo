use vcheckdb;

CREATE TABLE `vcheckdb`.`mst_user` (
  `UserID` INT NOT NULL AUTO_INCREMENT,
  `EmployeeID` VARCHAR(50) NOT NULL,
  `Title` VARCHAR(50) NOT NULL,
  `FirstName` VARCHAR(100) NOT NULL,
  `LastName` VARCHAR(100) NOT NULL,
  `RegistrationNo` VARCHAR(50) NOT NULL,
  `Gender` VARCHAR(2) NOT NULL,
  `DateofBirth` DATE NOT NULL,
  `EmailAddress` VARCHAR(100) NOT NULL,
  `Status` INT NOT NULL,
  `RoleID` INT NOT NULL,
  `CreatedDate` DATETIME(2) NULL,
  `CreatedBy` VARCHAR(100) NULL,
  `UpdatedDate` DATETIME(2) NULL,
  `updatedBy` VARCHAR(100) NULL,
  PRIMARY KEY (`UserID`));
    
Insert into `vcheckdb`.`mst_user` (EmployeeID, Title, FirstName, LastName, RegistrationNo, Gender, DateOfBirth, EmailAddress, Status, RoleID)
Values
('456783', 'Dr.', 'Kim', 'Taehyung', '456783', 'M', '1991-03-15', 'kimtaeh@gmail.com', 1, 2),
('456783', 'Dr.', 'Park', 'Joon-ho', '456783', 'M', '1991-03-15', 'parkjoon@gmail.com', 1, 2),
('456783', 'Dr.', 'Lee', 'Minwoo', '456783', 'M', '1991-03-15', 'minwoo@gmail.com', 1, 2)