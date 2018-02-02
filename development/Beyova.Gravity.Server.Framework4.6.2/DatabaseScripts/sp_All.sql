IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ThrowException]') AND type in (N'P', N'PC'))
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

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateOrUpdateCentralConfiguration]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CreateOrUpdateCentralConfiguration];

GO
CREATE PROCEDURE [dbo].[sp_CreateOrUpdateCentralConfiguration](
    @Key [UNIQUEIDENTIFIER],
    @ProductKey [UNIQUEIDENTIFIER],
    @Name [NVARCHAR](256),
    @Configuration [NVARCHAR](MAX),
    @OperatorKey [UNIQUEIDENTIFIER]
)
AS
DECLARE @ErrerMessage AS NVARCHAR(MAX);
DECLARE @ErrorSeverity AS INT;
DECLARE @ErrorState AS INT;
DECLARE @ErrorCode AS INT;
DECLARE @NowTime AS DATETIME = GETUTCDATE();
DECLARE @SnapshotKey AS UNIQUEIDENTIFIER = NEWID();
BEGIN
    BEGIN TRY
    BEGIN TRANSACTION

        IF @Key IS NULL
        BEGIN
            SET @Key = NEWID();

            INSERT INTO [dbo].[CentralConfigurationSnapshot]
                ([Key]
                ,[ConfigurationKey]
                ,[Name]
                ,[Configuration]
                ,[CreatedStamp]
                ,[CreatedBy]
                ,[State])
            VALUES
                (@SnapshotKey
                ,@Key
                ,ISNULL(@Name, '')
                ,ISNULL(@Configuration, '')
                ,@NowTime
                ,@OperatorKey
                ,0);

            
            INSERT INTO [dbo].[CentralConfiguration]
                ([Key]
                ,[SnapshotKey]
                ,[ProductKey]
                ,[CreatedStamp]
                ,[CreatedBy])
            VALUES
                (@Key
                ,@SnapshotKey
                ,@ProductKey
                ,@NowTime
                ,@OperatorKey);
        END
        ELSE IF EXISTS (SELECT TOP 1 1 FROM [dbo].[view_CentralConfiguration] WHERE [Key] = @Key 
            AND [dbo].[fn_ObjectIsWorkable]([State]) = 1)
        BEGIN
            INSERT INTO [dbo].[CentralConfigurationSnapshot]
                ([Key]
                ,[ConfigurationKey]
                ,[Name]
                ,[Configuration]
                ,[CreatedStamp]
                ,[CreatedBy]
                ,[State])
            VALUES
                (@SnapshotKey
                ,@Key
                ,ISNULL(@Name, '')
                ,ISNULL(@Configuration, '')
                ,@NowTime
                ,@OperatorKey
                ,0);

            UPDATE [dbo].[CentralConfiguration]
                SET [SNapshotKey] = @SnapshotKey
                WHERE [Key] = @Key;
        END
        ELSE
        BEGIN
            EXEC [dbo].[sp_ThrowException]
                @Name = N'sp_CreateOrUpdateCentralConfiguration',
                @Code = 403,
                @Reason = N'[State]',
                @Message = N'Target is not found or cannot modify.';
            RETURN;
        END

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

    SELECT @Key;
END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteCentralConfiguration]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_DeleteCentralConfiguration];

GO
CREATE PROCEDURE [dbo].[sp_DeleteCentralConfiguration](
    @Key [UNIQUEIDENTIFIER],
    @OperatorKey [UNIQUEIDENTIFIER]
)
AS
DECLARE @ErrerMessage AS NVARCHAR(MAX);
DECLARE @ErrorSeverity AS INT;
DECLARE @ErrorState AS INT;
DECLARE @ErrorCode AS INT;
DECLARE @NowTime AS DATETIME = GETUTCDATE();
DECLARE @SnapshotKey AS UNIQUEIDENTIFIER = NEWID();
BEGIN
    IF @Key IS NOT NULL 
        AND @OperatorKey IS NOT NULL
        AND EXISTS (SELECT TOP 1 1 FROM [dbo].[view_CentralConfigurationSnapshot]
            WHERE [Key] = @Key
                AND [dbo].[fn_ObjectCanUpdateOrDelete]([State]) = 1)
    BEGIN
        BEGIN TRY
        BEGIN TRANSACTION
            INSERT INTO [dbo].[CentralConfigurationSnapshot]
                ([Key]
                ,[ConfigurationKey]
                ,[Name]
                ,[Configuration]
                ,[CreatedStamp]
                ,[CreatedBy]
                ,[State])
            VALUES
                (@SnapshotKey
                ,@Key
                ,''
                ,''
                ,@NowTime
                ,@OperatorKey
                ,1);

            UPDATE [dbo].[CentralConfiguration]
                SET [SNapshotKey] = @SnapshotKey,
                    [IsDeleted] = 1
                WHERE [Key] = @Key;
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
            @Name = N'sp_DeleteCentralConfiguration',
            @Code = 403,
            @Reason = N'[State]',
            @Message = N'Target is not found or cannot modify.';
        RETURN;
    END
