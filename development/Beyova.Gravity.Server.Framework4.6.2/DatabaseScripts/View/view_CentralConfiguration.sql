IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[view_CentralConfiguration]') AND type in (N'V'))
DROP VIEW [dbo].[view_CentralConfiguration]
GO

CREATE VIEW [dbo].[view_CentralConfiguration]
AS
    SELECT CC.[Key]
      ,CC.[SnapshotKey]      
      ,CC.[ProductKey]
      ,CCS.[Name]
      ,CCS.[Configuration]
      ,CC.[CreatedStamp]
      ,CCS.[CreatedStamp] AS [LastUpdatedStamp]
      ,CC.[CreatedBy]
      ,CCS.[CreatedBy] AS [LastUpdatedBy]
      ,CCS.[State]
  FROM [dbo].[CentralConfiguration] AS CC
    LEFT JOIN [dbo].[CentralConfigurationSnapshot] AS CCS
        ON CC.[SnapshotKey] = CCS.[Key]
    WHERE CC.[IsDeleted] = 0;


GO



