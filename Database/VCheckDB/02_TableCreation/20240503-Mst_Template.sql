use vcheckdb;

CREATE TABLE `mst_template` (
  `TemplateID` int NOT NULL AUTO_INCREMENT,
  `TemplateType` varchar(50) NOT NULL,
  `TemplateCode` varchar(5) NOT NULL,
  `TemplateTitle` varchar(200) NOT NULL,
  `TemplateContent` Text NOT NULL,
  `CreatedDate` datetime(2) DEFAULT NULL,
  `CreatedBy` varchar(50) DEFAULT NULL,
  `UpdatedDate` datetime(2) DEFAULT NULL,
  `UpdatedBy` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`TemplateID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


INSERT INTO vcheckdb.mst_template (TemplateType, TemplateCode, TemplateTitle, TemplateContent) Values
("Test Schedule & Completed Test Reminder","T01","The Test Result is Available for Viewing","The test results for Patient ID: ###<patient_id>### are now accessible. Kindly navigate to the Results section to view or print the detailed report."),
("Test Schedule & Completed Test Reminder","T02","Scheduled Test Appointment","A scheduled test is upcoming for Patient ID: ###<patient_id>###, Patient Name: ###<patient_name>###, set for ###<time>###. The attending doctor is ###<doctor_name>###."),
("Analyzer Software/Firmware Update Reminder","SF01","Reminder for Software Update","Kindly remember to update your analyzer's software/firmware for optimal performance. Visit https://www.bionote.com/software-updates to verify available updates."),
("Language & Country Setting","LC01","Language & Country Setting Update","You've updated the system language and country settings. Please log in again to apply the changes."),
("PMS/LIS/HIS Setting","CS01","PMS/LIS/HIS Setting Update","The PMS/LIS/HIS setting has been modified by ###<admin_fullname>### (###<admin_id>###). Please log in again to apply the changes."),
("Device Setting","D01","Successful Addition of New Device","A new analyzer named ###<analyzer_name>### has been added by ###<admin_fullname>### (###<admin_id>###). Please log in again to apply the changes."),
("Device Setting","D02","Device Details Change","The analyzer previously known as ###<analyzer_name>### has been renamed to ###<new_analyzer_name>### by ###<admin_fullname>### (###<admin_id>###)."),
("Device Setting","D03","Device Removal","The analyzer named ###<analyzer_name>### has been deleted by ###<admin_fullname>### (###<admin_id>###). Please log in again to apply the changes."),
("User Setting","U01","Profile Update","The staff details of ###<staff_fullname>### (###<staff_id>###) has been updated by ###<admin_fullname>### (###<admin_id>###)."),
("User Setting","U02","Profile Update","Your staff details has been updated. Please login in again to apply the changes."),
("User Setting","U03","Staff Reactivation","The user account of ###<staff_fullname>### (###<staff_id>###) has been reactivated by ###<admin_fullname>### (###<admin_id>###)."),
("User Setting","U04","Staff Deactivation","The user account of ###<staff_fullname>### (###<staff_id>###) has been deactivated by ###<admin_fullname>### (###<admin_id>###)."),
("User Setting","U05","Successful Creation of New User","The successful creation of a new user with Staff ID: ###<staff_id>###, Staff Name: ###<staff_fullname>###.")