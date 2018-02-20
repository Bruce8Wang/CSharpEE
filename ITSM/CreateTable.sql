USE [ITSM]
GO

/****** Object:  Table [dbo].[DealMethods]    Script Date: 2015-05-28 9:40:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DealMethods](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[Note] [nvarchar](500) NULL,
 CONSTRAINT [PK_dbo.DealMethods] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[Emloyees]    Script Date: 2015-05-28 9:40:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Emloyees](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Emloyees] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FaultTypes]    Script Date: 2015-05-28 9:40:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FaultTypes](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[Note] [nvarchar](500) NULL,
 CONSTRAINT [PK_dbo.FaultTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FlowConfigs]    Script Date: 2015-05-28 9:40:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FlowConfigs](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Level] [int] NOT NULL,
	[Dealer] [nvarchar](20) NOT NULL,
	[Note] [nvarchar](500) NULL,
	[EMail] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.FlowConfigs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[OnwayFlows]    Script Date: 2015-05-28 9:40:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OnwayFlows](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RepairAppyBillId] [bigint] NOT NULL,
	[CurrentDealer] [nvarchar](20) NULL,
	[NextDealer] [nvarchar](20) NULL,
	[EMail] [nvarchar](50) NULL,
	[DealMethodId] [bigint] NOT NULL,
	[DealNote] [nvarchar](max) NULL,
	[DealDate] [datetime] NOT NULL,
	[DealProcess] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.OnwayFlows] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[Priorities]    Script Date: 2015-05-28 9:40:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Priorities](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Level] [int] NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_dbo.Priorities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RepairApplyBills]    Script Date: 2015-05-28 9:40:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RepairApplyBills](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BillNo] [nvarchar](50) NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Note] [nvarchar](max) NOT NULL,
	[FaultTypeId] [bigint] NOT NULL,
	[ApplyEmployee] [nvarchar](20) NOT NULL,
	[ApplyDept] [nvarchar](50) NOT NULL,
	[BXDate] [datetime] NOT NULL,
	[PrevOperation] [nvarchar](max) NULL,
	[PriorityId] [bigint] NOT NULL,
	[HopeTime] [datetime] NOT NULL,
	[BXDealTime] [datetime] NOT NULL,
	[BXDealNote] [nvarchar](max) NULL,
	[ImagePath] [nvarchar](max) NULL,
	[VedioPath] [nvarchar](max) NULL,
	[BXEmployee] [nvarchar](20) NOT NULL,
	[BXDealEmployee] [nvarchar](20) NULL,
	[NextEmployee] [nvarchar](max) NULL,
	[BXDept] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](50) NULL,
	[EMail] [nvarchar](50) NULL,
	[AssetCode] [nvarchar](max) NOT NULL,
	[ComputerName] [nvarchar](50) NOT NULL,
	[StatusId] [bigint] NOT NULL,
	[SatisfactionLevelId] [bigint] NOT NULL,
	[DeviceType] [nvarchar](10) NULL,
	[BXDealProcess] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.RepairApplyBills] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SatisfactionLevels]    Script Date: 2015-05-28 9:40:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SatisfactionLevels](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Level] [int] NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_dbo.SatisfactionLevels] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[Status]    Script Date: 2015-05-28 9:40:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Status](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[Note] [nvarchar](500) NULL,
 CONSTRAINT [PK_dbo.Status] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[SuperUsers]    Script Date: 2015-05-28 9:40:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuperUsers](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NULL,
	[PermLevl] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.SuperUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[OnwayFlows]  WITH CHECK ADD  CONSTRAINT [FK_dbo.OnwayFlows_dbo.DealMethods_DealMethodId] FOREIGN KEY([DealMethodId])
REFERENCES [dbo].[DealMethods] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[OnwayFlows] CHECK CONSTRAINT [FK_dbo.OnwayFlows_dbo.DealMethods_DealMethodId]
GO

ALTER TABLE [dbo].[OnwayFlows]  WITH CHECK ADD  CONSTRAINT [FK_dbo.OnwayFlows_dbo.RepairApplyBills_RepairAppyBillId] FOREIGN KEY([RepairAppyBillId])
REFERENCES [dbo].[RepairApplyBills] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[OnwayFlows] CHECK CONSTRAINT [FK_dbo.OnwayFlows_dbo.RepairApplyBills_RepairAppyBillId]
GO

ALTER TABLE [dbo].[RepairApplyBills]  WITH CHECK ADD  CONSTRAINT [FK_dbo.RepairApplyBills_dbo.FaultTypes_FaultTypeId] FOREIGN KEY([FaultTypeId])
REFERENCES [dbo].[FaultTypes] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[RepairApplyBills] CHECK CONSTRAINT [FK_dbo.RepairApplyBills_dbo.FaultTypes_FaultTypeId]
GO

ALTER TABLE [dbo].[RepairApplyBills]  WITH CHECK ADD  CONSTRAINT [FK_dbo.RepairApplyBills_dbo.Priorities_PriorityId] FOREIGN KEY([PriorityId])
REFERENCES [dbo].[Priorities] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[RepairApplyBills] CHECK CONSTRAINT [FK_dbo.RepairApplyBills_dbo.Priorities_PriorityId]
GO

ALTER TABLE [dbo].[RepairApplyBills]  WITH CHECK ADD  CONSTRAINT [FK_dbo.RepairApplyBills_dbo.SatisfactionLevels_SatisfactionLevelId] FOREIGN KEY([SatisfactionLevelId])
REFERENCES [dbo].[SatisfactionLevels] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[RepairApplyBills] CHECK CONSTRAINT [FK_dbo.RepairApplyBills_dbo.SatisfactionLevels_SatisfactionLevelId]
GO

ALTER TABLE [dbo].[RepairApplyBills]  WITH CHECK ADD  CONSTRAINT [FK_dbo.RepairApplyBills_dbo.Status_StatusId] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[RepairApplyBills] CHECK CONSTRAINT [FK_dbo.RepairApplyBills_dbo.Status_StatusId]
GO


