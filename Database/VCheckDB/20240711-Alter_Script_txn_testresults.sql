ALTER TABLE `txn_testresults`
CHANGE COLUMN `ObservationStatus` `ObservationStatus` VARCHAR(300) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci' AFTER `InchargePerson`;