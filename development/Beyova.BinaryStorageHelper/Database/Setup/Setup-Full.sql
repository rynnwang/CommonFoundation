IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BinaryStorageMetaData]') AND type in (N'U'))
BEGIN
    DROP TABLE [dbo].[BinaryStorageMetaData];
END
GO
CREATE TABLE [dbo].[BinaryStorageMetaData](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Identifier] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
    [Container] [nvarchar](128) NOT NULL,
    [Name] [nvarchar](512) NOT NULL,
    [Mime] [varchar](64) NOT NULL,
    [Hash] [varchar](256) NULL,
    [Length] [int] NULL,
    [Height] [int] NULL,
    [Width] [int] NULL,
    [Duration] [int] NULL,
    [KVMeta] [NVARCHAR] (MAX),
    [CreatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [LastUpdatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] UNIQUEIDENTIFIER NULL,
    [LastUpdatedBy] UNIQUEIDENTIFIER NULL,
    [State] [int] NOT NULL DEFAULT 0,
CONSTRAINT [PK_BinaryStorageMetaData_Key] PRIMARY KEY NONCLUSTERED 
(
    [Identifier] ASC
),
CONSTRAINT [CIX_BinaryStorageMetaData] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserBinaryStorageMetaData]') AND type in (N'U'))
BEGIN
    DROP TABLE [dbo].[UserBinaryStorageMetaData]
END
GO
CREATE TABLE [dbo].[UserBinaryStorageMetaData](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
    [Identifier] UNIQUEIDENTIFIER NOT NULL,
    [OwnerKey] NVARCHAR(128) NOT NULL,
    [CreatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [LastUpdatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] UNIQUEIDENTIFIER NULL,
    [LastUpdatedBy] UNIQUEIDENTIFIER NULL,
    [State] [int] NOT NULL DEFAULT 0,
CONSTRAINT [PK_UserBinaryStorageMetaData_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key] ASC
),
CONSTRAINT [CIX_UserBinaryStorageMetaData] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);
GO


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_JsonToBinaryIdentifiers]') AND type in (N'FN',N'TF',N'IF'))
DROP FUNCTION [dbo].[fn_JsonToBinaryIdentifiers];
GO
/* -----------------------------------
@Identifiers SAMPLE:
[{
    "container": "xxx",
    "identifier": "xxx"
}]
*/
CREATE FUNCTION [dbo].[fn_JsonToBinaryIdentifiers](
    @Json NVARCHAR(max)
)
RETURNS @DataTable TABLE (
    [Identifier] UNIQUEIDENTIFIER NOT NULL,
    [Container] NVARCHAR(128) NOT NULL
 )
AS
BEGIN
    IF ISJSON(@Json) > 0
    BEGIN
        INSERT INTO @DataTable([Container],[Identifier])
            SELECT * FROM 
            OPENJSON(@json) 
            WITH
            (
                Container NVARCHAR(MAX) '$.container',
                Identifier UNIQUEIDENTIFIER '$.identifier' 
            ) AS J;
    END
    RETURN;
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[view_UserBinaryStorageMetaData]') AND type in (N'V'))
DROP VIEW [dbo].[view_UserBinaryStorageMetaData]
GO
CREATE VIEW [dbo].[view_UserBinaryStorageMetaData]
AS
SELECT BSM.[RowId]
      ,BSM.[Identifier]
      ,BSM.[Container]
      ,BSM.[Name]
      ,BSM.[Mime]
      ,BSM.[Hash]
      ,BSM.[Length]
      ,BSM.[Height]
      ,BSM.[Width]
      ,BSM.[Duration]
      ,BSM.[KVMeta]
      ,UBSM.[OwnerKey]
      ,BSM.[CreatedStamp]
      ,BSM.[LastUpdatedStamp]
      ,BSM.[CreatedBy]
      ,BSM.[LastUpdatedBy]
      ,BSM.[State]
    FROM [dbo].[BinaryStorageMetaData] AS BSM
        JOIN [dbo].[UserBinaryStorageMetaData] AS UBSM
            ON BSM.[Identifier] = UBSM.[Identifier]
                AND [dbo].[fn_ObjectIsWorkable]([UBSM].[State]) = 1
                AND BSM.[State] = 2; --Committed
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ThrowException]') AND type in (N'P'))
DROP PROCEDURE [dbo].[sp_ThrowException];
GO
CREATE PROCEDURE [dbo].[sp_ThrowException](
    @Name [NVARCHAR](256),
    @Code INT,
    @Reason [NVARCHAR](256),
    @Message [NVARCHAR](512)
)
AS
BEGIN
    SELECT
        @Name AS [SqlStoredProcedureName],
        ISNULL(@Code, 500) AS [SqlErrorCode],
        @Reason AS [SqlErrorReason],
        @Message AS [SqlErrorMessage];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CommitBinaryStorage]') AND type in (N'P'))
