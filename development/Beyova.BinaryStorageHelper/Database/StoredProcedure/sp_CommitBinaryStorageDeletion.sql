/* -----------------------------------
@Identifiers SAMPLE:
[{
    "Container": "xxx",
    "Identifier": "xxx"
}]
----------------------------------- 
*/
CREATE PROCEDURE [dbo].[sp_CommitBinaryStorageDeletion](
    @Identifiers NVARCHAR(MAX)
)
AS
SET NOCOUNT ON;
BEGIN
    DECLARE @NowTime AS DATETIME = GETUTCDATE();

    IF @Identifiers IS NOT NULL
    BEGIN
        UPDATE [dbo].[BinaryStorageMetaData]
            SET [State] = 4, --Deleted
                [LastUpdatedStamp] = @NowTime
            FROM [dbo].[BinaryStorageMetaData] AS BSM
                JOIN [dbo].[fn_JsonToBinaryIdentifiers](@Identifiers) AS B
                    ON B.[Container] = BSM.[Container] 
                        AND B.[Identifier] = BSM.[Identifier] 
                        AND BSM.[State] = 3; --DeletePending
    END
END
GO


