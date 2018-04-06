
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DisposeSessionInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_DisposeSessionInfo]
GO

CREATE PROCEDURE [dbo].[sp_DisposeSessionInfo](
    @Token VARCHAR(512),
    @Realm NVARCHAR(64)
)
AS
SET NOCOUNT ON;
BEGIN
    DECLARE @NowTime AS DATETIME = GETUTCDATE();

    UPDATE [dbo].[SessionInfo]
        SET [ExpiredStamp] = @NowTime,
            [LastUpdatedStamp] = @NowTime
            WHERE [Token] = @Token
                AND ([Realm] IS NULL OR [Realm] = '' OR [Realm] = @Realm);
END
GO


