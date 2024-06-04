CREATE TABLE `txn_notification` (
  `NotificationID` int NOT NULL AUTO_INCREMENT,
  `NotificationType` varchar(50) NOT NULL,
  `NotificationTitle` varchar(200) NOT NULL,
  `NotificationContent` Text NOT NULL,
  `Receiver` varchar(500) NULL,
  `CreatedDate` datetime(2) NOT NULL,
  `CreatedBy` varchar(50) NOT NULL,
  PRIMARY KEY (`NotificationID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
