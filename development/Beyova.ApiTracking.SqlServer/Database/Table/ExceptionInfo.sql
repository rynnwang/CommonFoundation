CREATE TABLE [dbo].[ExceptionInfo](
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