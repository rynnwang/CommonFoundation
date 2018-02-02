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