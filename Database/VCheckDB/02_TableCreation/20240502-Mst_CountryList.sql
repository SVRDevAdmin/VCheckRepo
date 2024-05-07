Use vcheckdb;

CREATE TABLE `mst_countrylist` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `CountryCode` varchar(50) NOT NULL,
  `CountryName` varchar(150) NOT NULL,
  `IsActive` bit(2) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

Insert into mst_countrylist (CountryCode, CountryName, IsActive) Values
('001','Afghanistan',1),
('002','Albania',1),
('003','Algeria',1),
('004','Andorra',1),
('005','Angola',1),
('006','Antigua and Barbuda',1),
('007','Argentina',1),
('008','Armenia',1),
('009','Austria',1),
('010','Azerbaijan',1),
('011','Bahrain',1),
('012','Bangladesh',1),
('013','Barbados',1),
('014','Belarus',1),
('015','Belgium',1),
('016','Belize',1),
('017','Benin',1),
('018','Bhutan',1),
('019','Bolivia',1),
('020','Bosnia and Herzegovina',1)