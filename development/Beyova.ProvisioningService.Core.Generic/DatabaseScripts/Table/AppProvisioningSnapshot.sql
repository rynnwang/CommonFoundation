IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AppProvisioningSnapshot]') AND type in (N'U'))
    DROP TABLE [dbo].[AppProvisioningSnapshot]
GO

CREATE TABLE [dbo].[AppProvisioningSnapshot](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
    [ProvisioningKey] UNIQUEIDENTIFIER NOT NULL,
    [Content] [NVARCHAR](MAX) NOT NULL DEFAULT '',
    [CreatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] UNIQUEIDENTIFIER NULL,
    [State] [int] NOT NULL DEFAULT 0,
CONSTRAINT [PK_AppProvisioningSnapshot_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key] ASC
),
CONSTRAINT [CIX_AppProvisioningSnapshot] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);

GO

