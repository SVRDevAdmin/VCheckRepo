CREATE TABLE `mst_countrylist` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `CountryCode` varchar(50) NOT NULL,
  `CountryName` varchar(150) NOT NULL,
  `IsActive` bit(2) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
