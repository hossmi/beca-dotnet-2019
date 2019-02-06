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


