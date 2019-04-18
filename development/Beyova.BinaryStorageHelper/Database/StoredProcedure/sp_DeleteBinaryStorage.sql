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