END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryCentralConfiguration]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryCentralConfiguration];

GO
CREATE PROCEDURE [dbo].[sp_QueryCentralConfiguration](
    @Key [UNIQUEIDENTIFIER],
    @ProductKey [UNIQUEIDENTIFIER],
    @Name [NVARCHAR](256)
)
AS
BEGIN
    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX) = '[dbo].[fn_ObjectIsWorkable]([State]) = 1 AND ';

    SET @SqlStatement = 'SELECT [Key]
      ,[SnapshotKey]
      ,[ProductKey]
      ,[Name]
      ,[Configuration]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[CreatedBy]
      ,[LastUpdatedBy]
      ,[State]
    FROM [dbo].[view_CentralConfiguration]';

    IF @Key IS NOT NULL
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Key','=',CONVERT(NVARCHAR(MAX), @Key),1);
    ELSE
    BEGIN
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('ProductKey','=',CONVERT(NVARCHAR(MAX), @ProductKey),1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Name','=',@Name,1);
    END

    IF(@WhereStatement <> '')
    BEGIN
        SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
        SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement;
        SET @SqlStatement = @SqlStatement + ' ORDER BY [ProductKey], [Name];';
    END

    PRINT @SqlStatement;
    EXECUTE sp_executesql @SqlStatement;
END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryCentralConfigurationSnapshot]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryCentralConfigurationSnapshot];

GO
CREATE PROCEDURE [dbo].[sp_QueryCentralConfigurationSnapshot](
    @Key [UNIQUEIDENTIFIER],
    @SnapshotKey [UNIQUEIDENTIFIER]
)
AS
BEGIN
    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX) = '';

    SET @SqlStatement = 'SELECT [Key]
      ,[SnapshotKey]
      ,[ProductKey]
      ,[Name]
      ,[Configuration]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[CreatedBy]
      ,[LastUpdatedBy]
      ,[State]
    FROM [dbo].[view_CentralConfigurationSnapshot]';

    IF @SnapshotKey IS NOT NULL
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('SnapshotKey','=',CONVERT(NVARCHAR(MAX), @SnapshotKey),1);
    ELSE
    BEGIN
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Key','=',CONVERT(NVARCHAR(MAX), @Key),1);
    END

    IF(@WhereStatement <> '')
    BEGIN
        SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
        SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement;
        SET @SqlStatement = @SqlStatement + ' ORDER BY [CreatedStamp] DESC;';
    END

    PRINT @SqlStatement;
    EXECUTE sp_executesql @SqlStatement;
END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CommitCommandResult]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CommitCommandResult];

GO
CREATE PROCEDURE [dbo].[sp_CommitCommandResult](
    @RequestKey [UNIQUEIDENTIFIER],
    @ClientKey [UNIQUEIDENTIFIER],
    @Content NVARCHAR(MAX)
)
AS
DECLARE @NowTime AS DATETIME = GETUTCDATE();
DECLARE @Key AS UNIQUEIDENTIFIER;
BEGIN
    IF EXISTS (SELECT TOP 1 1 
        FROM [dbo].[CommandRequest] AS CRQ
            JOIN [dbo].[ProductClient] AS PC
                ON PC.[ProductKey] = CRQ.[ProductKey]
                    AND (CRQ.[ExpiredStamp] IS NULL OR [ExpiredStamp] > @NowTime)
        WHERE CRQ.[Key] = @RequestKey
            AND PC.[Key] = @ClientKey)
    BEGIN
        IF EXISTS (SELECT TOP 1 1 
            FROM [dbo].[CommandResult]
                WHERE [ClientKey] = @ClientKey
                    AND [RequestKey] = @RequestKey)
        BEGIN
            EXEC [dbo].[sp_ThrowException]
                @Name = N'sp_CommitCommandResult',
                @Code = 409,
                @Reason = N'',
                @Message = N'Result already existed for client-request.';
            RETURN;
        END
        
        SET @Key = NEWID();

        INSERT INTO [dbo].[CommandResult]
           ([Key]
           ,[ClientKey]
           ,[RequestKey]
           ,[Content]
           ,[CreatedStamp]
           ,[LastUpdatedStamp]
           ,[State])
        VALUES
            (@Key
            ,@ClientKey
            ,@RequestKey
            ,@Content
            ,@NowTime
            ,@NowTime
            ,0);
    END
    ELSE
    BEGIN
        EXEC [dbo].[sp_ThrowException]
            @Name = N'sp_CommitCommandResult',
            @Code = 403,
            @Reason = N'Request',
            @Message = N'Invalid command request for match result.';
        RETURN;
    END

    SELECT @Key;
