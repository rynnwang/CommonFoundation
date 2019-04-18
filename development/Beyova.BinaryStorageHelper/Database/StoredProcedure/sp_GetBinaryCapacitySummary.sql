CREATE PROCEDURE [dbo].[sp_GetBinaryCapacitySummary](
    @Container NVARCHAR(128),
    @OwnerKey UNIQUEIDENTIFIER
)
AS
SET NOCOUNT ON;
BEGIN
    SELECT 
        @Container AS [Container],
        @OwnerKey AS [OwnerKey],
        (SELECT COUNT(*)
            FROM [dbo].[view_UserBinaryStorageMetaData]
                WHERE (@OwnerKey IS NULL OR [OwnerKey] = @OwnerKey) 
                AND (@Container IS NULL OR [Container] = @Container)) AS [Count],
        (SELECT SUM([Length])
            FROM [dbo].[view_UserBinaryStorageMetaData]
                WHERE (@OwnerKey IS NULL OR [OwnerKey] = @OwnerKey) 
                AND (@Container IS NULL OR [Container] = @Container)) AS [Size];
END
GO