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
INSERT INTO "User" ("Username", "Password", "Type", "IsFirstLogin") Values('a', 'a', 'Staff', 'true'); /* This row is used for testing purposes, it can be deleted. */
INSERT INTO "User" ("Username", "Password", "Type", "IsFirstLogin") Values('b', 'b', 'HoD', 'true'); /* This row is used for testing purposes, it can be deleted. */
INSERT INTO "User" ("Username", "Password", "Type", "IsFirstLogin") Values('c', 'c', 'Dean', 'true'); /* This row is used for testing purposes, it can be deleted. */
INSERT INTO "Form" ("Title", "Type") Values(N'إضافة', 'Student');
INSERT INTO "Form" ("Title", "Type") Values(N'حذف', 'Student');
INSERT INTO "Department" ("Code", "Name") Values('CS', N'علوم الحاسب');
INSERT INTO "Department" ("Code", "Name") Values('IT', N'تقنية المعلومات');
INSERT INTO "Department" ("Code", "Name") Values('COE', N'هندسة الحاسب');
INSERT INTO "Department" ("Code", "Name") Values('Other', N'أخرى');