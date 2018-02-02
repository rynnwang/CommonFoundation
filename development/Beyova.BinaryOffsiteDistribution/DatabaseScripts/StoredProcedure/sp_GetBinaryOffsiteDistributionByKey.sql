IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetBinaryOffsiteDistributionByKey]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_GetBinaryOffsiteDistributionByKey]
GO

CREATE PROCEDURE [dbo].[sp_GetBinaryOffsiteDistributionByKey](
    @Key UNIQUEIDENTIFIER
)
AS
SET NOCOUNT ON;
BEGIN
    SELECT TOP 1 [Key]
        ,[Identifier]
        ,[Container]
        ,[HostRegion]
        ,[UploadCredentialExpiredStamp]
        ,[CreatedStamp]
        ,[LastUpdatedStamp]
        ,[State]
    FROM [dbo].[BinaryOffsiteDistribution]
    WHERE [Key] = @Key;
END
GO


