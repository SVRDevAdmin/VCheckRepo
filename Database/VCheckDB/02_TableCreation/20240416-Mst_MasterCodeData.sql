CREATE TABLE `vcheckdb`.`mst_mastercodedata` (
  `ID` INT NOT NULL AUTO_INCREMENT,
  `CodeGroup` VARCHAR(20) NOT NULL,
  `CodeID` VARCHAR(20) NOT NULL,
  `CodeName` VARCHAR(20) NOT NULL,
  `Description` VARCHAR(100) NOT NULL,
  `IsActive` BIT(1) NOT NULL,
  `SeqOrder` INT NOT NULL,
  `CreatedDate` DATETIME(2) NULL,
  `CreatedBy` VARCHAR(100) NULL,
  `UpdatedDate` DATETIME(2) NULL,
  `updatedBy` VARCHAR(100) NULL,
  PRIMARY KEY (`ID`));
  
  
Insert into `vcheckdb`.`mst_mastercodedata` (CodeGroup, CodeID, CodeName, Description, IsActive, SeqOrder)
Values
('UserStatus', 1, 'Active', 'Status of user', 1, 1),
('UserStatus', 2, 'Inactive', 'Status of user', 1, 2),
('Title', 'Mr.', 'Mister', 'Title of user', 1, 1),
('Title', 'Mrs.', 'Misses', 'Title of user', 1, 2),
('Title', 'Ms.', 'Miss', 'Title of user', 1, 3),
('Title', 'Dr.', 'Doctor', 'Title of user', 1, 4),
('Gender', 'M', 'Male', 'Gender of user', 1, 1),
('Gender', 'F', 'Female', 'Gender of user', 1, 2)