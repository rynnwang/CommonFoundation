CREATE TABLE [dbo].[ApiEvent](
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