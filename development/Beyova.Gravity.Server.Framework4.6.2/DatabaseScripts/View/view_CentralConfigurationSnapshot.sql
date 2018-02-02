IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[view_CentralConfigurationSnapshot]') AND type in (N'V'))
DROP VIEW [dbo].[view_CentralConfigurationSnapshot]
GO

CREATE VIEW [dbo].[view_CentralConfigurationSnapshot]
AS
    SELECT CC.[Key]
      ,CC.[SnapshotKey]      
      ,CC.[ProductKey]
      ,CCS.[Name]
      ,CCS.[Configuration]
      ,CCS.[CreatedStamp]
      ,CCS.[CreatedStamp] AS [LastUpdatedStamp]
      ,CCS.[CreatedBy]
      ,CCS.[CreatedBy] AS [LastUpdatedBy]
      ,CCS.[State]
  FROM [dbo].[CentralConfigurationSnapshot] AS CCS
    LEFT JOIN [dbo].[CentralConfiguration] AS CC
        ON CC.[SnapshotKey] = CCS.[Key]


GO