END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetPendingCommandRequest]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_GetPendingCommandRequest];

GO
CREATE PROCEDURE [dbo].[sp_GetPendingCommandRequest](
    @ClientKey [UNIQUEIDENTIFIER]
)
AS
BEGIN
    IF @ClientKey IS NOT NULL
    BEGIN
        SELECT [Key]
      ,[ProductKey]
      ,[Action]
      ,[Parameters]
      ,[ExpiredStamp]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[State]
      FROM 
	  -- [S] Starts
      (SELECT *, ROW_NUMBER() OVER(PARTITION BY [Action] ORDER BY [CreatedStamp] DESC) AS [RowNumber]
          FROM 
			-- [R] Starts
			(SELECT CRQ.[Key]
            ,CRQ.[ProductKey]
            ,CRQ.[Action]
            ,CRQ.[Parameters]
            ,CRQ.[ExpiredStamp]
            ,CRQ.[CreatedStamp]
            ,CRQ.[LastUpdatedStamp]
            ,CRQ.[State]
            ,CRR.[Key] AS [ResponseKey]
            FROM [dbo].[CommandRequest] AS CRQ
                LEFT JOIN [dbo].[CommandResult] AS CRR
                    ON CRR.[ClientKey] = @ClientKey
                    AND CRR.[RequestKey] = CRQ.[Key]
                WHERE (CRQ.[ExpiredStamp] IS NULL OR CRQ.[ExpiredStamp] > GETUTCDATE())) 
                     -- [R] Ends
                    AS R)
                -- [S] Ends
                AS S                
            WHERE [RowNumber] < 2;
    END
END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryCommandRequest]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryCommandRequest];

GO
CREATE PROCEDURE [dbo].[sp_QueryCommandRequest](
    @ProductKey [UNIQUEIDENTIFIER],
    @Action [NVARCHAR](256),
    @FromStamp DATETIME,
    @ToStamp DATETIME
)
AS
BEGIN
    SET @ToStamp = ISNULL(@ToStamp, GETUTCDATE());

    -- Keep duration is no longer than 14 days.
    IF(@FromStamp IS NULL OR DATEDIFF(mi,@FromStamp,@ToStamp) > 20160)
        SET @FromStamp = DATEADD(mi,-20160,@ToStamp);

    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX) = '';

    SET @SqlStatement = 'SELECT [Key]
      ,[HostName]
      ,[IpAddress]
      ,[ProductKey]
      ,[ClientKey]
      ,[CpuUsage]
      ,[MemoryUsage]
      ,[GCMemory]
      ,[AssemblyHash]
      ,[CreatedStamp]
    FROM [dbo].[view_Heartbeat]';

    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('ProductKey','=',CONVERT(NVARCHAR(MAX), @ProductKey),1);
    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Action','=',@Action,1);

    IF(@WhereStatement <> '')
    BEGIN
        SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
        SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement;
        SET @SqlStatement = @SqlStatement + ' ORDER BY [CreatedStamp] DESC;';
    END

    PRINT @SqlStatement;
    EXECUTE sp_executesql @SqlStatement;
END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryCommandResult]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryCommandResult];

