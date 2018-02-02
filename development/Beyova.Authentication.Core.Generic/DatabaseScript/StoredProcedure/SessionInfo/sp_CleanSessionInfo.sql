CREATE PROCEDURE [dbo].[sp_CleanSessionInfo] (
    @Stamp DATETIME
)
AS
SET NOCOUNT ON;
BEGIN
    IF @Stamp IS NOT NULL AND @Stamp < GETUTCDATE()
        DELETE FROM [dbo].[SessionInfo]
            WHERE [CreatedStamp] < @Stamp
                AND [ExpiredStamp] < @Stamp;
END

GO
