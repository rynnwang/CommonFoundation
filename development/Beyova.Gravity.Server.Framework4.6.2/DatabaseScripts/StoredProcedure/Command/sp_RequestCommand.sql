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