ALTER TABLE `mst_deviceslist`
ADD COLUMN `DeviceSerialNo` VARCHAR(50) NULL DEFAULT NULL AFTER `DeviceName`;

ALTER TABLE `txn_testresults`
ADD COLUMN `DeviceSerialNo` VARCHAR(50) NULL DEFAULT NULL AFTER `OperatorID`;