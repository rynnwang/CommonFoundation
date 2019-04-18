IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ApiEvent]') AND type in (N'U'))
DROP TABLE [dbo].[ApiEvent];
GO
﻿CREATE TABLE [dbo].[ApiEvent](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWID(),
    [ServiceIdentifier] [NVARCHAR](128) NULL,
    [ServerIdentifier] [NVARCHAR](128) NULL,
    [HttpMethod]  [NVARCHAR](16) NULL,
    [Path]  [NVARCHAR](256) NULL,
    [RawUrl] [NVARCHAR](512) NULL,
    [EntryStamp] [DATETIME] NULL,
    [ExitStamp] [DATETIME] NULL,
    [ContentLength] [BIGINT] NULL,
    [CultureCode] [NVARCHAR](16) NULL,
    [ClientIdentifier] [NVARCHAR](256) NULL,
    [IpAddress] [NVARCHAR](64) NULL,
    [TraceId] [NVARCHAR](256) NULL,
    [GeoInfo] [NVARCHAR](MAX) NULL,
    [ExceptionKey] [UNIQUEIDENTIFIER] NULL,
    [Duration] [float] NULL,
    [OperatorCredential] [NVARCHAR](MAX) NULL,
    [CreatedStamp] [DATETIME] NOT NULL DEFAULT GETUTCDATE(),
CONSTRAINT [PK_ApiEvent_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key] ASC
),
CONSTRAINT [CIX_ApiEvent] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ApiMessage]') AND type in (N'U'))
DROP TABLE [dbo].[ApiMessage];
GO
﻿CREATE TABLE [dbo].[ApiMessage](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWID(),
    [Category] [NVARCHAR](256) NULL,
    [Message] [NVARCHAR](MAX) NOT NULL,
    [ServiceIdentifier] [NVARCHAR](64) NULL,
    [ServerIdentifier] [NVARCHAR](128) NULL,
    [CreatedStamp] [DATETIME] NOT NULL DEFAULT GETUTCDATE(),
CONSTRAINT [PK_ApiMessage_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key] ASC
),
CONSTRAINT [CIX_ApiMessage] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExceptionInfo]') AND type in (N'U'))
DROP TABLE [dbo].[ExceptionInfo];
GO
﻿CREATE TABLE [dbo].[ExceptionInfo](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWID(),
    [MajorCode] INT NULL,
    [MinorCode] NVARCHAR(64) NULL,
    [ServiceIdentifier] [NVARCHAR](64) NULL,
    [ServerIdentifier] [NVARCHAR](128) NULL,
    [HttpMethod]  [NVARCHAR](16) NULL,
    [Path]  [NVARCHAR](256) NULL,
    [RawUrl] [NVARCHAR](512) NULL,
    [Message] [NVARCHAR](512) NULL,
    [TargetSite] [NVARCHAR](MAX) NULL,
    [StackTrace] [NVARCHAR](MAX) NULL,
    [ExceptionType] [NVARCHAR](128) NULL,
    [Source] [NVARCHAR](512) NULL,
    [EventKey] [UNIQUEIDENTIFIER] NULL,
    [OperatorCredential] [NVARCHAR](MAX) NULL,
    [InnerException] [NVARCHAR](MAX) NULL,
    [Data] [NVARCHAR](MAX)  NULL,
    [Scene] [NVARCHAR](MAX) NULL,
    [CreatedStamp] [DATETIME] NOT NULL DEFAULT GETUTCDATE(),
CONSTRAINT [PK_ExceptionInfo_Key] PRIMARY KEY NONCLUSTERED
(
[Key] ASC
),
CONSTRAINT [CIX_ExceptionInfo] UNIQUE CLUSTERED
(
[RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON));
GO

