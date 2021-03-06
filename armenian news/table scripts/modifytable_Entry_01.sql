/*
   Saturday, December 7, 20135:09:01 PM
   User: 
   Server: MHER\SQLEXPRESS
   Database: Hilur
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Entry ADD
	ImageSrc varchar(300) NULL,
	ImageWidth int NULL,
	ImageHeight int NULL,
	ImageTitle varchar(100) NULL
GO
ALTER TABLE dbo.Entry SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
