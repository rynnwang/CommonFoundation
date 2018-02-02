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