DROP PROCEDURE [dbo].[sp_CommitBinaryStorage];
GO
CREATE PROCEDURE [dbo].[sp_CommitBinaryStorage](
    @Container NVARCHAR(128),
    @Identifier UNIQUEIDENTIFIER,
    @Mime VARCHAR(64),
    @Hash VARCHAR(128),
    @Length INT,
    @CommitOption INT,
    @OperatorKey UNIQUEIDENTIFIER
)
AS
SET NOCOUNT ON;
DECLARE @NowTime AS DATETIME = GETUTCDATE();
DECLARE @ExistedState AS INT;
DECLARE @OwnerKey AS UNIQUEIDENTIFIER;
DECLARE @InstanceContainer AS VARCHAR(128);
DECLARE @InstanceIdentifier AS UNIQUEIDENTIFIER;
DECLARE @IsDuplicated AS BIT = 0;
DECLARE @ErrerMessage AS NVARCHAR(MAX);
DECLARE @ErrorSeverity AS INT;
DECLARE @ErrorState AS INT;
DECLARE @ErrorCode AS INT;
BEGIN
    IF @Container IS NOT NULL 
        AND @Identifier IS NOT NULL 
        AND @Hash IS NOT NULL 
        AND @Length IS NOT NULL
        AND @OperatorKey IS NOT NULL
    BEGIN
        SELECT TOP 1 @ExistedState = [State], @OwnerKey = [CreatedBy] FROM [dbo].[BinaryStorageMetaData] 
            WHERE [Identifier] = @Identifier
                AND [Container] = @Container;
        IF @ExistedState = 1
        BEGIN
            IF (@OwnerKey IS NULL OR @OperatorKey = @OwnerKey)
            BEGIN
                BEGIN TRY
                BEGIN TRANSACTION
                    IF @CommitOption IS NULL OR @CommitOption = 0
                        SET @CommitOption = 1;
                    IF @CommitOption = 2
                    BEGIN
                        SELECT TOP 1 @InstanceContainer = [Container], @InstanceIdentifier = [Identifier]
                            FROM [dbo].[BinaryStorageMetaData] 
                            WHERE [Hash] = @Hash AND [Length] = @Length;
                        IF @InstanceContainer IS NOT NULL
                        BEGIN
                            UPDATE [dbo].[BinaryStorageMetaData]
                                SET [LastUpdatedStamp] = @NowTime,
                                    [LastUpdatedBy] = @OperatorKey,
                                    [State] = 7 --Duplicated
                                WHERE [Identifier] = @Identifier;
                            SET @IsDuplicated= 1;
                        END
                    END
                    IF @IsDuplicated = 0
                    BEGIN
                        SET @InstanceContainer = @Container;
                        SET @InstanceIdentifier = @Identifier;
                        UPDATE [dbo].[BinaryStorageMetaData]
                            SET [Mime] = ISNULL(@Mime, [Mime]),
                                [Hash] = ISNULL(@Hash, [Hash]),
                                [Length] = ISNULL(@Length, [Length]),
                                [LastUpdatedStamp] = @NowTime,
                                [LastUpdatedBy] = @OperatorKey,
                                [State] = 2 --Committed
                            WHERE [Identifier] = @Identifier;
                    END
                    IF NOT EXISTS (SELECT TOP 1 * FROM [dbo].[UserBinaryStorageMetaData]
                        WHERE [Identifier] = @InstanceIdentifier 
                            AND [OwnerKey] = @OperatorKey
                            AND [dbo].[fn_ObjectIsWorkable]([State]) = 1 )
                    BEGIN
                    INSERT INTO [dbo].[UserBinaryStorageMetaData]
                           ([Identifier]
                           ,[OwnerKey]
                           ,[CreatedStamp]
                           ,[LastUpdatedStamp]
                           ,[CreatedBy]
                           ,[LastUpdatedBy]
                           ,[State])
                        VALUES
                           (@InstanceIdentifier
                           ,@OperatorKey
                           ,@NowTime
                           ,@NowTime
                           ,@OperatorKey
                           ,@OperatorKey
                           ,0);
                    END
                    SELECT TOP 1 [Identifier]
                      ,[Container]
                      ,[Name]
                      ,[Mime]
                      ,[Hash]
                      ,[Length]
                      ,[Height]
                      ,[Width]
                      ,[Duration]
                      ,[KVMeta]
                      ,@OperatorKey AS [OwnerKey]
                      ,[CreatedStamp]
                      ,[CreatedBy]
                      ,[LastUpdatedStamp]
                      ,[LastUpdatedBy]
                      ,[State]
                    FROM [dbo].[BinaryStorageMetaData]
                    WHERE [Identifier] = @InstanceIdentifier;
                    COMMIT TRANSACTION
                END TRY
                BEGIN CATCH
                    ROLLBACK TRANSACTION;
                    SET @ErrerMessage = ERROR_MESSAGE();
                    SET @ErrorSeverity = ERROR_SEVERITY();
                    SET @ErrorState = ERROR_STATE();
                    SET @ErrorCode = ERROR_NUMBER();
                    RAISERROR(@ErrorCode, @ErrorSeverity,@ErrorState, @ErrerMessage);
                END CATCH
            END
            ELSE
            BEGIN
                EXEC [dbo].[sp_ThrowException]
                    @Name = N'sp_CommitBinaryStorage',
                    @Code = 403,
                    @Reason = N'[OwnerKey]',
                    @Message = N'Owner specified is not who create this binary meta.';
                RETURN;
            END
        END
        ELSE
        BEGIN
            EXEC [dbo].[sp_ThrowException]
                @Name = N'sp_CommitBinaryStorage',
                @Code = 403,
                @Reason = N'[State]',
                @Message = N'Binary object can be committed only when state is pending commit.';
            RETURN;
        END
    END
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CommitBinaryStorageDeletion]') AND type in (N'P'))
DROP PROCEDURE [dbo].[sp_CommitBinaryStorageDeletion];
GO
/* -----------------------------------
@Identifiers SAMPLE:
[{
    "Container": "xxx",
    "Identifier": "xxx"
}]
*/
CREATE PROCEDURE [dbo].[sp_CommitBinaryStorageDeletion](
    @Identifiers NVARCHAR(MAX)
)
AS
SET NOCOUNT ON;
BEGIN
    DECLARE @NowTime AS DATETIME = GETUTCDATE();
    IF @Identifiers IS NOT NULL
    BEGIN
        UPDATE [dbo].[BinaryStorageMetaData]
            SET [State] = 4, --Deleted
                [LastUpdatedStamp] = @NowTime
            FROM [dbo].[BinaryStorageMetaData] AS BSM
                JOIN [dbo].[fn_JsonToBinaryIdentifiers](@Identifiers) AS B
                    ON B.[Container] = BSM.[Container] 
                        AND B.[Identifier] = BSM.[Identifier] 
                        AND BSM.[State] = 3; --DeletePending
    END
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ConvertPendingCommitToPendingDelete]') AND type in (N'P'))
DROP PROCEDURE [dbo].[sp_ConvertPendingCommitToPendingDelete];
GO
CREATE PROCEDURE [dbo].[sp_ConvertPendingCommitToPendingDelete](
    @Stamp DATETIME
)
AS
SET NOCOUNT OFF;
BEGIN
    UPDATE [dbo].[BinaryStorageMetaData]
        SET 
            [LastUpdatedStamp] = GETUTCDATE(),
            [State] = 3 -- DeletePending
        WHERE 
            [CreatedStamp] < @Stamp AND
            [State] = 1; --CommitPending
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateBinaryStorageMetaData]') AND type in (N'P'))
DROP PROCEDURE [dbo].[sp_CreateBinaryStorageMetaData];
GO
CREATE PROCEDURE [dbo].[sp_CreateBinaryStorageMetaData](
    @Container NVARCHAR(128),
    @Identifier UNIQUEIDENTIFIER,
    @Name NVARCHAR(512),
    @Mime VARCHAR(64),
    @Height INT,
    @Width INT,
    @Duration INT,
    @KVMeta NVARCHAR(MAX),
    @OperatorKey UNIQUEIDENTIFIER
)
AS
SET NOCOUNT ON;
DECLARE @NowTime AS DATETIME = GETUTCDATE();
DECLARE @ExistedState AS INT;
DECLARE @ExistedOwnerKey AS UNIQUEIDENTIFIER;
BEGIN
    IF @Container IS NOT NULL AND @OperatorKey IS NOT NULL
    BEGIN
        IF @Identifier IS NOT NULL AND EXISTS (SELECT TOP 1 * FROM [dbo].[BinaryStorageMetaData] WHERE [Identifier] = @Identifier)
        BEGIN
            EXEC [dbo].[sp_ThrowException]
                @Name = N'sp_CreateBinaryStorageMetaData',
                @Code = 409,
                @Reason = N'',
                @Message = N'Binary in same identifier is already existed.';
            RETURN;
        END
        ELSE
        BEGIN
            SET @Identifier = NEWID();
        END
        INSERT INTO [dbo].[BinaryStorageMetaData]
               ([Identifier]
               ,[Container]
               ,[Name]
               ,[Mime]
               ,[Hash]
               ,[Length]
               ,[Height]
               ,[Width]
               ,[Duration]
               ,[KVMeta]
               ,[CreatedStamp]
               ,[CreatedBy]
               ,[LastUpdatedStamp]
               ,[LastUpdatedBy]
               ,[State])
         VALUES
               (@Identifier
               ,@Container
               ,ISNULL(@Name, CONVERT(VARCHAR(512),@Identifier))
               ,ISNULL(@Mime, N'application/octet-stream')
               ,NULL
               ,NULL
               ,@Height
               ,@Width
               ,@Duration
               ,@KVMeta
               ,@NowTime
               ,@OperatorKey
               ,@NowTime
               ,@OperatorKey
               ,1 -- Commit pending: 1
        );
        SELECT TOP 1 [Identifier]
          ,[Container]
          ,[Name]
          ,[Mime]
          ,[Hash]
          ,[Length]
          ,[Height]
          ,[Width]
          ,[Duration]
          ,[KVMeta]
          ,[CreatedBy] AS [OwnerKey]
          ,[CreatedStamp]
          ,[CreatedBy]
          ,[LastUpdatedStamp]
          ,[LastUpdatedBy]
          ,[State]
        FROM [dbo].[BinaryStorageMetaData]
        WHERE [Identifier] = @Identifier;
    END
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteBinaryStorage]') AND type in (N'P'))
DROP PROCEDURE [dbo].[sp_DeleteBinaryStorage];
GO
CREATE PROCEDURE [dbo].[sp_DeleteBinaryStorage](
    @Identifier UNIQUEIDENTIFIER,
    @OperatorKey UNIQUEIDENTIFIER
)
AS
DECLARE @NowTime AS DATETIME = GETUTCDATE();
BEGIN
    IF @Identifier IS NOT NULL 
        AND @OperatorKey IS NOT NULL 
    BEGIN
        UPDATE [dbo].[UserBinaryStorageMetaData]
            SET
                [LastUpdatedStamp] = @NowTime,
                [LastUpdatedBy]  = @OperatorKey,
                [State] = [dbo].[fn_SetObjectDeleted]([State])
            WHERE [Identifier] = @Identifier
                AND [OwnerKey] = @OperatorKey
                AND [dbo].[fn_ObjectCanUpdateOrDelete]([State]) = 1;
    END
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetBinaryCapacitySummary]') AND type in (N'P'))
DROP PROCEDURE [dbo].[sp_GetBinaryCapacitySummary];
GO
CREATE PROCEDURE [dbo].[sp_GetBinaryCapacitySummary](
    @Container NVARCHAR(128),
    @OwnerKey UNIQUEIDENTIFIER
)
AS
SET NOCOUNT ON;
BEGIN
    SELECT 
        @Container AS [Container],
        @OwnerKey AS [OwnerKey],
        (SELECT COUNT(*)
            FROM [dbo].[view_UserBinaryStorageMetaData]
                WHERE (@OwnerKey IS NULL OR [OwnerKey] = @OwnerKey) 
                AND (@Container IS NULL OR [Container] = @Container)) AS [Count],
        (SELECT SUM([Length])
            FROM [dbo].[view_UserBinaryStorageMetaData]
                WHERE (@OwnerKey IS NULL OR [OwnerKey] = @OwnerKey) 
                AND (@Container IS NULL OR [Container] = @Container)) AS [Size];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetBinaryStorageMetaDataByIdentifiers]') AND type in (N'P'))
