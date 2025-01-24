INSERT INTO `vcheckdb`.`mst_roles`(`RoleID`,`RoleName`,`IsActive`,`IsSuperadmin`,`IsAdmin`,`CreatedDate`,`CreatedBy`) VALUES
('4d1b3040-d871-4c86-a361-de26a6224bad','Superadmin',1,1,0,NOW(), 'SYSTEM'),
('81d73be5-703a-4e0b-bf38-ad01fb74e01f','Lab User',1,0,0,NOW(), 'SYSTEM'),
('c5bf0730-2ef0-456d-abe5-b35c58ecb329','Lab Superadmin',1,0,1,NOW(), 'SYSTEM');

INSERT INTO `vcheckdb`.`mst_user`(`UserID`,`EmployeeID`,`Title`,`FullName`,`RegistrationNo`,`Gender`,`DateofBirth`,`EmailAddress`,`Status`,`RoleID`,`IsDeleted`,`CreatedDate`,`CreatedBy`) VALUES
('1ada0905-ed03-4310-b444-49bbec3ab4567','Superadmin','Mr.','Superadmin','Superadmin','M',NOW(),'superadmin@vcheck.com',1,'4d1b3040-d871-4c86-a361-de26a6224bad',0,NOW(), 'SYSTEM');
