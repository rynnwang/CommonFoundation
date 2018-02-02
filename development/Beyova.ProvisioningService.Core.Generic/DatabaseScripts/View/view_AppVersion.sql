IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[view_AppVersion]') AND type in (N'V'))
DROP VIEW [dbo].[view_AppVersion]
GO

CREATE VIEW [dbo].[view_AppVersion]
AS
    SELECT AV.[Key]
      ,AP.[Name]
      ,AV.[PlatformKey]
      ,AV.[LatestBuild]
      ,AV.[LatestVersion]
      ,AV.[MinRequiredBuild]
      ,AV.[Note]
      ,AV.[AppServiceStatus]
      ,AP.[Url]
      ,AP.[BundleId]
      ,AP.[MinOSVersion]
      ,AP.[PlatformType]
      ,AV.[CreatedStamp]
      ,AV.[LastUpdatedStamp]
      ,AV.[CreatedBy]
      ,AV.[LastUpdatedBy]
      ,AV.[State]
    FROM [dbo].[AppVersion] AS AV
        JOIN [dbo].[AppPlatform] AS AP
            ON AP.[Key] = AV.[PlatformKey]
                AND [dbo].[fn_ObjectIsWorkable](AP.[State]) = 1
        WHERE [dbo].[fn_ObjectIsWorkable](AV.[State]) = 1
GO