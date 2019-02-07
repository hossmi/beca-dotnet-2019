﻿USE [CarManagement]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_vehicle_enrollment]') AND parent_object_id = OBJECT_ID(N'[dbo].[vehicle]'))
ALTER TABLE [dbo].[vehicle] DROP CONSTRAINT [FK_vehicle_enrollment]
GO

USE [CarManagement]
GO

/****** Object:  Table [dbo].[vehicle]    Script Date: 02/06/2019 16:20:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[vehicle]') AND type in (N'U'))
DROP TABLE [dbo].[vehicle]
GO


USE [CarManagement]
GO

/****** Object:  Table [dbo].[enrollment]    Script Date: 02/06/2019 15:57:47 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[enrollment]') AND type in (N'U'))
DROP TABLE [dbo].[enrollment]
GO

USE [CarManagement]
GO

/****** Object:  Table [dbo].[Wheels]    Script Date: 07/02/2019 12:52:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Wheels]') AND type in (N'U'))
DROP TABLE [dbo].[Wheels]
GO

USE [CarManagement]
GO

/****** Object:  Table [dbo].[Doors]    Script Date: 07/02/2019 12:56:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Drop]') AND type in (N'U'))
DROP TABLE [dbo].[Doors]
GO





