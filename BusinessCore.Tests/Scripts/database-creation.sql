
USE [CarManagement]

CREATE TABLE [enrollment]
(
	[serial] [varchar](3) NOT NULL,
	[number] [smallint] NOT NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,
	CONSTRAINT [PK_enrollment] PRIMARY KEY NONCLUSTERED ( [id] ASC )
)

CREATE UNIQUE CLUSTERED INDEX [IX_enrollment] ON [enrollment] ([serial] ASC,[number] ASC)



CREATE TABLE [vehicle](

	[enrollmentId] [int] NOT NULL,
	[color] [smallint] NULL,
	[engineHorsePower] [smallint] NULL,
	[engineIsStarted] [bit] NULL,

CONSTRAINT [PK_vehicle] PRIMARY KEY CLUSTERED (	[enrollmentId] ASC)
) 


ALTER TABLE [dbo].[vehicle]  WITH CHECK ADD  CONSTRAINT [FK_vehicle_enrollment] FOREIGN KEY([enrollmentId])
REFERENCES [dbo].[enrollment] ([id])

ALTER TABLE [dbo].[vehicle] CHECK CONSTRAINT [FK_vehicle_enrollment]


CREATE TABLE [dbo].[Wheels](

	[pressure] [float] NOT NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,

 CONSTRAINT [PK_Wheels] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)
) 

CREATE TABLE [dbo].[Doors](

	[vehicleid] [int] NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[isopen] [bit] NULL,

 CONSTRAINT [PK_Doors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
) 






