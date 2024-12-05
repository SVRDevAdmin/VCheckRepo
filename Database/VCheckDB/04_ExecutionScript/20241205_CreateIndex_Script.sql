
/*------ Mst_Cliet ----------*/
CREATE INDEX IX_ID_Status
ON mst_client(ID, STATUS);

CREATE INDEX IX_ClientKey_DateRange
ON mst_client_auth(ClientKey, StartDate, EndDate);

/*------- Test Results --------*/
CREATE INDEX IX_TestResultID_PatientID
ON txn_testresults(ID, PatientID);

CREATE INDEX IX_TestResultRowID
ON txn_testresults_details(TestResultRowID);

CREATE INDEX IX_ID_TestResultDateTime_DESC
ON txn_testresults(ID, TestResultDateTime DESC);

CREATE INDEX IX_TestResultDateTime
ON txn_testresults(TestResultDateTime);


/*-------- Device List ---------*/
CREATE INDEX IX_Device_Status
ON mst_deviceslist(Status);

CREATE INDEX IX_DeviceType_Status_Seqno
ON mst_devicetype(STATUS, Seqno ASC);

CREATE INDEX IX_DeviceType_Status
ON mst_devicetype(STATUS);

/*--------- Processing Log --------*/
CREATE INDEX IX_TaskName_ProcesisngDataDESC
ON system_processinglog(ProcessingTaskName, ProcessingStartDate DESC);

CREATE INDEX IX_TaskName
ON system_processinglog(ProcessingTaskName);

/*---------- Schedule Test ---------*/
CREATE INDEX IX_ScheduledDateTime
ON txn_scheduledtests(ScheduledDateTime);

CREATE INDEX IX_ScheduledDateTime_DESC
ON txn_scheduledtests(ScheduledDateTime DESC);

/*---------- Mst Configuration ---------*/
CREATE INDEX IX_configurationKey
ON mst_configuration(ConfigurationKey);


/*---------- Mst Country ---------------*/
CREATE INDEX IX_countryname
ON mst_countrylist(CountryName);

/*---------- MastercodeData -------------*/
CREATE INDEX IX_CodeGroup_IsActive
ON mst_mastercodedata(CodeGroup, IsActive);

/*----------- Mst Template --------------*/
CREATE INDEX IX_TemplateCode
ON mst_template(TemplateCode);

CREATE INDEX IX_TemplateID_TemplateCode
ON mst_template(TemplateID, TemplateCode);

CREATE INDEX IX_TemplateID_LangCode
ON mst_template_details(TemplateID, LangCode);