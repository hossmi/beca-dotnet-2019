﻿USE [CarManagement]

CREATE TABLE [enrollment]
(
	[serial] [varchar](3) NOT NULL,
	[number] [smallint] NOT NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,
	CONSTRAINT [PK_enrollment] PRIMARY KEY NONCLUSTERED ( [id] ASC )
)

CREATE UNIQUE CLUSTERED INDEX [IX_enrollment] ON [enrollment] ( [serial] ASC, [number] ASC )

CREATE TABLE [vehicle]
(
	[enrollmentId] [int] NOT NULL,
	[color] [smallint] NULL,
	[engineHorsePower] [smallint] NULL,
	[engineIsStarted] [bit] NULL,
	CONSTRAINT [PK_vehicle] PRIMARY KEY CLUSTERED ([enrollmentId] ASC),
	CONSTRAINT [FK_vehicle_enrollment] FOREIGN KEY([enrollmentId]) REFERENCES [enrollment] ([id]),
)

CREATE TABLE [dbo].[wheel]
(
	[vehicleId] [int] NOT NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,
	[pressure] [float] NULL,
	CONSTRAINT [PK_wheel] PRIMARY KEY CLUSTERED ([id] ASC),
	CONSTRAINT [FK_wheel_vehicle] FOREIGN KEY([vehicleId]) REFERENCES [dbo].[vehicle] ([enrollmentId])
)

CREATE TABLE [dbo].[door]
(
	[vehicleId] [int] NOT NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,
	[isOpen] [bit] NULL,
	CONSTRAINT [PK_door] PRIMARY KEY CLUSTERED ([id] ASC),
	CONSTRAINT [FK_door_vehicle] FOREIGN KEY([vehicleId]) REFERENCES [dbo].[vehicle] ([enrollmentId])
)
