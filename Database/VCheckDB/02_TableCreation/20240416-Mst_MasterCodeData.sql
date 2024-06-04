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
  
  
Insert into `mst_mastercodedata` (CodeGroup, CodeID, CodeName, Description, IsActive, SeqOrder)
Values
('UserStatus', 1, 'Active', 'Status of user', 1, 1),
('UserStatus', 2, 'Inactive', 'Status of user', 1, 2),
('Title', 'Mr.', 'Mister', 'Title of user', 1, 1),
('Title', 'Ms.', 'Miss', 'Title of user', 1, 2),
('Title', 'Mrs.', 'Misses', 'Title of user', 1, 3),
('Title', 'Mdm.', 'Madam', 'Title of user', 1, 4),
('Title', 'Sir', 'Sir', 'Title of user', 1, 5),
('Title', 'Dr.', 'Doctor', 'Title of user', 1, 6),
('Title', 'Prof.', 'Professor', 'Title of user', 1, 7),
('Gender', 'M', 'Male', 'Gender of user', 1, 1),
('Gender', 'F', 'Female', 'Gender of user', 1, 2)
-- ,('LanguageSelection', 'zh-Hans', 'Chinese, Simplified', '', 1, 1),
-- ('LanguageSelection', 'es', 'Spanish', '', 1, 2),
-- ('LanguageSelection', 'en', 'English', '', 1, 3),
-- ('LanguageSelection', 'vi', 'Vietnamese', '', 1, 4),
-- ('LanguageSelection', 'hi', 'Hindi', '', 1, 5),
-- ('LanguageSelection', 'pt', 'Portugese', '', 1, 6),
-- ('LanguageSelection', 'ru', 'Russian', '', 1, 7),
-- ('LanguageSelection', 'ja', 'Japanese', '', 1, 8),
-- ('LanguageSelection', 'de', 'German', '', 1, 9),
-- ('LanguageSelection', 'zh-Hant', 'Chinese, Traditional', '', 1, 10),
-- ('LanguageSelection', 'id', 'Indonesia', '', 1, 11),
-- ('LanguageSelection', 'ko', 'Korean', '', 1, 12),
-- ('LanguageSelection', 'fr', 'French', '', 1, 13)