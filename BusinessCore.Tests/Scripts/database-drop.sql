USE [CarManagement]

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_vehicle_enrollment]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[vehicle]'))
	ALTER TABLE [dbo].[vehicle] DROP CONSTRAINT [FK_vehicle_enrollment]

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wheel_vehicle]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[wheel]'))
	ALTER TABLE [dbo].[wheel] DROP CONSTRAINT [FK_wheel_vehicle]

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_door_vehicle]') 
	AND parent_object_id = OBJECT_ID(N'[dbo].[door]'))
	ALTER TABLE [dbo].[door] DROP CONSTRAINT [FK_door_vehicle]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[enrollment]') AND type in (N'U'))
	DROP TABLE [dbo].[enrollment]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[vehicle]') AND type in (N'U'))
	DROP TABLE [dbo].[vehicle]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wheel]') AND type in (N'U'))
	DROP TABLE [dbo].[wheel]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[door]') AND type in (N'U'))
DROP TABLE [dbo].[door]