CREATE TABLE `mst_mastercodedata` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `CodeGroup` varchar(20) NOT NULL,
  `CodeID` varchar(20) NOT NULL,
  `CodeName` varchar(20) NOT NULL,
  `Description` varchar(100) NOT NULL,
  `IsActive` bit(1) NOT NULL,
  `SeqOrder` int NOT NULL,
  `CreatedDate` datetime(2) DEFAULT NULL,
  `CreatedBy` varchar(100) DEFAULT NULL,
  `UpdatedDate` datetime(2) DEFAULT NULL,
  `updatedBy` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
  