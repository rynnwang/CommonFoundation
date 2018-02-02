IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BinaryOffsiteDistribution]') AND type in (N'U'))
BEGIN
    DROP TABLE [dbo].[BinaryOffsiteDistribution];
END
GO

CREATE TABLE [dbo].[BinaryOffsiteDistribution](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
    [Identifier] UNIQUEIDENTIFIER NOT NULL,
    [Container] [nvarchar](128) NOT NULL,
    [HostRegion] [nvarchar](128) NOT NULL,
    [UploadCredentialExpiredStamp] [DateTime] NOT NULL,
    [CreatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [LastUpdatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [State] [int] NOT NULL DEFAULT 0,
CONSTRAINT [PK_BinaryOffsiteDistribution_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key] ASC
),
CONSTRAINT [CIX_BinaryOffsiteDistribution] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);

GO
