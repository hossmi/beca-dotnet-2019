/****** Object:  Table [dbo].[enrollment]    Script Date: 02/06/2019 16:20:13 ******/
USE [CarManagement]

SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

CREATE TABLE [dbo].[enrollment]
(
	[serial] [varchar](3) NOT NULL,
	[number] [smallint] NOT NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,
	CONSTRAINT [PK_enrollment] PRIMARY KEY NONCLUSTERED ( [id] ASC )
)


SET ANSI_PADDING OFF

CREATE UNIQUE CLUSTERED INDEX [IX_enrollment] ON [dbo].[enrollment] 
(
	[serial] ASC,
	[number] ASC
)


CREATE TABLE [dbo].[vehicle](
	[enrollmentId] [int] NOT NULL,
	[color] [smallint] NULL,
	[engineHorsePower] [smallint] NULL,
	[engineIsStarted] [bit] NULL,
	CONSTRAINT [PK_vehicle] PRIMARY KEY CLUSTERED ([enrollmentId] ASC),
	CONSTRAINT [FK_vehicle_enrollment] FOREIGN KEY([enrollmentId]) REFERENCES [dbo].[enrollment] ([id])
) 


CREATE TABLE [dbo].[wheel](
	[id] [int] NOT NULL,
	[pressure] [float] NULL,
	[vehicleId] [int] NOT NULL,
	CONSTRAINT [PK_wheel] PRIMARY KEY CLUSTERED ([id] ASC),
	CONSTRAINT [FK_wheel_vehicle] FOREIGN KEY([vehicleId]) REFERENCES [dbo].[vehicle] ([enrollmentId])
)


CREATE TABLE [dbo].[door](
	[id] [int] NOT NULL,
	[isOpen] [bit] NULL,
	[vehicleId] [int] NOT NULL,
	CONSTRAINT [PK_door] PRIMARY KEY CLUSTERED ([id] ASC),
	CONSTRAINT [FK_door_vehicle] FOREIGN KEY([vehicleId]) REFERENCES [dbo].[vehicle] ([enrollmentId])
)
