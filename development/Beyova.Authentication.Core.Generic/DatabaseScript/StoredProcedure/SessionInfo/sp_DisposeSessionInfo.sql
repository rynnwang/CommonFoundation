CREATE PROCEDURE [dbo].[sp_DisposeSessionInfo](
    @Token VARCHAR(512)
)
AS
SET NOCOUNT ON;
BEGIN
    DECLARE @NowTime AS DATETIME = GETUTCDATE();
    UPDATE [dbo].[SessionInfo]
        SET [ExpiredStamp] = @NowTime,
            [LastUpdatedStamp] = @NowTime
            WHERE [Token] = @Token;
END
GO