DROP PROCEDURE [dbo].[sp_GetBinaryStorageMetaDataByIdentifiers];
GO
CREATE PROCEDURE [dbo].[sp_GetBinaryStorageMetaDataByIdentifiers](
    @Identifiers NVARCHAR(MAX)
)
AS
SET NOCOUNT ON;
BEGIN
    IF @Identifiers IS NOT NULL
    BEGIN

        SELECT BSMD.[Identifier]
                ,BSMD.[Container]
                ,BSMD.[Name]
                ,BSMD.[Mime]
                ,BSMD.[Hash]
                ,BSMD.[Length]
                ,BSMD.[Height]
                ,BSMD.[Width]
                ,BSMD.[Duration]
                ,BSMD.[KVMeta]
                ,NULL AS [OwnerKey]
                -- CUSTOMIZED COLUMNS START 
                -- You can put any additonal field as customized columns here.
                -- CUSTOMIZED COLUMNS END 
                ,BSMD.[CreatedStamp]
                ,BSMD.[LastUpdatedStamp]
                ,BSMD.[CreatedBy]
                ,BSMD.[LastUpdatedBy]
                ,BSMD.[State]
            FROM [dbo].[BinaryStorageMetaData] AS BSMD
                JOIN OPENJSON(@Identifiers) WITH(
                    [Identifier] UNIQUEIDENTIFIER '$.identifier',
                    [Container] NVARCHAR(128) '$.container'
                    ) 
                    AS IDTABLE
                    ON BSMD.[Identifier] = IDTABLE.[Identifier] AND (IDTABLE.[Container] IS NULL OR IDTABLE.[Container] = '' OR BSMD.[Container] = IDTABLE.[Container])
                WHERE BSMD.[State] = 2;

    END
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetPendingDeleteBinaryStorages]') AND type in (N'P'))
DROP PROCEDURE [dbo].[sp_GetPendingDeleteBinaryStorages];
GO
CREATE PROCEDURE [dbo].[sp_GetPendingDeleteBinaryStorages]
AS
SET NOCOUNT ON;
BEGIN
    SELECT TOP 50 [Identifier]
          ,[Container]
          ,[Name]
          ,[Mime]
          ,[Hash]
          ,[Length]
          ,[Height]
          ,[Width]
          ,[Duration]
          ,[KVMeta]
          ,NULL AS [OwnerKey]
          ,[CreatedStamp]
          ,[LastUpdatedStamp]
          ,[CreatedBy]
          ,[LastUpdatedBy]
          ,[State]
      FROM [dbo].[BinaryStorageMetaData]
      WHERE [State] = 3; --DeletePending
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryBinaryStorageMetaData]') AND type in (N'P'))
DROP PROCEDURE [dbo].[sp_QueryBinaryStorageMetaData];
GO
CREATE PROCEDURE [dbo].[sp_QueryBinaryStorageMetaData](
    @Container NVARCHAR(128),
    @Identifier UNIQUEIDENTIFIER,
    @Name [NVARCHAR](512),
    @Mime VARCHAR(64),
    @Hash VARCHAR(128),
    @MinLength INT,
    @MaxLength INT,
    @MinHeight INT,
    @MaxHeight INT,
    @MinWidth INT,
    @MaxWidth INT,
    @MinDuration INT,
    @MaxDuration INT,
    @FromStamp DATETIME,
    @ToStamp DATETIME,
    @KVMetaCriteria NVARCHAR(MAX),
    @Count INT
)
AS
SET NOCOUNT ON;
BEGIN
    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX) = '[State] = 2 AND ';

    IF @Count IS NULL OR @Count < 1
        SET @Count = 50;

    SET @SqlStatement = 'SELECT TOP ' + CONVERT(NVARCHAR(MAX), @Count) + ' [Identifier]
        ,[Container]
        ,[Name]
        ,[Mime]
        ,[Hash]
        ,[Length]
        ,[Height]
        ,[Width]
        ,[Duration]
        ,[KVMeta]
        ,NULL AS [OwnerKey]
        ,[CreatedStamp]
        ,[LastUpdatedStamp]
        ,[CreatedBy]
        ,[LastUpdatedBy]
        ,[State]
    FROM [dbo].[BinaryStorageMetaData]';

    IF @Identifier IS NOT NULL
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Identifier','=',CONVERT(NVARCHAR(MAX), @Identifier),1);
    ELSE
    BEGIN
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Container','=',@Container,1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Name','LIKE',@Name,1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Mime','=',@Mime,1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Hash','=',@Hash,1);

        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Length','>=',CONVERT(NVARCHAR(MAX), @MinLength),0);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Length','<',CONVERT(NVARCHAR(MAX), @MaxLength),0);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Height','>=',CONVERT(NVARCHAR(MAX), @MinHeight),0);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Height','<',CONVERT(NVARCHAR(MAX), @MaxHeight),0);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Width','>=',CONVERT(NVARCHAR(MAX), @MinWidth),0);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Width','<',CONVERT(NVARCHAR(MAX), @MaxWidth),0);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Duration','>=',CONVERT(NVARCHAR(MAX), @MinDuration),0);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Duration','<',CONVERT(NVARCHAR(MAX), @MaxDuration),0);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('LastUpdatedStamp','>=',CONVERT(NVARCHAR(MAX), @FromStamp, 121),1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('LastUpdatedStamp','<',CONVERT(NVARCHAR(MAX), @ToStamp, 121),1);

        IF @KVMetaCriteria IS NOT NULL
        BEGIN
            SET @WhereStatement = @WhereStatement + N'(' + @KVMetaCriteria + N') AND ';
        END

    END

    IF(@WhereStatement <> '')
    BEGIN
        SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
        SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement;
        SET @SqlStatement = @SqlStatement + ' ORDER BY [Container],[Name] ';
    END

    PRINT @SqlStatement;
    EXECUTE sp_executesql @SqlStatement;
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryUserBinaryStorageMetaData]') AND type in (N'P'))
DROP PROCEDURE [dbo].[sp_QueryUserBinaryStorageMetaData];
GO
CREATE PROCEDURE [dbo].[sp_QueryUserBinaryStorageMetaData](
    @Container NVARCHAR(128),
    @Identifier UNIQUEIDENTIFIER,
    @Name [NVARCHAR](512),
    @Mime VARCHAR(64),
    @Hash VARCHAR(128),
    @MinLength INT,
    @MaxLength INT,
    @MinHeight INT,
    @MaxHeight INT,
    @MinWidth INT,
    @MaxWidth INT,
    @MinDuration INT,
    @MaxDuration INT,
    @FromStamp DATETIME,
    @ToStamp DATETIME,
    @OwnerKey UNIQUEIDENTIFIER,
    @KVMetaCriteria NVARCHAR(MAX),
    @Count INT
)
AS
SET NOCOUNT ON;
BEGIN
    IF @Container IS NOT NULL
    BEGIN
        DECLARE @SqlStatement AS NVARCHAR(MAX);
        -- In View [view_UserBinaryStorageMetaData], it already filter for State= 2 only.
        DECLARE @WhereStatement AS NVARCHAR(MAX) = '[Container] = ''' + @Container + ''' AND ';

        IF @Count IS NULL OR @Count < 1
            SET @Count = 50;

        SET @SqlStatement = 'SELECT TOP ' + CONVERT(NVARCHAR(MAX), @Count) + ' [Identifier]
          ,[Container]
          ,[Name]
          ,[Mime]
          ,[Hash]
          ,[Length]
          ,[Height]
          ,[Width]
          ,[Duration]
          ,[KVMeta]
          ,[OwnerKey]
          ,[CreatedStamp]
          ,[LastUpdatedStamp]
          ,[CreatedBy]
          ,[LastUpdatedBy]
          ,[State]
      FROM [dbo].[view_UserBinaryStorageMetaData]';

        IF @Identifier IS NOT NULL
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Identifier','=',CONVERT(NVARCHAR(MAX), @Identifier),1);
        ELSE
        BEGIN
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Name','LIKE',@Name,1);
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Mime','=',@Mime,1);
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Hash','=',@Hash,1);
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('OwnerKey','=',CONVERT(NVARCHAR(MAX), @OwnerKey),1);

            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Length','>=',CONVERT(NVARCHAR(MAX), @MinLength),0);
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Length','<',CONVERT(NVARCHAR(MAX), @MaxLength),0);
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Height','>=',CONVERT(NVARCHAR(MAX), @MinHeight),0);
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Height','<',CONVERT(NVARCHAR(MAX), @MaxHeight),0);
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Width','>=',CONVERT(NVARCHAR(MAX), @MinWidth),0);
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Width','<',CONVERT(NVARCHAR(MAX), @MaxWidth),0);
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Duration','>=',CONVERT(NVARCHAR(MAX), @MinDuration),0);
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Duration','<',CONVERT(NVARCHAR(MAX), @MaxDuration),0);
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('LastUpdatedStamp','>=',CONVERT(NVARCHAR(MAX), @FromStamp, 121),1);
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('LastUpdatedStamp','<',CONVERT(NVARCHAR(MAX), @ToStamp, 121),1);

            IF @KVMetaCriteria IS NOT NULL
            BEGIN
                SET @WhereStatement = @WhereStatement + N'(' + @KVMetaCriteria + N') AND ';
            END
        END

        IF(@WhereStatement <> '')
        BEGIN
            SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
            SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement;
            SET @SqlStatement = @SqlStatement + ' ORDER BY [Container],[Name] ';
        END

        PRINT @SqlStatement;
        EXECUTE sp_executesql @SqlStatement;
    END
END
GO



