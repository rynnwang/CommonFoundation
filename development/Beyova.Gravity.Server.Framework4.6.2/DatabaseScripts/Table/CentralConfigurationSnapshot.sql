IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CentralConfigurationSnapshot]') AND type in (N'U'))
DROP TABLE [dbo].[CentralConfigurationSnapshot]
GO

CREATE TABLE [dbo].[CentralConfigurationSnapshot](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWID(),
    [ConfigurationKey] [UNIQUEIDENTIFIER] NOT NULL,
    [Name] [NVARCHAR](256) NULL,
    [Configuration] [NVARCHAR](MAX) NOT NULL DEFAULT '',
    [CreatedStamp] [DATETIME] NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] [UNIQUEIDENTIFIER] NOT NULL,
    [State] [int] NOT NULL DEFAULT 0,
CONSTRAINT [PK_CentralConfigurationSnapshot_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key] ASC
),
CONSTRAINT [CIX_CentralConfigurationSnapshot] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);

GO

