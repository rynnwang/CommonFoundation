CREATE PROCEDURE [dbo].[sp_ConvertPendingCommitToPendingDelete](
    @Stamp DATETIME
)
AS
SET NOCOUNT OFF;
BEGIN
    UPDATE [dbo].[BinaryStorageMetaData]
        SET 
            [LastUpdatedStamp] = GETUTCDATE(),
            [State] = 3 -- DeletePending
        WHERE 
            [CreatedStamp] < @Stamp AND
            [State] = 1; --CommitPending
END
GO


