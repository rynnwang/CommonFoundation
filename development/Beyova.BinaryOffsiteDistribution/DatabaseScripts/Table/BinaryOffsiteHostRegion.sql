IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BinaryOffsiteHostRegion]') AND type in (N'U'))
BEGIN
    DROP TABLE [dbo].[BinaryOffsiteHostRegion];
END
GO

CREATE TABLE [dbo].[BinaryOffsiteHostRegion](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
    [HostRegion] [NVARCHAR](128) NOT NULL,
    [Country] [NVARCHAR](16) NOT NULL,
    [State] [int] NOT NULL DEFAULT 0,
CONSTRAINT [PK_BinaryOffsiteHostRegion_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key] ASC
),
CONSTRAINT [CIX_BinaryOffsiteHostRegion] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);

GO
