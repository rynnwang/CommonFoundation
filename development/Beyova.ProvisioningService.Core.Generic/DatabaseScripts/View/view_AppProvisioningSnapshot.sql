IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[view_AppProvisioningSnapshot]') AND type in (N'V'))
DROP VIEW [dbo].[view_AppProvisioningSnapshot]
GO

CREATE VIEW [dbo].[view_AppProvisioningSnapshot]
AS
    SELECT APS.[Key]
      ,APS.[SnapshotKey]
      ,APS.[PlatformKey]
      ,AP.[Name]
      ,APS.[Content]
      ,APS.[CreatedStamp]
      ,APS.[CreatedStamp] AS [LastUpdatedStamp]
      ,APS.[CreatedBy]
      ,APS.[CreatedBy] AS [LastUpdatedBy]
      ,APS.[State]
    FROM [dbo].[AppProvisioningSnapshot] AS APS
        JOIN [dbo].[AppProvisioning] AS AP
            ON AP.[Key] = APS.[ProvisioningKey];
GO