IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CentralConfiguration]') AND type in (N'U'))
DROP TABLE [dbo].[CentralConfiguration]
GO

CREATE TABLE [dbo].[CentralConfiguration](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWID(),
    [SnapshotKey] [UNIQUEIDENTIFIER] NOT NULL, 
    [ProductKey] [UNIQUEIDENTIFIER] NOT NULL, 
    [CreatedStamp] [DATETIME] NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] [UNIQUEIDENTIFIER] NOT NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
CONSTRAINT [PK_CentralConfiguration_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key] ASC
),
CONSTRAINT [CIX_CentralConfiguration] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);

GO