GO
CREATE PROCEDURE [dbo].[sp_QueryCommandResult](
    @Key [UNIQUEIDENTIFIER],
    @RequestKey [UNIQUEIDENTIFIER],
    @ProductKey [UNIQUEIDENTIFIER],
    @ClientKey [UNIQUEIDENTIFIER]
)
AS
BEGIN
    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX) = '';

    SET @SqlStatement = 'SELECT [Key]
      ,[ClientKey]
      ,[RequestKey]
      ,[Content]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[State]
    FROM [dbo].[CommandResult]';

    IF @Key IS NOT NULL
    BEGIN
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Key','=',CONVERT(NVARCHAR(MAX), @Key),1);
    END
    ELSE
    BEGIN
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('RequestKey','=',CONVERT(NVARCHAR(MAX), @RequestKey),1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('ProductKey','=',CONVERT(NVARCHAR(MAX), @ProductKey),1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('ClientKey','=',CONVERT(NVARCHAR(MAX), @ClientKey),1);
    END

    IF(@WhereStatement <> '')
    BEGIN
        SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
        SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement;
        SET @SqlStatement = @SqlStatement + ' ORDER BY [CreatedStamp] DESC;';
    END

    PRINT @SqlStatement;
    EXECUTE sp_executesql @SqlStatement;
END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_RequestCommand]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_RequestCommand];

GO
CREATE PROCEDURE [dbo].[sp_RequestCommand](
    @ProductKey [UNIQUEIDENTIFIER],
    @Action [NVARCHAR](256),
    @Parameters NVARCHAR(MAX),
    @ExpiredStamp DATETIME
)
AS
DECLARE @NowTime AS DATETIME = GETUTCDATE();
DECLARE @Key AS UNIQUEIDENTIFIER;
BEGIN
    IF @ProductKey IS NOT NULL AND @Action IS NOT NULL
    BEGIN
        SET @Key = NEWID();

        INSERT INTO [dbo].[CommandRequest]
           ([Key]
           ,[ProductKey]
           ,[Action]
           ,[Parameters]
           ,[ExpiredStamp]
           ,[CreatedStamp]
           ,[LastUpdatedStamp]
           ,[State])
     VALUES
           (@Key
           ,@ProductKey
           ,@Action
           ,@Parameters
           ,@ExpiredStamp
           ,@NowTime
           ,@NowTime
           ,0);
    END
    ELSE
    BEGIN
        EXEC [dbo].[sp_ThrowException]
            @Name = N'sp_RequestCommand',
            @Code = 400,
            @Reason = N'@ProductKey/@Action',
            @Message = N'Invalid @ProductKey or @Action';
        RETURN;
    END

    SELECT @Key;
END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryHeartbeat]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryHeartbeat];

GO
CREATE PROCEDURE [dbo].[sp_QueryHeartbeat](
    @ProductKey [UNIQUEIDENTIFIER],
    @ClientKey [UNIQUEIDENTIFIER],
    @FromStamp [DATETIME],
    @ToStamp [DATETIME]
)
AS
BEGIN
    SET @ToStamp = ISNULL(@ToStamp, GETUTCDATE());

    -- Keep duration is no longer than 14 days.
    IF(@FromStamp IS NULL OR DATEDIFF(mi,@FromStamp,@ToStamp) > 20160)
        SET @FromStamp = DATEADD(mi,-20160,@ToStamp);

    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX) = '';

    SET @SqlStatement = 'SELECT [Key]
      ,[HostName]
      ,[IpAddress]
      ,[ProductKey]
      ,[ClientKey]
      ,[ConfigurationName]
      ,[CpuUsage]
      ,[MemoryUsage]
      ,[GCMemory]
      ,[CreatedStamp]
    FROM [dbo].[view_Heartbeat]';

    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('ProductKey','=',CONVERT(NVARCHAR(MAX), @ProductKey),1);
    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('ClientKey','=',CONVERT(NVARCHAR(MAX), @ClientKey),1);
    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('CreatedStamp','>=',CONVERT(NVARCHAR(MAX), @FromStamp, 121),1);
    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('CreatedStamp','<',CONVERT(NVARCHAR(MAX), @ToStamp, 121),1);

    IF(@WhereStatement <> '')
    BEGIN
        SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
        SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement;
        SET @SqlStatement = @SqlStatement + ' ORDER BY [CreatedStamp];';
    END

    PRINT @SqlStatement;
    EXECUTE sp_executesql @SqlStatement;
END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SaveHeartbeat]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SaveHeartbeat];

