IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[view_AppProvisioning]') AND type in (N'V'))
DROP VIEW [dbo].[view_AppProvisioning]
GO

CREATE VIEW [dbo].[view_AppProvisioning]
AS
    SELECT AP.[Key]
      ,AP.[SnapshotKey]
      ,AP.[PlatformKey]
      ,AP.[Name]
      ,APS.[Content]
      ,AP.[CreatedStamp]
      ,APS.[CreatedStamp] AS [LastUpdatedStamp]
      ,AP.[CreatedBy]
      ,APS.[CreatedBy] AS [LastUpdatedBy]
      ,APS.[State]
    FROM [dbo].[AppProvisioning] AS AP
        JOIN [dbo].[AppProvisioningSnapshot] AS APS
            ON AP.[SnapshotKey] = APS.[Key];
GO