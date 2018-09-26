IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ApiEvent]') AND type in (N'U'))
BEGIN
DROP TABLE [dbo].[ApiEvent];
END
GO

CREATE TABLE [dbo].[ApiEvent](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWID(),
    [ServiceIdentifier] [NVARCHAR](128) NULL,
    [ServerIdentifier] [NVARCHAR](128) NULL,
    [ServerHost] [NVARCHAR](128) NULL,
    [RawUrl] [NVARCHAR](512) NULL,
    [EntryStamp] [DATETIME] NULL,
    [ExitStamp] [DATETIME] NULL,
    [ContentLength] [BIGINT] NULL,
    [Duration] [float] NULL,
    [CreatedStamp] [DATETIME] NOT NULL DEFAULT GETUTCDATE(),
    [LastUpdatedStamp] [DATETIME] NOT NULL DEFAULT GETUTCDATE(),
    [State] [INT] NOT NULL DEFAULT 0,
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