GO
CREATE PROCEDURE [dbo].[sp_SaveHeartbeat](
    @ProductKey [UNIQUEIDENTIFIER],
    @HostName [NVARCHAR](128),
    @ServerName [NVARCHAR](128),
    @IpAddress [NVARCHAR](128) ,
    @ConfigurationName [NVARCHAR](256) ,
    @CpuUsage [FLOAT], -- %, @0.00, 1.00]
    @MemoryUsage [DECIMAL](18,0), -- KB 
    @TotalMemory [DECIMAL](18,0) -- KB
)
AS
DECLARE @ClientKey AS UNIQUEIDENTIFIER;
DECLARE @NowTime AS DATETIME = GETUTCDATE();
DECLARE @HeartbeatKey AS UNIQUEIDENTIFIER;
BEGIN
    IF @ProductKey IS NOT NULL
    BEGIN
        SELECT TOP 1 @ClientKey = [Key]
            FROM [dbo].[ProductClient]
            WHERE ISNULL([HostName],'') = ISNULL(@HostName, '')
                AND ISNULL([IpAddress],'') = ISNULL(@IpAddress, '')
                AND [ProductKey] = @ProductKey;

        IF @ClientKey IS NULL
        BEGIN
            SET @ClientKey = NEWID();
            INSERT INTO [dbo].[ProductClient]
                ([Key]
                ,[HostName]
                ,[ServerName]
                ,[IpAddress]
                ,[ConfigurationName]
                ,[ProductKey]
                ,[LastHeartbeatStamp]
                ,[CreatedStamp]
                ,[LastUpdatedStamp]
                ,[State])
            VALUES
                (@ClientKey
                ,@HostName
                ,@ServerName
                ,@IpAddress
                ,@ConfigurationName
                ,@ProductKey
                ,@NowTime
                ,@NowTime
                ,@NowTime
                ,0) ;
        END

        SET @HeartbeatKey = NEWID();
        INSERT INTO [dbo].[Heartbeat]
            ([Key]
            ,[ClientKey]
            ,[CpuUsage]
            ,[MemoryUsage]
            ,[TotalMemory]
            ,[ConfigurationName]
            ,[CreatedStamp])
        VALUES
            (@HeartbeatKey
            ,@ClientKey
            ,@CpuUsage
            ,@MemoryUsage
            ,@TotalMemory
            ,@ConfigurationName
            ,@NowTime);
    END

    SELECT @ClientKey;    
END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryProductClient]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryProductClient];

GO
CREATE PROCEDURE [dbo].[sp_QueryProductClient](
    @ProductKey [UNIQUEIDENTIFIER],
    @HostName [NVARCHAR](128),
    @ServerName [NVARCHAR](128),
    @IpAddress [NVARCHAR](128),
    @FromStamp [DATETIME],
    @ToStamp [DATETIME]
)
AS
BEGIN
    SET @ToStamp = ISNULL(@ToStamp, GETUTCDATE());

    -- Keep duration is no longer than 14 days.
    IF(@FromStamp IS NULL OR DATEDIFF(mi,@FromStamp,@ToStamp) > 20160)
        SET @FromStamp = DATEADD(mi,-20160,@ToStamp);

    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX) = '';

    SET @SqlStatement = 'SELECT [Key]
      ,[HostName]
      ,[ServerName]
      ,[IpAddress]
      ,[ProductKey]
      ,[ConfigurationName]
      ,[TotalMemory]
      ,[LastHeartbeatStamp]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[State]
    FROM [dbo].[ProductClient]';

    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('ProductKey','=',CONVERT(NVARCHAR(MAX), @ProductKey),1);
    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('HostName','=',@HostName,1);
    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('ServerName','=',@ServerName,1);
    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('IpAddress','=',@IpAddress,1);
    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('LastHeartbeatStamp','>=',CONVERT(NVARCHAR(MAX), @FromStamp, 121),1);
    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('LastHeartbeatStamp','<',CONVERT(NVARCHAR(MAX), @ToStamp, 121),1);

    IF(@WhereStatement <> '')
    BEGIN
        SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
        SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement;
        SET @SqlStatement = @SqlStatement + ' ORDER BY [HostName],[ServerName];';
    END

    PRINT @SqlStatement;
    EXECUTE sp_executesql @SqlStatement;
END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateOrUpdateProduct]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CreateOrUpdateProduct];

