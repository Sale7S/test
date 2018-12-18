/*
Warning:
1. Please execute these queries before running the program the first time.
	If you run the program before executing these queries, it will not work properly.
*/

Use COCAS;
INSERT INTO "UserType" ("Type", "TypeAr") Values ('Staff', N'موظف');
INSERT INTO "UserType" Values ('Student', N'طالب');
INSERT INTO "UserType" Values ('HoD', N'رئيس القسم');
INSERT INTO "UserType" Values ('Dean', N'العميد');
INSERT INTO "User" ("Username", "Password", "Type", "IsFirstLogin") Values('staff', 'staff', 'Staff', 'true'); /* This row is used for testing purposes, it can be deleted. */
INSERT INTO "User" ("Username", "Password", "Type", "IsFirstLogin") Values('cs_head', 'cs_head', 'HoD', 'true'); /* This row is used for testing purposes, it can be deleted. */
INSERT INTO "User" ("Username", "Password", "Type", "IsFirstLogin") Values('it_head', 'it_head', 'HoD', 'true'); /* This row is used for testing purposes, it can be deleted. */
INSERT INTO "User" ("Username", "Password", "Type", "IsFirstLogin") Values('coe_head', 'coe_head', 'HoD', 'true'); /* This row is used for testing purposes, it can be deleted. */
INSERT INTO "User" ("Username", "Password", "Type", "IsFirstLogin") Values('dean', 'dean_head', 'Dean', 'true'); /* This row is used for testing purposes, it can be deleted. */
INSERT INTO "Form" ("Title", "Type") Values(N'إضافة', 'Student');
INSERT INTO "Form" ("Title", "Type") Values(N'حذف', 'Student');
/*
INSERT INTO "Form" ("Title", "Type") Values(N'إضافة اختياري', 'Student'); //To be added soon
INSERT INTO "Form" ("Title", "Type") Values(N'إضافة من خارج الكلية', 'Student');
INSERT INTO "Form" ("Title", "Type") Values(N'معادلة', 'Student');
*/
INSERT INTO "Department" ("Code", "Name") Values('CS', N'علوم الحاسب');
INSERT INTO "Department" ("Code", "Name") Values('IT', N'تقنية المعلومات');
INSERT INTO "Department" ("Code", "Name") Values('COE', N'هندسة الحاسب');
INSERT INTO "Department" ("Code", "Name") Values('Other', N'أخرى');
INSERT INTO "HoD" ("FullName", "DepartmentCode", "Username") Values(N'سليمان السحيباني', 'CS', 'cs_head');
INSERT INTO "HoD" ("FullName", "DepartmentCode", "Username") Values(N'صالح الباهلي', 'IT', 'it_head');
INSERT INTO "HoD" ("FullName", "DepartmentCode", "Username") Values(N'رئيس قسم هندسة الحاسب', 'COE', 'coe_head');