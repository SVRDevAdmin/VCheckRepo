CREATE VIEW `vcheckdb`.`userlist` AS 
select `vcheckdb`.`mst_user`.`UserID` AS `UserID`,`vcheckdb`.`mst_user`.`EmployeeID` AS `EmployeeID`,
`vcheckdb`.`mst_user`.`Title` AS `Title`,`vcheckdb`.`mst_user`.`FullName` AS `FullName`,
`vcheckdb`.`mst_user`.`RegistrationNo` AS `RegistrationNo`,
(select `vcheckdb`.`mst_mastercodedata`.`CodeName` from `vcheckdb`.`mst_mastercodedata` where ((`vcheckdb`.`mst_mastercodedata`.`CodeID` = `vcheckdb`.`mst_user`.`Gender`) and (`vcheckdb`.`mst_mastercodedata`.`CodeGroup` = 'Gender'))) AS `Gender`,
`vcheckdb`.`mst_user`.`DateofBirth` AS `DateOfBirth`,`vcheckdb`.`mst_user`.`EmailAddress` AS `EmailAddress`,
(select `vcheckdb`.`mst_mastercodedata`.`CodeName` from `vcheckdb`.`mst_mastercodedata` where ((`vcheckdb`.`mst_mastercodedata`.`CodeID` = `vcheckdb`.`mst_user`.`Status`) and (`vcheckdb`.`mst_mastercodedata`.`CodeGroup` = 'UserStatus'))) AS `Status`,
`vcheckdb`.`mst_roles`.`RoleName` AS `Role`,`vcheckdb`.`aspnetusers`.`UserName` AS `LoginID`
From (`vcheckdb`.`mst_user` 
left join `vcheckdb`.`mst_roles` on((`vcheckdb`.`mst_user`.`RoleID` = `vcheckdb`.`mst_roles`.`RoleID`))
left join `vcheckdb`.`aspnetusers` on((`vcheckdb`.`mst_user`.`UserID` = `vcheckdb`.`aspnetusers`.`Id`)));