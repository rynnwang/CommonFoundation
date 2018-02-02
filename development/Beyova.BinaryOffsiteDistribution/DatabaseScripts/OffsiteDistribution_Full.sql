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


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[view_BinaryOffsiteDistribution]') AND type in (N'V'))
DROP VIEW [dbo].[view_BinaryOffsiteDistribution]
GO

CREATE VIEW [dbo].[view_BinaryOffsiteDistribution]
AS
    SELECT BOD.[Key]
        ,BOD.[Identifier]
        ,BOD.[Container]
        ,BOD.[HostRegion]
        ,BOHR.[Country]
        ,BOD.[UploadCredentialExpiredStamp]
        ,BOD.[CreatedStamp]
        ,BOD.[LastUpdatedStamp]
        ,BOD.[State]
    FROM [dbo].[BinaryOffsiteDistribution] AS BOD
        JOIN [dbo].[BinaryOffsiteHostRegion] AS BOHR
            ON BOD.[HostRegion] = BOHR.[HostRegion]
                AND [dbo].[fn_ObjectIsWorkable](BOHR.[State]) = 1
                AND BOD.[State] = 2; --Committed

GO

 --- SP

 IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateOrUpdateBinaryOffsiteDistribution]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CreateOrUpdateBinaryOffsiteDistribution]
GO

CREATE PROCEDURE [dbo].[sp_CreateOrUpdateBinaryOffsiteDistribution](
    @Identifier UNIQUEIDENTIFIER,
    @Container NVARCHAR(128),
    @HostRegion [NVARCHAR](128),
    @ExpiredStamp DATETIME
)
AS
SET NOCOUNT ON;
DECLARE @UploadCredentialExpiredStamp AS DATETIME;
DECLARE @State AS INT;
DECLARE @Key AS UNIQUEIDENTIFIER;
DECLARE @NowTime AS DATETIME = GETUTCDATE();
BEGIN
    SELECT TOP 1 @Key = [Key], @State = [State], @UploadCredentialExpiredStamp = [UploadCredentialExpiredStamp]
        FROM [dbo].[BinaryOffsiteDistribution]
            WHERE [Identifier] = @Identifier
                AND (@Container IS NULL OR [Container] = @Container)
                AND [HostRegion] = @HostRegion;

    IF @ExpiredStamp IS NULL
        SET @ExpiredStamp = DATEADD(mi, 60, @NowTime);

    IF @State IS NOT NULL AND @State <> 1
    BEGIN
        RETURN;
    END
    ELSE IF @Key IS NULL
    BEGIN
        SET @Key = NEWID();

        INSERT INTO [dbo].[BinaryOffsiteDistribution]
                ([Key]
                ,[Identifier]
                ,[Container]
                ,[HostRegion]
                ,[UploadCredentialExpiredStamp]
                ,[CreatedStamp]
                ,[LastUpdatedStamp]
                ,[State])
            VALUES
                (@Key
                ,@Identifier
                ,@Container
                ,@HostRegion
                ,@ExpiredStamp
                ,@NowTime
                ,@NowTime
                ,1)
    END
    ELSE IF @UploadCredentialExpiredStamp < @NowTime
    BEGIN
        UPDATE [dbo].[BinaryOffsiteDistribution]
            SET 
                [LastUpdatedStamp] = @NowTime,
                [UploadCredentialExpiredStamp] = @ExpiredStamp
            WHERE [Key] = @Key;
    END

    SELECT TOP 1 [Key]
        ,[Identifier]
        ,[Container]
        ,[HostRegion]
        ,[UploadCredentialExpiredStamp]
        ,[CreatedStamp]
        ,[LastUpdatedStamp]
        ,[State]
     FROM [dbo].[BinaryOffsiteDistribution]
     WHERE [Key] = @Key;

END
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetBinaryOffsiteDistributionByCountry]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_GetBinaryOffsiteDistributionByCountry]
GO

CREATE PROCEDURE [dbo].[sp_GetBinaryOffsiteDistributionByCountry](
    @Identifier UNIQUEIDENTIFIER,
    @Container NVARCHAR(128),
    @Country [NVARCHAR](16)
)
AS
SET NOCOUNT ON;
BEGIN
    SELECT TOP 1 [Key]
        ,[Identifier]
        ,[Container]
        ,[HostRegion]
        ,[UploadCredentialExpiredStamp]
        ,[Country]
        ,[CreatedStamp]
        ,[LastUpdatedStamp]
        ,[State]
     FROM [dbo].[view_BinaryOffsiteDistribution]
     WHERE [Identifier] = @Identifier
        AND (@Container IS NULL OR [Container] = @Container)
        AND [Country] = @Country;
