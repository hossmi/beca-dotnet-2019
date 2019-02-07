/****** Object:  Table [dbo].[enrollment]    Script Date: 02/06/2019 16:20:13 ******/
USE [CarManagement]

CREATE TABLE [dbo].[enrollment]
(
	[id] [int] IDENTITY(1,1) NOT NULL,
	[serial] [varchar](3) NOT NULL,
	[number] [smallint] NOT NULL,
	CONSTRAINT [PK_enrollment] PRIMARY KEY NONCLUSTERED ( [id] ASC )
)

CREATE UNIQUE CLUSTERED INDEX [IX_enrollment] ON [dbo].[enrollment] ( [serial] ASC, [number] ASC )


/****** Object:  Table [dbo].[vehicle]    Script Date: 02/06/2019 16:20:13 ******/
CREATE TABLE [dbo].[vehicle](
	[enrollmentId] [int] NOT NULL,
	[color] [smallint] NULL,
	[engineHorsePower] [smallint] NULL,
	[engineIsStarted] [bit] NULL,
 CONSTRAINT [PK_vehicle] PRIMARY KEY CLUSTERED ( [enrollmentId] ASC ),
 CONSTRAINT [FK_vehicle_enrollment] FOREIGN KEY([enrollmentId]) REFERENCES [dbo].[enrollment] ([id])
)

/****** Object:  Table [dbo].[door]    Script Date: 06/02/2019 16:56:09 ******/
CREATE TABLE [dbo].[door](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[vehicleId] [int] NOT NULL,
	[isOpen] [bit] NOT NULL,
 CONSTRAINT [PK_door] PRIMARY KEY CLUSTERED([id] ASC),
 CONSTRAINT [FK_door_vehicle] FOREIGN KEY([vehicleId]) REFERENCES [dbo].[vehicle] ([enrollmentId])
)

ALTER TABLE [dbo].[door] ADD  CONSTRAINT [DF_door_isOpen]  DEFAULT ((0)) FOR [isOpen]

/****** Object:  Table [dbo].[wheel]    Script Date: 06/02/2019 17:08:08 ******/
CREATE TABLE [dbo].[wheel](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[vehicleId] [int] NOT NULL,
	[pressure] [float] NULL,
 CONSTRAINT [PK_wheel] PRIMARY KEY CLUSTERED ( [id] ASC ),
 CONSTRAINT [FK_wheel_vehicle] FOREIGN KEY([vehicleId]) REFERENCES [dbo].[vehicle] ([enrollmentId])
)

