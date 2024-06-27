CREATE 
VIEW `userlist` AS
    SELECT 
        `mst_user`.`UserID` AS `UserID`,
        `mst_user`.`EmployeeID` AS `EmployeeID`,
        `mst_user`.`Title` AS `Title`,
        `mst_user`.`FullName` AS `FullName`,
        `mst_user`.`RegistrationNo` AS `RegistrationNo`,
        `mst_user`.`Status` AS `StatusID`,
        `mst_user`.`RoleID` AS `RoleID`,
        (SELECT 
                `mst_mastercodedata`.`CodeName`
            FROM
                `mst_mastercodedata`
            WHERE
                ((`mst_mastercodedata`.`CodeID` = `mst_user`.`Gender`)
                    AND (`mst_mastercodedata`.`CodeGroup` = 'Gender'))) AS `Gender`,
        `mst_user`.`DateofBirth` AS `DateOfBirth`,
        `mst_user`.`EmailAddress` AS `EmailAddress`,
        (SELECT 
                `mst_mastercodedata`.`CodeName`
            FROM
                `mst_mastercodedata`
            WHERE
                ((`mst_mastercodedata`.`CodeID` = `mst_user`.`Status`)
                    AND (`mst_mastercodedata`.`CodeGroup` = 'UserStatus'))) AS `Status`,
        `mst_roles`.`RoleName` AS `Role`,
        `aspnetusers`.`UserName` AS `LoginID`,
        `mst_user`.`IsDeleted` AS `IsDeleted`
    FROM
        ((`mst_user`
        LEFT JOIN `mst_roles` ON ((`mst_user`.`RoleID` = `mst_roles`.`RoleID`)))
        LEFT JOIN `aspnetusers` ON ((`mst_user`.`UserID` = `aspnetusers`.`Id`)))