END
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteBinaryOffsiteDistribution]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_DeleteBinaryOffsiteDistribution]
GO

CREATE PROCEDURE [dbo].[sp_DeleteBinaryOffsiteDistribution](
    @Identifier UNIQUEIDENTIFIER,
    @Container NVARCHAR(128)
)
AS
SET NOCOUNT ON;
BEGIN
   UPDATE [dbo].[BinaryOffsiteDistribution]
        SET 
            [LastUpdatedStamp] = GETUTCDATE(),
            [State] = 3 --DeletePending
        WHERE [Identifier] = @Identifier
            AND (@Container IS NULL OR [Container] = @Container)
END
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CommitBinaryOffsiteDistribution]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CommitBinaryOffsiteDistribution]
GO

CREATE PROCEDURE [dbo].[sp_CommitBinaryOffsiteDistribution](
    @Identifier UNIQUEIDENTIFIER,
    @Container NVARCHAR(128),
    @HostRegion [NVARCHAR](128)
)
AS
SET NOCOUNT ON;
BEGIN
    UPDATE [dbo].[BinaryOffsiteDistribution]
        SET [LastUpdatedStamp] = GETUTCDATE(),
            [State] = 2
        WHERE [Identifier] = @Identifier
                AND [Container] = @Container
                AND [State] = 1
                AND [HostRegion] = @HostRegion;
END
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CommitBinaryOffsiteDistributionDeletion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CommitBinaryOffsiteDistributionDeletion]
GO

/* -----------------------------------
@Xml XML SAMPLE:
<Storage>
    <Item Container="Container"></Item>
</Storage>
----------------------------------- */
CREATE PROCEDURE [dbo].[sp_CommitBinaryOffsiteDistributionDeletion](
    @Xml XML
)
AS
SET NOCOUNT ON;
BEGIN
    DECLARE @NowTime AS DATETIME = GETUTCDATE();

    IF @Xml IS NOT NULL
    BEGIN
        CREATE TABLE #BlobIdentifiers(
            [Container] [nvarchar](128) NOT NULL,
            [Identifier] UNIQUEIDENTIFIER NOT NULL
        );

        INSERT INTO #BlobIdentifiers([Container],[Identifier])
            SELECT 
            Items.R.value('(@Container)[1]','[nvarchar](128)'),
            Items.R.value('.','UNIQUEIDENTIFIER')
            FROM @Xml.nodes('/Storage/Item') AS Items(R);

        UPDATE [dbo].[BinaryOffsiteDistribution]
            SET [State] = 4, --Deleted
                [LastUpdatedStamp] = @NowTime
            FROM [dbo].[BinaryOffsiteDistribution] AS BSM
                JOIN #BlobIdentifiers AS B
                    ON B.[Container] = BSM.[Container] 
                        AND B.[Identifier] = BSM.[Identifier] 
                        AND BSM.[State] = 3; --DeletePending
    END
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetBinaryOffsiteDistributionByIdentifiers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_GetBinaryOffsiteDistributionByIdentifiers]
GO

CREATE PROCEDURE [dbo].[sp_GetBinaryOffsiteDistributionByIdentifiers](
    @Xml XML,
    @Container NVARCHAR(128)
)
AS
SET NOCOUNT ON;
BEGIN
    SELECT TOP 1 [Key]
        ,[Identifier]
        ,[Container]
        ,[HostRegion]
        ,[UploadCredentialExpiredStamp]
        ,[CreatedStamp]
        ,[LastUpdatedStamp]
        ,[State]
     FROM [dbo].[BinaryOffsiteDistribution] AS BOD
        JOIN [dbo].[fn_XmlListToGuidTable](@Xml) AS [Keys]
            ON [Keys].[Value] = BOD.[Identifier]
     WHERE (@Container IS NULL OR [Container] = @Container)
        AND ([State] IN (1,2));
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetBinaryOffsiteDistributionByKey]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_GetBinaryOffsiteDistributionByKey]
GO

CREATE PROCEDURE [dbo].[sp_GetBinaryOffsiteDistributionByKey](
    @Key UNIQUEIDENTIFIER
)
AS
SET NOCOUNT ON;
BEGIN
    SELECT TOP 1 [Key]
        ,[Identifier]
        ,[Container]
        ,[HostRegion]
        ,[UploadCredentialExpiredStamp]
        ,[CreatedStamp]
        ,[LastUpdatedStamp]
        ,[State]
    FROM [dbo].[BinaryOffsiteDistribution]
    WHERE [Key] = @Key;
END
GO


