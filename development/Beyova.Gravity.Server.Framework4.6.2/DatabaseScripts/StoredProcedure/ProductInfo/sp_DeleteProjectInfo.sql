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