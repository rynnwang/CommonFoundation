IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductClient]') AND type in (N'U'))
DROP TABLE [dbo].[ProductClient]
GO

CREATE TABLE [dbo].[ProductClient](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWID(),
    [HostName] [NVARCHAR](128) NOT NULL,
    [ServerName] [NVARCHAR](128) NOT NULL,
    [IpAddress] [NVARCHAR](128) NULL,
    [ProductKey] [UNIQUEIDENTIFIER] NOT NULL,
    [ConfigurationName] NVARCHAR(256) NULL,
    [TotalMemory] [DECIMAL](18,0) NULL, -- KB
    [LastHeartbeatStamp] [DATETIME] NOT NULL,
    [CreatedStamp] [DATETIME] NOT NULL DEFAULT GETUTCDATE(),
    [LastUpdatedStamp] [DATETIME] NOT NULL DEFAULT GETUTCDATE(),
    [State] [int] NOT NULL DEFAULT 0,
CONSTRAINT [PK_ProductClient_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key] ASC
),
CONSTRAINT [CIX_ProductClient] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);

GO

