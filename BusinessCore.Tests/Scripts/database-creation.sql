/****** Object:  Table [dbo].[enrollment]    Script Date: 02/06/2019 16:20:13 ******/
USE [CarManagement]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[enrollment]
(
	[serial] [varchar](3) NOT NULL,
	[number] [smallint] NOT NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,
	CONSTRAINT [PK_enrollment] PRIMARY KEY NONCLUSTERED ( [id] ASC )
	WITH (
		PAD_INDEX  = OFF, 
		STATISTICS_NORECOMPUTE  = OFF, 
		IGNORE_DUP_KEY = OFF, 
		ALLOW_ROW_LOCKS  = ON, 
		ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


USE [CarManagement]
GO

CREATE UNIQUE CLUSTERED INDEX [IX_enrollment] ON [dbo].[enrollment] 
(
	[serial] ASC,
	[number] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


USE [CarManagement]
GO

/****** Object:  Table [dbo].[vehicle]    Script Date: 02/06/2019 16:20:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[vehicle](
	[enrollmentId] [int] NOT NULL,
	[color] [smallint] NULL,
	[engineHorsePower] [smallint] NULL,
	[engineIsStarted] [bit] NULL,
 CONSTRAINT [PK_vehicle] PRIMARY KEY CLUSTERED 
(
	[enrollmentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[vehicle]  WITH CHECK ADD  CONSTRAINT [FK_vehicle_enrollment] FOREIGN KEY([enrollmentId])
REFERENCES [dbo].[enrollment] ([id])
GO

ALTER TABLE [dbo].[vehicle] CHECK CONSTRAINT [FK_vehicle_enrollment]
GO

USE [CarManagement]
GO

/****** Object:  Table [dbo].[wheel]    Script Date: 06/02/2019 16:45:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

USE [CarManagement]
GO

/****** Object:  Table [dbo].[door]    Script Date: 06/02/2019 16:56:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[door](
	[vehicleId] [int] NOT NULL,
	[id] [int] NOT NULL,
	[isOpen] [bit] NOT NULL,
 CONSTRAINT [PK_door] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[door] ADD  CONSTRAINT [DF_door_isOpen]  DEFAULT ((0)) FOR [isOpen]
GO

ALTER TABLE [dbo].[door]  WITH CHECK ADD  CONSTRAINT [FK_door_vehicle1] FOREIGN KEY([vehicleId])
REFERENCES [dbo].[vehicle] ([enrollmentId])
GO

ALTER TABLE [dbo].[door] CHECK CONSTRAINT [FK_door_vehicle1]
GO

USE [CarManagement]
GO

/****** Object:  Table [dbo].[wheel]    Script Date: 06/02/2019 17:08:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[wheel](
	[id] [int] NOT NULL,
	[vehicleId] [int] NOT NULL,
	[pressure] [float] NULL,
 CONSTRAINT [PK_wheel] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[wheel]  WITH CHECK ADD  CONSTRAINT [FK_wheel_vehicle1] FOREIGN KEY([id])
REFERENCES [dbo].[vehicle] ([enrollmentId])
GO

ALTER TABLE [dbo].[wheel] CHECK CONSTRAINT [FK_wheel_vehicle1]
GO

