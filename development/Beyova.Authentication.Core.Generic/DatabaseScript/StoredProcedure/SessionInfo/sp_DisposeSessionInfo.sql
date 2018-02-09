CREATE PROCEDURE [dbo].[sp_DisposeSessionInfo]
(
    @Token VARCHAR(512),
    @Realm VARCHAR(128)
)
AS
SET NOCOUNT ON;
BEGIN
    DECLARE @sql NVARCHAR(MAX);
    SET @sql= '
    DECLARE @NowTime AS DATETIME = GETUTCDATE();
	
    UPDATE [dbo].[SessionInfo]
    SET [ExpiredStamp] = @NowTime,
        [LastUpdatedStamp] = @NowTime';
    SET @sql = @sql + ' WHERE [Token] = ''' + CONVERT(NVARCHAR(MAX), @Token) + '''  ';
    IF @Realm IS NOT NULL AND @Realm != ''
    BEGIN
        SET @sql = @sql + ' AND [Realm] = ''' + CONVERT(NVARCHAR(MAX), @Realm) + '''  ';
    END;

    --PRINT @sql;
    EXECUTE sp_executesql @sql;
END;
GO