GO
CREATE PROCEDURE [dbo].[sp_CreateOrUpdateProduct](
    @Key UNIQUEIDENTIFIER,
    @Name NVARCHAR(MAX),
    @Token NVARCHAR(512),
    @PublicKey NVARCHAR(MAX),
    @PrivateKey NVARCHAR(MAX),
    @ExpiredStamp DATETIME NULL
)
AS
DECLARE @NowTime AS DATETIME = GETUTCDATE();
BEGIN
    IF @Key IS NULL
    BEGIN
        SET @Key = NEWID();

        INSERT INTO [dbo].[ProductInfo]
           ([Key]
           ,[Name]
           ,[Token]
           ,[PublicKey]
           ,[PrivateKey]
           ,[ExpiredStamp]
           ,[CreatedStamp]
           ,[LastUpdatedStamp]
           ,[State])
     VALUES
           (@Key
           ,ISNULL(@Name, '')
           ,@Token
           ,@PublicKey
           ,@PrivateKey
           ,@ExpiredStamp
           ,@NowTime
           ,@NowTime
           ,0);
    END
    ELSE IF EXISTS (SELECT TOP 1 1 FROM [dbo].[ProductInfo] WHERE [Key] = @Key 
        AND [dbo].[fn_ObjectCanUpdateOrDelete]([State]) = 1)
    BEGIN
        UPDATE [dbo].[ProductInfo]
            SET 
                [Name] = ISNULL(@Name, [Name]),
                [Token] = ISNULL(@Token, [Token]),
                [PublicKey] = ISNULL(@PublicKey, [PublicKey]),
                [PrivateKey] = ISNULL(@PrivateKey, [PrivateKey]),
                [ExpiredStamp] = ISNULL(@ExpiredStamp, [ExpiredStamp]),                
                [LastUpdatedStamp] = @NowTime
            WHERE [Key] = @Key;
    END
    ELSE
    BEGIN
        EXEC [dbo].[sp_ThrowException]
            @Name = N'sp_CreateOrUpdateProduct',
            @Code = 403,
            @Reason = N'[State]',
            @Message = N'Target is not found or cannot modify.';
        RETURN;
    END

    SELECT @Key;
END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetProductInfoByToken]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_GetProductInfoByToken];

GO
CREATE PROCEDURE [dbo].[sp_GetProductInfoByToken](
    @Token [NVARCHAR](512)
)
AS
BEGIN
    SELECT TOP 1 [Key]
        ,[Name]
        ,[Token]
        ,[PublicKey]
        ,[PrivateKey]
        ,[ExpiredStamp]
        ,[CreatedStamp]
        ,[LastUpdatedStamp]
        ,[State]
    FROM [dbo].[ProductInfo]
    WHERE [Token] = @Token
        AND ([ExpiredStamp] IS NULL OR [ExpiredStamp] > GETUTCDATE())
        AND [dbo].[fn_ObjectIsWorkable]([State]) = 1
END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryProductInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryProductInfo];

GO
CREATE PROCEDURE [dbo].[sp_QueryProductInfo](
    @Key UNIQUEIDENTIFIER
)
AS
BEGIN
    SELECT [Key]
        ,[Name]
        ,[Token]
        ,[PublicKey]
        ,[PrivateKey]
        ,[ExpiredStamp]
        ,[CreatedStamp]
        ,[LastUpdatedStamp]
        ,[State]
    FROM [dbo].[ProductInfo]
    WHERE (@Key IS NULL OR [Key] = @Key)
        AND [dbo].[fn_ObjectIsWorkable]([State]) = 1;
END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteProductInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_DeleteProductInfo];

GO
CREATE PROCEDURE [dbo].[sp_DeleteProductInfo](
    @Key UNIQUEIDENTIFIER
)
AS
DECLARE @NowTime AS DATETIME = GETUTCDATE();
BEGIN
    IF EXISTS (SELECT TOP 1 1 FROM [dbo].[ProductInfo] WHERE [Key] = @Key 
        AND [dbo].[fn_ObjectCanUpdateOrDelete]([State]) = 1)
    BEGIN
        UPDATE [dbo].[ProductInfo]
            SET 
                [State] = [dbo].[fn_SetObjectAsDeleted]([State]),          
                [LastUpdatedStamp] = @NowTime
            WHERE [Key] = @Key;
    END
    ELSE
    BEGIN
        EXEC [dbo].[sp_ThrowException]
            @Name = N'sp_DeleteProductInfo',
            @Code = 403,
            @Reason = N'[State]',
            @Message = N'Target is not found or cannot modify.';
        RETURN;
    END

    SELECT @Key;
END

GO