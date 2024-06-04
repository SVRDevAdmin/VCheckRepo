CREATE TABLE `mst_client` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) NOT NULL,
  `Description` varchar(200) NOT NULL,
  `Status` int NOT NULL,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` varchar(50) NOT NULL,
  `UpdatedDate` datetime DEFAULT NULL,
  `UpdatedBy` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `mst_client_auth` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `ClientID` int NOT NULL,
  `ClientKey` varchar(50) NOT NULL,
  `StartDate` date NOT NULL,
  `EndDate` date NOT NULL,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` varchar(50) NOT NULL,
  `UpdatedDate` date DEFAULT NULL,
  `UpdatedBy` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;



-- Insert Into `mst_client`(Name, Description, Status, CreatedDate, CreatedBy) values 
-- ("TestClient", "TestClient", 1, "2024-05-23", "Admin");

-- Insert Into `mst_client_auth`(ClientID, ClientKey, StartDate, EndDate, CreatedDate, CreatedBy) values 
-- (1, "TestClient", "2024-05-22", "2024-05-24", "2024-05-23", "Admin");
