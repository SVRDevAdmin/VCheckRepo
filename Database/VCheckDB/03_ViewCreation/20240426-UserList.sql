CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `root`@`localhost` 
    SQL SECURITY DEFINER
VIEW `vcheckdb`.`userlist` AS
    SELECT 
        `vcheckdb`.`mst_user`.`UserID` AS `UserID`,
        `vcheckdb`.`mst_user`.`EmployeeID` AS `EmployeeID`,
        `vcheckdb`.`mst_user`.`Title` AS `Title`,
        `vcheckdb`.`mst_user`.`FullName` AS `FullName`,
        `vcheckdb`.`mst_user`.`RegistrationNo` AS `RegistrationNo`,
        (SELECT 
                `vcheckdb`.`mst_mastercodedata`.`CodeName`
            FROM
                `vcheckdb`.`mst_mastercodedata`
            WHERE
                ((`vcheckdb`.`mst_mastercodedata`.`CodeID` = `vcheckdb`.`mst_user`.`Gender`)
                    AND (`vcheckdb`.`mst_mastercodedata`.`CodeGroup` = 'Gender'))) AS `Gender`,
        `vcheckdb`.`mst_user`.`DateofBirth` AS `DateOfBirth`,
        `vcheckdb`.`mst_user`.`EmailAddress` AS `EmailAddress`,
        (SELECT 
                `vcheckdb`.`mst_mastercodedata`.`CodeName`
            FROM
                `vcheckdb`.`mst_mastercodedata`
            WHERE
                ((`vcheckdb`.`mst_mastercodedata`.`CodeID` = `vcheckdb`.`mst_user`.`Status`)
                    AND (`vcheckdb`.`mst_mastercodedata`.`CodeGroup` = 'UserStatus'))) AS `Status`,
        `vcheckdb`.`mst_roles`.`RoleName` AS `Role`,
        `vcheckdb`.`aspnetusers`.`UserName` AS `LoginID`,
        `vcheckdb`.`mst_user`.`IsDeleted` AS `IsDeleted`
    FROM
        ((`vcheckdb`.`mst_user`
        LEFT JOIN `vcheckdb`.`mst_roles` ON ((`vcheckdb`.`mst_user`.`RoleID` = `vcheckdb`.`mst_roles`.`RoleID`)))
        LEFT JOIN `vcheckdb`.`aspnetusers` ON ((`vcheckdb`.`mst_user`.`UserID` = `vcheckdb`.`aspnetusers`.`Id`)))