IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetBinaryOffsiteDistributionByCountry]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_GetBinaryOffsiteDistributionByCountry]
GO

CREATE PROCEDURE [dbo].[sp_GetBinaryOffsiteDistributionByCountry](
    @Identifier UNIQUEIDENTIFIER,
    @Container NVARCHAR(128),
    @Country [NVARCHAR](16)
)
AS
SET NOCOUNT ON;
BEGIN
    SELECT TOP 1 [Key]
        ,[Identifier]
        ,[Container]
        ,[HostRegion]
        ,[UploadCredentialExpiredStamp]
        ,[Country]
        ,[CreatedStamp]
        ,[LastUpdatedStamp]
        ,[State]
     FROM [dbo].[view_BinaryOffsiteDistribution]
     WHERE [Identifier] = @Identifier
        AND (@Container IS NULL OR [Container] = @Container)
        AND [Country] = @Country;
END
GO


