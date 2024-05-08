CREATE TABLE `mst_configuration` (
  `ConfigurationID` int NOT NULL AUTO_INCREMENT,
  `ConfigurationKey` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
  `ConfigurationValue` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`ConfigurationID`,`ConfigurationKey`,`ConfigurationValue`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

Insert into vcheckdb.mst_configuration (ConfigurationKey, ConfigurationValue) Values
("SystemSettings_Country","160"),
("SystemSettings_Language","en")