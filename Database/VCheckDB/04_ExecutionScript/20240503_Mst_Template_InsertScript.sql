 INSERT INTO mst_template (TemplateType, TemplateCode, TemplateTitle, TemplateContent, CreatedDate, Createdby) Values
 ("Test Schedule & Completed Test Reminder","TR01","The Test Result is Available for Viewing","The test results for Patient ID: ###<patient_id>### are now accessible. Kindly navigate to the Results section to view or print the detailed report.", now(), "Admin"),
 ("Test Schedule & Completed Test Reminder","TR02","Scheduled Test Appointment","A scheduled test is upcoming for Patient ID: ###<patient_id>###, Patient Name: ###<patient_name>###, set for ###<time>###. The attending doctor is ###<doctor_name>###.", now(), "Admin"),
 ("Analyzer Software/Firmware Update Reminder","SF01","Reminder for Software Update","Kindly remember to update your analyzer's software/firmware for optimal performance. Visit ###<link>### to verify available updates.", now(), "Admin"),
 ("Language & Country Setting","LC01","Language & Country Setting Update","You've updated the system language and country settings. Please log in again to apply the changes.", now(), "Admin"),
 ("PMS/LIS/HIS Setting","CS01","PMS/LIS/HIS Setting Update","The PMS/LIS/HIS setting has been modified by ###<admin_fullname>### (###<admin_id>###). Please log in again to apply the changes.", now(), "Admin"),
 ("Device Setting","DS01","Successful Addition of New Device","A new analyzer named ###<analyzer_name>### has been added by ###<admin_fullname>### (###<admin_id>###). Please log in again to apply the changes.", now(), "Admin"),
 ("Device Setting","DS02","Device Details Change","The analyzer previously known as ###<analyzer_name>### has been renamed to ###<new_analyzer_name>### by ###<admin_fullname>### (###<admin_id>###).", now(), "Admin"),
 ("Device Setting","DS03","Device Removal","The analyzer named ###<analyzer_name>### has been deleted by ###<admin_fullname>### (###<admin_id>###). Please log in again to apply the changes.", now(), "Admin"),
 ("User Setting","US01","Profile Update","The staff details of ###<staff_fullname>### (###<staff_id>###) has been updated by ###<admin_fullname>### (###<admin_id>###).", now(), "Admin"),
 ("User Setting","US02","Profile Update","Your staff details has been updated. Please login in again to apply the changes.", now(), "Admin"),
 ("User Setting","US03","Staff Reactivation","The user account of ###<staff_fullname>### (###<staff_id>###) has been reactivated by ###<admin_fullname>### (###<admin_id>###).", now(), "Admin"),
 ("User Setting","US04","Staff Deactivation","The user account of ###<staff_fullname>### (###<staff_id>###) has been deactivated by ###<admin_fullname>### (###<admin_id>###).", now(), "Admin"),
 ("User Setting","US05","Successful Creation of New User","The successful creation of a new user with Staff ID: ###<staff_id>###, Staff Name: ###<staff_fullname>###.", now(), "Admin"),
 ("Email Notification","EN01","New Account Creation","
 <!DOCTYPE html>
 <html>
 	<body>
 		Dear ###<staff_fullname>###, </br></br>

 		We are pleased to inform you that your account has been successfully created!</br></br>

 		Below are your login details:</br>
 		Login ID: ###<login_id>###</br>
 		Temporary Password: ###<password>###</br></br>

 		Please refrain from replying to this email as it is auto-generated.</br></br>

 		Best regards,</br>
 		VCheck Viewer Team</br>
 	</body>
 </html>
 ", now(), "Admin"),
 ("Email Notification","EN02","Password Reset & Recovery","
 <!DOCTYPE html>
 <html>
 	<body>
 		Dear ###<staff_fullname>###, </br></br>

 		This is your temporary password ###<password>###.</br></br>

 		Please utilize this temporary password to regain access to your account.</br></br>
 		
 		Please refrain from replying to this email as it is auto-generated.</br></br>

 		Best regards,</br>
 		VCheck Viewer Team</br>
 	</body>
 </html>
 ", now(), "Admin")