IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[view_BinaryOffsiteDistribution]') AND type in (N'V'))
DROP VIEW [dbo].[view_BinaryOffsiteDistribution]
GO

CREATE VIEW [dbo].[view_BinaryOffsiteDistribution]
AS
    SELECT BOD.[Key]
        ,BOD.[Identifier]
        ,BOD.[Container]
        ,BOD.[HostRegion]
        ,BOHR.[Country]
        ,BOD.[UploadCredentialExpiredStamp]
        ,BOD.[CreatedStamp]
        ,BOD.[LastUpdatedStamp]
        ,BOD.[State]
    FROM [dbo].[BinaryOffsiteDistribution] AS BOD
        JOIN [dbo].[BinaryOffsiteHostRegion] AS BOHR
            ON BOD.[HostRegion] = BOHR.[HostRegion]
                AND [dbo].[fn_ObjectIsWorkable](BOHR.[State]) = 1
                AND BOD.[State] = 2; --Committed

GO
