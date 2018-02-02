IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetBinaryOffsiteDistributionByIdentifiers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_GetBinaryOffsiteDistributionByIdentifiers]
GO

CREATE PROCEDURE [dbo].[sp_GetBinaryOffsiteDistributionByIdentifiers](
    @Xml XML,
    @Container NVARCHAR(128)
)
AS
SET NOCOUNT ON;
BEGIN
    SELECT [Key]
        ,[Identifier]
        ,[Container]
        ,[HostRegion]
        ,[UploadCredentialExpiredStamp]
        ,[CreatedStamp]
        ,[LastUpdatedStamp]
        ,[State]
     FROM [dbo].[BinaryOffsiteDistribution] AS BOD
        JOIN [dbo].[fn_XmlListToGuidTable](@Xml) AS [Keys]
            ON [Keys].[Value] = BOD.[Identifier]
     WHERE (@Container IS NULL OR [Container] = @Container)
        AND ([State] IN (1,2));
END
GO


