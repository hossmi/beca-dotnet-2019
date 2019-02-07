USE [CarManagement]
CREATE TABLE [enrollment](
	[id] [int]  NOT NULL,
	[serial] [varchar](3) NOT NULL,
	[number] [smallint] NOT NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,
	CONSTRAINT [PK_enrollment] PRIMARY KEY NONCLUSTERED ( [id] ASC ))
	CREATE UNIQUE CLUSTERED INDEX [IX_enrollment] ON [enrollment] ([serial] ASC,[number] ASC)

CREATE TABLE [vehicle](
	[id] [int]  NOT NULL,
	[enrollmentId] [int] NOT NULL,
	[color] [smallint] NULL,
	[engineHorsePower] [smallint] NULL,
	[engineIsStarted] [bit] NULL,
	CONSTRAINT [PK_vehicle] PRIMARY KEY CLUSTERED  ( [enrollmentId] ASC ))
	ALTER TABLE [vehicle]  WITH CHECK ADD  
	CONSTRAINT [FK_vehicle_enrollment] FOREIGN KEY([enrollmentId])REFERENCES [enrollment] ([id])
	ALTER TABLE [vehicle] CHECK CONSTRAINT [FK_vehicle_enrollment]

CREATE TABLE [door](
	[id] [int]  NOT NULL,
	[vehicleId] [int] NOT NULL,
	[isOpen] [bit] NULL,
	CONSTRAINT [PK_door] PRIMARY KEY CLUSTERED ([vehicleId] ASC))
	ALTER TABLE [door]  WITH CHECK ADD  CONSTRAINT [FK_door_door] FOREIGN KEY([vehicleId])
	REFERENCES [vehicle] ([enrollmentId])
	ALTER TABLE [door] CHECK CONSTRAINT [FK_door_door]

CREATE TABLE [wheel](
	[id] [int]  NOT NULL,
	[vehicleId] [int] NOT NULL,
	[pressure] [float] NULL,
	CONSTRAINT [PK_wheel] PRIMARY KEY CLUSTERED ([vehicleId] ASC)) 
	ALTER TABLE [wheel]  WITH CHECK ADD  CONSTRAINT [FK_wheel_vehicle] FOREIGN KEY([vehicleId])
	REFERENCES [vehicle] ([enrollmentId])
	ALTER TABLE [wheel] CHECK CONSTRAINT [FK_wheel_vehicle]
	   	  