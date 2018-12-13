/*
Warning:
1. Please execute these queries before running the program the first time.
	If you run the program before executing these queries, it will not work properly.
*/

Use COCAS;
INSERT INTO "UserType" Values ('Staff');
INSERT INTO "UserType" Values ('Student');
INSERT INTO "User" ("Username", "Password", "Type", "IsFirstLogin") Values('a', 'a', 'Staff', 'true'); /* This row is used for testing purposes, it can be deleted. */
INSERT INTO "FormType" Values('Student');
INSERT INTO "Form" ("Title", "Type") Values(N'إضافة مواد', 'Student');
INSERT INTO "Form" ("Title", "Type") Values(N'حذف مواد', 'Student');
INSERT INTO "Department" ("Code", "Name") Values('CS', N'علوم الحاسب');
INSERT INTO "Department" ("Code", "Name") Values('IT', N'تقنية المعلومات');
INSERT INTO "Department" ("Code", "Name") Values('COE', N'هندسة الحاسب');
INSERT INTO "Department" ("Code", "Name") Values('Other', N'أخرى');