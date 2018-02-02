IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Heartbeat]') AND type in (N'U'))
DROP TABLE [dbo].[Heartbeat]
GO

CREATE TABLE [dbo].[Heartbeat](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWID(),
    [ClientKey] [UNIQUEIDENTIFIER] NOT NULL,
    [ConfigurationName] NVARCHAR(256) NULL,
    [CpuUsage] [FLOAT] NULL, -- %, [0.00, 1.00]
    [MemoryUsage] [DECIMAL](18,0) NULL, -- KB
    [TotalMemory] [DECIMAL](18,0) NULL, -- KB
    [CreatedStamp] [DATETIME] NOT NULL DEFAULT GETUTCDATE()
CONSTRAINT [PK_Heartbeat_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key] ASC
),
CONSTRAINT [CIX_Heartbeat] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);

GO

